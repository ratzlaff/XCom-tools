using YamlDotNet.Serialization;

namespace RulesetView.Models
{
    public class CraftItem
    {
        [YamlMember(Alias = "type")]
        public string Name { get; set; }

        [YamlMember(Alias = "costSell")]
        public decimal SellPrice { get; set; }

        public override string ToString()
        {
            return "Craft: " + Name;
        }

    }
}