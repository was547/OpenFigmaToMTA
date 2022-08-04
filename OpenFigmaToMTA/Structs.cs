using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFigmaToMTA
{
    internal class Structs
    {
        public class AbsoluteBoundingBox
        {
            public double x { get; set; }
            public double y { get; set; }
            public double width { get; set; }
            public double height { get; set; }
        }

        public class AbsoluteRenderBounds
        {
            public double x { get; set; }
            public double y { get; set; }
            public double width { get; set; }
            public double height { get; set; }
        }

        public class BackgroundColor
        {
            public double r { get; set; }
            public double g { get; set; }
            public double b { get; set; }
            public double a { get; set; }
        }

        public class Child
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public List<Child> children { get; set; }
            public BackgroundColor backgroundColor { get; set; }
            public object prototypeStartNodeID { get; set; }
            public List<object> flowStartingPoints { get; set; }
            public PrototypeDevice prototypeDevice { get; set; }
            public List<ExportSetting> exportSettings { get; set; }
            public bool locked { get; set; }
            public string blendMode { get; set; }
            public AbsoluteBoundingBox absoluteBoundingBox { get; set; }
            public AbsoluteRenderBounds absoluteRenderBounds { get; set; }
            public bool preserveRatio { get; set; }
            public Constraints constraints { get; set; }
            public List<Fill> fills { get; set; }
            public List<object> strokes { get; set; }
            public double strokeWeight { get; set; }
            public string strokeAlign { get; set; }
            public List<object> effects { get; set; }
            public string characters { get; set; }
            public Style style { get; set; }
            public int? layoutVersion { get; set; }
            public List<object> characterStyleOverrides { get; set; }
            public StyleOverrideTable styleOverrideTable { get; set; }
            public List<string> lineTypes { get; set; }
            public List<int> lineIndentations { get; set; }
        }

        public class Color
        {
            public double r { get; set; }
            public double g { get; set; }
            public double b { get; set; }
            public double a { get; set; }
        }

        public class Components
        {
        }

        public class ComponentSets
        {
        }

        public class Constraint
        {
            public string type { get; set; }
            public double value { get; set; }
        }

        public class Constraints
        {
            public string vertical { get; set; }
            public string horizontal { get; set; }
        }

        public class Document
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public List<Child> children { get; set; }
        }

        public class ExportSetting
        {
            public string suffix { get; set; }
            public string format { get; set; }
            public Constraint constraint { get; set; }
        }

        public class Fill
        {
            public string blendMode { get; set; }
            public string type { get; set; }
            public string scaleMode { get; set; }
            public string imageRef { get; set; }
            public double? opacity { get; set; }
            public Color color { get; set; }
        }

        public class PrototypeDevice
        {
            public string type { get; set; }
            public string rotation { get; set; }
        }

        public class Root
        {
            public Document document { get; set; }
            public Components components { get; set; }
            public ComponentSets componentSets { get; set; }
            public int schemaVersion { get; set; }
            public Styles styles { get; set; }
            public string name { get; set; }
            public DateTime lastModified { get; set; }
            public string thumbnailUrl { get; set; }
            public string version { get; set; }
            public string role { get; set; }
            public string editorType { get; set; }
            public string linkAccess { get; set; }
        }

        public class Style
        {
            public string fontFamily { get; set; }
            public object fontPostScriptName { get; set; }
            public double fontWeight { get; set; }
            public double fontSize { get; set; }
            public string textAlignHorizontal { get; set; }
            public string textAlignVertical { get; set; }
            public double letterSpacing { get; set; }
            public double lineHeightPx { get; set; }
            public double lineHeightPercent { get; set; }
            public string lineHeightUnit { get; set; }
        }

        public class StyleOverrideTable
        {
        }

        public class Styles
        {
        }
    }
}
