using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RulesetView.Models;

namespace RulesetView.Forms.MainRulesetViews
{
    public class CostGridModelConverter
    {
        public List<CostGridModel> Convert(RuleSet ruleset)
        {
            var costList = new List<CostGridModel>();
            if (ruleset.ManufactureItems == null) return costList;
            foreach (var item in ruleset.ManufactureItems)
            {
                var model = new CostGridModel();
                model.DataModel = item;
                model.OriginalName = item.Name;
                model.Name = GetSimpleName(item.Name);
                model.Cost = item.Cost;
                model.Time = item.Time;
                costList.Add(model);
            }
            foreach (var model in costList)
            {
                var costAndTime = GetDependencyCostAndTime(
                    model.DataModel.RequiredItems, costList,ruleset);
                UpdateModeCosts(ruleset, model, costAndTime);
            }
            return costList;
        }
         
        private CostAndTime GetDependencyCostAndTime(
            Dictionary<string, decimal> requiredItems,
            List<CostGridModel> costList,RuleSet ruleset)
        {
            var cost = new CostAndTime();
            if (requiredItems == null) return cost;
            foreach (var requiredItem in requiredItems)
            {
                var amount = requiredItem.Value;
                if (requiredItem.Key == "STR_ELERIUM_115")
                {
                    cost.UseElerium = true;
                    cost.EleriumRequired += (int)amount;
                }
                var costItem = GetCostGridModel(costList, requiredItem);
                if (costItem == null) continue;
                if (!costItem.TotalCost.HasValue)
                {
                    var costAndTime = GetDependencyCostAndTime(
                        costItem.DataModel.RequiredItems,  costList, ruleset);
                    UpdateModeCosts(ruleset, costItem, costAndTime);
                }
                if (!costItem.TotalCost.HasValue) continue;

                if (costItem.RequiresElerium)
                {
                    cost.UseElerium = true;
                    cost.EleriumRequired += (int)(costItem.EleriumRequired * amount);
                }
                cost.Cost += costItem.TotalCost.Value * amount;
                cost.Time += (int)(costItem.Time + costItem.DependencyTime) * (int)amount;
            }
            return cost;
        }

        private static void UpdateModeCosts(RuleSet ruleset, CostGridModel model, CostAndTime costAndTime)
        {
            model.RequiresElerium = costAndTime.UseElerium;
            model.EleriumRequired = costAndTime.EleriumRequired;
            model.DependencyCost = costAndTime.Cost;
            model.DependencyTime = costAndTime.Time;
            model.TimeCost = model.Time * 34.72M;
            model.TotalCost = model.DependencyCost + model.Cost + model.TimeCost;
            model.SellPrice = GetSellPrice(ruleset, model.OriginalName);

            model.ProfitPerHour = (model.SellPrice - model.TotalCost.Value);
            var totalTime = (model.Time + model.DependencyTime);
            if (totalTime > 0) model.ProfitPerHour /= totalTime;

            model.ProfitWithoutDepdendencyPerHour = (model.SellPrice - model.TimeCost - model.Cost);
            if (totalTime > 0) model.ProfitWithoutDepdendencyPerHour /= totalTime;
        }

        private static CostGridModel GetCostGridModel(List<CostGridModel> costList, KeyValuePair<string, decimal> requiredItem)
        {
            foreach (var  costItem in costList)
            {
                if (costItem.OriginalName == requiredItem.Key)
                {
                    return  costItem;
                }
            }
            return null;
        }

        private static decimal GetSellPrice(RuleSet ruleset, string originalName)
        {
            if (ruleset.Items != null)
            {
                foreach (var item in ruleset.Items)
                {
                    if (originalName == item.Name)
                    {
                        return item.SellPrice;
                    }
                }
            }
            if (ruleset.Crafts != null)
            {
                foreach (var item in ruleset.Crafts)
                {
                    if (originalName == item.Name)
                    {
                        return item.SellPrice;
                    }
                }
            }
            return 0;
        }

        private static string GetSimpleName(string name)
        {
            if (string.IsNullOrEmpty(name)) name = "Without name";
            name = name.Replace("STR_", "");
            name = name.Replace("_", " ");
            return name;
        }

        private class CostAndTime
        {
            public int Time;
            public decimal Cost;
            public bool UseElerium;
            public int EleriumRequired;
        }
    }
}
