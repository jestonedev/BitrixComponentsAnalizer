using System;
using System.Collections.Generic;
using System.Linq;
using BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.FilesAccess.Interfaces;

namespace UnitTests.FilesAccess
{
    class FakeBitrixFilesComponentsBinder: IBitrixFilesComponentsBinder
    {
        public IEnumerable<BitrixFile> BindComponents(IEnumerable<IFile> files, Action<double, double> progressCallback)
        {
            var component1 = new BitrixComponent
            {
                Template = null,
                Category = "bitrix:news",
                Name = "news.list"
            };
            var component2 = new BitrixComponent
            {
                Template = ".default",
                Category = "bitrix:datetimepicker",
                Name = "datetimepicker.current"
            };
            var bitrixFiles = new List<BitrixFile>();
            var filesList = files.ToList();
            var total = filesList.Count-1;
            for (var i = 0; i < filesList.Count; i++)
            {
                var file = filesList[i];
                var bitrixFile = new BitrixFile
                {
                    FileName = file.FileName,
                    Components = i < 2 ? new[] { component1 }.ToList().AsReadOnly() :
                            i < 4 ? new[] { component1, component2 }.ToList().AsReadOnly() :
                            new[] { component2 }.ToList().AsReadOnly()
                };
                bitrixFiles.Add(bitrixFile);
                if (i < 4)
                {
                    component1.Files = (component1.Files ?? new List<BitrixFile>().AsReadOnly()).Concat(
                        new List<BitrixFile> {bitrixFile}
                        ).ToList().AsReadOnly();
                }
                if (i > 1)
                {
                    component2.Files = (component2.Files ?? new List<BitrixFile>().AsReadOnly()).Concat(
                        new List<BitrixFile> { bitrixFile }
                        ).ToList().AsReadOnly();
                }
                progressCallback(i, total);
            }
            return bitrixFiles;
        }
    }
}
