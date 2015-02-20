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

        public override string ToString()
        {
            return "ManufactureItem: " + Name;
        }
    }
}
