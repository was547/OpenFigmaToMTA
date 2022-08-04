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

            Structs.Child parentObj = null;

            if(_root.document.children.Count > 1)
            {
                _list.Add("-- REMOVE ALL GROUPING IN YOUR PROJECT TO CONTINUE!!!");
                return;
            }

            //Get parent window size
            foreach(var el in _root.document.children[0].children)
            {
                if(el == null) 
                    continue;

                if(el.name.Equals(_parent))
                {
                    parentObj = el;
                    break;
                }
            }

            if (parentObj == null)
            {
                _list.Add("-- CAN'T FIND THE PARENT ELEMENT, TRY AGAIN!");
                return;
            }

            if(parentObj.absoluteBoundingBox == null)
            {
                _list.Add("-- YOUR BACKGROUND IS INVALID, PLEASE SELECT THE MOST DEEP IMAGE");
                return;
            }

            _list.Add(string.Format("local resW, resH = {0},{1}",
                int.Parse(parentObj.absoluteBoundingBox.width.ToString()), 
                int.Parse(parentObj.absoluteBoundingBox.height.ToString())));
            _list.Add("local x, y = (sW/resW), (sH/resH)");
            _list.Add("");
            _list.Add("function onClientRender_OpenFigmaToMTA()");
            foreach(var el in _root.document.children[0].children)
            {
                if (el == null) 
                    continue;

                if (el.name.Equals(_parent))
                    continue;

                var elType = el.type;

                if(elType.Equals("RECTANGLE"))
                {
                    if (el.fills[0].type.Equals("IMAGE"))
                    {
                        double opacity = 1.0f;

                        if (el.fills[0].opacity != null)
                            opacity = el.fills[0].opacity.GetValueOrDefault();
                        else
                            opacity = 1.0f;

                        if(el.absoluteBoundingBox == null)
                        {
                            _list.Add(string.Format("   -- ELEMENT {0} IS INVALID, PLEASE CONFIGURE IT PROPERLY", el.name));
                            continue;
                        }

                        int val_0 = int.Parse(el.absoluteBoundingBox.x.ToString());
                        int val_1 = int.Parse(el.absoluteBoundingBox.y.ToString());
                        int val_2 = int.Parse(el.absoluteBoundingBox.width.ToString());
                        int val_3 = int.Parse(el.absoluteBoundingBox.height.ToString());
                        string val_4 = string.Format("assets/images/{0}.png", el.name);

                        if(el.fills[0].color == null)
                        {
                            Structs.Color tempColor = new Structs.Color();
                            tempColor.r = 0.0f;
                            tempColor.g = 0.0f;
                            tempColor.b = 0.0f;
                            tempColor.a = 1.0f;

                            el.fills[0].color = tempColor;
                        }
                        var rVal = (el.fills[0].color.r * 255);
                        int val_5 = (int)Math.Round(rVal);

                        var gVal = (el.fills[0].color.g * 255);
                        int val_6 = (int)Math.Round(gVal);

                        var bVal = (el.fills[0].color.b * 255);
                        int val_7 = (int)Math.Round(bVal);

                        int val_8 = (int)Math.Round(opacity * 255);
                        _list.Add(string.Format("     -- DON'T FORGET TO ADD THE IMAGE FILE ON THE FOLDER TO MAKE IT WORK PROPERLY: {0}", val_4));
                        _list.Add(string.Format("     dxDrawImage(x*{0}, y*{1}, x*{2}, y*{3}, \"{4}\", 0, 0, 0, tocolor({5}, {6}, {7}, {8}), false)",
                            val_0, val_1, val_2, val_3, val_4, val_5, val_6, val_7, val_8));
                    }
                    else
                    {
                        double opacity = 1.0f;

                        if (el.fills[0].opacity != null)
                            opacity = el.fills[0].opacity.GetValueOrDefault();
                        else
                            opacity = 1.0f;

                        if (el.absoluteBoundingBox == null)
                        {
                            _list.Add(string.Format("   -- ELEMENT {0} IS INVALID, PLEASE CONFIGURE IT PROPERLY", el.name));
                            continue;
                        }

                        int val_0 = int.Parse(el.absoluteBoundingBox.x.ToString());
                        int val_1 = int.Parse(el.absoluteBoundingBox.y.ToString());
                        int val_2 = int.Parse(el.absoluteBoundingBox.width.ToString());
                        int val_3 = int.Parse(el.absoluteBoundingBox.height.ToString());

                        if (el.fills[0].color == null)
                        {
                            Structs.Color tempColor = new Structs.Color();
                            tempColor.r = 0.0f;
                            tempColor.g = 0.0f;
                            tempColor.b = 0.0f;
                            tempColor.a = 1.0f;

                            el.fills[0].color = tempColor;
                        }

                        var rVal = (el.fills[0].color.r * 255);
                        int val_4 = (int)Math.Round(rVal);

                        var gVal = (el.fills[0].color.g * 255);
                        int val_5 = (int)Math.Round(gVal);

                        var bVal = (el.fills[0].color.b * 255);
                        int val_6 = (int)Math.Round(bVal);

                        int val_7 = (int)Math.Round(opacity * 255);


                        _list.Add(
                            string.Format("     dxDrawRectangle(x*{0}, y*{1}, x*{2}, y*{3}, tocolor({4}, {5}, {6}, {7}), false)",
                            val_0, val_1, val_2, val_3, val_4, val_5, val_6, val_7));
                    }

                }
                else if(elType.Equals("TEXT"))
                {
                    double opacity = 0.0f;

                    if (el.fills[0].opacity != null)
                        opacity = el.fills[0].opacity.GetValueOrDefault();
                    else
                        opacity = 1.0f;

                    if (el.absoluteBoundingBox == null)
                    {
                        _list.Add(string.Format("   -- ELEMENT {0} IS INVALID, PLEASE CONFIGURE IT PROPERLY", el.name));
                        continue;
                    }

                    string val_0 = el.name;
                    int val_1 = int.Parse(el.absoluteBoundingBox.x.ToString());
                    int val_2 = int.Parse(el.absoluteBoundingBox.y.ToString());
                    int val_3 = int.Parse(el.absoluteBoundingBox.width.ToString());
                    int val_4 = int.Parse(el.absoluteBoundingBox.height.ToString());

                    var rVal = (el.fills[0].color.r * 255);
                    int val_5 = (int)Math.Round(rVal);

                    var gVal = (el.fills[0].color.g * 255);
                    int val_6 = (int)Math.Round(gVal);

                    var bVal = (el.fills[0].color.b * 255);
                    int val_7 = (int)Math.Round(bVal);

                    int val_8 = (int)Math.Round(opacity * 255);

                    string val_9 = el.constraints.vertical.ToLower();
                    string val_10 = el.constraints.horizontal.ToLower();

                    _list.Add(
                        string.Format("     dxDrawText(\"{0}\", x*{1}, y*{2}, x*{3}, y*{4}, tocolor({5}, {6}, {7}, {8}), x*2.0, \"default\", \"{9}\", \"{10}\", false, false, false, true, false)",
                        val_0, val_1, val_2, val_3, val_4, val_5, val_6, val_7, val_8, val_9, val_10));
                }
                else
                {
                    _list.Add(string.Format("     -- ELEMENT [{0}] ISN'T AVAILABLE YET, CHECK FOR UPDATES IN OUR REPOSITORY", elType));
                }
            }
            _list.Add("end");
            _list.Add("addEventHandler(\"onClientRender\", getRootElement(), onClientRender_OpenFigmaToMTA)");
            _list.Add("");
            _list.Add("");
            _list.Add("-- USEFUL");
            _list.Add("");
            _list.Add("");
            _list.Add("function dxDrawRoundedRectangle(x, y, rx, ry, color, radius)");
            _list.Add("     rx = rx - radius * 2");
            _list.Add("     ry = ry - radius * 2");
            _list.Add("     x = x + radius");
            _list.Add("     y = y + radius");
            _list.Add("     if (rx >= 0) and(ry >= 0) then");
            _list.Add("         dxDrawRectangle(x, y, rx, ry, color)");
            _list.Add("         dxDrawRectangle(x, y - radius, rx, radius, color)");
            _list.Add("         dxDrawRectangle(x, y + ry, rx, radius, color)");
            _list.Add("         dxDrawRectangle(x - radius, y, radius, ry, color)");
            _list.Add("         dxDrawRectangle(x + rx, y, radius, ry, color)");
            _list.Add("         dxDrawCircle(x, y, radius, 180, 270, color, color, 7)");
            _list.Add("         dxDrawCircle(x + rx, y, radius, 270, 360, color, color, 7)");
            _list.Add("         dxDrawCircle(x + rx, y + ry, radius, 0, 90, color, color, 7)");
            _list.Add("         dxDrawCircle(x, y + ry, radius, 90, 180, color, color, 7)");
            _list.Add("     end");
            _list.Add("end");
            _list.Add("-- END OF AUTO GENERATED FILE");
            return;
        }

        public List<string> getResult() => _list;
    }
}
