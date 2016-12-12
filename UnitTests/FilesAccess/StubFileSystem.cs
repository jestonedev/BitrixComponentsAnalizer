using System.Collections.Generic;
using System.Linq;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace UnitTests.FilesAccess
{
    internal class StubFileSystem: IFileSystem
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
            return _files.ContainsKey(fileName);
        }

        public bool DirectoryExists(string path)
        {
            return true;
        }


        public string[] GetFiles(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            return _files.Keys.ToArray();
        }
    }
}
