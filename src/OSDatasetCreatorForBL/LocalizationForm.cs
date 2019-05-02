using GithubProcessor;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OSDatasetCreatorForBL
{
    public partial class LocalizationForm : Form, ILog
    {
        public LocalizationForm()
        {
            InitializeComponent();

            CheckBoxPMI.Checked = true;
            CheckBoxVSM.Checked = true;
            TextBoxLambda.Text = "0.8";

            CheckBoxPMIAnalyze.Checked = true;
            CheckBoxVSMAnalyze.Checked = true;
        }

        #region Log

        public void Log(string message)
        {
            LabelLog.Text = message;
            LabelLog.Refresh();
        }

        #endregion Log

        private LocalizationRunner _localizationRunner;

        public void ButtonSetRepository_Click(object sender, EventArgs e)
        {
            SetRepository(TextBoxOutputDir.Text);
        }

        public void SetRepository(string path)
        {
            TextBoxOutputDir.Text = path;

            _localizationRunner = new LocalizationRunner();
            _localizationRunner.SetRepositoryFullPath(path);
            _localizationRunner.SetLogger(this);

            LoadTags();
        }

        public void LoadTags()
        {

        }

        private void ButtonVsm_Click(object sender, EventArgs e)
        {
            _localizationRunner.RunVsmOnIssues();
        }

        private void ButtonPmi_Click(object sender, EventArgs e)
        {
            _localizationRunner.RunPmiOnIssues();
        }

        private void ButtonLSI_Click(object sender, EventArgs e)
        {
            _localizationRunner.RunLsiOnIssues();
        }

        private void ButtonLambda_Click(object sender, EventArgs e)
        {
            var list = new List<string>();
            if (CheckBoxPMI.Checked) list.Add("Pmi");
            if (CheckBoxVSM.Checked) list.Add("Vsm");
            if (CheckBoxLSI.Checked) list.Add("Lsi");
            var lambda = double.Parse(TextBoxLambda.Text);
            _localizationRunner.RunLambdaCombinationOnIssues(list[0], list[1], lambda);
        }

        private void ButtonAddition_Click(object sender, EventArgs e)
        {
            var list = new List<string>();
            if (CheckBoxPMI.Checked) list.Add("Pmi");
            if (CheckBoxVSM.Checked) list.Add("Vsm");
            if (CheckBoxLSI.Checked) list.Add("Lsi");
            _localizationRunner.RunAdditionCombinationOnIssues(list[0], list[1]);
        }

        private void ButtonBorda_Click(object sender, EventArgs e)
        {
            var list = new List<string>();
            if (CheckBoxPMI.Checked) list.Add("Pmi");
            if (CheckBoxVSM.Checked) list.Add("Vsm");
            if (CheckBoxLSI.Checked) list.Add("Lsi");
            _localizationRunner.RunBordaCountCombinationOnIssues(list[0], list[1]);
        }

        private void ButtonVSMAnalyze_Click(object sender, EventArgs e)
        {
            _localizationRunner.RunVsmResult();
        }

        private void ButtonPMIAnalyze_Click(object sender, EventArgs e)
        {
            _localizationRunner.RunPmiResult();
        }

        private void ButtonLSIAnalyze_Click(object sender, EventArgs e)
        {
            _localizationRunner.RunLsiResult();
        }

        private void ButtonLambdaAnalyze_Click(object sender, EventArgs e)
        {
            var list = new List<string>();
            if (CheckBoxPMIAnalyze.Checked) list.Add("Pmi");
            if (CheckBoxVSMAnalyze.Checked) list.Add("Vsm");
            if (CheckBoxLSIAnalyze.Checked) list.Add("Lsi");
            _localizationRunner.RunLambdaResult(list[0], list[1]);
        }

        private void ButtonAdditionAnalyze_Click(object sender, EventArgs e)
        {
            var list = new List<string>();
            if (CheckBoxPMIAnalyze.Checked) list.Add("Pmi");
            if (CheckBoxVSMAnalyze.Checked) list.Add("Vsm");
            if (CheckBoxLSIAnalyze.Checked) list.Add("Lsi");
            _localizationRunner.RunAdditionResult(list[0], list[1]);
        }

        private void ButtonBordaAnalyze_Click(object sender, EventArgs e)
        {
            var list = new List<string>();
            if (CheckBoxPMIAnalyze.Checked) list.Add("Pmi");
            if (CheckBoxVSMAnalyze.Checked) list.Add("Vsm");
            if (CheckBoxLSIAnalyze.Checked) list.Add("Lsi");
            _localizationRunner.RunBordaResult(list[0], list[1]);
        }
    }
}
