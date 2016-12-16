using System.Collections.ObjectModel;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;

namespace BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects
{
    public class BitrixComponent: IComponent
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public bool IsExistsIntoSelectedTemplates { get; set; }
        public bool IsExistsIntoDefaultTemplate { get; set; }
        public bool IsExistsIntoBitrix { get; set; }

        public bool IsExistsAnyWhere
        {
            get { return IsExistsIntoSelectedTemplates || IsExistsIntoDefaultTemplate || IsExistsIntoBitrix; }
        }

        public ReadOnlyCollection<BitrixFile> Files { get; set; }
    }
}
