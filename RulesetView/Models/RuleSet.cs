using YamlDotNet.Serialization;

namespace RulesetView.Models
{
    public class RuleSet
    {
        [YamlMember(Alias = "manufacture")]
        public ManufactureItem[] ManufactureItems { get; set; }

        [YamlMember(Alias = "research")]
        public ResearchItem[] ResearchItems { get; set; }

        [YamlMember(Alias = "items")]
        public Item[] Items { get; set; }

        [YamlMember(Alias = "crafts")]
        public CraftItem[] Crafts { get; set; }
    }
}