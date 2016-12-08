using System;
using System.Collections.Generic;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using Newtonsoft.Json;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public class BitrixFilesStorage: IBitrixFilesStorage
    {
        private readonly IFileFetcher _fileManager;
        private readonly string _storeJsonFileName;

        public BitrixFilesStorage(string storeJsonFileName, IFileFetcher fileManager)
        {
            if (fileManager == null)
            {
                throw new ArgumentNullException("fileManager");
            }
            if (storeJsonFileName == null)
            {
                throw new ArgumentNullException("storeJsonFileName");
            }
            _fileManager = fileManager;
            _storeJsonFileName = storeJsonFileName;
        }

        public IEnumerable<BitrixFile> LoadFiles()
        {
            return JsonConvert.DeserializeObject<IEnumerable<BitrixFile>>
                (_fileManager.ReadTextFile(_storeJsonFileName));
        }

        public void SaveFiles(IEnumerable<BitrixFile> files)
        {
            _fileManager.WriteTextFile(_storeJsonFileName, JsonConvert.SerializeObject(files));
        }
    }
}
