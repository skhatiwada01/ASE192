namespace OSDatasetCreatorForBL
{
    partial class LocalizationForm
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
            this.GroupBoxRepo = new System.Windows.Forms.GroupBox();
            this.ButtonSetRepository = new System.Windows.Forms.Button();
            this.LabelOutputDir = new System.Windows.Forms.Label();
            this.TextBoxOutputDir = new System.Windows.Forms.TextBox();
            this.GroupBoxTag = new System.Windows.Forms.GroupBox();
            this.ListBoxTags = new System.Windows.Forms.ListBox();
            this.ButtonVsm = new System.Windows.Forms.Button();
            this.ButtonPmi = new System.Windows.Forms.Button();
            this.ButtonLambda = new System.Windows.Forms.Button();
            this.ButtonAddition = new System.Windows.Forms.Button();
            this.ButtonBorda = new System.Windows.Forms.Button();
            this.CheckBoxVSM = new System.Windows.Forms.CheckBox();
            this.CheckBoxPMI = new System.Windows.Forms.CheckBox();
            this.GroupBoxSingle = new System.Windows.Forms.GroupBox();
            this.ButtonLSI = new System.Windows.Forms.Button();
            this.GroupBoxCombination = new System.Windows.Forms.GroupBox();
            this.TextBoxLambda = new System.Windows.Forms.TextBox();
            this.LabelLog = new System.Windows.Forms.Label();
            this.GroupBoxAnalyze = new System.Windows.Forms.GroupBox();
            this.ButtonLambdaAnalyze = new System.Windows.Forms.Button();
            this.ButtonPMIAnalyze = new System.Windows.Forms.Button();
            this.ButtonAdditionAnalyze = new System.Windows.Forms.Button();
            this.ButtonVSMAnalyze = new System.Windows.Forms.Button();
            this.CheckBoxPMIAnalyze = new System.Windows.Forms.CheckBox();
            this.ButtonBordaAnalyze = new System.Windows.Forms.Button();
            this.CheckBoxVSMAnalyze = new System.Windows.Forms.CheckBox();
            this.ButtonLSIAnalyze = new System.Windows.Forms.Button();
            this.CheckBoxLSI = new System.Windows.Forms.CheckBox();
            this.CheckBoxLSIAnalyze = new System.Windows.Forms.CheckBox();
            this.GroupBoxRepo.SuspendLayout();
            this.GroupBoxTag.SuspendLayout();
            this.GroupBoxSingle.SuspendLayout();
            this.GroupBoxCombination.SuspendLayout();
            this.GroupBoxAnalyze.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBoxRepo
            // 
            this.GroupBoxRepo.Controls.Add(this.ButtonSetRepository);
            this.GroupBoxRepo.Controls.Add(this.LabelOutputDir);
            this.GroupBoxRepo.Controls.Add(this.TextBoxOutputDir);
            this.GroupBoxRepo.Location = new System.Drawing.Point(12, 12);
            this.GroupBoxRepo.Name = "GroupBoxRepo";
            this.GroupBoxRepo.Size = new System.Drawing.Size(476, 93);
            this.GroupBoxRepo.TabIndex = 4;
            this.GroupBoxRepo.TabStop = false;
            this.GroupBoxRepo.Text = "Repository Info";
            // 
            // ButtonSetRepository
            // 
            this.ButtonSetRepository.AccessibleRole = System.Windows.Forms.AccessibleRole.PageTabList;
            this.ButtonSetRepository.Location = new System.Drawing.Point(395, 54);
            this.ButtonSetRepository.Name = "ButtonSetRepository";
            this.ButtonSetRepository.Size = new System.Drawing.Size(75, 23);
            this.ButtonSetRepository.TabIndex = 7;
            this.ButtonSetRepository.Text = "Set Repo";
            this.ButtonSetRepository.UseVisualStyleBackColor = true;
            this.ButtonSetRepository.Click += new System.EventHandler(this.ButtonSetRepository_Click);
            // 
            // LabelOutputDir
            // 
            this.LabelOutputDir.AutoSize = true;
            this.LabelOutputDir.Location = new System.Drawing.Point(6, 27);
            this.LabelOutputDir.Name = "LabelOutputDir";
            this.LabelOutputDir.Size = new System.Drawing.Size(76, 13);
            this.LabelOutputDir.TabIndex = 0;
            this.LabelOutputDir.Text = "Input Directory";
            // 
            // TextBoxOutputDir
            // 
            this.TextBoxOutputDir.Location = new System.Drawing.Point(96, 24);
            this.TextBoxOutputDir.Name = "TextBoxOutputDir";
            this.TextBoxOutputDir.Size = new System.Drawing.Size(374, 20);
            this.TextBoxOutputDir.TabIndex = 1;
            // 
            // GroupBoxTag
            // 
            this.GroupBoxTag.Controls.Add(this.ListBoxTags);
            this.GroupBoxTag.Location = new System.Drawing.Point(14, 126);
            this.GroupBoxTag.Name = "GroupBoxTag";
            this.GroupBoxTag.Size = new System.Drawing.Size(204, 170);
            this.GroupBoxTag.TabIndex = 5;
            this.GroupBoxTag.TabStop = false;
            this.GroupBoxTag.Text = "Tags";
            // 
            // ListBoxTags
            // 
            this.ListBoxTags.FormattingEnabled = true;
            this.ListBoxTags.Location = new System.Drawing.Point(8, 28);
            this.ListBoxTags.Name = "ListBoxTags";
            this.ListBoxTags.Size = new System.Drawing.Size(190, 121);
            this.ListBoxTags.TabIndex = 0;
            // 
            // ButtonVsm
            // 
            this.ButtonVsm.Location = new System.Drawing.Point(6, 29);
            this.ButtonVsm.Name = "ButtonVsm";
            this.ButtonVsm.Size = new System.Drawing.Size(75, 23);
            this.ButtonVsm.TabIndex = 6;
            this.ButtonVsm.Text = "VSM";
            this.ButtonVsm.UseVisualStyleBackColor = true;
            this.ButtonVsm.Click += new System.EventHandler(this.ButtonVsm_Click);
            // 
            // ButtonPmi
            // 
            this.ButtonPmi.Location = new System.Drawing.Point(6, 58);
            this.ButtonPmi.Name = "ButtonPmi";
            this.ButtonPmi.Size = new System.Drawing.Size(75, 23);
            this.ButtonPmi.TabIndex = 7;
            this.ButtonPmi.Text = "PMI";
            this.ButtonPmi.UseVisualStyleBackColor = true;
            this.ButtonPmi.Click += new System.EventHandler(this.ButtonPmi_Click);
            // 
            // ButtonLambda
            // 
            this.ButtonLambda.Location = new System.Drawing.Point(95, 19);
            this.ButtonLambda.Name = "ButtonLambda";
            this.ButtonLambda.Size = new System.Drawing.Size(75, 23);
            this.ButtonLambda.TabIndex = 8;
            this.ButtonLambda.Text = "Lambda";
            this.ButtonLambda.UseVisualStyleBackColor = true;
            this.ButtonLambda.Click += new System.EventHandler(this.ButtonLambda_Click);
            // 
            // ButtonAddition
            // 
            this.ButtonAddition.Location = new System.Drawing.Point(95, 48);
            this.ButtonAddition.Name = "ButtonAddition";
            this.ButtonAddition.Size = new System.Drawing.Size(75, 23);
            this.ButtonAddition.TabIndex = 9;
            this.ButtonAddition.Text = "Addition";
            this.ButtonAddition.UseVisualStyleBackColor = true;
            this.ButtonAddition.Click += new System.EventHandler(this.ButtonAddition_Click);
            // 
            // ButtonBorda
            // 
            this.ButtonBorda.Location = new System.Drawing.Point(95, 77);
            this.ButtonBorda.Name = "ButtonBorda";
            this.ButtonBorda.Size = new System.Drawing.Size(75, 23);
            this.ButtonBorda.TabIndex = 10;
            this.ButtonBorda.Text = "Borda";
            this.ButtonBorda.UseVisualStyleBackColor = true;
            this.ButtonBorda.Click += new System.EventHandler(this.ButtonBorda_Click);
            // 
            // CheckBoxVSM
            // 
            this.CheckBoxVSM.AutoSize = true;
            this.CheckBoxVSM.Location = new System.Drawing.Point(7, 46);
            this.CheckBoxVSM.Name = "CheckBoxVSM";
            this.CheckBoxVSM.Size = new System.Drawing.Size(49, 17);
            this.CheckBoxVSM.TabIndex = 11;
            this.CheckBoxVSM.Text = "VSM";
            this.CheckBoxVSM.UseVisualStyleBackColor = true;
            // 
            // CheckBoxPMI
            // 
            this.CheckBoxPMI.AutoSize = true;
            this.CheckBoxPMI.Location = new System.Drawing.Point(7, 23);
            this.CheckBoxPMI.Name = "CheckBoxPMI";
            this.CheckBoxPMI.Size = new System.Drawing.Size(45, 17);
            this.CheckBoxPMI.TabIndex = 12;
            this.CheckBoxPMI.Text = "PMI";
            this.CheckBoxPMI.UseVisualStyleBackColor = true;
            // 
            // GroupBoxSingle
            // 
            this.GroupBoxSingle.Controls.Add(this.ButtonLSI);
            this.GroupBoxSingle.Controls.Add(this.ButtonVsm);
            this.GroupBoxSingle.Controls.Add(this.ButtonPmi);
            this.GroupBoxSingle.Location = new System.Drawing.Point(224, 126);
            this.GroupBoxSingle.Name = "GroupBoxSingle";
            this.GroupBoxSingle.Size = new System.Drawing.Size(91, 126);
            this.GroupBoxSingle.TabIndex = 13;
            this.GroupBoxSingle.TabStop = false;
            this.GroupBoxSingle.Text = "Single";
            // 
            // ButtonLSI
            // 
            this.ButtonLSI.Location = new System.Drawing.Point(6, 87);
            this.ButtonLSI.Name = "ButtonLSI";
            this.ButtonLSI.Size = new System.Drawing.Size(75, 23);
            this.ButtonLSI.TabIndex = 8;
            this.ButtonLSI.Text = "LSI";
            this.ButtonLSI.UseVisualStyleBackColor = true;
            this.ButtonLSI.Click += new System.EventHandler(this.ButtonLSI_Click);
            // 
            // GroupBoxCombination
            // 
            this.GroupBoxCombination.Controls.Add(this.CheckBoxLSI);
            this.GroupBoxCombination.Controls.Add(this.TextBoxLambda);
            this.GroupBoxCombination.Controls.Add(this.ButtonLambda);
            this.GroupBoxCombination.Controls.Add(this.ButtonAddition);
            this.GroupBoxCombination.Controls.Add(this.CheckBoxPMI);
            this.GroupBoxCombination.Controls.Add(this.ButtonBorda);
            this.GroupBoxCombination.Controls.Add(this.CheckBoxVSM);
            this.GroupBoxCombination.Location = new System.Drawing.Point(321, 126);
            this.GroupBoxCombination.Name = "GroupBoxCombination";
            this.GroupBoxCombination.Size = new System.Drawing.Size(176, 126);
            this.GroupBoxCombination.TabIndex = 14;
            this.GroupBoxCombination.TabStop = false;
            this.GroupBoxCombination.Text = "Combination";
            // 
            // TextBoxLambda
            // 
            this.TextBoxLambda.Location = new System.Drawing.Point(65, 21);
            this.TextBoxLambda.Name = "TextBoxLambda";
            this.TextBoxLambda.Size = new System.Drawing.Size(26, 20);
            this.TextBoxLambda.TabIndex = 13;
            // 
            // LabelLog
            // 
            this.LabelLog.AutoSize = true;
            this.LabelLog.Location = new System.Drawing.Point(12, 377);
            this.LabelLog.Name = "LabelLog";
            this.LabelLog.Size = new System.Drawing.Size(0, 13);
            this.LabelLog.TabIndex = 15;
            // 
            // GroupBoxAnalyze
            // 
            this.GroupBoxAnalyze.Controls.Add(this.CheckBoxLSIAnalyze);
            this.GroupBoxAnalyze.Controls.Add(this.ButtonLSIAnalyze);
            this.GroupBoxAnalyze.Controls.Add(this.ButtonLambdaAnalyze);
            this.GroupBoxAnalyze.Controls.Add(this.ButtonPMIAnalyze);
            this.GroupBoxAnalyze.Controls.Add(this.ButtonAdditionAnalyze);
            this.GroupBoxAnalyze.Controls.Add(this.ButtonVSMAnalyze);
            this.GroupBoxAnalyze.Controls.Add(this.CheckBoxPMIAnalyze);
            this.GroupBoxAnalyze.Controls.Add(this.ButtonBordaAnalyze);
            this.GroupBoxAnalyze.Controls.Add(this.CheckBoxVSMAnalyze);
            this.GroupBoxAnalyze.Location = new System.Drawing.Point(224, 258);
            this.GroupBoxAnalyze.Name = "GroupBoxAnalyze";
            this.GroupBoxAnalyze.Size = new System.Drawing.Size(273, 105);
            this.GroupBoxAnalyze.TabIndex = 16;
            this.GroupBoxAnalyze.TabStop = false;
            this.GroupBoxAnalyze.Text = "Analyze";
            // 
            // ButtonLambdaAnalyze
            // 
            this.ButtonLambdaAnalyze.Location = new System.Drawing.Point(192, 15);
            this.ButtonLambdaAnalyze.Name = "ButtonLambdaAnalyze";
            this.ButtonLambdaAnalyze.Size = new System.Drawing.Size(75, 23);
            this.ButtonLambdaAnalyze.TabIndex = 14;
            this.ButtonLambdaAnalyze.Text = "Lambda";
            this.ButtonLambdaAnalyze.UseVisualStyleBackColor = true;
            this.ButtonLambdaAnalyze.Click += new System.EventHandler(this.ButtonLambdaAnalyze_Click);
            // 
            // ButtonPMIAnalyze
            // 
            this.ButtonPMIAnalyze.Location = new System.Drawing.Point(6, 48);
            this.ButtonPMIAnalyze.Name = "ButtonPMIAnalyze";
            this.ButtonPMIAnalyze.Size = new System.Drawing.Size(75, 23);
            this.ButtonPMIAnalyze.TabIndex = 8;
            this.ButtonPMIAnalyze.Text = "PMI";
            this.ButtonPMIAnalyze.UseVisualStyleBackColor = true;
            this.ButtonPMIAnalyze.Click += new System.EventHandler(this.ButtonPMIAnalyze_Click);
            // 
            // ButtonAdditionAnalyze
            // 
            this.ButtonAdditionAnalyze.Location = new System.Drawing.Point(192, 44);
            this.ButtonAdditionAnalyze.Name = "ButtonAdditionAnalyze";
            this.ButtonAdditionAnalyze.Size = new System.Drawing.Size(75, 23);
            this.ButtonAdditionAnalyze.TabIndex = 15;
            this.ButtonAdditionAnalyze.Text = "Addition";
            this.ButtonAdditionAnalyze.UseVisualStyleBackColor = true;
            this.ButtonAdditionAnalyze.Click += new System.EventHandler(this.ButtonAdditionAnalyze_Click);
            // 
            // ButtonVSMAnalyze
            // 
            this.ButtonVSMAnalyze.Location = new System.Drawing.Point(6, 19);
            this.ButtonVSMAnalyze.Name = "ButtonVSMAnalyze";
            this.ButtonVSMAnalyze.Size = new System.Drawing.Size(75, 23);
            this.ButtonVSMAnalyze.TabIndex = 7;
            this.ButtonVSMAnalyze.Text = "VSM";
            this.ButtonVSMAnalyze.UseVisualStyleBackColor = true;
            this.ButtonVSMAnalyze.Click += new System.EventHandler(this.ButtonVSMAnalyze_Click);
            // 
            // CheckBoxPMIAnalyze
            // 
            this.CheckBoxPMIAnalyze.AutoSize = true;
            this.CheckBoxPMIAnalyze.Location = new System.Drawing.Point(104, 19);
            this.CheckBoxPMIAnalyze.Name = "CheckBoxPMIAnalyze";
            this.CheckBoxPMIAnalyze.Size = new System.Drawing.Size(45, 17);
            this.CheckBoxPMIAnalyze.TabIndex = 18;
            this.CheckBoxPMIAnalyze.Text = "PMI";
            this.CheckBoxPMIAnalyze.UseVisualStyleBackColor = true;
            // 
            // ButtonBordaAnalyze
            // 
            this.ButtonBordaAnalyze.Location = new System.Drawing.Point(192, 73);
            this.ButtonBordaAnalyze.Name = "ButtonBordaAnalyze";
            this.ButtonBordaAnalyze.Size = new System.Drawing.Size(75, 23);
            this.ButtonBordaAnalyze.TabIndex = 16;
            this.ButtonBordaAnalyze.Text = "Borda";
            this.ButtonBordaAnalyze.UseVisualStyleBackColor = true;
            this.ButtonBordaAnalyze.Click += new System.EventHandler(this.ButtonBordaAnalyze_Click);
            // 
            // CheckBoxVSMAnalyze
            // 
            this.CheckBoxVSMAnalyze.AutoSize = true;
            this.CheckBoxVSMAnalyze.Location = new System.Drawing.Point(104, 42);
            this.CheckBoxVSMAnalyze.Name = "CheckBoxVSMAnalyze";
            this.CheckBoxVSMAnalyze.Size = new System.Drawing.Size(49, 17);
            this.CheckBoxVSMAnalyze.TabIndex = 17;
            this.CheckBoxVSMAnalyze.Text = "VSM";
            this.CheckBoxVSMAnalyze.UseVisualStyleBackColor = true;
            // 
            // ButtonLSIAnalyze
            // 
            this.ButtonLSIAnalyze.Location = new System.Drawing.Point(6, 76);
            this.ButtonLSIAnalyze.Name = "ButtonLSIAnalyze";
            this.ButtonLSIAnalyze.Size = new System.Drawing.Size(75, 23);
            this.ButtonLSIAnalyze.TabIndex = 9;
            this.ButtonLSIAnalyze.Text = "LSI";
            this.ButtonLSIAnalyze.UseVisualStyleBackColor = true;
            this.ButtonLSIAnalyze.Click += new System.EventHandler(this.ButtonLSIAnalyze_Click);
            // 
            // CheckBoxLSI
            // 
            this.CheckBoxLSI.AutoSize = true;
            this.CheckBoxLSI.Location = new System.Drawing.Point(7, 69);
            this.CheckBoxLSI.Name = "CheckBoxLSI";
            this.CheckBoxLSI.Size = new System.Drawing.Size(42, 17);
            this.CheckBoxLSI.TabIndex = 14;
            this.CheckBoxLSI.Text = "LSI";
            this.CheckBoxLSI.UseVisualStyleBackColor = true;
            // 
            // CheckBoxLSIAnalyze
            // 
            this.CheckBoxLSIAnalyze.AutoSize = true;
            this.CheckBoxLSIAnalyze.Location = new System.Drawing.Point(104, 65);
            this.CheckBoxLSIAnalyze.Name = "CheckBoxLSIAnalyze";
            this.CheckBoxLSIAnalyze.Size = new System.Drawing.Size(42, 17);
            this.CheckBoxLSIAnalyze.TabIndex = 19;
            this.CheckBoxLSIAnalyze.Text = "LSI";
            this.CheckBoxLSIAnalyze.UseVisualStyleBackColor = true;
            // 
            // LocalizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 399);
            this.Controls.Add(this.GroupBoxAnalyze);
            this.Controls.Add(this.LabelLog);
            this.Controls.Add(this.GroupBoxCombination);
            this.Controls.Add(this.GroupBoxSingle);
            this.Controls.Add(this.GroupBoxTag);
            this.Controls.Add(this.GroupBoxRepo);
            this.Name = "LocalizationForm";
            this.Text = "LocalizationForm";
            this.GroupBoxRepo.ResumeLayout(false);
            this.GroupBoxRepo.PerformLayout();
            this.GroupBoxTag.ResumeLayout(false);
            this.GroupBoxSingle.ResumeLayout(false);
            this.GroupBoxCombination.ResumeLayout(false);
            this.GroupBoxCombination.PerformLayout();
            this.GroupBoxAnalyze.ResumeLayout(false);
            this.GroupBoxAnalyze.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBoxRepo;
        private System.Windows.Forms.Label LabelOutputDir;
        private System.Windows.Forms.TextBox TextBoxOutputDir;
        private System.Windows.Forms.GroupBox GroupBoxTag;
        private System.Windows.Forms.ListBox ListBoxTags;
        private System.Windows.Forms.Button ButtonVsm;
        private System.Windows.Forms.Button ButtonPmi;
        private System.Windows.Forms.Button ButtonLambda;
        private System.Windows.Forms.Button ButtonAddition;
        private System.Windows.Forms.Button ButtonBorda;
        private System.Windows.Forms.CheckBox CheckBoxVSM;
        private System.Windows.Forms.CheckBox CheckBoxPMI;
        private System.Windows.Forms.GroupBox GroupBoxSingle;
        private System.Windows.Forms.GroupBox GroupBoxCombination;
        private System.Windows.Forms.Button ButtonSetRepository;
        private System.Windows.Forms.Label LabelLog;
        private System.Windows.Forms.TextBox TextBoxLambda;
        private System.Windows.Forms.GroupBox GroupBoxAnalyze;
        private System.Windows.Forms.Button ButtonLambdaAnalyze;
        private System.Windows.Forms.Button ButtonPMIAnalyze;
        private System.Windows.Forms.Button ButtonAdditionAnalyze;
        private System.Windows.Forms.Button ButtonVSMAnalyze;
        private System.Windows.Forms.CheckBox CheckBoxPMIAnalyze;
        private System.Windows.Forms.Button ButtonBordaAnalyze;
        private System.Windows.Forms.CheckBox CheckBoxVSMAnalyze;
        private System.Windows.Forms.Button ButtonLSI;
        private System.Windows.Forms.Button ButtonLSIAnalyze;
        private System.Windows.Forms.CheckBox CheckBoxLSI;
        private System.Windows.Forms.CheckBox CheckBoxLSIAnalyze;
    }
}