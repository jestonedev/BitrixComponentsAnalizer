using System;
using System.Collections.Generic;
using System.Linq;
using BitrixComponentsAnalizer.FilesAccess;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public class BitrixFilesFilter
    {
        private readonly IComponentExtractor _componentExtractor;
        private readonly IFileManager _fileManager;

        public BitrixFilesFilter(IComponentExtractor componentExtractor, IFileManager fileManager)
        {
            if (componentExtractor == null)
            {
                throw new ArgumentNullException("componentExtractor");
            }
            if (fileManager == null)
            {
                throw new ArgumentNullException("fileManager");
            }
            _componentExtractor = componentExtractor;
            _fileManager = fileManager;
        }

        public IEnumerable<BitrixFile> FilterFiles(IEnumerable<IFile> files)
        {
            var bitrixFiles = new List<BitrixFile>();
            foreach (var file in files)
            {
                var components = _componentExtractor.GetComponentsFromCode(
                    _fileManager.LoadTextFile(file.FileName)).ToList();
                if (components.Count == 0)
                    continue;
                bitrixFiles.Add(new BitrixFile
                {
                    FileName = file.FileName,
                    Components = components.Select(v => (BitrixComponent)v).ToList().AsReadOnly()
                });
            }
            return bitrixFiles;
        }
    }
}
