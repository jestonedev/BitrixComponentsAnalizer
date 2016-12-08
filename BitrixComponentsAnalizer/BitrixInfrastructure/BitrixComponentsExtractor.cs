using System.Collections.Generic;
using System.Text.RegularExpressions;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public class BitrixComponentsExtractor : IComponentsExtractor
    {
        public IEnumerable<IComponent> GetComponentsFromCode(string code)
        {
            const string pattern = @"\$APPLICATION\s*->\s*IncludeComponent\s*\(\s*""(.+?)""\s*,\s*""(.+?)""[\w\W]*?;";
            const string templatePattern = @"\$APPLICATION\s*->\s*IncludeComponent[\w\W]*?""COMPONENT_TEMPLATE""\s*=\s*>\s*""(.*?)""[\w\W]*?;";
            var match = Regex.Match(code, pattern, RegexOptions.IgnoreCase);
            var components = new List<IComponent>();
            while (match.Success)
            {
                var componentString = match.Groups[0].Value;
                string componentTemplate = null;
                var componentMatch = Regex.Match(componentString, templatePattern, RegexOptions.IgnoreCase);
                if (componentMatch.Success)
                {
                    componentTemplate = componentMatch.Groups[1].Value;
                }
                var componentCategory = match.Groups[1].Value;
                var componentName = match.Groups[2].Value;
                components.Add(new BitrixComponent
                {
                    Template = componentTemplate,
                    Category = componentCategory,
                    Name = componentName
                });
                match = match.NextMatch();
            }
            return components;
        }
    }
}