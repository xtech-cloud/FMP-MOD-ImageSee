
using System.Xml.Serialization;

namespace XTC.FMP.MOD.ImageSee.LIB.Unity
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class MyConfig : MyConfigBase
    {
        public class CloseButton
        {
            [XmlAttribute("visible")]
            public bool visible { get; set; } = false;

            [XmlArray("OnClickSubjects"), XmlArrayItem("Subject")]
            public Subject[] OnClickSubjectS { get; set; } = new Subject[0];
        }


        public class Pending
        {
            [XmlAttribute("image")]
            public string image { get; set; } = "";
            [XmlAttribute("size")]
            public int size { get; set; } = 160;
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
            [XmlAttribute("visible")]
            public string visible { get; set; } = "auto";
            [XmlElement("Anchor")]
            public Anchor anchor { get; set; } = new Anchor();

            [XmlAttribute("maxScale")]
            public float maxScale { get; set; } = 4.0f;

            [XmlElement("CloseButton")]
            public CloseButton closeButton { get; set; } = new CloseButton();
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

