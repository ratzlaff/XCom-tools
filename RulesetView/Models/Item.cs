using YamlDotNet.Serialization;

namespace RulesetView.Models
{
    public class Item
    {
        [YamlMember(Alias = "type")]
        public string Name { get; set; }

        [YamlMember(Alias = "costSell")]
        public decimal SellPrice { get; set; }
    }
}