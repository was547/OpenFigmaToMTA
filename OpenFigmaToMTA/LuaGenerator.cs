using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFigmaToMTA
{
    internal class LuaGenerator
    {
        private Structs.Root _root;
        private string _parent;
        private List<string> _list;
        public LuaGenerator(Structs.Root rootElement, string parent)
        {
            _root = rootElement;
            _parent = parent;
        }

        public void GenerateLua()
        {
            _list = new List<string>();

            _list.Add("-- Generated with OpenFigmaToMTA");
            _list.Add("-- https://github.com/was547/OpenFigmaToMTA");
            _list.Add("");
            _list.Add("");
            _list.Add("local sW, sH = guiGetScreenSize()");

            if (_root.document.children.Count > 1)
            {
                _list.Add("-- REMOVE ALL GROUPING IN YOUR PROJECT TO CONTINUE!!!");
                return;
            }

            var parentObj = _root.document.children[0]?.children?.FirstOrDefault(el => el != null && el.name.Equals(_parent));

            if (parentObj == null || parentObj.absoluteBoundingBox == null)
            {
                _list.Add("-- CAN'T FIND THE PARENT ELEMENT, TRY AGAIN!");
                return;
            }

            AddWindowSizeInfo(parentObj);

            _list.Add("function onClientRender_OpenFigmaToMTA()");
            foreach (var el in _root.document.children[0].children.Where(el => el != null && !el.name.Equals(_parent)))
            {
                ProcessElement(el);
            }
            _list.Add("end");

            _list.Add("addEventHandler(\"onClientRender\", getRootElement(), onClientRender_OpenFigmaToMTA)");

            // Add command to toggle window rendering
            _list.Add("function toggleFigmaWindow()");
            _list.Add("     if isEventHandlerAdded(\"onClientRender\", getRootElement(), onClientRender_OpenFigmaToMTA) then");
            _list.Add("          removeEventHandler(\"onClientRender\", getRootElement(), onClientRender_OpenFigmaToMTA)");
            _list.Add("     else");
            _list.Add("          addEventHandler(\"onClientRender\", getRootElement(), onClientRender_OpenFigmaToMTA)");
            _list.Add("     end");
            _list.Add("end");

            // Register the command to toggleFigmaWindow
            _list.Add("addCommandHandler(\"panel\", toggleFigmaWindow)");

        }

        private void AddWindowSizeInfo(Structs.Child parentObj)
        {
            _list.Add($"local resW, resH = {int.Parse(parentObj.absoluteBoundingBox.width.ToString())},{int.Parse(parentObj.absoluteBoundingBox.height.ToString())}");
            _list.Add("local x, y = (sW/resW), (sH/resH)");
            _list.Add("");
        }

        private void ProcessElement(Structs.Child el)
        {
            var elType = el.type;
            if (elType.Equals("RECTANGLE"))
            {
                ProcessRectangleElement(el);
            }
            else if (elType.Equals("TEXT"))
            {
                ProcessTextElement(el);
            }
            else
            {
                _list.Add($"     -- ELEMENT [{elType}] ISN'T AVAILABLE YET, CHECK FOR UPDATES IN OUR REPOSITORY");
            }
        }

        private void ProcessRectangleElement(Structs.Child el)
        {
            var fill = el.fills[0];
            double opacity = fill.opacity ?? 1.0;

            if (el.absoluteBoundingBox == null)
            {
                _list.Add($"   -- ELEMENT {el.name} IS INVALID, PLEASE CONFIGURE IT PROPERLY");
                return;
            }

            int val_0 = int.Parse(el.absoluteBoundingBox.x.ToString());
            int val_1 = int.Parse(el.absoluteBoundingBox.y.ToString());
            int val_2 = int.Parse(el.absoluteBoundingBox.width.ToString());
            int val_3 = int.Parse(el.absoluteBoundingBox.height.ToString());

            if (fill.type.Equals("IMAGE"))
            {
                ProcessImageElement(el, opacity, val_0, val_1, val_2, val_3);
            }
            else
            {
                ProcessRegularRectangleElement(el, opacity, val_0, val_1, val_2, val_3);
            }
        }

        private void ProcessImageElement(Structs.Child el, double opacity, int val_0, int val_1, int val_2, int val_3)
        {
            var imagePath = $"assets/images/{el.name}.png";
            var colorValues = GetColorValues(el.fills[0]);
            _list.Add($"     -- DON'T FORGET TO ADD THE IMAGE FILE ON THE FOLDER TO MAKE IT WORK PROPERLY: {imagePath}");
            _list.Add($"     dxDrawImage(x*{val_0}, y*{val_1}, x*{val_2}, y*{val_3}, \"{imagePath}\", 0, 0, 0, tocolor({colorValues}), false)");
        }

        private void ProcessRegularRectangleElement(Structs.Child el, double opacity, int val_0, int val_1, int val_2, int val_3)
        {
            var colorValues = GetColorValues(el.fills[0]);
            _list.Add($"     dxDrawRectangle(x*{val_0}, y*{val_1}, x*{val_2}, y*{val_3}, tocolor({colorValues}), false)");
        }

        private void ProcessTextElement(Structs.Child el)
        {
            double opacity = el.fills[0].opacity ?? 1.0;

            if (el.absoluteBoundingBox == null)
            {
                _list.Add($"   -- ELEMENT {el.name} IS INVALID, PLEASE CONFIGURE IT PROPERLY");
                return;
            }

            string val_0 = el.name;
            int val_1 = int.Parse(el.absoluteBoundingBox.x.ToString());
            int val_2 = int.Parse(el.absoluteBoundingBox.y.ToString());
            int val_3 = int.Parse(el.absoluteBoundingBox.width.ToString());
            int val_4 = int.Parse(el.absoluteBoundingBox.height.ToString());

            var colorValues = GetColorValues(el.fills[0]);
            var constraints = el.constraints;
            _list.Add($"     dxDrawText(\"{val_0}\", x*{val_1}, y*{val_2}, x*{val_3}, y*{val_4}, tocolor({colorValues}), x*2.0, \"default\", \"{constraints.vertical.ToLower()}\", \"{constraints.horizontal.ToLower()}\", false, false, false, true, false)");
        }

        private string GetColorValues(Structs.Fill fill)
        {
            var color = fill.color ?? new Structs.Color { r = 0.0f, g = 0.0f, b = 0.0f, a = 1.0f };
            var opacity = fill.opacity ?? 1.0;

            return $"{Math.Round(color.r * 255)}, {Math.Round(color.g * 255)}, {Math.Round(color.b * 255)}, {Math.Round(opacity * 255)}";
        }

        public List<string> getResult() => _list;
    }
}
