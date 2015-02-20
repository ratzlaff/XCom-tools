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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("STR_HEAVY_PLASMA");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("STR_PLASMA_RIFLE");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("STR_PLASMA_PISTOL", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.RuleTree = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // RuleTree
            // 
            this.RuleTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RuleTree.Location = new System.Drawing.Point(0, 0);
            this.RuleTree.Name = "RuleTree";
            treeNode1.Name = "STR_HEAVY_PLASMA";
            treeNode1.Text = "STR_HEAVY_PLASMA";
            treeNode2.Name = "Node2";
            treeNode2.Text = "STR_PLASMA_RIFLE";
            treeNode3.Name = "STR_PLASMA_PISTOL";
            treeNode3.Text = "STR_PLASMA_PISTOL";
            this.RuleTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.RuleTree.Size = new System.Drawing.Size(467, 401);
            this.RuleTree.TabIndex = 0;
            // 
            // MainRulesetView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 401);
            this.Controls.Add(this.RuleTree);
            this.Name = "MainRulesetView";
            this.Text = "Ruleset Viewer";
            this.Load += new System.EventHandler(this.MainRulesetView_Load);
            this.Shown += new System.EventHandler(this.MainRulesetView_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView RuleTree;
    }
}

