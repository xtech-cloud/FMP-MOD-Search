
using System.Xml.Serialization;

namespace XTC.FMP.MOD.Search.LIB.Unity
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class MyConfig : MyConfigBase
    {
        public class Margin
        {
            [XmlAttribute("top")]
            public int top { get; set; } = 0;
            [XmlAttribute("bottom")]
            public int bottom { get; set; } = 0;
            [XmlAttribute("left")]
            public int left { get; set; } = 0;
            [XmlAttribute("right")]
            public int right { get; set; } = 0;
        }

        public class Input
        {
            [XmlAttribute("marginTop")]
            public int marginTop { get; set; } = 64;
            [XmlAttribute("width")]
            public int width { get; set; } = 1720;
            [XmlAttribute("height")]
            public int height { get; set; } = 96;
            [XmlAttribute("fontSize")]
            public int fontSize { get; set; } = 28;
            [XmlAttribute("iconSize")]
            public int iconSize { get; set; } = 40;
        }

        public class Keyboard
        {
            [XmlAttribute("keyImage")]
            public string keyImage { get; set; } = "";
            [XmlAttribute("keySize")]
            public int keySize { get; set; } = 64;
            [XmlAttribute("fontSize")]
            public int fontSize { get; set; } = 24;
            [XmlAttribute("width")]
            public int width { get; set; } = 1024;
            [XmlAttribute("height")]
            public int height { get; set; } = 280;
            [XmlAttribute("spacingH")]
            public int spacingH { get; set; } = 12;
            [XmlAttribute("spacingV")]
            public int spacingV { get; set; } = 12;
        }

        public class Background
        {
            [XmlAttribute("visible")]
            public bool visible { get; set; } = false;
            [XmlAttribute("color")]
            public string color { get; set; } = "#000000FF";
        }

        public class Record
        {
            [XmlAttribute("color")]
            public string color { get; set; } = "#000000FF";
            [XmlAttribute("capacity")]
            public int capacity { get; set; } = 30;
            [XmlAttribute("iconVisible")]
            public bool iconVisible { get; set; } = true;
            [XmlAttribute("iconImage")]
            public string iconImage { get; set; } = "";
            [XmlAttribute("iconSize")]
            public int iconSize { get; set; } = 64;
            [XmlAttribute("iconMarginLeft")]
            public int iconMarginLeft { get; set; } = 24;
            [XmlAttribute("width")]
            public int width { get; set; } = 450;
            [XmlAttribute("height")]
            public int height { get; set; } = 96;
            [XmlAttribute("fontSize")]
            public int fontSize { get; set; } = 24;
            [XmlElement("TextMargin")]
            public Margin textMargin { get; set; } = new Margin();
        }

        public class Result
        {
            [XmlElement("Margin")]
            public Margin margin { get; set; } = new Margin();
            [XmlAttribute("spacingH")]
            public int spacingH { get; set; } = 40;
            [XmlAttribute("spacingV")]
            public int spacingV { get; set; } = 20;
        }

        public class Style
        {
            [XmlAttribute("name")]
            public string name { get; set; } = "";
            [XmlAttribute("primaryColor")]
            public string primaryColor { get; set; } = "";
            [XmlElement("Background")]
            public Background background { get; set; } = new Background();
            [XmlElement("Input")]
            public Input input { get; set; } = new Input();
            [XmlElement("Keyboard")]
            public Keyboard keyboard { get; set; } = new Keyboard();
            [XmlElement("Result")]
            public Result result { get; set; } = new Result();
            [XmlElement("Record")]
            public Record record { get; set; } = new Record();

            [XmlArray("ActivateSubjects"), XmlArrayItem("Subject")]
            public Subject[] activateSubjects { get; set; } = new Subject[0];
        }


        [XmlArray("Styles"), XmlArrayItem("Style")]
        public Style[] styles { get; set; } = new Style[0];
    }
}

