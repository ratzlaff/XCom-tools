using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace RulesetView.Models
{
    public class ManufactureItem
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "requires")]
        public string[] Requirements { get; set; }

        [YamlMember(Alias = "requiredItems")]
        public Dictionary<string,decimal> RequiredItems { get; set; }

        [YamlMember(Alias = "cost")]
        public decimal Cost { get; set; }

        [YamlMember(Alias = "time")]
        public int Time { get; set; }

        public override string ToString()
        {
            return "ManufactureItem: " + Name;
        }
    }
}
