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
        public static Bitmap GenerateBitmap(string str)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var font = new Bitmap(assembly.GetManifestResourceStream("StrikersTools.Resources.Unnamed.png"));
            var fontMapping = BitmapFonts.BuildDictionnary();

            var size = GetImageDimension(str, fontMapping);
            var image = new Bitmap(size.Width, size.Height);

            var x = 0;
            var y = 0;

            foreach(var car in str)
            {
                var fontEntry = fontMapping[car];
                var fontRect = fontEntry.rect;
                var width = fontRect.Width;
                var height = fontRect.Height;

                CopyRegionIntoImage(font, new Rectangle(fontRect.X, fontRect.Y, width, height), ref image,
                    new Rectangle((int)x + fontEntry.xoffset, 0+fontEntry.yoffset, width, height));
                x += fontEntry.xadvance; // Magic number
                if (car != ' ')
                    x -= 4;
            }
            Console.WriteLine("ok");
            image.Save(@"C:\Users\PCHMD\source\repos\CopaEditor\CopaEditor\bin\Debug.png");
            return image;
        }

        private static Size GetImageDimension(string str, 
            Dictionary<char, (int xoffset, int yoffset,int xadvance, Rectangle rect)> fontMapping)
        {
            
            int width = 0;
            int height = 0;

            foreach(char car in str)
            {
                var fontEntry = fontMapping[car];

                // The idea of this method is not to get the exact size, but to get one that is big enough
                width += Math.Max(fontEntry.rect.Width,fontEntry.xadvance);
                if (fontEntry.xoffset > 0) width += fontEntry.xoffset;

                var entryHeight = fontEntry.rect.Height + fontEntry.yoffset;
                if (entryHeight > height)
                    height = entryHeight;
            }

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
