using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RulesetView.Models;

namespace RulesetView.Services
{
    public class NodeService
    {
        public List<Node> GetNodes(RuleSet ruleset)
        { 
            var researchNodes = GetResearchNodes(ruleset);
            var manufactureNodes = GetManufactureNodes(ruleset);
            var roots = GetRoots(researchNodes, manufactureNodes);

            var researchNodesNotInRootChilds = researchNodes.Where(n => !roots.Contains(n)).ToList();
            var manufactureNodesNotInRootChilds = manufactureNodes.Where(n => !roots.Contains(n)).ToList();

            AddNodeChildsAndGoDeep(roots, researchNodesNotInRootChilds, manufactureNodesNotInRootChilds);
            return roots;
        }

        private static void AddNodeChildsAndGoDeep(
            List<Node> roots,
            List<Node<ResearchItem>> researchNodes,
            List<Node<ManufactureItem>> manufactureNodes)
        {
            // Add childs of childs 
            foreach (var root in roots)
            {
                AddNodeChilds(root, researchNodes, manufactureNodes);
            }
            foreach (var root in roots)
            {
                var newRoots = root.ChildList;
                var researchNodesNotInRootChilds = researchNodes.Where(n => !newRoots.Contains(n)).ToList();
                var manufactureNodesNotInRootChilds = manufactureNodes.Where(n => !newRoots.Contains(n)).ToList();
                AddNodeChildsAndGoDeep(newRoots, researchNodesNotInRootChilds, manufactureNodesNotInRootChilds);
            }
        }

        private static void AddNodeChilds(
            Node node, 
            List<Node<ResearchItem>> researchNodesNotInRootChilds,
            List<Node<ManufactureItem>> manufactureNodesNotInRootChilds)
        {
            foreach (var researchNode in researchNodesNotInRootChilds)
            {
                if (!researchNode.Data.Dependencies.Contains(node.Name)) continue;
                if (node.ChildList.Contains(researchNode)) continue;
                node.ChildList.Add(researchNode);
            }

            if (node is Node<ResearchItem>)
            {
                foreach (var manufactureNode in manufactureNodesNotInRootChilds)
                {
                    if (!manufactureNode.Data.Requirements.Contains(node.Name)) continue;
                    if (node.ChildList.Contains(manufactureNode)) continue;
                    node.ChildList.Add(manufactureNode);
                }
            }
        }

        private static List<Node> GetRoots(
            IList<Node<ResearchItem>> researchNodes,
            IList<Node<ManufactureItem>> manufactureNodes)
        {
            var roots = new List<Node>();
            foreach (var researchNode in researchNodes)
            {
                if (researchNode.Data.Dependencies == null ||
                    researchNode.Data.Dependencies.Length == 0)
                    roots.Add(researchNode);
            }
            foreach (var manufactureNode in manufactureNodes)
            {
                if (manufactureNode.Data.Requirements == null ||
                    manufactureNode.Data.Requirements.Length == 0) roots.Add(manufactureNode);
            } 
            return roots;
        }

        private static List<Node<ManufactureItem>> GetManufactureNodes(RuleSet ruleset)
        {
            var manufactureNodes = new List<Node<ManufactureItem>>();
            if (ruleset.ManufactureItems == null) return manufactureNodes;
            foreach (var manufactureItem in ruleset.ManufactureItems)
            {
                var node = new Node<ManufactureItem>();
                node.Name = manufactureItem.Name;
                node.Data = manufactureItem;
                node.ChildList = new List<Node>();
                manufactureNodes.Add(node);
            }
            return manufactureNodes;
        }

        private static List<Node<ResearchItem>> GetResearchNodes(RuleSet ruleset)
        {
            var researchNodes = new List<Node<ResearchItem>>();
            if (ruleset.ResearchItems == null) return researchNodes;
            foreach (var researchItem in ruleset.ResearchItems)
            {
                var node = new Node<ResearchItem>();
                node.Name = researchItem.Name;
                node.Data = researchItem;
                node.ChildList = new List<Node>();
                researchNodes.Add(node);
            }
            return researchNodes;
        }
    }
}
