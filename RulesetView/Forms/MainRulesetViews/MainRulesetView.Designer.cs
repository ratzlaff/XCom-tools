namespace RulesetView.Forms.MainRulesetViews
{
    partial class MainRulesetView
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("STR_HEAVY_PLASMA");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("STR_PLASMA_RIFLE");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("STR_PLASMA_PISTOL", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainRulesetView));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.RuleTree = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.RefreshButton = new System.Windows.Forms.ToolStripButton();
            this.CostBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sellPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProfitWithoutDepdendencyPerHour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requiresEleriumDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.EleriumRequired = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dependencyCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DependencyTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.profitPerHourDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CostBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // RuleTree
            // 
            this.RuleTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RuleTree.Location = new System.Drawing.Point(3, 3);
            this.RuleTree.Name = "RuleTree";
            treeNode1.Name = "STR_HEAVY_PLASMA";
            treeNode1.Text = "STR_HEAVY_PLASMA";
            treeNode2.Name = "Node2";
            treeNode2.Text = "STR_PLASMA_RIFLE";
            treeNode3.Name = "STR_PLASMA_PISTOL";
            treeNode3.Text = "STR_PLASMA_PISTOL";
            this.RuleTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.RuleTree.Size = new System.Drawing.Size(940, 470);
            this.RuleTree.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1001, 502);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.RuleTree);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(946, 476);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tech Tree";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(993, 476);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Costs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.costDataGridViewTextBoxColumn,
            this.timeDataGridViewTextBoxColumn,
            this.TimeCost,
            this.sellPriceDataGridViewTextBoxColumn,
            this.ProfitWithoutDepdendencyPerHour,
            this.requiresEleriumDataGridViewCheckBoxColumn,
            this.EleriumRequired,
            this.dependencyCostDataGridViewTextBoxColumn,
            this.DependencyTime,
            this.profitPerHourDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.CostBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(987, 470);
            this.dataGridView1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RefreshButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1001, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // RefreshButton
            // 
            this.RefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshButton.Image")));
            this.RefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(50, 22);
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // CostBindingSource
            // 
            this.CostBindingSource.DataSource = typeof(RulesetView.Models.CostGridModel);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 200;
            // 
            // costDataGridViewTextBoxColumn
            // 
            this.costDataGridViewTextBoxColumn.DataPropertyName = "Cost";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "C2";
            this.costDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.costDataGridViewTextBoxColumn.HeaderText = "Cost";
            this.costDataGridViewTextBoxColumn.Name = "costDataGridViewTextBoxColumn";
            this.costDataGridViewTextBoxColumn.ReadOnly = true;
            this.costDataGridViewTextBoxColumn.Width = 90;
            // 
            // timeDataGridViewTextBoxColumn
            // 
            this.timeDataGridViewTextBoxColumn.DataPropertyName = "Time";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.timeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.timeDataGridViewTextBoxColumn.HeaderText = "Time";
            this.timeDataGridViewTextBoxColumn.Name = "timeDataGridViewTextBoxColumn";
            this.timeDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeDataGridViewTextBoxColumn.Width = 60;
            // 
            // TimeCost
            // 
            this.TimeCost.DataPropertyName = "TimeCost";
            dataGridViewCellStyle3.Format = "C2";
            this.TimeCost.DefaultCellStyle = dataGridViewCellStyle3;
            this.TimeCost.HeaderText = "TimeCost";
            this.TimeCost.Name = "TimeCost";
            this.TimeCost.ReadOnly = true;
            // 
            // sellPriceDataGridViewTextBoxColumn
            // 
            this.sellPriceDataGridViewTextBoxColumn.DataPropertyName = "SellPrice";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "C2";
            this.sellPriceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.sellPriceDataGridViewTextBoxColumn.HeaderText = "Price";
            this.sellPriceDataGridViewTextBoxColumn.Name = "sellPriceDataGridViewTextBoxColumn";
            this.sellPriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.sellPriceDataGridViewTextBoxColumn.Width = 90;
            // 
            // ProfitWithoutDepdendencyPerHour
            // 
            this.ProfitWithoutDepdendencyPerHour.DataPropertyName = "ProfitWithoutDepdendencyPerHour";
            dataGridViewCellStyle5.Format = "C2";
            this.ProfitWithoutDepdendencyPerHour.DefaultCellStyle = dataGridViewCellStyle5;
            this.ProfitWithoutDepdendencyPerHour.HeaderText = "Profit/hr";
            this.ProfitWithoutDepdendencyPerHour.Name = "ProfitWithoutDepdendencyPerHour";
            this.ProfitWithoutDepdendencyPerHour.ReadOnly = true;
            this.ProfitWithoutDepdendencyPerHour.Width = 60;
            // 
            // requiresEleriumDataGridViewCheckBoxColumn
            // 
            this.requiresEleriumDataGridViewCheckBoxColumn.DataPropertyName = "RequiresElerium";
            this.requiresEleriumDataGridViewCheckBoxColumn.HeaderText = "Elerium";
            this.requiresEleriumDataGridViewCheckBoxColumn.Name = "requiresEleriumDataGridViewCheckBoxColumn";
            this.requiresEleriumDataGridViewCheckBoxColumn.ReadOnly = true;
            this.requiresEleriumDataGridViewCheckBoxColumn.Width = 45;
            // 
            // EleriumRequired
            // 
            this.EleriumRequired.DataPropertyName = "EleriumRequired";
            this.EleriumRequired.HeaderText = "Elerium";
            this.EleriumRequired.Name = "EleriumRequired";
            this.EleriumRequired.ReadOnly = true;
            this.EleriumRequired.Width = 45;
            // 
            // dependencyCostDataGridViewTextBoxColumn
            // 
            this.dependencyCostDataGridViewTextBoxColumn.DataPropertyName = "DependencyCost";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "C2";
            this.dependencyCostDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.dependencyCostDataGridViewTextBoxColumn.HeaderText = "Dependency Cost";
            this.dependencyCostDataGridViewTextBoxColumn.Name = "dependencyCostDataGridViewTextBoxColumn";
            this.dependencyCostDataGridViewTextBoxColumn.ReadOnly = true;
            this.dependencyCostDataGridViewTextBoxColumn.Width = 90;
            // 
            // DependencyTime
            // 
            this.DependencyTime.DataPropertyName = "DependencyTime";
            this.DependencyTime.HeaderText = "Dependency Time";
            this.DependencyTime.Name = "DependencyTime";
            this.DependencyTime.ReadOnly = true;
            this.DependencyTime.Width = 80;
            // 
            // profitPerHourDataGridViewTextBoxColumn
            // 
            this.profitPerHourDataGridViewTextBoxColumn.DataPropertyName = "ProfitPerHour";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "C2";
            dataGridViewCellStyle7.NullValue = null;
            this.profitPerHourDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.profitPerHourDataGridViewTextBoxColumn.HeaderText = "Full Profit/hr";
            this.profitPerHourDataGridViewTextBoxColumn.Name = "profitPerHourDataGridViewTextBoxColumn";
            this.profitPerHourDataGridViewTextBoxColumn.ReadOnly = true;
            this.profitPerHourDataGridViewTextBoxColumn.Width = 60;
            // 
            // MainRulesetView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 527);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainRulesetView";
            this.Text = "Ruleset Viewer";
            this.Load += new System.EventHandler(this.MainRulesetView_Load);
            this.Shown += new System.EventHandler(this.MainRulesetView_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CostBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView RuleTree;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource CostBindingSource;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton RefreshButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn sellPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProfitWithoutDepdendencyPerHour;
        private System.Windows.Forms.DataGridViewCheckBoxColumn requiresEleriumDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EleriumRequired;
        private System.Windows.Forms.DataGridViewTextBoxColumn dependencyCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DependencyTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn profitPerHourDataGridViewTextBoxColumn;
    }
}

