using System.Linq;
using BitrixComponentsAnalizer.BitrixInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.BitrixInfrastructure
{
    [TestClass]
    public class BitrixComponentsExtractorTests
    {
        [TestMethod]
        public void TestComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION->IncludeComponent(\"bitrix:news\", \"list\", array(;").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
        }

        [TestMethod]
        public void TestSpacesComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION -> IncludeComponent ( \"bitrix:news\", \"list\", array(\r\n" +
                "\"IBLOCK_TYPE\" => \"feedback\",;").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
        }

        [TestMethod]
        public void TestSingleQuotesComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("<?\r\n$APPLICATION->IncludeComponent(\"bitrix:im.messenger\", \"\", Array(\"DESKTOP\" => \"Y\"), false, Array(\"HIDE_ICONS\" => \"Y\"));").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("bitrix:im.messenger", components[0].Category);
            Assert.AreEqual("", components[0].Name);
        }

        [TestMethod]
        public void TestTwoLineComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION -> IncludeComponent\r\n " +
                "( \"bitrix:news\", \"list\", array(\r\n" +
                "\"IBLOCK_TYPE\" => \"feedback\",;").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
        }

        [TestMethod]
        public void TestTwoComponentsComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("<?$APPLICATION -> IncludeComponent\r\n " +
                "( \"bitrix:news\", \"list\", array(\r\n" +
                "\"IBLOCK_TYPE\" => \"feedback\",;\r\n" +
                "<?$APPLICATION -> IncludeComponent\r\n " +
                "( \"bitrix:news.list\", \"any\", array(\r\n" +
                "\"IBLOCK_TYPE\" => \"feedback\",;").ToList();
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual("list", components[0].Name);
            Assert.AreEqual("bitrix:news", components[0].Category);
            Assert.AreEqual("any", components[1].Name);
            Assert.AreEqual("bitrix:news.list", components[1].Category);
        }

        [TestMethod]
        public void TestComponentTemplateComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("$APPLICATION->SetTitle(\"Репортажи\");\r\n?><div class=\"hide-date-modify\"></div>\r\n<?$APPLICATION->IncludeComponent(\r\n\t\"bitrix:photogallery\",\r\n\t\".default\",\r\n\tArray(\r\n\t\t\"USE_LIGHT_VIEW\" => \"N\",\r\n\t\t\"IBLOCK_TYPE\" => \"photogallery\",\r\n\t\t\"IBLOCK_ID\" => \"58\",\r\n\t\t\"SECTION_SORT_BY\" => \"UF_DATE\",\r\n\t\t\"SECTION_SORT_ORD\" => \"DESC\",\r\n\t\t\"ELEMENT_SORT_FIELD\" => \"sort\",\r\n\t\t\"ELEMENT_SORT_ORDER\" => \"asc\",\r\n\t\t\"PATH_TO_USER\" => \"\",\r\n\t\t\"DRAG_SORT\" => \"Y\",\r\n\t\t\"USE_COMMENTS\" => \"N\",\r\n\t\t\"SEF_MODE\" => \"Y\",\r\n\t\t\"SEF_FOLDER\" => \"/city/photogallery/report/\",\r\n\t\t\"CACHE_TYPE\" => \"A\",\r\n\t\t\"CACHE_TIME\" => \"36000000\",\r\n\t\t\"DATE_TIME_FORMAT_DETAIL\" => \"d.m.Y\",\r\n\t\t\"DATE_TIME_FORMAT_SECTION\" => \"d.m.Y\",\r\n\t\t\"SET_TITLE\" => \"Y\",\r\n\t\t\"SECTION_PAGE_ELEMENTS\" => \"15\",\r\n\t\t\"ELEMENTS_PAGE_ELEMENTS\" => \"25\",\r\n\t\t\"PAGE_NAVIGATION_TEMPLATE\" => \"\",\r\n\t\t\"ALBUM_PHOTO_SIZE\" => \"100\",\r\n\t\t\"THUMBNAIL_SIZE\" => \"100\",\r\n\t\t\"JPEG_QUALITY1\" => \"95\",\r\n\t\t\"ORIGINAL_SIZE\" => \"0\",\r\n\t\t\"JPEG_QUALITY\" => \"90\",\r\n\t\t\"ADDITIONAL_SIGHTS\" => array(),\r\n\t\t\"PHOTO_LIST_MODE\" => \"Y\",\r\n\t\t\"SHOWN_ITEMS_COUNT\" => \"5\",\r\n\t\t\"SHOW_NAVIGATION\" => \"N\",\r\n\t\t\"USE_RATING\" => \"N\",\r\n\t\t\"SHOW_TAGS\" => \"N\",\r\n\t\t\"UPLOADER_TYPE\" => \"form\",\r\n\t\t\"APPLET_LAYOUT\" => \"extended\",\r\n\t\t\"UPLOAD_MAX_FILE_SIZE\" => \"1024M\",\r\n\t\t\"USE_WATERMARK\" => \"N\",\r\n\t\t\"WATERMARK_RULES\" => \"USER\",\r\n\t\t\"PATH_TO_FONT\" => \"\",\r\n\t\t\"WATERMARK_MIN_PICTURE_SIZE\" => \"200\",\r\n\t\t\"SHOW_LINK_ON_MAIN_PAGE\" => array(),\r\n\t\t\"COMPONENT_TEMPLATE\" => \".default\",\r\n\t\t\"SEF_URL_TEMPLATES\" => Array(\r\n\t\t\t\"index\" => \"index.php\",\r\n\t\t\t\"section\" => \"#SECTION_ID#/\",\r\n\t\t\t\"section_edit\" => \"#SECTION_ID#/action/#ACTION#/\",\r\n\t\t\t\"section_edit_icon\" => \"#SECTION_ID#/icon/action/#ACTION#/\",\r\n\t\t\t\"upload\" => \"#SECTION_ID#/action/upload/\",\r\n\t\t\t\"detail\" => \"#SECTION_ID#/#ELEMENT_ID#/\",\r\n\t\t\t\"detail_edit\" => \"#SECTION_ID#/#ELEMENT_ID#/action/#ACTION#/\",\r\n\t\t\t\"detail_list\" => \"list/\",\r\n\t\t\t\"search\" => \"search/\"\r\n\t\t),\r\n\t\t\"VARIABLE_ALIASES\" => Array(\r\n\t\t\t\"index\" => Array(),\r\n\t\t\t\"section\" => Array(),\r\n\t\t\t\"section_edit\" => Array(),\r\n\t\t\t\"section_edit_icon\" => Array(),\r\n\t\t\t\"upload\" => Array(),\r\n\t\t\t\"detail\" => Array(),\r\n\t\t\t\"detail_edit\" => Array(),\r\n\t\t\t\"detail_list\" => Array(),\r\n\t\t\t\"search\" => Array(),\r\n\t\t)\r\n\t)\r\n);?>").ToList();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual(".default", components[0].Name);
            Assert.AreEqual("bitrix:photogallery", components[0].Category);
        }

        [TestMethod]
        public void TestTwoComponentTemplateComponentExtractor()
        {
            var extractor = new BitrixComponentsExtractor();
            var components = extractor.GetComponentsFromCode("$APPLICATION->SetTitle(\"Репортажи\");\r\n?><div class=\"hide-date-modify\"></div>\r\n<?$APPLICATION->IncludeComponent(\r\n\t\"bitrix:photogallery\",\r\n\t\".default\",\r\n\tArray(\r\n\t\t\"USE_LIGHT_VIEW\" => \"N\",\r\n\t\t\"IBLOCK_TYPE\" => \"photogallery\",\r\n\t\t\"IBLOCK_ID\" => \"58\",\r\n\t\t\"SECTION_SORT_BY\" => \"UF_DATE\",\r\n\t\t\"SECTION_SORT_ORD\" => \"DESC\",\r\n\t\t\"ELEMENT_SORT_FIELD\" => \"sort\",\r\n\t\t\"ELEMENT_SORT_ORDER\" => \"asc\",\r\n\t\t\"PATH_TO_USER\" => \"\",\r\n\t\t\"DRAG_SORT\" => \"Y\",\r\n\t\t\"USE_COMMENTS\" => \"N\",\r\n\t\t\"SEF_MODE\" => \"Y\",\r\n\t\t\"SEF_FOLDER\" => \"/city/photogallery/report/\",\r\n\t\t\"CACHE_TYPE\" => \"A\",\r\n\t\t\"CACHE_TIME\" => \"36000000\",\r\n\t\t\"DATE_TIME_FORMAT_DETAIL\" => \"d.m.Y\",\r\n\t\t\"DATE_TIME_FORMAT_SECTION\" => \"d.m.Y\",\r\n\t\t\"SET_TITLE\" => \"Y\",\r\n\t\t\"SECTION_PAGE_ELEMENTS\" => \"15\",\r\n\t\t\"ELEMENTS_PAGE_ELEMENTS\" => \"25\",\r\n\t\t\"PAGE_NAVIGATION_TEMPLATE\" => \"\",\r\n\t\t\"ALBUM_PHOTO_SIZE\" => \"100\",\r\n\t\t\"THUMBNAIL_SIZE\" => \"100\",\r\n\t\t\"JPEG_QUALITY1\" => \"95\",\r\n\t\t\"ORIGINAL_SIZE\" => \"0\",\r\n\t\t\"JPEG_QUALITY\" => \"90\",\r\n\t\t\"ADDITIONAL_SIGHTS\" => array(),\r\n\t\t\"PHOTO_LIST_MODE\" => \"Y\",\r\n\t\t\"SHOWN_ITEMS_COUNT\" => \"5\",\r\n\t\t\"SHOW_NAVIGATION\" => \"N\",\r\n\t\t\"USE_RATING\" => \"N\",\r\n\t\t\"SHOW_TAGS\" => \"N\",\r\n\t\t\"UPLOADER_TYPE\" => \"form\",\r\n\t\t\"APPLET_LAYOUT\" => \"extended\",\r\n\t\t\"UPLOAD_MAX_FILE_SIZE\" => \"1024M\",\r\n\t\t\"USE_WATERMARK\" => \"N\",\r\n\t\t\"WATERMARK_RULES\" => \"USER\",\r\n\t\t\"PATH_TO_FONT\" => \"\",\r\n\t\t\"WATERMARK_MIN_PICTURE_SIZE\" => \"200\",\r\n\t\t\"SHOW_LINK_ON_MAIN_PAGE\" => array(),\r\n\t\t\"COMPONENT_TEMPLATE\" => \".default\",\r\n\t\t\"SEF_URL_TEMPLATES\" => Array(\r\n\t\t\t\"index\" => \"index.php\",\r\n\t\t\t\"section\" => \"#SECTION_ID#/\",\r\n\t\t\t\"section_edit\" => \"#SECTION_ID#/action/#ACTION#/\",\r\n\t\t\t\"section_edit_icon\" => \"#SECTION_ID#/icon/action/#ACTION#/\",\r\n\t\t\t\"upload\" => \"#SECTION_ID#/action/upload/\",\r\n\t\t\t\"detail\" => \"#SECTION_ID#/#ELEMENT_ID#/\",\r\n\t\t\t\"detail_edit\" => \"#SECTION_ID#/#ELEMENT_ID#/action/#ACTION#/\",\r\n\t\t\t\"detail_list\" => \"list/\",\r\n\t\t\t\"search\" => \"search/\"\r\n\t\t),\r\n\t\t\"VARIABLE_ALIASES\" => Array(\r\n\t\t\t\"index\" => Array(),\r\n\t\t\t\"section\" => Array(),\r\n\t\t\t\"section_edit\" => Array(),\r\n\t\t\t\"section_edit_icon\" => Array(),\r\n\t\t\t\"upload\" => Array(),\r\n\t\t\t\"detail\" => Array(),\r\n\t\t\t\"detail_edit\" => Array(),\r\n\t\t\t\"detail_list\" => Array(),\r\n\t\t\t\"search\" => Array(),\r\n\t\t)\r\n\t)\r\n);?>" +
                "$APPLICATION->SetTitle(\"Репортажи\");\r\n?><div class=\"hide-date-modify\"></div>\r\n<?$APPLICATION->IncludeComponent(\r\n\t\"bitrix:photogallery2\",\r\n\t\"news\",\r\n\tArray(\r\n\t\t\"USE_LIGHT_VIEW\" => \"N\",\r\n\t\t\"IBLOCK_TYPE\" => \"photogallery\",\r\n\t\t\"IBLOCK_ID\" => \"58\",\r\n\t\t\"SECTION_SORT_BY\" => \"UF_DATE\",\r\n\t\t\"SECTION_SORT_ORD\" => \"DESC\",\r\n\t\t\"ELEMENT_SORT_FIELD\" => \"sort\",\r\n\t\t\"ELEMENT_SORT_ORDER\" => \"asc\",\r\n\t\t\"PATH_TO_USER\" => \"\",\r\n\t\t\"DRAG_SORT\" => \"Y\",\r\n\t\t\"USE_COMMENTS\" => \"N\",\r\n\t\t\"SEF_MODE\" => \"Y\",\r\n\t\t\"SEF_FOLDER\" => \"/city/photogallery/report/\",\r\n\t\t\"CACHE_TYPE\" => \"A\",\r\n\t\t\"CACHE_TIME\" => \"36000000\",\r\n\t\t\"DATE_TIME_FORMAT_DETAIL\" => \"d.m.Y\",\r\n\t\t\"DATE_TIME_FORMAT_SECTION\" => \"d.m.Y\",\r\n\t\t\"SET_TITLE\" => \"Y\",\r\n\t\t\"SECTION_PAGE_ELEMENTS\" => \"15\",\r\n\t\t\"ELEMENTS_PAGE_ELEMENTS\" => \"25\",\r\n\t\t\"PAGE_NAVIGATION_TEMPLATE\" => \"\",\r\n\t\t\"ALBUM_PHOTO_SIZE\" => \"100\",\r\n\t\t\"THUMBNAIL_SIZE\" => \"100\",\r\n\t\t\"JPEG_QUALITY1\" => \"95\",\r\n\t\t\"ORIGINAL_SIZE\" => \"0\",\r\n\t\t\"JPEG_QUALITY\" => \"90\",\r\n\t\t\"ADDITIONAL_SIGHTS\" => array(),\r\n\t\t\"PHOTO_LIST_MODE\" => \"Y\",\r\n\t\t\"SHOWN_ITEMS_COUNT\" => \"5\",\r\n\t\t\"SHOW_NAVIGATION\" => \"N\",\r\n\t\t\"USE_RATING\" => \"N\",\r\n\t\t\"SHOW_TAGS\" => \"N\",\r\n\t\t\"UPLOADER_TYPE\" => \"form\",\r\n\t\t\"APPLET_LAYOUT\" => \"extended\",\r\n\t\t\"UPLOAD_MAX_FILE_SIZE\" => \"1024M\",\r\n\t\t\"USE_WATERMARK\" => \"N\",\r\n\t\t\"WATERMARK_RULES\" => \"USER\",\r\n\t\t\"PATH_TO_FONT\" => \"\",\r\n\t\t\"WATERMARK_MIN_PICTURE_SIZE\" => \"200\",\r\n\t\t\"SHOW_LINK_ON_MAIN_PAGE\" => array(),\r\n\t\t\"COMPONENT_TEMPLATE\" => \".any\",\r\n\t\t\"SEF_URL_TEMPLATES\" => Array(\r\n\t\t\t\"index\" => \"index.php\",\r\n\t\t\t\"section\" => \"#SECTION_ID#/\",\r\n\t\t\t\"section_edit\" => \"#SECTION_ID#/action/#ACTION#/\",\r\n\t\t\t\"section_edit_icon\" => \"#SECTION_ID#/icon/action/#ACTION#/\",\r\n\t\t\t\"upload\" => \"#SECTION_ID#/action/upload/\",\r\n\t\t\t\"detail\" => \"#SECTION_ID#/#ELEMENT_ID#/\",\r\n\t\t\t\"detail_edit\" => \"#SECTION_ID#/#ELEMENT_ID#/action/#ACTION#/\",\r\n\t\t\t\"detail_list\" => \"list/\",\r\n\t\t\t\"search\" => \"search/\"\r\n\t\t),\r\n\t\t\"VARIABLE_ALIASES\" => Array(\r\n\t\t\t\"index\" => Array(),\r\n\t\t\t\"section\" => Array(),\r\n\t\t\t\"section_edit\" => Array(),\r\n\t\t\t\"section_edit_icon\" => Array(),\r\n\t\t\t\"upload\" => Array(),\r\n\t\t\t\"detail\" => Array(),\r\n\t\t\t\"detail_edit\" => Array(),\r\n\t\t\t\"detail_list\" => Array(),\r\n\t\t\t\"search\" => Array(),\r\n\t\t)\r\n\t)\r\n);?>").ToList();
            Assert.AreEqual(2, components.Count);
            Assert.AreEqual(".default", components[0].Name);
            Assert.AreEqual("bitrix:photogallery", components[0].Category);
            Assert.AreEqual("news", components[1].Name);
            Assert.AreEqual("bitrix:photogallery2", components[1].Category);
        }
    }
}
