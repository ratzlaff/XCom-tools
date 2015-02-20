using YamlDotNet.Serialization;

namespace RulesetView.Models
{
    public class ResearchItem
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        public string Cost { get; set; }

        public string Points { get; set; }

        [YamlMember(Alias = "dependencies")]
        public string[] Dependencies { get; set; }

        public override string ToString()
        {
            return "ManufactureItem: " + Name;
        }
    }
}