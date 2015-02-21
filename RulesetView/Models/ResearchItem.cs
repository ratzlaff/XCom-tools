using YamlDotNet.Serialization;

namespace RulesetView.Models
{
    public class ResearchItem
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        public int Cost { get; set; }

        public int Points { get; set; }

        [YamlMember(Alias = "dependencies")]
        public string[] Dependencies { get; set; }

        public override string ToString()
        {
            return "ManufactureItem: " + Name;
        }
    }
}