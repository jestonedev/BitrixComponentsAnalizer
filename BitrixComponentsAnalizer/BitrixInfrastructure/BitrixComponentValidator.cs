using System;
using System.IO;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public class BitrixComponentValidator: IBitrixComponentValidator
    {
        private readonly IFileSystem _fileSystem;

        public BitrixComponentValidator(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            _fileSystem = fileSystem;
        }

        public bool ComponentExists(BitrixComponent component, string[] templateAbsolutePaths)
        {
            var exists = true;
            foreach (var path in templateAbsolutePaths)
            {
                if (!_fileSystem.DirectoryExists(Path.Combine(path, 
                    component.Category.Replace(":","\\"), component.Name)))
                {
                    exists = false;
                }
            }
            return exists;
        }
    }
}
