
using System.Xml.Serialization;

namespace XTC.FMP.MOD.ImageSee.LIB.Unity
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class MyConfig : MyConfigBase
    {

        public class Pending
        {
            [XmlAttribute("image")]
            public string image { get; set; } = "";
        }

        public class Background
        {
            [XmlAttribute("visible")]
            public bool visible { get; set; } = true;
            [XmlAttribute("color")]
            public string color { get; set; } = "#00000000";
        }

        public class ToolBar
        {
            [XmlElement("Anchor")]
            public Anchor anchor { get; set; } = new Anchor();
        }

        public class Style
        {
            [XmlAttribute("name")]
            public string name { get; set; } = "";
            [XmlElement("Background")]
            public Background background { get; set; } = new Background();
            [XmlElement("Pending")]
            public Pending pending { get; set; } = new Pending();
            [XmlElement("ToolBar")]
            public ToolBar toolBar { get; set; } = new ToolBar();
        }


        [XmlArray("Styles"), XmlArrayItem("Style")]
        public Style[] styles { get; set; } = new Style[0];
    }
}

