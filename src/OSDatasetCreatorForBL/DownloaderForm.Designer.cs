namespace OSDatasetCreatorForBL
{
    partial class FormDownloader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelOutputDir = new System.Windows.Forms.Label();
            this.TextBoxOutputDir = new System.Windows.Forms.TextBox();
            this.GroupBoxLogin = new System.Windows.Forms.GroupBox();
            this.ButtonLogin = new System.Windows.Forms.Button();
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.TextBoxUserName = new System.Windows.Forms.TextBox();
            this.LabelPassword = new System.Windows.Forms.Label();
            this.LabelUserName = new System.Windows.Forms.Label();
            this.GroupBoxRepo = new System.Windows.Forms.GroupBox();
            this.ButtonSetRepository = new System.Windows.Forms.Button();
            this.ButtonFindRepo = new System.Windows.Forms.Button();
            this.TextBoxRepoName = new System.Windows.Forms.TextBox();
            this.LabelRepoName = new System.Windows.Forms.Label();
            this.TextBoxRepoId = new System.Windows.Forms.TextBox();
            this.LabelRepoId = new System.Windows.Forms.Label();
            this.LabelLog = new System.Windows.Forms.Label();
            this.GroupBoxAction = new System.Windows.Forms.GroupBox();
            this.ButtonFilterIssueByTag = new System.Windows.Forms.Button();
            this.ButtonCleanEmpty = new System.Windows.Forms.Button();
            this.TextBoxExtension = new System.Windows.Forms.TextBox();
            this.ButtonLocalization = new System.Windows.Forms.Button();
            this.ButtonCreateBugReports = new System.Windows.Forms.Button();
            this.ButtonCreateLocalizationDataset = new System.Windows.Forms.Button();
            this.ButtonDownloadCommitsChangeInfo = new System.Windows.Forms.Button();
            this.ButtonDownloadAllCommits = new System.Windows.Forms.Button();
            this.ButtonCreateIssueCommits = new System.Windows.Forms.Button();
            this.ButtonDownloadAllIssues = new System.Windows.Forms.Button();
            this.ButtonDownloadTags = new System.Windows.Forms.Button();
            this.TextBoxFilterIssueByTag = new System.Windows.Forms.TextBox();
            this.ButtonCreateIssueTag = new System.Windows.Forms.Button();
            this.GroupBoxLogin.SuspendLayout();
            this.GroupBoxRepo.SuspendLayout();
            this.GroupBoxAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelOutputDir
            // 
            this.LabelOutputDir.AutoSize = true;
            this.LabelOutputDir.Location = new System.Drawing.Point(6, 27);
            this.LabelOutputDir.Name = "LabelOutputDir";
            this.LabelOutputDir.Size = new System.Drawing.Size(84, 13);
            this.LabelOutputDir.TabIndex = 0;
            this.LabelOutputDir.Text = "Output Directory";
            // 
            // TextBoxOutputDir
            // 
            this.TextBoxOutputDir.Location = new System.Drawing.Point(96, 24);
            this.TextBoxOutputDir.Name = "TextBoxOutputDir";
            this.TextBoxOutputDir.Size = new System.Drawing.Size(374, 20);
            this.TextBoxOutputDir.TabIndex = 1;
            // 
            // GroupBoxLogin
            // 
            this.GroupBoxLogin.Controls.Add(this.ButtonLogin);
            this.GroupBoxLogin.Controls.Add(this.TextBoxPassword);
            this.GroupBoxLogin.Controls.Add(this.TextBoxUserName);
            this.GroupBoxLogin.Controls.Add(this.LabelPassword);
            this.GroupBoxLogin.Controls.Add(this.LabelUserName);
            this.GroupBoxLogin.Location = new System.Drawing.Point(16, 12);
            this.GroupBoxLogin.Name = "GroupBoxLogin";
            this.GroupBoxLogin.Size = new System.Drawing.Size(200, 113);
            this.GroupBoxLogin.TabIndex = 2;
            this.GroupBoxLogin.TabStop = false;
            this.GroupBoxLogin.Text = "GitHub Login";
            // 
            // ButtonLogin
            // 
            this.ButtonLogin.Location = new System.Drawing.Point(119, 84);
            this.ButtonLogin.Name = "ButtonLogin";
            this.ButtonLogin.Size = new System.Drawing.Size(75, 23);
            this.ButtonLogin.TabIndex = 4;
            this.ButtonLogin.Text = "Login";
            this.ButtonLogin.UseVisualStyleBackColor = true;
            this.ButtonLogin.Click += new System.EventHandler(this.ButtonLogin_Click);
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Location = new System.Drawing.Point(76, 56);
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.PasswordChar = '*';
            this.TextBoxPassword.Size = new System.Drawing.Size(118, 20);
            this.TextBoxPassword.TabIndex = 3;
            // 
            // TextBoxUserName
            // 
            this.TextBoxUserName.Location = new System.Drawing.Point(76, 24);
            this.TextBoxUserName.Name = "TextBoxUserName";
            this.TextBoxUserName.Size = new System.Drawing.Size(118, 20);
            this.TextBoxUserName.TabIndex = 2;
            // 
            // LabelPassword
            // 
            this.LabelPassword.AutoSize = true;
            this.LabelPassword.Location = new System.Drawing.Point(13, 59);
            this.LabelPassword.Name = "LabelPassword";
            this.LabelPassword.Size = new System.Drawing.Size(53, 13);
            this.LabelPassword.TabIndex = 1;
            this.LabelPassword.Text = "Password";
            // 
            // LabelUserName
            // 
            this.LabelUserName.AutoSize = true;
            this.LabelUserName.Location = new System.Drawing.Point(13, 27);
            this.LabelUserName.Name = "LabelUserName";
            this.LabelUserName.Size = new System.Drawing.Size(57, 13);
            this.LabelUserName.TabIndex = 0;
            this.LabelUserName.Text = "UserName";
            // 
            // GroupBoxRepo
            // 
            this.GroupBoxRepo.Controls.Add(this.ButtonSetRepository);
            this.GroupBoxRepo.Controls.Add(this.ButtonFindRepo);
            this.GroupBoxRepo.Controls.Add(this.TextBoxRepoName);
            this.GroupBoxRepo.Controls.Add(this.LabelRepoName);
            this.GroupBoxRepo.Controls.Add(this.TextBoxRepoId);
            this.GroupBoxRepo.Controls.Add(this.LabelRepoId);
            this.GroupBoxRepo.Controls.Add(this.LabelOutputDir);
            this.GroupBoxRepo.Controls.Add(this.TextBoxOutputDir);
            this.GroupBoxRepo.Location = new System.Drawing.Point(234, 12);
            this.GroupBoxRepo.Name = "GroupBoxRepo";
            this.GroupBoxRepo.Size = new System.Drawing.Size(476, 113);
            this.GroupBoxRepo.TabIndex = 3;
            this.GroupBoxRepo.TabStop = false;
            this.GroupBoxRepo.Text = "Repository Info";
            // 
            // ButtonSetRepository
            // 
            this.ButtonSetRepository.AccessibleRole = System.Windows.Forms.AccessibleRole.PageTabList;
            this.ButtonSetRepository.Location = new System.Drawing.Point(395, 84);
            this.ButtonSetRepository.Name = "ButtonSetRepository";
            this.ButtonSetRepository.Size = new System.Drawing.Size(75, 23);
            this.ButtonSetRepository.TabIndex = 7;
            this.ButtonSetRepository.Text = "Set Repo";
            this.ButtonSetRepository.UseVisualStyleBackColor = true;
            this.ButtonSetRepository.Click += new System.EventHandler(this.ButtonSetRepository_Click);
            // 
            // ButtonFindRepo
            // 
            this.ButtonFindRepo.Location = new System.Drawing.Point(238, 54);
            this.ButtonFindRepo.Name = "ButtonFindRepo";
            this.ButtonFindRepo.Size = new System.Drawing.Size(43, 23);
            this.ButtonFindRepo.TabIndex = 6;
            this.ButtonFindRepo.Text = "Find";
            this.ButtonFindRepo.UseVisualStyleBackColor = true;
            this.ButtonFindRepo.Click += new System.EventHandler(this.ButtonFindRepo_Click);
            // 
            // TextBoxRepoName
            // 
            this.TextBoxRepoName.Location = new System.Drawing.Point(96, 56);
            this.TextBoxRepoName.Name = "TextBoxRepoName";
            this.TextBoxRepoName.Size = new System.Drawing.Size(137, 20);
            this.TextBoxRepoName.TabIndex = 5;
            // 
            // LabelRepoName
            // 
            this.LabelRepoName.AutoSize = true;
            this.LabelRepoName.Location = new System.Drawing.Point(19, 59);
            this.LabelRepoName.Name = "LabelRepoName";
            this.LabelRepoName.Size = new System.Drawing.Size(64, 13);
            this.LabelRepoName.TabIndex = 4;
            this.LabelRepoName.Text = "Repo Name";
            // 
            // TextBoxRepoId
            // 
            this.TextBoxRepoId.Location = new System.Drawing.Point(345, 56);
            this.TextBoxRepoId.Name = "TextBoxRepoId";
            this.TextBoxRepoId.Size = new System.Drawing.Size(125, 20);
            this.TextBoxRepoId.TabIndex = 3;
            // 
            // LabelRepoId
            // 
            this.LabelRepoId.AutoSize = true;
            this.LabelRepoId.Location = new System.Drawing.Point(287, 59);
            this.LabelRepoId.Name = "LabelRepoId";
            this.LabelRepoId.Size = new System.Drawing.Size(45, 13);
            this.LabelRepoId.TabIndex = 2;
            this.LabelRepoId.Text = "Repo Id";
            // 
            // LabelLog
            // 
            this.LabelLog.AutoSize = true;
            this.LabelLog.Location = new System.Drawing.Point(13, 409);
            this.LabelLog.Name = "LabelLog";
            this.LabelLog.Size = new System.Drawing.Size(25, 13);
            this.LabelLog.TabIndex = 4;
            this.LabelLog.Text = "Log";
            // 
            // GroupBoxAction
            // 
            this.GroupBoxAction.Controls.Add(this.ButtonCreateIssueTag);
            this.GroupBoxAction.Controls.Add(this.TextBoxFilterIssueByTag);
            this.GroupBoxAction.Controls.Add(this.ButtonFilterIssueByTag);
            this.GroupBoxAction.Controls.Add(this.ButtonCleanEmpty);
            this.GroupBoxAction.Controls.Add(this.TextBoxExtension);
            this.GroupBoxAction.Controls.Add(this.ButtonLocalization);
            this.GroupBoxAction.Controls.Add(this.ButtonCreateBugReports);
            this.GroupBoxAction.Controls.Add(this.ButtonCreateLocalizationDataset);
            this.GroupBoxAction.Controls.Add(this.ButtonDownloadCommitsChangeInfo);
            this.GroupBoxAction.Controls.Add(this.ButtonDownloadAllCommits);
            this.GroupBoxAction.Controls.Add(this.ButtonCreateIssueCommits);
            this.GroupBoxAction.Controls.Add(this.ButtonDownloadAllIssues);
            this.GroupBoxAction.Controls.Add(this.ButtonDownloadTags);
            this.GroupBoxAction.Location = new System.Drawing.Point(16, 133);
            this.GroupBoxAction.Name = "GroupBoxAction";
            this.GroupBoxAction.Size = new System.Drawing.Size(694, 273);
            this.GroupBoxAction.TabIndex = 5;
            this.GroupBoxAction.TabStop = false;
            this.GroupBoxAction.Text = "Action";
            // 
            // ButtonFilterIssueByTag
            // 
            this.ButtonFilterIssueByTag.Location = new System.Drawing.Point(248, 223);
            this.ButtonFilterIssueByTag.Name = "ButtonFilterIssueByTag";
            this.ButtonFilterIssueByTag.Size = new System.Drawing.Size(153, 23);
            this.ButtonFilterIssueByTag.TabIndex = 12;
            this.ButtonFilterIssueByTag.Text = "Filter Issues By Tags";
            this.ButtonFilterIssueByTag.UseVisualStyleBackColor = true;
            this.ButtonFilterIssueByTag.Click += new System.EventHandler(this.ButtonFilterIssueByTag_Click);
            // 
            // ButtonCleanEmpty
            // 
            this.ButtonCleanEmpty.Location = new System.Drawing.Point(248, 194);
            this.ButtonCleanEmpty.Name = "ButtonCleanEmpty";
            this.ButtonCleanEmpty.Size = new System.Drawing.Size(153, 23);
            this.ButtonCleanEmpty.TabIndex = 11;
            this.ButtonCleanEmpty.Text = "Clean Empty Issue";
            this.ButtonCleanEmpty.UseVisualStyleBackColor = true;
            this.ButtonCleanEmpty.Click += new System.EventHandler(this.ButtonCleanEmpty_Click);
            // 
            // TextBoxExtension
            // 
            this.TextBoxExtension.Location = new System.Drawing.Point(130, 136);
            this.TextBoxExtension.Name = "TextBoxExtension";
            this.TextBoxExtension.Size = new System.Drawing.Size(112, 20);
            this.TextBoxExtension.TabIndex = 10;
            // 
            // ButtonLocalization
            // 
            this.ButtonLocalization.Location = new System.Drawing.Point(550, 78);
            this.ButtonLocalization.Name = "ButtonLocalization";
            this.ButtonLocalization.Size = new System.Drawing.Size(112, 23);
            this.ButtonLocalization.TabIndex = 9;
            this.ButtonLocalization.Text = "Bug Localization";
            this.ButtonLocalization.UseVisualStyleBackColor = true;
            this.ButtonLocalization.Click += new System.EventHandler(this.ButtonLocalization_Click);
            // 
            // ButtonCreateBugReports
            // 
            this.ButtonCreateBugReports.Location = new System.Drawing.Point(248, 165);
            this.ButtonCreateBugReports.Name = "ButtonCreateBugReports";
            this.ButtonCreateBugReports.Size = new System.Drawing.Size(153, 23);
            this.ButtonCreateBugReports.TabIndex = 6;
            this.ButtonCreateBugReports.Text = "Create Bug Reports";
            this.ButtonCreateBugReports.UseVisualStyleBackColor = true;
            this.ButtonCreateBugReports.Click += new System.EventHandler(this.ButtonCreateBugReports_Click);
            // 
            // ButtonCreateLocalizationDataset
            // 
            this.ButtonCreateLocalizationDataset.Location = new System.Drawing.Point(248, 136);
            this.ButtonCreateLocalizationDataset.Name = "ButtonCreateLocalizationDataset";
            this.ButtonCreateLocalizationDataset.Size = new System.Drawing.Size(153, 23);
            this.ButtonCreateLocalizationDataset.TabIndex = 5;
            this.ButtonCreateLocalizationDataset.Text = "Create Localization Dataset";
            this.ButtonCreateLocalizationDataset.UseVisualStyleBackColor = true;
            this.ButtonCreateLocalizationDataset.Click += new System.EventHandler(this.ButtonCreateLocalizationDataset_Click);
            // 
            // ButtonDownloadCommitsChangeInfo
            // 
            this.ButtonDownloadCommitsChangeInfo.Location = new System.Drawing.Point(248, 107);
            this.ButtonDownloadCommitsChangeInfo.Name = "ButtonDownloadCommitsChangeInfo";
            this.ButtonDownloadCommitsChangeInfo.Size = new System.Drawing.Size(153, 23);
            this.ButtonDownloadCommitsChangeInfo.TabIndex = 4;
            this.ButtonDownloadCommitsChangeInfo.Text = "Download Issue Change Info";
            this.ButtonDownloadCommitsChangeInfo.UseVisualStyleBackColor = true;
            this.ButtonDownloadCommitsChangeInfo.Click += new System.EventHandler(this.ButtonDownloadIssueCommitsChangeInfo_Click);
            // 
            // ButtonDownloadAllCommits
            // 
            this.ButtonDownloadAllCommits.Location = new System.Drawing.Point(130, 78);
            this.ButtonDownloadAllCommits.Name = "ButtonDownloadAllCommits";
            this.ButtonDownloadAllCommits.Size = new System.Drawing.Size(112, 52);
            this.ButtonDownloadAllCommits.TabIndex = 3;
            this.ButtonDownloadAllCommits.Text = "Download All Commits";
            this.ButtonDownloadAllCommits.UseVisualStyleBackColor = true;
            this.ButtonDownloadAllCommits.Click += new System.EventHandler(this.ButtonDownloadAllCommits_Click);
            // 
            // ButtonCreateIssueCommits
            // 
            this.ButtonCreateIssueCommits.Location = new System.Drawing.Point(248, 78);
            this.ButtonCreateIssueCommits.Name = "ButtonCreateIssueCommits";
            this.ButtonCreateIssueCommits.Size = new System.Drawing.Size(153, 23);
            this.ButtonCreateIssueCommits.TabIndex = 2;
            this.ButtonCreateIssueCommits.Text = "Create Issue Commits";
            this.ButtonCreateIssueCommits.UseVisualStyleBackColor = true;
            this.ButtonCreateIssueCommits.Click += new System.EventHandler(this.ButtonCreateIssueCommits_Click);
            // 
            // ButtonDownloadAllIssues
            // 
            this.ButtonDownloadAllIssues.Location = new System.Drawing.Point(12, 78);
            this.ButtonDownloadAllIssues.Name = "ButtonDownloadAllIssues";
            this.ButtonDownloadAllIssues.Size = new System.Drawing.Size(112, 23);
            this.ButtonDownloadAllIssues.TabIndex = 1;
            this.ButtonDownloadAllIssues.Text = "Download All Issues";
            this.ButtonDownloadAllIssues.UseVisualStyleBackColor = true;
            this.ButtonDownloadAllIssues.Click += new System.EventHandler(this.ButtonDownloadAllIssues_Click);
            // 
            // ButtonDownloadTags
            // 
            this.ButtonDownloadTags.Location = new System.Drawing.Point(12, 34);
            this.ButtonDownloadTags.Name = "ButtonDownloadTags";
            this.ButtonDownloadTags.Size = new System.Drawing.Size(112, 23);
            this.ButtonDownloadTags.TabIndex = 0;
            this.ButtonDownloadTags.Text = "Download Tags";
            this.ButtonDownloadTags.UseVisualStyleBackColor = true;
            this.ButtonDownloadTags.Click += new System.EventHandler(this.ButtonDownloadTags_Click);
            // 
            // TextBoxFilterIssueByTag
            // 
            this.TextBoxFilterIssueByTag.Location = new System.Drawing.Point(130, 225);
            this.TextBoxFilterIssueByTag.Name = "TextBoxFilterIssueByTag";
            this.TextBoxFilterIssueByTag.Size = new System.Drawing.Size(112, 20);
            this.TextBoxFilterIssueByTag.TabIndex = 13;
            // 
            // ButtonCreateIssueTag
            // 
            this.ButtonCreateIssueTag.Location = new System.Drawing.Point(12, 107);
            this.ButtonCreateIssueTag.Name = "ButtonCreateIssueTag";
            this.ButtonCreateIssueTag.Size = new System.Drawing.Size(112, 23);
            this.ButtonCreateIssueTag.TabIndex = 14;
            this.ButtonCreateIssueTag.Text = "Create Issue Tag";
            this.ButtonCreateIssueTag.UseVisualStyleBackColor = true;
            this.ButtonCreateIssueTag.Click += new System.EventHandler(this.ButtonCreateIssueTag_Click);
            // 
            // FormDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 431);
            this.Controls.Add(this.GroupBoxAction);
            this.Controls.Add(this.LabelLog);
            this.Controls.Add(this.GroupBoxRepo);
            this.Controls.Add(this.GroupBoxLogin);
            this.Name = "FormDownloader";
            this.Text = "Downloader";
            this.GroupBoxLogin.ResumeLayout(false);
            this.GroupBoxLogin.PerformLayout();
            this.GroupBoxRepo.ResumeLayout(false);
            this.GroupBoxRepo.PerformLayout();
            this.GroupBoxAction.ResumeLayout(false);
            this.GroupBoxAction.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelOutputDir;
        private System.Windows.Forms.TextBox TextBoxOutputDir;
        private System.Windows.Forms.GroupBox GroupBoxLogin;
        private System.Windows.Forms.TextBox TextBoxPassword;
        private System.Windows.Forms.TextBox TextBoxUserName;
        private System.Windows.Forms.Label LabelPassword;
        private System.Windows.Forms.Label LabelUserName;
        private System.Windows.Forms.GroupBox GroupBoxRepo;
        private System.Windows.Forms.TextBox TextBoxRepoId;
        private System.Windows.Forms.Label LabelRepoId;
        private System.Windows.Forms.Button ButtonLogin;
        private System.Windows.Forms.TextBox TextBoxRepoName;
        private System.Windows.Forms.Label LabelRepoName;
        private System.Windows.Forms.Button ButtonFindRepo;
        private System.Windows.Forms.Label LabelLog;
        private System.Windows.Forms.GroupBox GroupBoxAction;
        private System.Windows.Forms.Button ButtonDownloadTags;
        private System.Windows.Forms.Button ButtonSetRepository;
        private System.Windows.Forms.Button ButtonDownloadAllIssues;
        private System.Windows.Forms.Button ButtonCreateIssueCommits;
        private System.Windows.Forms.Button ButtonDownloadAllCommits;
        private System.Windows.Forms.Button ButtonDownloadCommitsChangeInfo;
        private System.Windows.Forms.Button ButtonCreateLocalizationDataset;
        private System.Windows.Forms.Button ButtonCreateBugReports;
        private System.Windows.Forms.Button ButtonLocalization;
        private System.Windows.Forms.TextBox TextBoxExtension;
        private System.Windows.Forms.Button ButtonCleanEmpty;
        private System.Windows.Forms.Button ButtonFilterIssueByTag;
        private System.Windows.Forms.TextBox TextBoxFilterIssueByTag;
        private System.Windows.Forms.Button ButtonCreateIssueTag;
    }
}

