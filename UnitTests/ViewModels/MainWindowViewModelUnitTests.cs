using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BitrixInfrastructure;
using UnitTests.Common;
using UnitTests.FilesAccess;

namespace UnitTests.ViewModels
{
    [TestClass]
    public class MainWindowViewModelUnitTests
    {
        [TestMethod]
        public void TestLoadStoredComponentsAsync()
        {
            var fileStorage = new StubBitrixFilesStorage();
            var bitrixComponents = BitrixHelper.GetBitrixComponentsSample();
            fileStorage.SaveComponents(bitrixComponents);
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(),
                new StubFileLoader(), fileStorage,
                new StubBitrixFilesComponentsBinder(), new StubBitrixComponentValidator(), 
                new StubLogger(), new StubSettings());  
            var components = viewModel.LoadStoredComponentsAsync().Result.ToList();
            Assert.AreEqual(3, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("any", components[1].Name);
            Assert.AreEqual("any2", components[2].Name);      
        }

        [TestMethod]
        public void TestAnalizeAsync()
        {
            // Необходимо для _context.Post
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(),
                new StubFileLoader(), new StubBitrixFilesStorage(),
                new StubBitrixFilesComponentsBinder(), new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings());
            var components = viewModel.AnalizeAsync().Result.ToList();
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual(4, components[0].Files.Count);
            Assert.AreEqual(3, components[1].Files.Count);
        }

        [TestMethod]
        public void TestAnalizeAsyncWithTemplates()
        {
            // Необходимо для _context.Post
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(),
                new StubFileLoader(), new StubBitrixFilesStorage(),
                new StubBitrixFilesComponentsBinder(), new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings())
            {
                Templates = new ObservableCollection<BitrixTemplate>(
                    new[]
                    {
                        new BitrixTemplate
                        {
                            Name = "template"
                        }
                    }),
                SelectedPath = "d:\\root\\"
            };
            var components = viewModel.AnalizeAsync().Result.ToList();
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual(true, components[0].IsExistsIntoSelectedTemplates);
            Assert.AreEqual(true, components[1].IsExistsIntoSelectedTemplates);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewModelNullFileSystem()
        {
            var viewModel = new MainWindowViewModel(
                null, new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(), 
                new StubLogger(), new StubSettings());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewModelNullBitrixFilesStorage()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                null, new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(), 
                new StubLogger(), new StubSettings());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewModelNullFileLoader()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), null,
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(), 
                new StubLogger(), new StubSettings());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestviewModelNullBitrixFilesComponentsBinder()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), null, 
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewModelNullLogger()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(), 
                null, new StubSettings());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewModelNullSettings()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), 
                new StubBitrixFilesComponentsBinder(), new StubBitrixComponentValidator(), 
                new StubLogger(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewModelNullComponentValidator()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(),
                new StubBitrixFilesComponentsBinder(), null,
                new StubLogger(), new StubSettings());
        }

        [TestMethod]
        public void TestProgressIsIndeterminate()
        {
            string propertyName = null;
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(), 
                new StubLogger(), new StubSettings());
            viewModel.PropertyChanged += (sender, args) =>
            {
                propertyName = args.PropertyName;
            };
            Assert.AreEqual(false, viewModel.AnalizeProgressIsIndeterminate);
            viewModel.AnalizeProgressIsIndeterminate = true;
            Assert.AreEqual(true, viewModel.AnalizeProgressIsIndeterminate);
            Assert.AreEqual("AnalizeProgressIsIndeterminate", propertyName);
        }

        [TestMethod]
        public void TestAnalizeProgressCurrent()
        {
            string propertyName = null;
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(), 
                new StubLogger(), new StubSettings());
            viewModel.PropertyChanged += (sender, args) =>
            {
                propertyName = args.PropertyName;
            };
            Assert.AreEqual(0, viewModel.AnalizeProgressCurrent);
            viewModel.AnalizeProgressCurrent = 10;
            Assert.AreEqual(10, viewModel.AnalizeProgressCurrent);
            Assert.AreEqual("AnalizeProgressCurrent", propertyName);
        }

        [TestMethod]
        public void TestAnalizeProgressTotal()
        {
            string propertyName = null;
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(), 
                new StubLogger(), new StubSettings());
            viewModel.PropertyChanged += (sender, args) =>
            {
                propertyName = args.PropertyName;
            };
            Assert.AreEqual(int.MaxValue, viewModel.AnalizeProgressTotal);
            viewModel.AnalizeProgressTotal = 10;
            Assert.AreEqual(10, viewModel.AnalizeProgressTotal);
            Assert.AreEqual("AnalizeProgressTotal", propertyName);
        }

        [TestMethod]
        public void TestAnalizeProgressStatusMessage()
        {
            string propertyName = null;
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), 
                new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(), 
                new StubLogger(), new StubSettings());
            viewModel.PropertyChanged += (sender, args) =>
            {
                propertyName = args.PropertyName;
            };
            Assert.AreEqual("Прогресс", viewModel.AnalizeProgressStatusMessage);
            viewModel.AnalizeProgressStatusMessage = "Загрузка файлов";
            Assert.AreEqual("Загрузка файлов", viewModel.AnalizeProgressStatusMessage);
            Assert.AreEqual("AnalizeProgressStatusMessage", propertyName);
        }

        [TestMethod]
        public void TestSelectedPath()
        {
            string propertyName = null;
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(), 
                new StubLogger(), new StubSettings());
            viewModel.PropertyChanged += (sender, args) =>
            {
                propertyName = args.PropertyName;
            };
            Assert.AreEqual(null, viewModel.SelectedPath);
            viewModel.SelectedPath = "D:\\www.some-site.com\\";
            Assert.AreEqual("D:\\www.some-site.com\\", viewModel.SelectedPath);
            Assert.AreEqual("SelectedPath", propertyName);
        }

        [TestMethod]
        public void TestFilter()
        {
            string propertyFilterName = null;
            string propertyFilteredComponentsName = null;
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), 
                new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(), 
                new StubLogger(), new StubSettings());
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Filter")
                    propertyFilterName = args.PropertyName;
                if (args.PropertyName == "FilteredComponents")
                    propertyFilteredComponentsName = args.PropertyName;
            };
            Assert.AreEqual(null, viewModel.Filter);
            viewModel.Filter = "any filter";
            Assert.AreEqual("any filter", viewModel.Filter);
            Assert.AreEqual("Filter", propertyFilterName);
            Assert.AreEqual("FilteredComponents", propertyFilteredComponentsName);
        }

        [TestMethod]
        public void TestFilterComponentsByName()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), 
                new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(), 
                new StubLogger(), new StubSettings())
            {
                Components = new ObservableCollection<BitrixComponent>(new List<BitrixComponent>
                {
                    new BitrixComponent
                    {
                        Name = "Name1"
                    },
                    new BitrixComponent
                    {
                        Name = "Name2"
                    }
                }),
                Filter = "Name1"
            };
            Assert.AreEqual(1, viewModel.FilteredComponents.Count);
            Assert.AreEqual("Name1", viewModel.FilteredComponents[0].Name);
        }

        [TestMethod]
        public void TestFilterComponentsByCategory()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(), 
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings())
            {
                Components = new ObservableCollection<BitrixComponent>(new List<BitrixComponent>
                {
                    new BitrixComponent
                    {
                        Category = "Category1"
                    },
                    new BitrixComponent
                    {
                        Category = "Category2"
                    }
                }),
                Filter = "Category1"
            };
            Assert.AreEqual(1, viewModel.FilteredComponents.Count);
            Assert.AreEqual("Category1", viewModel.FilteredComponents[0].Category);
        }

        [TestMethod]
        public void TestFilterComponentsByFileName()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings())
            {
                Components = new ObservableCollection<BitrixComponent>(new List<BitrixComponent>
                {
                    new BitrixComponent
                    {
                        Name = "Name1",
                        Files = new List<BitrixFile>
                        {
                            new BitrixFile
                            {
                                FileName = "FileName1"
                            },
                            new BitrixFile
                            {
                                FileName = "FileName2"
                            }
                        }.AsReadOnly()
                    },
                    new BitrixComponent
                    {
                        Name = "Name2",
                        Files = new List<BitrixFile>
                        {
                            new BitrixFile()
                        }.AsReadOnly()
                    }
                }),
                Filter = "FileName2"
            };
            Assert.AreEqual(1, viewModel.FilteredComponents.Count);
            Assert.AreEqual(2, viewModel.FilteredComponents[0].Files.Count);
            Assert.AreEqual("FileName1", viewModel.FilteredComponents[0].Files[0].FileName);
            Assert.AreEqual("FileName2", viewModel.FilteredComponents[0].Files[1].FileName);
        }

        [TestMethod]
        public void TestFilterComponentsEmptyFilter()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings())
            {
                Components = new ObservableCollection<BitrixComponent>(new List<BitrixComponent>
                {
                    new BitrixComponent
                    {
                        Name = "Name1",
                        Files = new List<BitrixFile>
                        {
                            new BitrixFile
                            {
                                FileName = "FileName1"
                            }
                        }.AsReadOnly()
                    },
                    new BitrixComponent
                    {
                        Name = "Name2",
                        Files = new List<BitrixFile>
                        {
                            new BitrixFile()
                        }.AsReadOnly()
                    }
                }),
                Filter = ""
            };
            Assert.AreEqual(2, viewModel.FilteredComponents.Count);
            Assert.AreEqual(1, viewModel.FilteredComponents[0].Files.Count);
            Assert.AreEqual("FileName1", viewModel.FilteredComponents[0].Files[0].FileName);
            Assert.AreEqual("Name1", viewModel.FilteredComponents[0].Name);
            Assert.AreEqual("Name2", viewModel.FilteredComponents[1].Name);
        }

        [TestMethod]
        public void TestComponents()
        {
            string propertyComponentsName = null;
            string propertyFilteredComponentsName = null;
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings());
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Components")
                    propertyComponentsName = args.PropertyName;
                if (args.PropertyName == "FilteredComponents")
                    propertyFilteredComponentsName = args.PropertyName;

            };
            Assert.AreEqual(0, viewModel.Components.Count);
            viewModel.Components = new ObservableCollection<BitrixComponent>(new[]
            {
                new BitrixComponent
                {
                    Name = "Test"
                }
            });
            Assert.AreEqual(1, viewModel.Components.Count);
            Assert.AreEqual("Test", viewModel.Components[0].Name);
            Assert.AreEqual("Components", propertyComponentsName);
            Assert.AreEqual("FilteredComponents", propertyFilteredComponentsName);
        }

        [TestMethod]
        public void TestComponentsAddItem()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings())
            {
                Components = new ObservableCollection<BitrixComponent>(new[]
                {
                    new BitrixComponent
                    {
                        Name = "Test"
                    }
                })
            };
            string propertyComponentsName = null;
            string propertyFilteredComponentsName = null;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Components")
                    propertyComponentsName = args.PropertyName;
                if (args.PropertyName == "FilteredComponents")
                    propertyFilteredComponentsName = args.PropertyName;

            };
            viewModel.Components.Add(new BitrixComponent
            {
                Name = "NewComponent"
            });
            Assert.AreEqual(2, viewModel.Components.Count);
            Assert.AreEqual("Test", viewModel.Components[0].Name);
            Assert.AreEqual("NewComponent", viewModel.Components[1].Name);
            Assert.AreEqual("Components", propertyComponentsName);
            Assert.AreEqual("FilteredComponents", propertyFilteredComponentsName);
        }

        [TestMethod]
        public void TestTemplates()
        {
            string propertyName = null;
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings());
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Templates")
                    propertyName = args.PropertyName;

            };
            Assert.AreEqual(0, viewModel.Templates.Count);
            viewModel.Templates = new ObservableCollection<BitrixTemplate>(new []
            {
                new BitrixTemplate
                {
                    Name = "any"
                }, 
                new BitrixTemplate
                {
                    Name = "template"
                }
            });
            Assert.AreEqual(2, viewModel.Templates.Count);
            Assert.AreEqual("any", viewModel.Templates[0].Name);
            Assert.AreEqual("template", viewModel.Templates[1].Name);
            Assert.AreEqual("Templates", propertyName);
        }

        [TestMethod]
        public void TestTemplatesAddItem()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings())
            {
                Templates = new ObservableCollection<BitrixTemplate>(new[]
                {
                    new BitrixTemplate
                    {
                        Name = "any"
                    },
                    new BitrixTemplate
                    {
                        Name = "template"
                    }
                })
            };
            string propertyName = null;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Templates")
                    propertyName = args.PropertyName;

            };
            viewModel.Templates.Add(new BitrixTemplate
            {
                Name = "newTemplate"
            });
            Assert.AreEqual(3, viewModel.Templates.Count);
            Assert.AreEqual("any", viewModel.Templates[0].Name);
            Assert.AreEqual("template", viewModel.Templates[1].Name);
            Assert.AreEqual("newTemplate", viewModel.Templates[2].Name);
            Assert.AreEqual("Templates", propertyName);
        }

        [TestMethod]
        public void TestAnalizeCommand()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings());
            var command = viewModel.AnalizeCommand;
            Assert.AreSame(command, viewModel.AnalizeCommand);
        }

        [TestMethod]
        public void TestSelectPathCommand()
        {
            var viewModel = new MainWindowViewModel(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings());
            var command = viewModel.SelectPathCommand;
            Assert.AreSame(command, viewModel.SelectPathCommand);
        }

        [TestMethod]
        public void TestAnalizeCommandComplete()
        {
            string propertyComponentsName = null;
            string propertyFilteredComponentsName = null;
            var viewModel = new MainWindowViewModelProxy(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings());
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Components")
                    propertyComponentsName = args.PropertyName;
                if (args.PropertyName == "FilteredComponents")
                    propertyFilteredComponentsName = args.PropertyName;
            };
            viewModel.AnalizeCommandComplete(new List<BitrixComponent>
            {
                new BitrixComponent
                {
                    Name = "Test"
                }
            });
            Assert.AreEqual("Components", propertyComponentsName);
            Assert.AreEqual("FilteredComponents", propertyFilteredComponentsName);
            Assert.AreEqual(1, viewModel.Components.Count);
            Assert.AreEqual("Test", viewModel.Components[0].Name);
        }

        [TestMethod]
        public void TestSelectPathCommandComplete()
        {
            string propertyName = null;
            var viewModel = new MainWindowViewModelProxy(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings());
            viewModel.PropertyChanged += (sender, args) =>
            {
                propertyName = args.PropertyName;
            };
            viewModel.SelectPathCommandComplete("any path");
            Assert.AreEqual("SelectedPath", propertyName);
            Assert.AreEqual("any path", viewModel.SelectedPath);
        }

        [TestMethod]
        public void TestAnalizeCommandDirectoryNotFoundError()
        {
            string message = null;
            var logger = new StubLogger();
            logger.Logging += m =>
            {
                message = m;
            };
            var viewModel = new MainWindowViewModelProxy(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                logger, new StubSettings());
            viewModel.AnalizeCommandError(new DirectoryNotFoundException("Directory not found"));
            Assert.AreEqual("Directory not found", message);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAnalizeCommandError()
        {
            var viewModel = new MainWindowViewModelProxy(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), new StubSettings());
            viewModel.AnalizeCommandError(new ApplicationException());
        }

        [TestMethod]
        public void TestAnalizeCommandInnerExceptions()
        {
            string message = null;
            var logger = new StubLogger();
            logger.Logging += m =>
            {
                message = m;
            };
            var viewModel = new MainWindowViewModelProxy(
                new StubFileSystem(), new StubFileLoader(),
                new StubBitrixFilesStorage(), new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                logger, new StubSettings());
            viewModel.AnalizeCommandError(
                new TaskCanceledException("Cancel exception", 
                    new DirectoryNotFoundException("Directory not found")));
            Assert.AreEqual("Directory not found", message);
        }

        [TestMethod]
        public void TestSaveLoadState()
        {
            var settings = new StubSettings();
            var storage = new StubBitrixFilesStorage();
            var viewModel = new MainWindowViewModelProxy(
                new StubFileSystem(), new StubFileLoader(),
                storage, new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), settings)
            {
                Templates = new ObservableCollection<BitrixTemplate>(
                    new[]
                    {
                        new BitrixTemplate
                        {
                            Name = "Template1"
                        }
                    }),
                Components = new ObservableCollection<BitrixComponent>(
                    new[]
                    {
                        new BitrixComponent
                        {
                            Name = "ComponentName",
                            Category = "ComponentCategory",
                            IsExistsIntoSelectedTemplates = false,
                        },
                        new BitrixComponent
                        {
                            Name = "ComponentName2",
                            Category = "ComponentCategory2",
                            IsExistsIntoSelectedTemplates = true,
                            Files = new ReadOnlyCollection<BitrixFile>(
                                new List<BitrixFile>
                                {
                                    new BitrixFile
                                    {
                                        FileName = "Test"
                                    }
                                })
                        }
                    }),
                SelectedPath = "d:\\any\\dir\\"
            };
            viewModel.SaveState();
            var anyViewModel = new MainWindowViewModelProxy(
                new StubFileSystem(), new StubFileLoader(),
                storage, new StubBitrixFilesComponentsBinder(),
                new StubBitrixComponentValidator(),
                new StubLogger(), settings);
            var ev = new AutoResetEvent(false);
            anyViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Components")
                {
                    ev.Set();
                }
            };
            anyViewModel.LoadState();
            Assert.IsTrue(ev.WaitOne());
            Assert.AreEqual(viewModel.SelectedPath, anyViewModel.SelectedPath);
            Assert.AreEqual(viewModel.Templates.Count, anyViewModel.Templates.Count);
            for (var i = 0; i < viewModel.Templates.Count; i++)
            {
                Assert.AreEqual(viewModel.Templates[i].Name, anyViewModel.Templates[i].Name);
            }
            Assert.AreEqual(viewModel.Components.Count, anyViewModel.Components.Count);
            for (var i = 0; i < viewModel.Components.Count; i++)
            {
                Assert.AreEqual(viewModel.Components[i].Name, anyViewModel.Components[i].Name);
                Assert.AreEqual(viewModel.Components[i].Category, anyViewModel.Components[i].Category);
                Assert.AreEqual(viewModel.Components[i].IsExistsIntoSelectedTemplates, 
                    anyViewModel.Components[i].IsExistsIntoSelectedTemplates);
                Assert.AreEqual(
                    viewModel.Components[i].Files != null ?
                    viewModel.Components[i].Files.Count : 0,
                    anyViewModel.Components[i].Files != null ?
                    anyViewModel.Components[i].Files.Count : 0);
            }
        }

    }
}
