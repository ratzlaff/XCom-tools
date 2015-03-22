using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using RulesetView.Models;
using RulesetView.Services;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RulesetView.Forms.MainRulesetViews
{
    public partial class MainRulesetView : Form
    {
        private readonly BindingList<CostGridModel> _costList = new BindingList<CostGridModel>();
        private string _file;

        public MainRulesetView()
        {
            InitializeComponent();
        }

        private void MainRulesetView_Load(object sender, System.EventArgs e)
        {
            RuleTree.Nodes.Clear();
            CostBindingSource.DataSource = _costList;
        }

        private void MainRulesetView_Shown(object sender, EventArgs e)
        {
            var file = AskFile();
            if (file != null)
            {
                _file = file;
                LoadFile(_file);
            }
        }

        private void LoadFile(string file)
        {
            var ruleset = ReadRuleSet(file);
            if (ruleset == null) return;
            var rulesetToTreeConverter = new RulesetToTreeConverter();
            rulesetToTreeConverter.Convert(ruleset, RuleTree);
            var costGridModelConverter = new CostGridModelConverter();
            var items = costGridModelConverter.Convert(ruleset);
            items.Sort((x, y) => (int)((y.ProfitPerHour - x.ProfitPerHour)*10M));
            _costList.Clear();
            foreach (var item in items)
            {
                _costList.Add(item);
            }
        }

        private static RuleSet ReadRuleSet(string file)
        {
            using (var reader = File.OpenText(file))
            {
                try
                {
                    var deserializer = new Deserializer(
                        namingConvention: new CamelCaseNamingConvention(),
                        ignoreUnmatched: true);
                    return deserializer.Deserialize<RuleSet>(reader);
                }
                catch (YamlException ex)
                {
                    if (ex.Message.Contains("Expected 'MappingStart', got 'SequenceStart'"))
                    {
                        MessageBox.Show(
                            ex.Message + Environment.NewLine + Environment.NewLine +
                            @"To fix this issue try removing the '-' from this line #: " + ex.Start.Line,
                            @"Yaml Exception",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return null;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private string AskFile()
        {
            var form = new OpenFileDialog();
            if (form.ShowDialog() == DialogResult.OK)
            {
                return form.FileName;
            }
            return null;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadFile(_file);
        }
    }
}