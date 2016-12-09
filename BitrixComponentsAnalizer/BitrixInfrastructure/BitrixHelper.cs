using System.Collections.Generic;
using System.Linq;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public static class BitrixHelper
    {
        public static IEnumerable<BitrixComponent> InvertFilesAndComponentsCollection(IEnumerable<BitrixFile> files)
        {
            var filesWithComponents = files.Where(f => f.Components.Count > 0).ToList();
            if (filesWithComponents.Count == 0)
                return new List<BitrixComponent>();
            return filesWithComponents.Where(f => f.Components.Count > 0).
                Select(f => f.Components.AsEnumerable()).
                Aggregate((v, acc) => v.Concat(acc)).Distinct();
        }
    }
}
