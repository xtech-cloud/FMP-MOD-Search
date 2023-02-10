
using System.Xml.Serialization;

namespace XTC.FMP.MOD.Search.LIB.Unity
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class MyConfig : MyConfigBase
    {
        public class Keyboard
        {
            [XmlAttribute("keyImage")]
            public string keyImage { get; set; } = "";
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
            [XmlAttribute("width")]
            public int width { get; set; } = 450;
            [XmlAttribute("height")]
            public int height { get; set; } = 96;
        }

        public class Style
        {
            [XmlAttribute("name")]
            public string name { get; set; } = "";
            [XmlAttribute("primaryColor")]
            public string primaryColor { get; set; } = "";
            [XmlElement("Background")]
            public Background background { get; set; } = new Background();
            [XmlElement("Keyboard")]
            public Keyboard keyboard { get; set; } = new Keyboard();
            [XmlElement("Record")]
            public Record record { get; set; } = new Record();

            [XmlArray("ActivateSubjects"), XmlArrayItem("Subject")]
            public Subject[] activateSubjects { get; set; } = new Subject[0];
        }


        [XmlArray("Styles"), XmlArrayItem("Style")]
        public Style[] styles { get; set; } = new Style[0];
    }
}

