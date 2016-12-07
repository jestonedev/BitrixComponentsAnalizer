using System;
using System.Collections.Generic;
using BitrixComponentsAnalizer.FilesAccess;

namespace UnitTests.FilesAccess
{
    internal class FakeFileManager: IFileManager
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

        public string LoadTextFile(string fileName)
        {
            return _files.ContainsKey(fileName) ? _files[fileName] : null;
        }
    }
}
