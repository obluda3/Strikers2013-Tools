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
        public static Bitmap GenerateBitmap(string str, int mode)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var font = new Bitmap(assembly.GetManifestResourceStream("StrikersTools.Resources.Unnamed.png"));
            var fontMapping = BitmapFonts.BuildDictionnary();

            var size = GetImageDimension(str, fontMapping);
            var textBitmap = new Bitmap(size.Width, size.Height);

            var x = 0;
            var y = -4;

            foreach(var car in str)
            {
                var fontEntry = fontMapping[car];
                var fontRect = fontEntry.rect;
                var width = fontRect.Width;
                var height = fontRect.Height;

                CopyRegionIntoImage(font, new Rectangle(fontRect.X, fontRect.Y, width, height), ref textBitmap,
                    new Rectangle((int)x + fontEntry.xoffset, y+fontEntry.yoffset, width, height));
                x += fontEntry.xadvance; // Magic number
                if (car != ' ')
                    x -= 4;
            }

            // Get the appropriate size, depending on the type of portrait
            var targetSize = new Size(0, 0);
            switch (mode)
            {
                default:
                    break;
                case 0:
                    targetSize = new Size(51, 15);
                    break;
            }

            // If text is too long, stretches it
            if(textBitmap.Width > targetSize.Width)
                textBitmap = new Bitmap(textBitmap, targetSize.Width, textBitmap.Height);
            // Centers the text
            var xoffset = targetSize.Width / 2 - textBitmap.Width / 2;
            if (xoffset < 0)
                xoffset = 0;
            Bitmap centeredBitmap = new Bitmap(targetSize.Width, targetSize.Height);
            CopyRegionIntoImage(textBitmap, new Rectangle(0, 0, textBitmap.Width, textBitmap.Height), ref centeredBitmap,
                new Rectangle(xoffset, 0, textBitmap.Width, textBitmap.Height));




            Console.WriteLine("ok");
            centeredBitmap.Save(@"C:\Users\PCHMD\source\repos\CopaEditor\Strikers2013-Tools\bin\Debug.png");
            return centeredBitmap;
        }

        private static Size GetImageDimension(string str, 
            Dictionary<char, (int xoffset, int yoffset,int xadvance, Rectangle rect)> fontMapping)
        {
            
            int width = 0;
            int height = -4;

            foreach(char car in str)
            {
                var fontEntry = fontMapping[car];

                width += fontEntry.rect.Width;
                if (fontEntry.xadvance > fontEntry.rect.Width)
                    width += fontEntry.xadvance - fontEntry.rect.Width;
                width += fontEntry.xoffset;

                var entryHeight = fontEntry.rect.Height + fontEntry.yoffset;
                if (entryHeight > height)
                    height = entryHeight;
            }

            if (height < 0) height = 1;
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
