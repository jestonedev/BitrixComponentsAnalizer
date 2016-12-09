using System.Collections.Generic;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace UnitTests.FilesAccess
{
    internal class FakeFileFetcher: IFileFetcher
    {
        private readonly Dictionary<string, string> _files = new Dictionary<string, string>();

        public void WriteTextFile(string fileName, string text)
        {
            if (_files.ContainsKey(fileName))
            {
                _files[fileName] = text;
            }
            else
            {
                _files.Add(fileName, text);
            }
        }

        public string ReadTextFile(string fileName)
        {
            return _files.ContainsKey(fileName) ? _files[fileName] : null;
        }


        public bool FileExists(string fileName)
        {
            return true;
        }
    }
}
