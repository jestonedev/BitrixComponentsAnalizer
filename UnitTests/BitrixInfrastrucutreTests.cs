using System;
using System.Collections.Generic;
using System.Linq;
using BitrixComponentsAnalizer.BitrixInfrastructure;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;
using BitrixComponentsAnalizer.FilesAccess.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.FilesAccess;

namespace UnitTests
{
    [TestClass]
    public class BitrixInfrastrucutreTests
    {
        [TestMethod]
        public void TestComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION->IncludeComponent(\"bitrix:news\", \"list\", array(;").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
            Assert.IsNull(components[0].Template);
        }

        [TestMethod]
        public void TestSpacesComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION -> IncludeComponent ( \"bitrix:news\", \"list\", array(\r\n"+
                "\"IBLOCK_TYPE\" => \"feedback\",;").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
            Assert.IsNull(components[0].Template);
        }

        [TestMethod]
        public void TestTwoLineComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION -> IncludeComponent\r\n "+
                "( \"bitrix:news\", \"list\", array(\r\n" +
                "\"IBLOCK_TYPE\" => \"feedback\",;").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
            Assert.IsNull(components[0].Template);
        }

        [TestMethod]
        public void TestTwoComponentsComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION -> IncludeComponent\r\n " +
                "( \"bitrix:news\", \"list\", array(\r\n" +
                "\"IBLOCK_TYPE\" => \"feedback\",;\r\n"+
                "<?$APPLICATION -> IncludeComponent\r\n " +
                "( \"bitrix:news.list\", \"any\", array(\r\n" +
                "\"IBLOCK_TYPE\" => \"feedback\",;").ToList();
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
            Assert.AreEqual("any", components[1].Name);
            Assert.AreEqual("bitrix:news.list", components[1].Category);
            Assert.IsNull(components[0].Template);
            Assert.IsNull(components[1].Template);
        }

        [TestMethod]
        public void TestComponentTemplateComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("$APPLICATION->SetTitle(\"Репортажи\");\r\n?><div class=\"hide-date-modify\"></div>\r\n<?$APPLICATION->IncludeComponent(\r\n\t\"bitrix:photogallery\",\r\n\t\".default\",\r\n\tArray(\r\n\t\t\"USE_LIGHT_VIEW\" => \"N\",\r\n\t\t\"IBLOCK_TYPE\" => \"photogallery\",\r\n\t\t\"IBLOCK_ID\" => \"58\",\r\n\t\t\"SECTION_SORT_BY\" => \"UF_DATE\",\r\n\t\t\"SECTION_SORT_ORD\" => \"DESC\",\r\n\t\t\"ELEMENT_SORT_FIELD\" => \"sort\",\r\n\t\t\"ELEMENT_SORT_ORDER\" => \"asc\",\r\n\t\t\"PATH_TO_USER\" => \"\",\r\n\t\t\"DRAG_SORT\" => \"Y\",\r\n\t\t\"USE_COMMENTS\" => \"N\",\r\n\t\t\"SEF_MODE\" => \"Y\",\r\n\t\t\"SEF_FOLDER\" => \"/city/photogallery/report/\",\r\n\t\t\"CACHE_TYPE\" => \"A\",\r\n\t\t\"CACHE_TIME\" => \"36000000\",\r\n\t\t\"DATE_TIME_FORMAT_DETAIL\" => \"d.m.Y\",\r\n\t\t\"DATE_TIME_FORMAT_SECTION\" => \"d.m.Y\",\r\n\t\t\"SET_TITLE\" => \"Y\",\r\n\t\t\"SECTION_PAGE_ELEMENTS\" => \"15\",\r\n\t\t\"ELEMENTS_PAGE_ELEMENTS\" => \"25\",\r\n\t\t\"PAGE_NAVIGATION_TEMPLATE\" => \"\",\r\n\t\t\"ALBUM_PHOTO_SIZE\" => \"100\",\r\n\t\t\"THUMBNAIL_SIZE\" => \"100\",\r\n\t\t\"JPEG_QUALITY1\" => \"95\",\r\n\t\t\"ORIGINAL_SIZE\" => \"0\",\r\n\t\t\"JPEG_QUALITY\" => \"90\",\r\n\t\t\"ADDITIONAL_SIGHTS\" => array(),\r\n\t\t\"PHOTO_LIST_MODE\" => \"Y\",\r\n\t\t\"SHOWN_ITEMS_COUNT\" => \"5\",\r\n\t\t\"SHOW_NAVIGATION\" => \"N\",\r\n\t\t\"USE_RATING\" => \"N\",\r\n\t\t\"SHOW_TAGS\" => \"N\",\r\n\t\t\"UPLOADER_TYPE\" => \"form\",\r\n\t\t\"APPLET_LAYOUT\" => \"extended\",\r\n\t\t\"UPLOAD_MAX_FILE_SIZE\" => \"1024M\",\r\n\t\t\"USE_WATERMARK\" => \"N\",\r\n\t\t\"WATERMARK_RULES\" => \"USER\",\r\n\t\t\"PATH_TO_FONT\" => \"\",\r\n\t\t\"WATERMARK_MIN_PICTURE_SIZE\" => \"200\",\r\n\t\t\"SHOW_LINK_ON_MAIN_PAGE\" => array(),\r\n\t\t\"COMPONENT_TEMPLATE\" => \".default\",\r\n\t\t\"SEF_URL_TEMPLATES\" => Array(\r\n\t\t\t\"index\" => \"index.php\",\r\n\t\t\t\"section\" => \"#SECTION_ID#/\",\r\n\t\t\t\"section_edit\" => \"#SECTION_ID#/action/#ACTION#/\",\r\n\t\t\t\"section_edit_icon\" => \"#SECTION_ID#/icon/action/#ACTION#/\",\r\n\t\t\t\"upload\" => \"#SECTION_ID#/action/upload/\",\r\n\t\t\t\"detail\" => \"#SECTION_ID#/#ELEMENT_ID#/\",\r\n\t\t\t\"detail_edit\" => \"#SECTION_ID#/#ELEMENT_ID#/action/#ACTION#/\",\r\n\t\t\t\"detail_list\" => \"list/\",\r\n\t\t\t\"search\" => \"search/\"\r\n\t\t),\r\n\t\t\"VARIABLE_ALIASES\" => Array(\r\n\t\t\t\"index\" => Array(),\r\n\t\t\t\"section\" => Array(),\r\n\t\t\t\"section_edit\" => Array(),\r\n\t\t\t\"section_edit_icon\" => Array(),\r\n\t\t\t\"upload\" => Array(),\r\n\t\t\t\"detail\" => Array(),\r\n\t\t\t\"detail_edit\" => Array(),\r\n\t\t\t\"detail_list\" => Array(),\r\n\t\t\t\"search\" => Array(),\r\n\t\t)\r\n\t)\r\n);?>").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual(".default", components[0].Name);
            Assert.AreEqual("bitrix:photogallery", components[0].Category);
            Assert.AreEqual(".default", components[0].Template);
        }

        [TestMethod]
        public void TestTwoComponentTemplateComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("$APPLICATION->SetTitle(\"Репортажи\");\r\n?><div class=\"hide-date-modify\"></div>\r\n<?$APPLICATION->IncludeComponent(\r\n\t\"bitrix:photogallery\",\r\n\t\".default\",\r\n\tArray(\r\n\t\t\"USE_LIGHT_VIEW\" => \"N\",\r\n\t\t\"IBLOCK_TYPE\" => \"photogallery\",\r\n\t\t\"IBLOCK_ID\" => \"58\",\r\n\t\t\"SECTION_SORT_BY\" => \"UF_DATE\",\r\n\t\t\"SECTION_SORT_ORD\" => \"DESC\",\r\n\t\t\"ELEMENT_SORT_FIELD\" => \"sort\",\r\n\t\t\"ELEMENT_SORT_ORDER\" => \"asc\",\r\n\t\t\"PATH_TO_USER\" => \"\",\r\n\t\t\"DRAG_SORT\" => \"Y\",\r\n\t\t\"USE_COMMENTS\" => \"N\",\r\n\t\t\"SEF_MODE\" => \"Y\",\r\n\t\t\"SEF_FOLDER\" => \"/city/photogallery/report/\",\r\n\t\t\"CACHE_TYPE\" => \"A\",\r\n\t\t\"CACHE_TIME\" => \"36000000\",\r\n\t\t\"DATE_TIME_FORMAT_DETAIL\" => \"d.m.Y\",\r\n\t\t\"DATE_TIME_FORMAT_SECTION\" => \"d.m.Y\",\r\n\t\t\"SET_TITLE\" => \"Y\",\r\n\t\t\"SECTION_PAGE_ELEMENTS\" => \"15\",\r\n\t\t\"ELEMENTS_PAGE_ELEMENTS\" => \"25\",\r\n\t\t\"PAGE_NAVIGATION_TEMPLATE\" => \"\",\r\n\t\t\"ALBUM_PHOTO_SIZE\" => \"100\",\r\n\t\t\"THUMBNAIL_SIZE\" => \"100\",\r\n\t\t\"JPEG_QUALITY1\" => \"95\",\r\n\t\t\"ORIGINAL_SIZE\" => \"0\",\r\n\t\t\"JPEG_QUALITY\" => \"90\",\r\n\t\t\"ADDITIONAL_SIGHTS\" => array(),\r\n\t\t\"PHOTO_LIST_MODE\" => \"Y\",\r\n\t\t\"SHOWN_ITEMS_COUNT\" => \"5\",\r\n\t\t\"SHOW_NAVIGATION\" => \"N\",\r\n\t\t\"USE_RATING\" => \"N\",\r\n\t\t\"SHOW_TAGS\" => \"N\",\r\n\t\t\"UPLOADER_TYPE\" => \"form\",\r\n\t\t\"APPLET_LAYOUT\" => \"extended\",\r\n\t\t\"UPLOAD_MAX_FILE_SIZE\" => \"1024M\",\r\n\t\t\"USE_WATERMARK\" => \"N\",\r\n\t\t\"WATERMARK_RULES\" => \"USER\",\r\n\t\t\"PATH_TO_FONT\" => \"\",\r\n\t\t\"WATERMARK_MIN_PICTURE_SIZE\" => \"200\",\r\n\t\t\"SHOW_LINK_ON_MAIN_PAGE\" => array(),\r\n\t\t\"COMPONENT_TEMPLATE\" => \".default\",\r\n\t\t\"SEF_URL_TEMPLATES\" => Array(\r\n\t\t\t\"index\" => \"index.php\",\r\n\t\t\t\"section\" => \"#SECTION_ID#/\",\r\n\t\t\t\"section_edit\" => \"#SECTION_ID#/action/#ACTION#/\",\r\n\t\t\t\"section_edit_icon\" => \"#SECTION_ID#/icon/action/#ACTION#/\",\r\n\t\t\t\"upload\" => \"#SECTION_ID#/action/upload/\",\r\n\t\t\t\"detail\" => \"#SECTION_ID#/#ELEMENT_ID#/\",\r\n\t\t\t\"detail_edit\" => \"#SECTION_ID#/#ELEMENT_ID#/action/#ACTION#/\",\r\n\t\t\t\"detail_list\" => \"list/\",\r\n\t\t\t\"search\" => \"search/\"\r\n\t\t),\r\n\t\t\"VARIABLE_ALIASES\" => Array(\r\n\t\t\t\"index\" => Array(),\r\n\t\t\t\"section\" => Array(),\r\n\t\t\t\"section_edit\" => Array(),\r\n\t\t\t\"section_edit_icon\" => Array(),\r\n\t\t\t\"upload\" => Array(),\r\n\t\t\t\"detail\" => Array(),\r\n\t\t\t\"detail_edit\" => Array(),\r\n\t\t\t\"detail_list\" => Array(),\r\n\t\t\t\"search\" => Array(),\r\n\t\t)\r\n\t)\r\n);?>"+
                "$APPLICATION->SetTitle(\"Репортажи\");\r\n?><div class=\"hide-date-modify\"></div>\r\n<?$APPLICATION->IncludeComponent(\r\n\t\"bitrix:photogallery2\",\r\n\t\"news\",\r\n\tArray(\r\n\t\t\"USE_LIGHT_VIEW\" => \"N\",\r\n\t\t\"IBLOCK_TYPE\" => \"photogallery\",\r\n\t\t\"IBLOCK_ID\" => \"58\",\r\n\t\t\"SECTION_SORT_BY\" => \"UF_DATE\",\r\n\t\t\"SECTION_SORT_ORD\" => \"DESC\",\r\n\t\t\"ELEMENT_SORT_FIELD\" => \"sort\",\r\n\t\t\"ELEMENT_SORT_ORDER\" => \"asc\",\r\n\t\t\"PATH_TO_USER\" => \"\",\r\n\t\t\"DRAG_SORT\" => \"Y\",\r\n\t\t\"USE_COMMENTS\" => \"N\",\r\n\t\t\"SEF_MODE\" => \"Y\",\r\n\t\t\"SEF_FOLDER\" => \"/city/photogallery/report/\",\r\n\t\t\"CACHE_TYPE\" => \"A\",\r\n\t\t\"CACHE_TIME\" => \"36000000\",\r\n\t\t\"DATE_TIME_FORMAT_DETAIL\" => \"d.m.Y\",\r\n\t\t\"DATE_TIME_FORMAT_SECTION\" => \"d.m.Y\",\r\n\t\t\"SET_TITLE\" => \"Y\",\r\n\t\t\"SECTION_PAGE_ELEMENTS\" => \"15\",\r\n\t\t\"ELEMENTS_PAGE_ELEMENTS\" => \"25\",\r\n\t\t\"PAGE_NAVIGATION_TEMPLATE\" => \"\",\r\n\t\t\"ALBUM_PHOTO_SIZE\" => \"100\",\r\n\t\t\"THUMBNAIL_SIZE\" => \"100\",\r\n\t\t\"JPEG_QUALITY1\" => \"95\",\r\n\t\t\"ORIGINAL_SIZE\" => \"0\",\r\n\t\t\"JPEG_QUALITY\" => \"90\",\r\n\t\t\"ADDITIONAL_SIGHTS\" => array(),\r\n\t\t\"PHOTO_LIST_MODE\" => \"Y\",\r\n\t\t\"SHOWN_ITEMS_COUNT\" => \"5\",\r\n\t\t\"SHOW_NAVIGATION\" => \"N\",\r\n\t\t\"USE_RATING\" => \"N\",\r\n\t\t\"SHOW_TAGS\" => \"N\",\r\n\t\t\"UPLOADER_TYPE\" => \"form\",\r\n\t\t\"APPLET_LAYOUT\" => \"extended\",\r\n\t\t\"UPLOAD_MAX_FILE_SIZE\" => \"1024M\",\r\n\t\t\"USE_WATERMARK\" => \"N\",\r\n\t\t\"WATERMARK_RULES\" => \"USER\",\r\n\t\t\"PATH_TO_FONT\" => \"\",\r\n\t\t\"WATERMARK_MIN_PICTURE_SIZE\" => \"200\",\r\n\t\t\"SHOW_LINK_ON_MAIN_PAGE\" => array(),\r\n\t\t\"COMPONENT_TEMPLATE\" => \".any\",\r\n\t\t\"SEF_URL_TEMPLATES\" => Array(\r\n\t\t\t\"index\" => \"index.php\",\r\n\t\t\t\"section\" => \"#SECTION_ID#/\",\r\n\t\t\t\"section_edit\" => \"#SECTION_ID#/action/#ACTION#/\",\r\n\t\t\t\"section_edit_icon\" => \"#SECTION_ID#/icon/action/#ACTION#/\",\r\n\t\t\t\"upload\" => \"#SECTION_ID#/action/upload/\",\r\n\t\t\t\"detail\" => \"#SECTION_ID#/#ELEMENT_ID#/\",\r\n\t\t\t\"detail_edit\" => \"#SECTION_ID#/#ELEMENT_ID#/action/#ACTION#/\",\r\n\t\t\t\"detail_list\" => \"list/\",\r\n\t\t\t\"search\" => \"search/\"\r\n\t\t),\r\n\t\t\"VARIABLE_ALIASES\" => Array(\r\n\t\t\t\"index\" => Array(),\r\n\t\t\t\"section\" => Array(),\r\n\t\t\t\"section_edit\" => Array(),\r\n\t\t\t\"section_edit_icon\" => Array(),\r\n\t\t\t\"upload\" => Array(),\r\n\t\t\t\"detail\" => Array(),\r\n\t\t\t\"detail_edit\" => Array(),\r\n\t\t\t\"detail_list\" => Array(),\r\n\t\t\t\"search\" => Array(),\r\n\t\t)\r\n\t)\r\n);?>").ToList();
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual(".default", components[0].Name);
            Assert.AreEqual("bitrix:photogallery", components[0].Category);
            Assert.AreEqual(".default", components[0].Template);
            Assert.AreEqual("news", components[1].Name);
            Assert.AreEqual("bitrix:photogallery2", components[1].Category);
            Assert.AreEqual(".any", components[1].Template);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBitrixFilesFilterNullExtractor()
        {
            var bitrixFilesFilter = new BitrixFilesComponentsBinder(null, new FakeFileFetcher());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBitrixFilesFilterNullFileManager()
        {
            var bitrixFilesFilter = new BitrixFilesComponentsBinder(new BitrixComponentsExtractor(), null);
        }

        [TestMethod]
        public void TestBitrixFilesFilter()
        {
            var fileManager = new FakeFileFetcher();
            var bitrixFilesFilter = new BitrixFilesComponentsBinder(new BitrixComponentsExtractor(), fileManager);
            fileManager.WriteTextFile("file1.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:news\", \"list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;\r\n" +
                               "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:news.list\", \"any\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;");
            fileManager.WriteTextFile("file2.php", "<?\r\n" +
                                "require($_SERVER[\"DOCUMENT_ROOT\"].\"/bitrix/header.php\");");
            fileManager.WriteTextFile("file3.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:date.picker\", \"list.list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;");
            var files = bitrixFilesFilter.BindComponents(new[]
            {
                new File
                {
                    FileName = "file1.php"
                },
                new File
                {
                    FileName = "file2.php"
                },
                new File
                {
                    FileName = "file3.php"
                }
            }, (progress, total) => { }).ToList();
            Assert.AreEqual(3, files.Count);
            Assert.AreEqual(2, files[0].Components.Count);
            Assert.AreEqual("list", files[0].Components.ToList()[0].Name);
            Assert.AreEqual("bitrix:news", files[0].Components.ToList()[0].Category);
            Assert.IsNull(files[0].Components.ToList()[0].Template);
            Assert.AreEqual("any", files[0].Components.ToList()[1].Name);
            Assert.AreEqual("bitrix:news.list", files[0].Components.ToList()[1].Category);
            Assert.IsNull(files[0].Components.ToList()[1].Template);
            Assert.AreEqual(0, files[1].Components.Count);
            Assert.AreEqual(1, files[2].Components.Count);
            Assert.AreEqual("list.list", files[2].Components.ToList()[0].Name);
            Assert.AreEqual("bitrix:date.picker", files[2].Components.ToList()[0].Category);
            Assert.IsNull(files[2].Components.ToList()[0].Template);
        }

        [TestMethod]
        public void TestComponentsBindingBitrixFilesFilter()
        {
            var fileManager = new FakeFileFetcher();
            var bitrixFilesFilter = new BitrixFilesComponentsBinder(new BitrixComponentsExtractor(), fileManager);
            fileManager.WriteTextFile("file1.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:news\", \"list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;\r\n" +
                               "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:date.picker\", \"list.list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;");
            fileManager.WriteTextFile("file3.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:date.picker\", \"list.list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;");
            var files = bitrixFilesFilter.BindComponents(new[]
            {
                new File
                {
                    FileName = "file1.php"
                },
                new File
                {
                    FileName = "file3.php"
                }
            }, (progress, total) => { }).ToList();
            Assert.AreEqual(2, files[0].Components[1].Files.Count);
            Assert.AreSame(files[0].Components[1], files[1].Components[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBitrixFilesStorageNullFileManager()
        {
            var bitrixFilesStorage = new BitrixFilesStorage("1.json", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBitrixFilesStorageNullFileName()
        {
            var bitrixFilesStorage = new BitrixFilesStorage(null, new FakeFileFetcher());
        }

        [TestMethod]
        public void TestBitrixFilesStorage()
        {
            var bitrixFilesStorage = new BitrixFilesStorage("1.json", new FakeFileFetcher());
            var bitrixFiles = new List<BitrixFile>
            {
                new BitrixFile
                {
                    FileName = "1.php",
                    Components = new List<BitrixComponent>
                    {
                        new BitrixComponent
                        {
                            Name = "list",
                            Category = "bitrix:news"
                        }
                    }.AsReadOnly()
                },
                new BitrixFile
                {
                    FileName = "2.php",
                    Components = new List<BitrixComponent>
                    {
                        new BitrixComponent
                        {
                            Name = "any",
                            Category = "bitrix:news.list"
                        }
                    }.AsReadOnly()
                }
            };
            bitrixFilesStorage.SaveFiles(bitrixFiles);
            var loadedBitrixFiles = bitrixFilesStorage.LoadFiles().ToList();
            if (loadedBitrixFiles.Count != bitrixFiles.Count)
            {
                Assert.Fail("Not equal count of files into persistent storage");
            }
            for (var i = 0; i < bitrixFiles.Count; i++)
            {
                Assert.AreEqual(bitrixFiles[0].FileName, loadedBitrixFiles[0].FileName);
                Assert.AreEqual(bitrixFiles[0].Components.Count, loadedBitrixFiles[0].Components.Count);
                Assert.AreEqual(bitrixFiles[0].Components.ToList()[0].Name, 
                    loadedBitrixFiles[0].Components.ToList()[0].Name);
                Assert.AreEqual(bitrixFiles[0].Components.ToList()[0].Category,
                    loadedBitrixFiles[0].Components.ToList()[0].Category);
                Assert.AreEqual(bitrixFiles[1].FileName, loadedBitrixFiles[1].FileName);
                Assert.AreEqual(bitrixFiles[1].Components.Count, loadedBitrixFiles[1].Components.Count);
                Assert.AreEqual(bitrixFiles[1].Components.ToList()[0].Name,
                    loadedBitrixFiles[1].Components.ToList()[0].Name);
                Assert.AreEqual(bitrixFiles[1].Components.ToList()[0].Category,
                    loadedBitrixFiles[1].Components.ToList()[0].Category);
            }
        }


        [TestMethod]
        public void TestBitrixHelper()
        {
            var fileManager = new FakeFileFetcher();
            fileManager.WriteTextFile("file1.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:news\", \"list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;\r\n" +
                               "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:news.list\", \"any\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;");
            fileManager.WriteTextFile("file2.php", "<?\r\n" +
                                "require($_SERVER[\"DOCUMENT_ROOT\"].\"/bitrix/header.php\");");
            fileManager.WriteTextFile("file3.php", "<?$APPLICATION -> IncludeComponent\r\n " +
                               "( \"bitrix:date.picker\", \"list.list\", array(\r\n" +
                               "\"IBLOCK_TYPE\" => \"feedback\",;");
            var bitrixFilesFilter = new BitrixFilesComponentsBinder(new BitrixComponentsExtractor(), fileManager);
            var files = bitrixFilesFilter.BindComponents(new[]
            {
                new File
                {
                    FileName = "file1.php"
                },
                new File
                {
                    FileName = "file3.php"
                }
            }, (progress, total) => { }).ToList();
            var components = BitrixHelper.InvertFilesAndComponentsCollection(files).ToList();
            Assert.AreEqual(3, components.Count);
            Assert.AreEqual(1, components[0].Files.Count);
            Assert.AreEqual(1, components[1].Files.Count);
            Assert.AreEqual(1, components[2].Files.Count);
            Assert.AreEqual(components[1].Files[0], components[0].Files[0]);
        }
    }
}
