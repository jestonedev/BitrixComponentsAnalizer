﻿using System.Collections.ObjectModel;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;

namespace BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects
{
    public class BitrixComponent: IComponent
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public bool IsExists { get; set; }
        public ReadOnlyCollection<BitrixFile> Files { get; set; }
    }
}
