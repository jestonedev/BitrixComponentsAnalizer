using System;
using System.Collections.Generic;
using System.IO;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using Newtonsoft.Json;

namespace BitrixComponentsAnalizer.BitrixInfrastructure
{
    public class BitrixFilesStorage: IBitrixFilesStorage
    {
        private readonly IFileFetcher _fileFetcher;
        private readonly string _storeJsonFileName;

        public BitrixFilesStorage(string storeJsonFileName, IFileFetcher fileFetcher)
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

        public IEnumerable<BitrixFile> LoadFiles()
        {
            if (!_fileFetcher.FileExists(_storeJsonFileName))
                return new List<BitrixFile>();
            return JsonConvert.DeserializeObject<IEnumerable<BitrixFile>>
                (_fileFetcher.ReadTextFile(_storeJsonFileName));
        }

        public void SaveFiles(IEnumerable<BitrixFile> files)
        {
            _fileFetcher.WriteTextFile(_storeJsonFileName, JsonConvert.SerializeObject(files));
        }
    }
}
