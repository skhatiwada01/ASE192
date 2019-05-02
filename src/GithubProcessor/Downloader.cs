using GithubProcessor.BugLocalization;
using LibGit2Sharp;
using libsvm;
using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Credentials = Octokit.Credentials;
using Dtos = GithubProcessor.Models;

namespace GithubProcessor
{
    public class Downloader
    {
        public GitHubClient GitHubClient { get; }

        public long RepositoryId { get; private set; }

        public string RepositoryFullPath { get; private set; }

        private ILog _logger;

        public Downloader(string productName, string userName, string password)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                GitHubClient = new GitHubClient(new ProductHeaderValue(productName), new InMemoryCredentialStore(new Credentials(userName, password)));
            }
        }

        public void SetLogger(ILog logger)
        {
            _logger = logger;
        }

        private void Log(string text)
        {
            _logger?.Log(text);
        }

        public void SetProject(long repositoryId, string projectPath)
        {
            RepositoryId = repositoryId;
            RepositoryFullPath = Path.Combine(projectPath, repositoryId.ToString());
        }

        #region Download Tags

        public void DownloadTags(string fileName)
        {
            var result = GitHubClient.Issue.Labels.GetAllForRepository(RepositoryId).Result;
            var labels = result.Select((x, index) => new Dtos.Tag
            {
                Id = index + 1,
                Name = x.Name,
                Color = x.Color,
                Url = x.Url
            }).ToList();
            labels.SerializeToXmlFile(Path.Combine(RepositoryFullPath, fileName));
        }

        #endregion Download Tags

        #region Download Issues

        public void DownloadAllIssues(string fileName)
        {
            var issueFullPath = RepositoryFullPath + @"\Issues";
            if (!Directory.Exists(issueFullPath))
                Directory.CreateDirectory(issueFullPath);

            var requestOption = new RepositoryIssueRequest()
            {
                State = ItemStateFilter.All,
            };
            var apiOption = new ApiOptions
            {
                PageSize = 100,
                PageCount = 1,
                StartPage = 1
            };

            var count = new DirectoryInfo(issueFullPath).GetFiles()
                .Select(x => x.Name.Replace("Part_", "").Replace("_Issues.txt", ""))
                .Select(int.Parse)
                .OrderByDescending(x => x)
                .FirstOrDefault() + 1;

            var totalIssuesCount = 0;
            do
            {
                try
                {
                    apiOption.StartPage = count;

                    Log("Running: " + count);

                    var issues = GitHubClient.Issue.GetAllForRepository(RepositoryId, requestOption, apiOption).Result;
                    if (issues.Count == 0)
                        break;
                    var issueWithoutPullRequest = issues
                        .Where(x => x.PullRequest == null)
                        .Select(GithubIssueToIssue)
                        .ToList();
                    issueWithoutPullRequest.SerializeToXmlFile(Path.Combine(issueFullPath, @"Part_" + count + "_" + fileName));

                    count++;
                    totalIssuesCount += issueWithoutPullRequest.Count;
                    Log("Total Issues: " + totalIssuesCount);
                    Thread.Sleep(15000);
                }
                catch (Exception)
                {
                    Log("Sleeping. Will wake up at: " + DateTime.Now.AddMilliseconds(960000));
                    Thread.Sleep(960000);
                }

            } while (true);
        }

        private static Dtos.Issue GithubIssueToIssue(Issue issue)
        {
            if (issue == null)
                return null;

            return new Dtos.Issue
            {
                Id = issue.Id,
                IssueNumber = issue.Number,
                AssigneeId = issue.Assignee?.Id,
                AssigneeIds = issue.Assignees?.Select(x => x.Id).ToList(),
                ClosedBy = issue.ClosedBy?.Id,
                CreatedBy = issue.User?.Id,
                EventsUrl = issue.EventsUrl,
                Title = issue.Title,
                Body = issue.Body,
                CommentsCount = issue.Comments,
                CommentsUrl = issue.CommentsUrl,
                Labels = issue.Labels?.Select(x => x.Name).ToList(),
            };
        }

        #endregion Download Issues

        #region All Commits

        public void DownloadAllCommits()
        {
            Log("Commits Downloading");
            var commits = GitHubClient.Repository.Commit.GetAll(RepositoryId).Result;
            var result = commits.OrderByDescending(x => x.Commit.Committer.Date)
                .Select(x => new Dtos.Commit
                {
                    CommitDateTime = x.Commit.Committer.Date.UtcDateTime,
                    Sha = x.Sha,
                    UserId = x.Committer?.Id
                }).ToList();
            result.SerializeToXmlFile(Path.Combine(RepositoryFullPath, "AllCommits.txt"));

            Log("Commits downloaded");
        }

        #endregion All Commits

        #region Issue-Commits-Change

        public void CreateIssueCommits()
        {
            var issueCommitFilePath = Path.Combine(RepositoryFullPath, "IssueCommits.txt");
            if (!File.Exists(issueCommitFilePath))
            {
                var issueFolderPath = Path.Combine(RepositoryFullPath, "Issues");
                var newIssueWithCommits = new List<Dtos.IssueCommit>();
                new DirectoryInfo(issueFolderPath).GetFiles().ToList().ForEach(fileInfo =>
                {
                    var issues = SerializerHelper.DeserializeFromXmlFile<List<Dtos.Issue>>(fileInfo.FullName);
                    newIssueWithCommits.AddRange(issues.Select(x => new Dtos.IssueCommit { IssueNumber = x.IssueNumber, CommitSha = null }));
                });
                newIssueWithCommits.SerializeToXmlFile(issueCommitFilePath);
            }

            var issueWithCommits = SerializerHelper.DeserializeFromXmlFile<List<Dtos.IssueCommit>>(issueCommitFilePath);
            var allCommits = SerializerHelper.DeserializeFromXmlFile<List<Dtos.Commit>>(Path.Combine(RepositoryFullPath, "AllCommits.txt"))
                .Select(x => x.Sha)
                .ToList();
            int counter = 0;
            int total = issueWithCommits.Count;
            int foundCounter = 0;
            foreach (var issueWithCommit in issueWithCommits.ToList())
            {
                Log(++counter + " of " + total);
                try
                {
                    if (issueWithCommit.CommitSha == null)
                    {
                        var events = GitHubClient.Issue.Events.GetAllForIssue(RepositoryId, issueWithCommit.IssueNumber).Result;
                        var indexedEvents = events.Select((x, index) => new { EventInfo = x, Index = index }).ToList();
                        var closedIndex = indexedEvents.LastOrDefault(x => x.EventInfo.Event.StringValue == "closed")?.Index;
                        var commitId = "";
                        if (closedIndex.HasValue)
                        {
                            for (int i = closedIndex.Value; i >= 0; i--)
                            {
                                if (indexedEvents[i].EventInfo.Event.StringValue == "referenced" && allCommits.Contains(indexedEvents[i].EventInfo.CommitId))
                                {
                                    commitId = indexedEvents[i].EventInfo.CommitId;
                                }
                            }
                        }

                        issueWithCommit.CommitSha = commitId;
                        issueWithCommits.SerializeToXmlFile(issueCommitFilePath);
                        Log("Found: " + (string.IsNullOrWhiteSpace(commitId) ? foundCounter : ++foundCounter));
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.ToLowerInvariant().Contains("not found") || (e.InnerException != null && e.InnerException.Message.ToLowerInvariant().Contains("not found")) || (e.InnerException != null && e.InnerException.Message.ToUpperInvariant().Contains("VALUE WAS EITHER TOO LARGE OR TOO SMALL FOR")))
                        continue;

                    var rateLimit = GitHubClient.GetLastApiInfo().RateLimit;
                    double milliSeconds = 3600000;
                    if (rateLimit != null)
                    {
                        var resetAt = rateLimit.Reset.UtcDateTime;
                        milliSeconds = (resetAt - DateTime.UtcNow).TotalMilliseconds;
                    }
                    if (milliSeconds < 0)
                        milliSeconds = 15 * 1000 * 60; //15 mins
                    var intMili = (int)Math.Floor(milliSeconds) + 60000;
                    Log("Sleeping at: " + DateTime.Now + " for " + milliSeconds / 1000 / 60 + " minutes.");
                    Thread.Sleep(intMili);
                }
            }
        }

        public void DownloadCommitChangeInfo()
        {
            var commitChangeInfoFilePath = Path.Combine(RepositoryFullPath, "CommitChanges.txt");
            if (!File.Exists(commitChangeInfoFilePath))
            {
                var allIssueCommits = SerializerHelper.DeserializeFromXmlFile<List<Dtos.IssueCommit>>(Path.Combine(RepositoryFullPath, "IssueCommits.txt"));
                var newCommitChangeInfos = allIssueCommits
                    .Where(x => !string.IsNullOrWhiteSpace(x.CommitSha))
                    .Select(x => x.CommitSha)
                    .Distinct()
                    .Select(x => new Dtos.CommitChangeFileInfo
                    {
                        Sha = x,
                        CommitFiles = null,
                        Url = null,
                    })
                    .ToList();
                newCommitChangeInfos.SerializeToXmlFile(commitChangeInfoFilePath);
            }

            var commitChangeInfos = SerializerHelper.DeserializeFromXmlFile<List<Dtos.CommitChangeFileInfo>>(commitChangeInfoFilePath);
            int counter = 0;
            int total = commitChangeInfos.Count;
            foreach (var commitChangeInfo in commitChangeInfos.ToList())
            {
                Log(++counter + " of " + total);
                bool downloaded;
                do
                {
                    if (commitChangeInfo.CommitFiles.Count != 0)
                    {
                        downloaded = true;
                    }
                    else
                    {
                        try
                        {
                            var commitInfo = GitHubClient.Repository.Commit.Get(RepositoryId, commitChangeInfo.Sha).Result;
                            UpdateGithubCommitToCommitWithFile(commitInfo, commitChangeInfo);
                            commitChangeInfos.SerializeToXmlFile(commitChangeInfoFilePath);
                            downloaded = true;
                            Thread.Sleep(200);
                        }
                        catch (Exception e)
                        {
                            if (e.Message.ToLowerInvariant().Contains("not found") || (e.InnerException != null && e.InnerException.Message.ToLowerInvariant().Contains("not found")))
                            {
                                downloaded = true;
                            }
                            else
                            {
                                downloaded = false;

                                var rateLimit = GitHubClient.GetLastApiInfo().RateLimit;
                                double milliSeconds = 3600000;
                                if (rateLimit != null)
                                {
                                    var resetAt = rateLimit.Reset.UtcDateTime;
                                    milliSeconds = (resetAt - DateTime.UtcNow).TotalMilliseconds;
                                }
                                var intMili = (int)Math.Floor(milliSeconds) + 60000;
                                Log("Sleeping at: " + DateTime.Now + " for " + milliSeconds / 1000 / 60 + " minutes.");
                                Thread.Sleep(intMili < 0 ? 0 : intMili);
                            }
                        }
                    }
                } while (!downloaded);
            }
        }

        private static void UpdateGithubCommitToCommitWithFile(GitHubCommit commit, Dtos.CommitChangeFileInfo changeInfo)
        {
            changeInfo.CommitFiles = commit.Files.Select(GithubCommitFileToCommitFile).ToList();
            changeInfo.Sha = commit.Sha;
            changeInfo.Url = commit.HtmlUrl;
        }

        private static Dtos.CommitFile GithubCommitFileToCommitFile(GitHubCommitFile commitFile)
        {
            return new Dtos.CommitFile
            {
                Sha = commitFile.Sha,
                Filename = commitFile.Filename,
                Status = commitFile.Status,
                PreviousFileName = commitFile.PreviousFileName,
                RawUrl = commitFile.RawUrl,
            };
        }

        #endregion Issue-Commits-Change

        #region Localization Dataset

        public void CreateLocalizationDataset(string extensionUnparsed)
        {
            List<string> extensions = null;
            if (string.IsNullOrWhiteSpace(extensionUnparsed))
                extensions.Add("*");
            else
                extensions = extensionUnparsed.SplitWith("|").ToList();

            var localizationFolderPath = RepositoryFullPath + @"\Localization";
            if (!Directory.Exists(localizationFolderPath))
            {
                Directory.CreateDirectory(localizationFolderPath);
                Thread.Sleep(100);
            }

            var datasetCompletedFilePath = localizationFolderPath + @"\DatasetCompleted.txt";
            if (!File.Exists(datasetCompletedFilePath))
            {
                File.Create(datasetCompletedFilePath).Close();
                Thread.Sleep(100);
            }
            var datasetCompletedIssues = File.ReadAllLines(datasetCompletedFilePath).Select(int.Parse).ToList();

            var localizationAbortedFilePath = localizationFolderPath + @"\LocalizationAborted.txt";
            if (!File.Exists(localizationAbortedFilePath))
            {
                File.Create(localizationAbortedFilePath).Close();
                Thread.Sleep(100);
            }
            var localizationAbortedIssues = File.ReadAllLines(localizationAbortedFilePath).Select(int.Parse).ToList();

            var downloadAbortedFilePath = RepositoryFullPath + @"\Source\DownloadAborted.txt";
            if (!Directory.Exists(RepositoryFullPath + @"\Source"))
            {
                Directory.CreateDirectory((RepositoryFullPath + @"\Source"));
                Thread.Sleep(100);
            }
            if (!File.Exists(downloadAbortedFilePath))
            {
                File.Create(downloadAbortedFilePath).Close();
                Thread.Sleep(100);
            }
            var downloadAbortedShas = File.ReadAllLines(downloadAbortedFilePath).ToList();

            var allIssuesToLocalizeWithSha = SerializerHelper.DeserializeFromXmlFile<List<Dtos.IssueCommit>>(RepositoryFullPath + @"\IssueCommits.txt")
                .Where(x => !string.IsNullOrWhiteSpace(x.CommitSha))
                .Where(x => !datasetCompletedIssues.Contains(x.IssueNumber))
                .Where(x => !localizationAbortedIssues.Contains(x.IssueNumber))
                .Where(x => !downloadAbortedShas.Contains(x.CommitSha))
                .ToDictionary(x => x.IssueNumber, x => x.CommitSha);
            var commitChanges = SerializerHelper.DeserializeFromXmlFile<List<Dtos.CommitChangeFileInfo>>(RepositoryFullPath + @"\CommitChanges.txt")
                .Where(x => allIssuesToLocalizeWithSha.Values.Contains(x.Sha))
                .ToDictionary(x => x.Sha, x => x);

            Log("Downloading Original");
            DownloadOriginalIfNotExists();

            var myObj = new object();
            var originalSourceLink = RepositoryFullPath + @"\Source\Original";

            var issuesToLocalizeWithShaGrouped = allIssuesToLocalizeWithSha.GroupBy(x => x.Value).Select(x => new { Sha = x.Key, Issues = x.Select(y => y.Key).ToList() }).ToList();
            var total = issuesToLocalizeWithShaGrouped.Count;
            int counter = 0;
            foreach(var issuesToLocalizeWithSha in issuesToLocalizeWithShaGrouped)
            {
                var commitChangeFiles = commitChanges[issuesToLocalizeWithSha.Sha].CommitFiles
                    .Select(x => x.Filename.ToLowerInvariant())
                    .Select(x => x.Replace("/", @"\"))
                    .ToList();
                var commitFilesValidation = extensions == null ? commitChangeFiles.Any() : commitChangeFiles.Any(x => extensions.Any(e => x.EndsWith("." + e)));
                if (!commitFilesValidation)
                {
                    issuesToLocalizeWithSha.Issues.ForEach(x => localizationAbortedIssues.Add(x));
                    File.WriteAllLines(localizationAbortedFilePath, localizationAbortedIssues.Select(x => x.ToString()));
                    continue;
                }

                Log(counter++ + " of " + total + " Downloading Source: " + issuesToLocalizeWithSha.Sha + " Started at: " + DateTime.Now);
                try
                {
                    DownloadCommitSource(originalSourceLink, issuesToLocalizeWithSha.Sha, false);
                }
                catch (Exception)
                {
                    downloadAbortedShas.Add(issuesToLocalizeWithSha.Sha);
                    File.WriteAllText(downloadAbortedFilePath, issuesToLocalizeWithSha.Sha + Environment.NewLine);
                    continue;
                }

                string previousIssueFolderPathForSameSha = null;

                foreach (var issueNumer in issuesToLocalizeWithSha.Issues)
                {
                    var localizationIssueFolderPath = localizationFolderPath + @"\" + issueNumer;
                    if (!Directory.Exists(localizationIssueFolderPath))
                    {
                        Directory.CreateDirectory(localizationIssueFolderPath);
                        Thread.Sleep(100);
                    }

                    if (previousIssueFolderPathForSameSha == null)
                    {
                        Log(counter + " of " + total + " Processing Source " + issuesToLocalizeWithSha.Sha);

                        var indexedFiles = GetIndexedFiles(issuesToLocalizeWithSha.Sha, extensions);
                        var indexedFilePathReplacement = (RepositoryFullPath + @"\Source\Commits\" + issuesToLocalizeWithSha.Sha + @"\").ToLowerInvariant();
                        var indexedFilesRelativePath = indexedFiles.Select(x => new Dtos.IndexedFile { Index = x.Index, Path = x.Path.Replace(indexedFilePathReplacement, "") }).ToList();
                        indexedFilesRelativePath.SerializeToXmlFile(localizationIssueFolderPath + @"\FileList.txt");

                        var relevantList = indexedFilesRelativePath.Where(x => commitChangeFiles.Contains(x.Path)).ToList();
                        relevantList.SerializeToXmlFile(localizationIssueFolderPath + @"\RelevantList.txt");

                        var sourceOutputFilePath = localizationIssueFolderPath + @"\Source.txt";
                        if (File.Exists(sourceOutputFilePath))
                        {
                            File.Delete(sourceOutputFilePath);
                            Thread.Sleep(100);
                        }

                        foreach (var indexedFile in indexedFiles)
                        {
                            var text = string.Join(",", File.ReadAllText(indexedFile.Path).CamelCaseSplit().RemoveStopWords().Stem().RemoveStopWords());
                            File.AppendAllText(sourceOutputFilePath, indexedFile.Index + "##" + text + Environment.NewLine);
                        }

                        previousIssueFolderPathForSameSha = localizationIssueFolderPath;
                    }
                    else
                    {
                        File.Copy(previousIssueFolderPathForSameSha + @"\RelevantList.txt", localizationIssueFolderPath + @"\RelevantList.txt");
                        File.Copy(previousIssueFolderPathForSameSha + @"\FileList.txt", localizationIssueFolderPath + @"\FileList.txt");
                        File.Copy(previousIssueFolderPathForSameSha + @"\Source.txt", localizationIssueFolderPath + @"\Source.txt");
                    }

                    lock (myObj)
                    {
                        datasetCompletedIssues.Add(issueNumer);
                        File.WriteAllLines(datasetCompletedFilePath, datasetCompletedIssues.Select(x => x.ToString()));
                    }

                    try
                    {
                        Log(counter + " of " + total + ": Deleting: " + issuesToLocalizeWithSha.Sha);
                        Directory.Delete(RepositoryFullPath + @"\Source\Commits\" + issuesToLocalizeWithSha.Sha, true);
                    }
                    catch
                    {

                    }
                }

                Thread.Sleep(5000);
            }

            Log("Download completed");
        }

        public void DeleteCompletedShaDownloads()
        {
            var issueCommits = SerializerHelper.DeserializeFromXmlFile<List<Dtos.IssueCommit>>(RepositoryFullPath + @"\IssueCommits.txt")
                .Where(x => !string.IsNullOrWhiteSpace(x.CommitSha))
                .GroupBy(x => x.CommitSha)
                .ToDictionary(x => x.Key, x => x.Select(y => y.IssueNumber).ToList());
            var completedIssueFilePath = RepositoryFullPath + @"\Localization\DatasetCompleted.txt";

            do
            {
                var completedIssues = File.ReadAllLines(completedIssueFilePath).Select(int.Parse).ToList();
                var existingDownloadedShasCommits = new DirectoryInfo(RepositoryFullPath + @"\Source\Commits\").GetDirectories().Select(x => x.Name).ToList();
                var toDeleteShaFolder = existingDownloadedShasCommits.Where(downloadedSha => issueCommits.ContainsKey(downloadedSha) && issueCommits[downloadedSha].All(issueForThisSha => completedIssues.Contains(issueForThisSha))).ToList();

                foreach (var completedSha in toDeleteShaFolder)
                {
                    Log("Deleting " + completedSha);
                    var commitFolderPath = RepositoryFullPath + @"\Source\Commits\" + completedSha;
                    try
                    {
                        Directory.Delete(commitFolderPath, true);
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(1000);
                        Log("Error: " + completedSha + ". Msg: " + e.Message);
                        try
                        {
                            var newPath = RepositoryFullPath + @"\Source\Commits\" + DateTime.Now.ToString("hhmmsstt");
                            Directory.Move(commitFolderPath, newPath);
                            Directory.Delete(newPath, true);
                        }
                        catch (Exception e2)
                        {
                            Log("Error Attempt 2: " + e2.Message);
                        }
                    }
                }

                Log("Waiting");
                Thread.Sleep(180000);
            } while (true);
            // ReSharper disable once FunctionNeverReturns
        }

        private void DownloadOriginalIfNotExists()
        {
            var sourceFolderPath = RepositoryFullPath + @"\Source";
            if (!Directory.Exists(sourceFolderPath))
            {
                Directory.CreateDirectory(sourceFolderPath);
                Thread.Sleep(500);
            }

            var originalSourceFolderPath = sourceFolderPath + @"\Original";
            if (!Directory.Exists(originalSourceFolderPath))
            {
                var gitRepo = GitHubClient.Repository.Get(RepositoryId).Result;
                LibGit2Sharp.Repository.Clone(gitRepo.CloneUrl, originalSourceFolderPath);
            }

            var commitsDownloadFolderPath = sourceFolderPath + @"\Commits";
            if (Directory.Exists(commitsDownloadFolderPath))
                return;
            Directory.CreateDirectory(commitsDownloadFolderPath);
            Thread.Sleep(500);
        }

        private void DownloadCommitSource(string originalSourceLink, string commitSha, bool useClone = true)
        {
            var commitDownloadFolderPath = RepositoryFullPath + @"\Source\Commits\" + commitSha;

            // clone original
            if (Directory.Exists(commitDownloadFolderPath))
                return;

            if (useClone)
            {
                LibGit2Sharp.Repository.Clone(originalSourceLink, commitDownloadFolderPath, new CloneOptions() { IsBare = true });
            }
            else
            {
                new DirectoryInfo(originalSourceLink).CopyTo(commitDownloadFolderPath);
            }

            // revert
            using (var repo = new LibGit2Sharp.Repository(commitDownloadFolderPath))
            {
                var commit = repo.Commits.FirstOrDefault(x => x.Sha == commitSha);
                Commands.Checkout(repo, commit);
                Thread.Sleep(5000);
            }
        }

        private List<Dtos.IndexedFile> GetIndexedFiles(string commitSha, List<string> extensions)
        {
            var commitDownloadFolderPath = RepositoryFullPath + @"\Source\Commits\" + commitSha;
            var files = new DirectoryInfo(commitDownloadFolderPath).GetFilesWithSearch(extensions);
            return files
                .Select((x, index) => new { Index = index + 1, Path = x })
                .Select(x => new Dtos.IndexedFile { Index = x.Index, Path = x.Path.ToLowerInvariant() })
                .ToList();
        }

        public void CreateBugReportsIfNotExists()
        {
            var issuesFolderPath = RepositoryFullPath + @"\Issues";
            var allIssuesDictionary = new Dictionary<int, string>();
            Log("Reading Issues");
            new DirectoryInfo(issuesFolderPath).GetFiles("*_Issues.txt").ToList().ForEach(issueFileInfo =>
            {
                var result = SerializerHelper.DeserializeFromXmlFile<List<Dtos.Issue>>(issueFileInfo.FullName);
                result.ForEach(issue =>
                {
                    if (!allIssuesDictionary.ContainsKey(issue.IssueNumber))
                        allIssuesDictionary.Add(issue.IssueNumber, issue.Title + " " + issue.Body);
                });
            });

            var localizationFolderPath = RepositoryFullPath + @"\Localization";
            var completedIssueFilePath = RepositoryFullPath + @"\CompletedBugReport.txt";
            if (!File.Exists(completedIssueFilePath))
            {
                File.Create(completedIssueFilePath).Close();
                Thread.Sleep(100);
            }
            var completedIssues = File.ReadAllLines(completedIssueFilePath).ToList();
            var issueDirectoryInfos = new DirectoryInfo(localizationFolderPath).GetDirectories().Where(x => !completedIssues.Contains(x.Name)).ToList();
            int totalCount = issueDirectoryInfos.Count;
            int counter = 0;
            var myObj = new object();
            issueDirectoryInfos.ForEach(issueDirectoryInfo =>
            {
                try
                {
                    Log(++counter + " of " + totalCount + " Creating Report");
                    var bugReportFilePath = issueDirectoryInfo.FullName + @"\BugReport.txt";

                    var text = allIssuesDictionary[int.Parse(issueDirectoryInfo.Name)];
                    File.WriteAllLines(bugReportFilePath, text.CamelCaseSplit().RemoveStopWords().Stem().RemoveStopWords().Where(x => x.Length > 2));

                    var newSource = File.ReadAllLines(issueDirectoryInfo.FullName + @"\Source.txt").Select(x => x.SplitWith("##")).Select(x => x[0] + "##" + string.Join(",", x[1].SplitWith(",").Where(y => y.Length > 2)));
                    File.WriteAllLines(issueDirectoryInfo.FullName + @"\Source.txt", newSource);

                    lock (myObj)
                    {
                        File.AppendAllText(completedIssueFilePath, issueDirectoryInfo.Name + Environment.NewLine);
                    }
                }
                catch
                {
                    lock (myObj)
                    {
                        File.AppendAllText(RepositoryFullPath + @"\CreateReportError.txt", issueDirectoryInfo.Name + Environment.NewLine);
                    }
                }
            });

            Log("Create Report Completed");
        }

        public void CleanEmptyIssues()
        {
            var localizationFolderPath = RepositoryFullPath + @"\Localization";
            var issueDirectoryInfos = new DirectoryInfo(localizationFolderPath).GetDirectories().ToList();
            int totalCount = issueDirectoryInfos.Count;
            int counter = 0;
            foreach(var issueDirectoryInfo in issueDirectoryInfos)
            {
                Log(++counter + " of " + totalCount + " Checking.");

                var relListFilePath = issueDirectoryInfo.FullName + @"\RelevantList.txt";
                var sourceFilePath = issueDirectoryInfo.FullName + @"\Source.txt";
                var bugRepotFilePath = issueDirectoryInfo.FullName + @"\BugReport.txt";

                var isBad = false;

                if (!File.Exists(relListFilePath) || !File.Exists(sourceFilePath) || !File.Exists(bugRepotFilePath))
                {
                    isBad = true;
                }
                else if (SerializerHelper.DeserializeFromXmlFile<List<Dtos.IndexedFile>>(relListFilePath).Count == 0)
                {
                    isBad = true;
                }
                else if (File.ReadAllLines(bugRepotFilePath).Length == 0)
                {
                    isBad = true;
                }

                if (isBad)
                {
                    Directory.Delete(issueDirectoryInfo.FullName, true);
                }
            }
        }

        #endregion Dataset

        #region Find

        public string FindRepositoryIdByOwnerAndName(string owner, string name)
        {
            var response = GitHubClient.Repository.Get(owner, name).Result;
            return response.Id.ToString();
        }

        #endregion Find

        #region Issue - Tag

        public void FilterIssueByTag(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return;

            var tagsFromUserString = filter.SplitWith("|").ToList();
            var tagDictionary = SerializerHelper
                .DeserializeFromXmlFile<List<Dtos.Tag>>(RepositoryFullPath + @"\Tags.txt")
                .ToDictionary(x => x.Name.ToLowerInvariant(), x => x.Id);
            var userTags = new HashSet<int>(tagsFromUserString.Select(x => tagDictionary[x]));

            var allIssueTags = File.ReadAllLines(RepositoryFullPath + @"\IssueTags.txt")
                .Select(x => x.SplitWith("##"))
                .ToDictionary(x => x[0], x => x[1].SplitWith(",").Where(y => !string.IsNullOrWhiteSpace(y)).Select(int.Parse).ToList());

            var currentLocalizationDirectoryPath = RepositoryFullPath + @"\Localization\";
            var currentBugs = new DirectoryInfo(currentLocalizationDirectoryPath).GetDirectories().ToList();
            var bugsWithNoTags = currentBugs.Where(bugInfo => !allIssueTags[bugInfo.Name].Any(bugTag => userTags.Contains(bugTag))).ToList();

            var notBugLocalizationDirectoryPath = RepositoryFullPath + @"\NotBugLocalization\";
            if (!Directory.Exists(notBugLocalizationDirectoryPath))
            {
                Directory.CreateDirectory(notBugLocalizationDirectoryPath);
                Thread.Sleep(100);
            }

            foreach(var bugWithNoTags in bugsWithNoTags)
            {
                Directory.Move(bugWithNoTags.FullName, notBugLocalizationDirectoryPath + bugWithNoTags.Name);
            }
        }

        public void CreateIssueTag()
        {
            var tagDictionary = SerializerHelper
                .DeserializeFromXmlFile<List<Dtos.Tag>>(RepositoryFullPath + @"\Tags.txt")
                .ToDictionary(x => x.Name.ToLowerInvariant(), x => x.Id);

            var allIssuesDictionary = new Dictionary<int, List<string>>();
            new DirectoryInfo(RepositoryFullPath + @"\Issues").GetFiles("*_Issues.txt").ToList().ForEach(issueFileInfo =>
            {
                var result = SerializerHelper.DeserializeFromXmlFile<List<Dtos.Issue>>(issueFileInfo.FullName);
                result.ForEach(issue =>
                {
                    if (!allIssuesDictionary.ContainsKey(issue.IssueNumber))
                        allIssuesDictionary.Add(issue.IssueNumber, issue.Labels.Select(x => x.ToLowerInvariant()).ToList());
                });
            });

            var issueWithTagIds = allIssuesDictionary.ToDictionary(x => x.Key, x => x.Value.Select(y => tagDictionary[y]).ToList());
            File.WriteAllLines(RepositoryFullPath + @"\IssueTags.txt", issueWithTagIds.Select(x => x.Key + "##" + string.Join(",", x.Value)));
        }

        #endregion
    }
}
