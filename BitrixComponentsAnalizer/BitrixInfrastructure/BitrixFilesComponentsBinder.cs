using System;
using System.Collections.Generic;
using System.Linq;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public class BitrixFilesComponentsBinder : IBitrixFilesComponentsBinder
    {
        private readonly IComponentsExtractor _componentExtractor;
        private readonly IFileFetcher _fileManager;

        public BitrixFilesComponentsBinder(IComponentsExtractor componentExtractor, IFileFetcher fileManager)
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

        public IEnumerable<BitrixFile> BindComponents(IEnumerable<IFile> files, Action<double, double> progressCallback)
        {
            var bitrixFiles = new List<BitrixFile>();
            var componentsBuffer = new List<BitrixComponent>();
            var filesList = files.ToList();
            var totalProgress = filesList.Count - 1;
            for (var i = 0; i < filesList.Count; i++)
            {
                var file = filesList[i];
                var findedComponents = _componentExtractor.GetComponentsFromCode(
                    _fileManager.ReadTextFile(file.FileName)).ToList();
                var bitrixFile = new BitrixFile
                {
                    FileName = file.FileName
                };
                var components = new List<BitrixComponent>();
                foreach (var findedComponent in findedComponents)
                {
                    BitrixComponent component = null;
                    foreach (var bufferedComponent in componentsBuffer)
                    {
                        if (findedComponent.Category == bufferedComponent.Category &&
                            findedComponent.Name == bufferedComponent.Name &&
                            findedComponent.Template == bufferedComponent.Template)
                        {
                            component = bufferedComponent;
                        }
                    }
                    if (component == null)
                    {
                        component = new BitrixComponent
                        {
                            Category = findedComponent.Category,
                            Name = findedComponent.Name,
                            Template = findedComponent.Template
                        };
                        componentsBuffer.Add(component);
                    }
                    component.Files = component.Files == null ? 
                        new List<BitrixFile> { bitrixFile }.AsReadOnly() :
                        component.Files.Select(f => f).
                        Concat(new[] { bitrixFile }).ToList().AsReadOnly();
                    components.Add(component);
                }
                bitrixFile.Components = components.AsReadOnly();
                bitrixFiles.Add(bitrixFile);
                progressCallback(i, totalProgress);
            }
            return bitrixFiles;
        }
    }
}
