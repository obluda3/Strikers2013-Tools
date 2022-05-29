using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRendering
{
    class GameBackground
    {
        public Image SourceImage { get; private set; }
        public int X_Origin { get; private set; }
        public int Y_Origin { get; private set; }
        private int X_Max;

        public bool Exceeds(int xPos, int width)
        {
            return xPos + width - X_Max > 0;
        }
        public GameBackground(int xOrg, int yOrg, int xMax, Image image)
        {
            X_Origin = xOrg;
            Y_Origin = yOrg;
            X_Max = xMax;
            SourceImage = image;
        }

        public Bitmap BaseBitmap()
        {
            return new Bitmap(SourceImage);
        }
    }
}
