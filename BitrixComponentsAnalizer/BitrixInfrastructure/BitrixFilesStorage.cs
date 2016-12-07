using System;
using System.Collections.Generic;
using System.IO;
using BitrixComponentsAnalizer.FilesAccess;
using Newtonsoft.Json;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public class BitrixFilesStorage: IBitrixFilesStorage
    {
        private readonly IFileManager _fileManager;
        private readonly string _storeJsonFileName;

        public BitrixFilesStorage(string storeJsonFileName, IFileManager fileManager)
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
                (_fileManager.LoadTextFile(_storeJsonFileName));
        }

        public void SaveFiles(IEnumerable<BitrixFile> files)
        {
            _fileManager.WriteTextFile(_storeJsonFileName, JsonConvert.SerializeObject(files));
        }
    }
}
