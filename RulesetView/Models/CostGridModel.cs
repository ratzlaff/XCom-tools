using System;
using System.Linq;
using System.Text;
using System.Diagnostics; 

namespace RulesetView.Models
{
    public class CostGridModel : ModelBase
    {
        public string OriginalName;
        public ManufactureItem DataModel;

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public decimal Cost
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }
        public decimal DependencyCost
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }
        public int DependencyTime
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }
        public decimal? TotalCost
        {
            get { return GetValue<decimal?>(); }
            set { SetValue(value); }
        }
        public decimal TimeCost
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }
        public bool RequiresElerium
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }
        public int EleriumRequired
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public decimal Time
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public decimal SellPrice
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public decimal ProfitWithoutDepdendencyPerHour 
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public decimal ProfitPerHour
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }
    }
}
