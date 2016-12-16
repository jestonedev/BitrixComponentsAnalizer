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

        public bool ComponentExists(BitrixComponent component, string[] templateAbsolutePaths, bool skipDefaultComponentName)
        {
            var existsIntoSelectedTempaltes = true;
            var name = component.Name;
            if (skipDefaultComponentName && (name == ".default" || string.IsNullOrEmpty(name)))
            {
                name = "";
            } else
            if (string.IsNullOrEmpty(name))
            {
                name = ".default";
            }
            var category = component.Category.Replace(":", "\\");
            foreach (var path in templateAbsolutePaths)
            {   
                if (!_fileSystem.DirectoryExists(Path.Combine(path, category, name)))
                {
                    existsIntoSelectedTempaltes = false;
                }
            }
            return existsIntoSelectedTempaltes;
        }
    }
}
