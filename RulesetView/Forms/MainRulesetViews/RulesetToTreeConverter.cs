using System;
using System.Windows.Forms;
using RulesetView.Models;
using RulesetView.Services;

namespace RulesetView.Forms.MainRulesetViews
{
    public class RulesetToTreeConverter
    {
        public void Convert(RuleSet ruleset, TreeView ruleTree)
        {
            var nodeService = new NodeService();
            var roots = nodeService.GetNodes(ruleset);
            foreach (var root in roots)
            {
                var treeNode = ruleTree.Nodes.Add(Guid.NewGuid().ToString());
                SetupTreeNode(treeNode, root, null);
            }
        }

        private void SetupTreeNode(TreeNode treeNode, Node node, Node parentNode)
        {
            var name = GetName(node, parentNode);
            treeNode.Text = name;
            treeNode.Expand();
            foreach (var childNode in node.ChildList)
            {
                var childTreeNode = treeNode.Nodes.Add(Guid.NewGuid().ToString());
                SetupTreeNode(childTreeNode, childNode, node);
            }
        }

        private static string GetName(Node node, Node parentNode)
        {
            var name = GetSimpleName(node.Name);
            var manu = node as Node<ManufactureItem>;
            if (manu != null)
            {
                var otherDepend = GetOtherDepend(parentNode, manu.Data.Requirements);
                name = "(Manufacture) " + name + otherDepend;
            }
            var rese = node as Node<ResearchItem>;
            if (rese != null)
            {
                var otherDepend = GetOtherDepend(parentNode, rese.Data.Dependencies);
                name = "(Research) " + name + otherDepend;
            }
            return name;
        }

        private static string GetSimpleName(string name)
        {
            if (string.IsNullOrEmpty(name)) name = "Without name";
            name = name.Replace("STR_", "");
            name = name.Replace("_", " ");
            return name;
        }

        private static string GetOtherDepend(Node parent, string[] requirements)
        {
            var otherDepend = string.Empty;
            if (requirements == null) return null;
            if (parent == null) return null;
            foreach (var dependency in requirements)
            {
                if (dependency == parent.Name) continue;
                var dependName = GetSimpleName(dependency);
                if (!string.IsNullOrEmpty(otherDepend)) otherDepend += ",  ";
                otherDepend += dependName;
            }
            if (string.IsNullOrEmpty(otherDepend)) return null;
            return "  [ Also requires:  " + otherDepend + " ]";
        }
    }
}
