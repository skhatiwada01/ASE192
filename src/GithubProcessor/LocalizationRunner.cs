using GithubProcessor.BugLocalization;
using GithubProcessor.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace GithubProcessor
{
    public class LocalizationRunner
    {
        public string RepositoryFullPath { get; private set; }

        private ILog _logger;

        public void SetLogger(ILog logger)
        {
            _logger = logger;
        }

        private void Log(string text)
        {
            _logger?.Log(text);
        }

        public void SetRepositoryFullPath(string repositoryFullPath)
        {
            RepositoryFullPath = repositoryFullPath;
        }

        #region VSM

        public void RunVsmOnIssues()
        {
            var localizationFolderPath = RepositoryFullPath + @"\Localization";

            var vsmCompletedFilePath = localizationFolderPath + @"\VsmCompleted.txt";
            if (!File.Exists(vsmCompletedFilePath))
            {
                File.Create(vsmCompletedFilePath).Close();
                Thread.Sleep(100);
            }
            var vsmCompletedIssues = File.ReadAllLines(vsmCompletedFilePath).Select(int.Parse).ToList();

            var issuesToWorkOn = new DirectoryInfo(localizationFolderPath).GetDirectories().Select(x => x.Name).Select(int.Parse).Except(vsmCompletedIssues).OrderBy(x => x).ToList();
            var myObj = new object();
            var counter = 0;
            var total = issuesToWorkOn.Count;
            foreach(var issueToWorkOn in issuesToWorkOn)
            {
                Interlocked.Increment(ref counter);
                Log("VSM Working " + counter + " of " + total + ": " + issueToWorkOn);

                var issueFolderPath = localizationFolderPath + @"\" + issueToWorkOn;
                var resultFolderPath = issueFolderPath + @"\Result";
                if (!Directory.Exists(resultFolderPath))
                {
                    Directory.CreateDirectory(resultFolderPath);
                    Thread.Sleep(100);
                }

                new Vsm(issueFolderPath + @"\Source.txt", resultFolderPath).Execute(issueFolderPath + @"\BugReport.txt");

                lock (myObj)
                {
                    vsmCompletedIssues.Add(issueToWorkOn);
                    File.WriteAllLines(vsmCompletedFilePath, vsmCompletedIssues.Select(x => x.ToString()));
                }
            }
        }

        #endregion

        #region PMI

        public void RunPmiOnIssues()
        {
            var localizationFolderPath = RepositoryFullPath + @"\Localization";

            var pmiCompletedFilePath = localizationFolderPath + @"\PmiCompleted.txt";
            if (!File.Exists(pmiCompletedFilePath))
            {
                File.Create(pmiCompletedFilePath).Close();
                Thread.Sleep(100);
            }
            var pmiCompletedIssues = File.ReadAllLines(pmiCompletedFilePath).Select(int.Parse).ToList();

            var issuesToWorkOn = new DirectoryInfo(localizationFolderPath).GetDirectories().Select(x => x.Name).Select(int.Parse).Except(pmiCompletedIssues).OrderBy(x => x).ToList();

            var myObj = new object();
            var counter = 0;
            var total = issuesToWorkOn.Count;
            issuesToWorkOn.ForEach(issueToWorkOn =>
            {
                Log($"PMI Working " + ++counter + " of " + total + ": " + issueToWorkOn);

                var issueFolderPath = localizationFolderPath + @"\" + issueToWorkOn;
                var resultFolderPath = issueFolderPath + @"\Result";
                if (!Directory.Exists(resultFolderPath))
                {
                    Directory.CreateDirectory(resultFolderPath);
                    Thread.Sleep(100);
                }

                var queryTexts = File.ReadAllLines(issueFolderPath + @"\BugReport.txt").ToList();
                new PmiValueCache(issueFolderPath + @"\Source.txt", null, "Issue: " + counter + ": ").Execute(queryTexts, issueFolderPath + @"\PmiCache\");
                new PmiUsingCache(issueFolderPath + @"\Source.txt", resultFolderPath, "Issue: " + counter + ": ").Execute(queryTexts, issueFolderPath + @"\PmiCache\", "");

                lock (myObj)
                {
                    pmiCompletedIssues.Add(issueToWorkOn);
                    File.WriteAllLines(pmiCompletedFilePath, pmiCompletedIssues.Select(x => x.ToString()));
                }
            });
        }

        #endregion PMI

        #region JSM

        public void RunJsmOnIssues()
        {
            var localizationFolderPath = RepositoryFullPath + @"\Localization";

            var jsmCompletedFilePath = localizationFolderPath + @"\JsmCompleted.txt";
            if (!File.Exists(jsmCompletedFilePath))
            {
                File.Create(jsmCompletedFilePath).Close();
                Thread.Sleep(100);
            }
            var jsmCompletedIssues = File.ReadAllLines(jsmCompletedFilePath).Select(int.Parse).ToList();

            var issuesToWorkOn = new DirectoryInfo(localizationFolderPath).GetDirectories().Select(x => x.Name).Select(int.Parse).Except(jsmCompletedIssues).OrderBy(x => x).ToList();

            var myObj = new object();
            var counter = 0;
            var total = issuesToWorkOn.Count;
            issuesToWorkOn.ForEach(issueToWorkOn =>
            {
                Log($"JSM Working " + ++counter + " of " + total + ": " + issueToWorkOn);

                var issueFolderPath = localizationFolderPath + @"\" + issueToWorkOn;
                var resultFolderPath = issueFolderPath + @"\Result";
                if (!Directory.Exists(resultFolderPath))
                {
                    Directory.CreateDirectory(resultFolderPath);
                    Thread.Sleep(100);
                }

                var queryTexts = File.ReadAllLines(issueFolderPath + @"\BugReport.txt").ToList();
                new Jsm(issueFolderPath + @"\Source.txt", resultFolderPath).Execute(issueFolderPath + @"\BugReport.txt");

                lock (myObj)
                {
                    jsmCompletedIssues.Add(issueToWorkOn);
                    File.WriteAllLines(jsmCompletedFilePath, jsmCompletedIssues.Select(x => x.ToString()));
                }
            });
        }

        #endregion JSM

        #region LSI

        public void RunLsiOnIssues()
        {
            var localizationFolderPath = RepositoryFullPath + @"\Localization";

            var lsiCompletedFilePath = localizationFolderPath + @"\LsiCompleted.txt";
            if (!File.Exists(lsiCompletedFilePath))
            {
                File.Create(lsiCompletedFilePath).Close();
                Thread.Sleep(100);
            }
            var lsiCompletedIssues = File.ReadAllLines(lsiCompletedFilePath).Select(int.Parse).ToList();

            var issuesToWorkOn = new DirectoryInfo(localizationFolderPath).GetDirectories().Select(x => x.Name).Select(int.Parse).Except(lsiCompletedIssues).OrderBy(x => x).ToList();

            var myObj = new object();
            var counter = 0;
            var total = issuesToWorkOn.Count;
            issuesToWorkOn.ForEach(issueToWorkOn =>
            {
                try
                {
                    Log($"LSI Working " + ++counter + " of " + total + ": " + issueToWorkOn);

                    var issueFolderPath = localizationFolderPath + @"\" + issueToWorkOn;
                    var resultFolderPath = issueFolderPath + @"\Result";
                    if (!Directory.Exists(resultFolderPath))
                    {
                        Directory.CreateDirectory(resultFolderPath);
                        Thread.Sleep(100);
                    }

                    var queryTexts = File.ReadAllLines(issueFolderPath + @"\BugReport.txt").ToList();
                    var lsi = new Lsi(issueFolderPath + @"\Source.txt", resultFolderPath);
                    lsi.LogAdd = counter + " of " + total + ": " + issueToWorkOn;
                    lsi.Execute(issueFolderPath + @"\BugReport.txt");

                    lock (myObj)
                    {
                        lsiCompletedIssues.Add(issueToWorkOn);
                        File.WriteAllLines(lsiCompletedFilePath, lsiCompletedIssues.Select(x => x.ToString()));
                    }
                }
                catch
                {
                    // ignore for now
                }
            });
        }

        #endregion LSI

        #region Combination

        #region Lambda

        public void RunLambdaCombinationOnIssues(string method1, string method2, double lambda)
        {
            var localizationFolderPath = RepositoryFullPath + @"\Localization";

            int counter = 0;
            var issuesToWorkOn = new DirectoryInfo(localizationFolderPath).GetDirectories().Select(x => x.Name).Select(int.Parse).OrderByDescending(x => x).ToList();
            int total = issuesToWorkOn.Count;

            issuesToWorkOn.ForEach(issueToWorkOn =>
            {
                Log($"{method1} - {method2} Lambda Combination Working " + ++counter + " of " + total + ": " + issueToWorkOn);
                var resultFolderPath = localizationFolderPath + @"\" + issueToWorkOn + @"\Result";
                new LambdaCombination(resultFolderPath).Execute(method1, method2, lambda);
            });
        }

        #endregion Lambda

        #region Addition

        public void RunAdditionCombinationOnIssues(string method1, string method2)
        {
            var localizationFolderPath = RepositoryFullPath + @"\Localization";

            int counter = 0;
            var issuesToWorkOn = new DirectoryInfo(localizationFolderPath).GetDirectories().Select(x => x.Name).Select(int.Parse).OrderByDescending(x => x).ToList();
            int total = issuesToWorkOn.Count;

            issuesToWorkOn.ForEach(issueToWorkOn =>
            {
                Log($"{method1} - {method2} Addition Combination Working " + ++counter + " of " + total + ": " + issueToWorkOn);
                var resultFolderPath = localizationFolderPath + @"\" + issueToWorkOn + @"\Result";
                new AdditionCombination(resultFolderPath).Execute(method1, method2);
            });
        }

        #endregion Addition

        #region BordaCount

        public void RunBordaCountCombinationOnIssues(string method1, string method2)
        {
            var localizationFolderPath = RepositoryFullPath + @"\Localization";

            int counter = 0;
            var issuesToWorkOn = new DirectoryInfo(localizationFolderPath).GetDirectories().Select(x => x.Name).Select(int.Parse).OrderByDescending(x => x).ToList();
            int total = issuesToWorkOn.Count;

            issuesToWorkOn.ForEach(issueToWorkOn =>
            {
                Log($"{method1} - {method2} Borda Combination Working " + ++counter + " of " + total + ": " + issueToWorkOn);
                var resultFolderPath = localizationFolderPath + @"\" + issueToWorkOn + @"\Result";
                new BordaCombination(resultFolderPath).Execute(method1, method2);
            });
        }

        #endregion BordaCount

        #endregion Combination

        #region Analyze

        public void RunPmiResult()
        {
            ResultCalculator.Analyze(RepositoryFullPath + @"\Localization\", "Pmi", @"Result\", "Pmi_Result.txt", _logger);
        }

        public void RunVsmResult()
        {
            ResultCalculator.Analyze(RepositoryFullPath + @"\Localization\", "Vsm", @"Result\", "Vsm_Result.txt", _logger);
        }

        public void RunLsiResult()
        {
            var localizationFolderPath = RepositoryFullPath + @"\Localization";
            var lsiCompletedFilePath = localizationFolderPath + @"\LsiCompleted.txt";
            var issuesToWorkOn = File.ReadAllLines(lsiCompletedFilePath).Select(int.Parse).ToList();

            var total = issuesToWorkOn.Count;
            var counter = 0;
            issuesToWorkOn.ForEach(issueToWorkOn =>
            {
                Log("Calulating Best Lsi " + ++counter + " of " + total + ": " + issueToWorkOn);
                var issueFolderPath = localizationFolderPath + @"\" + issueToWorkOn;
                var resultFolderPath = issueFolderPath + @"\Result";
                var lsiFolderPath = resultFolderPath + @"\Lsi";

                var relevanceList = SerializerHelper.DeserializeFromXmlFile<List<IndexedFile>>(issueFolderPath + @"\RelevantList.txt").Select(x => x.Index.ToString()).ToList();

                var lsiFiles = new DirectoryInfo(lsiFolderPath).GetFiles().ToList();
                var lsiFileRRDictionary = new Dictionary<FileInfo, double>();
                foreach(var lsiFile in lsiFiles)
                {
                    var similarityList = File.ReadAllLines(lsiFile.FullName).Select(x => x.SplitWith(" ")[0]).ToList();
                    var rr = ResultCalculator.GetMetricNumbers(similarityList, relevanceList);
                    lsiFileRRDictionary.Add(lsiFile, rr.Rr);
                }

                var bestLsiFile = lsiFileRRDictionary.OrderByDescending(x => x.Value).First().Key;
                File.Copy(bestLsiFile.FullName, resultFolderPath + @"\Lsi.txt");
            });

            ResultCalculator.Analyze(RepositoryFullPath + @"\Localization\", "Lsi", @"Result\", "Lsi_Result.txt", _logger);
        }

        public void RunLambdaResult(string method1, string method2)
        {
            ResultCalculator.Analyze(RepositoryFullPath + @"\Localization\", $"Lambda_{method1}_{method2}", @"Result\", $"Lambda_{method1}_{method2}_Result.txt", _logger);
        }

        public void RunAdditionResult(string method1, string method2)
        {
            ResultCalculator.Analyze(RepositoryFullPath + @"\Localization\", $"Addition_{method1}_{method2}", @"Result\", $"Addition_{method1}_{method2}_Result.txt", _logger);
        }

        public void RunBordaResult(string method1, string method2)
        {
            ResultCalculator.Analyze(RepositoryFullPath + @"\Localization\", $"Borda_{method1}_{method2}", @"Result\", $"Borda_{method1}_{method2}_Result.txt", _logger);
        }

        #endregion Analyze
    }
}
