using System;
using System.IO;
using System.Windows.Forms;
using GithubProcessor;

namespace OSDatasetCreatorForBL
{
    public partial class FormDownloader : Form, ILog
    {
        public FormDownloader()
        {
            InitializeComponent();
        }

        private Downloader Downloader { get; set; }

        #region Constants

        public const string TagsFileName = "Tags.txt";

        public const string IssuesFileName = "Issues.txt";

        public const string GithubProductName = "test-git-repository-downloads-with-releases";

        #endregion Constants

        #region Login

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Downloader = new Downloader(GithubProductName, TextBoxUserName.Text, TextBoxPassword.Text);
                Downloader.SetLogger(this);
                Log("Login Successful.");
                OnLoginStateChanged(true);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                OnLoginStateChanged(false);
            }
        }

        private void OnLoginStateChanged(bool isSet)
        {
            if (isSet)
                Log("OnLoginStateChanged");
        }

        #endregion Login

        #region Set Project

        private void ButtonSetRepository_Click(object sender, EventArgs e)
        {
            try
            {
                var repositoryId = long.Parse(TextBoxRepoId.Text);
                Downloader.SetProject(repositoryId, TextBoxOutputDir.Text);
                if (!Directory.Exists(Downloader.RepositoryFullPath))
                    Directory.CreateDirectory(Downloader.RepositoryFullPath);
                Log("Project Set.");
                OnProjectChanged(true);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                OnProjectChanged(false);
            }
        }

        private void OnProjectChanged(bool isSet)
        {
            if (isSet)
                Log("OnProjectChanged");
        }

        #endregion

        #region Tags

        private void ButtonDownloadTags_Click(object sender, EventArgs e)
        {
            try
            {
                Downloader.DownloadTags(TagsFileName);
                Log("Tags Downloaded");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        #endregion Tags

        #region Find

        private void ButtonFindRepo_Click(object sender, EventArgs e)
        {
            try
            {
                var text = TextBoxRepoName.Text;
                var ownerAndName = text.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var response = Downloader.FindRepositoryIdByOwnerAndName(ownerAndName[0], ownerAndName[1]);
                TextBoxRepoId.Text = response;
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        #endregion Find

        #region Log

        public void Log(string message)
        {
            LabelLog.Text = message;
            LabelLog.Refresh();
        }

        #endregion Log

        #region Issues

        private void ButtonDownloadAllIssues_Click(object sender, EventArgs e)
        {
            try
            {
                Downloader.DownloadAllIssues(IssuesFileName);
                Log("Issues Downloaded");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        #endregion Issues

        #region Commit

        private void ButtonDownloadAllCommits_Click(object sender, EventArgs e)
        {
            Downloader.DownloadAllCommits();
        }

        private void ButtonCreateIssueCommits_Click(object sender, EventArgs e)
        {
            Downloader.CreateIssueCommits();
        }

        private void ButtonDownloadIssueCommitsChangeInfo_Click(object sender, EventArgs e)
        {
            Downloader.DownloadCommitChangeInfo();
        }

        #endregion Commit

        private void ButtonCreateLocalizationDataset_Click(object sender, EventArgs e)
        {
            Downloader.CreateLocalizationDataset(TextBoxExtension.Text);
        }

        private void ButtonCreateBugReports_Click(object sender, EventArgs e)
        {
            Downloader.CreateBugReportsIfNotExists();
        }

        private void ButtonLocalization_Click(object sender, EventArgs e)
        {
            var form = new LocalizationForm();
            form.SetRepository(Path.Combine(TextBoxOutputDir.Text, TextBoxRepoId.Text));
            form.Show();
        }

        private void ButtonCleanEmpty_Click(object sender, EventArgs e)
        {
            Downloader.CleanEmptyIssues();
        }

        private void ButtonFilterIssueByTag_Click(object sender, EventArgs e)
        {
            Downloader.FilterIssueByTag(TextBoxFilterIssueByTag.Text);
        }

        private void ButtonCreateIssueTag_Click(object sender, EventArgs e)
        {
            Downloader.CreateIssueTag();
        }
    }
}
