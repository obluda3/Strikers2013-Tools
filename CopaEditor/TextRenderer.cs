using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;
using StrikersTools.Dictionaries;

namespace StrikersTools
{
    class TextRenderer
    {
        public static void GenerateBitmap(string str)
        {
            const double spacing = -4.2;

            var assembly = Assembly.GetExecutingAssembly();
            var font = new Bitmap(assembly.GetManifestResourceStream("StrikersTools.Resources.Unnamed.png"));
            var fontMapping = BitmapFonts.BuildDictionnary();

            var size = GetImageDimension(str, spacing, fontMapping);
            var image = new Bitmap(100, 100);

            var x = 0.0;
            var y = 0;

            foreach(var car in str)
            {
                var fontEntry = fontMapping[car];
                var fontRect = fontEntry.rect;
                var width = fontRect.Width;
                var height = fontRect.Height;

                CopyRegionIntoImage(font, new Rectangle(fontRect.X, fontRect.Y, width, height), ref image,
                    new Rectangle((int)x + fontEntry.xoffset, 0+fontEntry.yoffset, width, height));
                x += fontEntry.xadvance + spacing; // Magic number
            }
            Console.WriteLine("ok");
            image.Save(@"C:\Users\PCHMD\source\repos\CopaEditor\CopaEditor\bin\Debug.png");
        }

        private static Size GetImageDimension(string str, double spacing, 
            Dictionary<char, (int xoffset, int yoffset,int xadvance, Rectangle rect)> fontMapping)
        {
            int width = 0;
            int height = 0;

            foreach(char car in str)
            {
                var fontEntry = fontMapping[car];

                width += fontEntry.xadvance;

                var entryHeight = fontEntry.rect.Height;
                if (entryHeight > height)
                    height = entryHeight;
            }
            var lastEntry = fontMapping[str[str.Length - 1]];
            width += lastEntry.rect.Width - lastEntry.xadvance + (Convert.ToInt32(spacing) *  str.Length-1); // for the last caracter we should use the width instead of xadvance

            return new Size(width, height);
        }

        public static void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
        {
            using (Graphics grD = Graphics.FromImage(destBitmap))
            {
                grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
            }
        }
    }
}
