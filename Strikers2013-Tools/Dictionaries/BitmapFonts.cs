using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace StrikersTools.Dictionaries
{
    class BitmapFonts
    {
        public static Dictionary<char, (int xoffset, int yoffset, int xadvance, Rectangle rect)> BuildDictionnary()
        {
            var fontEntries = new Dictionary<char, (int xoffset, int yoffset, int xadvance, Rectangle rect)>();
            var fntFile = Assembly.GetExecutingAssembly().GetManifestResourceStream("StrikersTools.Resources.Unnamed.fnt");
            using(var sr = new StreamReader(fntFile))
            {
                var entryCount = Convert.ToInt32(sr.ReadLine());
                for(var i = 0; i < entryCount; i++)
                {
                    var entry = sr.ReadLine().Split(' ');
                    
                    var entryChar = Convert.ToChar(Convert.ToInt32(entry[0].Split('=')[1]));
                    var x = Convert.ToInt32(entry[1].Split('=')[1]);
                    var y = Convert.ToInt32(entry[2].Split('=')[1]);
                    var w = Convert.ToInt32(entry[3].Split('=')[1]);
                    var h = Convert.ToInt32(entry[4].Split('=')[1]);
                    var xoff = Convert.ToInt32(entry[5].Split('=')[1]);
                    var yoff = Convert.ToInt32(entry[6].Split('=')[1]);
                    var xad = Convert.ToInt32(entry[7].Split('=')[1]);

                    fontEntries.Add(entryChar, (xoff, yoff, xad, new Rectangle(x, y, w, h)));
                }

            }


            return fontEntries;
        }
    }
}
