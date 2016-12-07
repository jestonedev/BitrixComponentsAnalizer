using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public class BitrixComponentExtractor : IComponentExtractor
    {
        public IEnumerable<IComponent> GetComponentsFromCode(string code)
        {
            const string pattern = @"\$APPLICATION\s*->\s*IncludeComponent\s*\(\s*""(.+?)""\s*,\s*""(.+?)""";
            var match = Regex.Match(code, pattern);
            var components = new List<IComponent>();
            while (match.Success)
            {
                var componentCategory = match.Groups[1].Value;
                var componentName = match.Groups[2].Value;
                components.Add(new BitrixComponent
                {
                    Category = componentCategory,
                    Name = componentName
                });
                match = match.NextMatch();
            }
            return components;
        }
    }
}
