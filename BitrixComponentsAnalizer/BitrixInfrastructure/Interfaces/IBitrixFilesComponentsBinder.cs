using System;
using System.Collections.Generic;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces
{
    public interface IBitrixFilesComponentsBinder
    {
        IEnumerable<BitrixFile> BindComponents(IEnumerable<IFile> files, Action<double, double> progressCallback);
    }
}
