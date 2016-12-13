using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using Newtonsoft.Json;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public class BitrixFilesStorage: IBitrixFilesStorage
    {
        private readonly IFileSystem _fileFetcher;
        private readonly string _storeJsonFileName;

        public BitrixFilesStorage(string storeJsonFileName, IFileSystem fileFetcher)
        {
            if (fileFetcher == null)
            {
                throw new ArgumentNullException("fileFetcher");
            }
            if (storeJsonFileName == null)
            {
                throw new ArgumentNullException("storeJsonFileName");
            }
            _fileFetcher = fileFetcher;
            _storeJsonFileName = Path.IsPathRooted(storeJsonFileName) ? storeJsonFileName :
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, storeJsonFileName);
        }

        public IEnumerable<BitrixComponent> LoadComponents()
        {
            if (!_fileFetcher.FileExists(_storeJsonFileName))
                return new List<BitrixComponent>();
            return JsonConvert.DeserializeObject<IEnumerable<BitrixComponent>>
                (_fileFetcher.ReadTextFile(_storeJsonFileName)).ToList(); ;
        }

        public void SaveComponents(IEnumerable<BitrixComponent> components)
        {
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
            _fileFetcher.WriteTextFile(_storeJsonFileName, JsonConvert.SerializeObject(components, settings));
        }
    }
}
