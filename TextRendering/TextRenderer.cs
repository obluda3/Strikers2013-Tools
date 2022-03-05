using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using StrikersTools.FileFormats;
using StrikersTools.Dictionaries;

namespace TextRendering
{
    class TextRenderer
    {
        public GameBackground GameScreen { get; set; }
        private static int[] OffsetsXadvance = { };
        private static int[] CodePoints = { 0x8140, 0x8149, 0x8168, 0x8194, 0x8190, 0x8193, 0x8195, 0x8166, 0x8169, 0x816A, 0x8196, 0x817B, 0x8143, 0x817C, 0x8144, 0x815E, 0x824F, 0x8250, 0x8251, 0x8252, 0x8253, 0x8254, 0x8255, 0x8256, 0x8257, 0x8258, 0x8146, 0x8147, 0x8183, 0x8181, 0x8184, 0x8148, 0x8197, 0x8260, 0x8261, 0x8262, 0x8263, 0x8264, 0x8265, 0x8266, 0x8267, 0x8268, 0x8269, 0x826A, 0x826B, 0x826C, 0x826D, 0x826E, 0x826F, 0x8270, 0x8271, 0x8272, 0x8273, 0x8274, 0x8275, 0x8276, 0x8277, 0x8278, 0x8279, 0x816D, 0x818F, 0x816E, 0x814F, 0x8151, 0x8165, 0x8281, 0x8282, 0x8283, 0x8284, 0x8285, 0x8286, 0x8287, 0x8288, 0x8289, 0x828A, 0x828B, 0x828C, 0x828D, 0x828E, 0x828F, 0x8290, 0x8291, 0x8292, 0x8293, 0x8294, 0x8295, 0x8296, 0x8297, 0x8298, 0x8299, 0x829A, 0x816F, 0x8162, 0x8170, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8140, 0x8740, 0x8741, 0x8742, 0x8743, 0x8744, 0x8745, 0x8746, 0x8747, 0x8748, 0x8749, 0x874A, 0x874B, 0x874C, 0x874D, 0x874E, 0x874F, 0x8750, 0x8751, 0x8752, 0x8753, 0x8754, 0x8755, 0x8756, 0x84A1, 0x84A2, 0x84A3, 0x84A4, 0x84A5, 0x84A6, 0x84A7, 0x84A8, 0x84A9, 0x84AA, 0x84AB, 0x84AC, 0x84AD, 0x84AE, 0x84AF, 0x84B0, 0x84B1, 0x84B2, 0x84B3, 0x84B4, 0x84B5, 0x84B6, 0x84B7, 0x84B8, 0x84B9, 0x84BA, 0x84BB, 0x84BC, 0x84BD, 0x84BE, 0x84BF, 0x84C0, 0x84C1, 0x84C2, 0x84C3, 0x84C4, 0x84C5, 0x84C6, 0x84C7, 0x84C8, 0x84C9, 0x84CA, 0x84CB, 0x84CC, 0x84CD, 0x84CE, 0x84CF, 0x84D0, 0x84D1, 0x84D2, 0x84D3, 0x84D4, 0x84D5, 0x84D6, 0x84D7, 0x84D8, 0x84D9, 0x84DA, 0x84DB, 0x84DC, 0x84DD, 0x84DE, 0x84DF, 0x84E0, 0x84E1 };
        private int CharToFullwidth(int codepoint)
        {
            if (codepoint < 256) return CodePoints[codepoint - 0x20];
            else return 0x8140;
        }

        public TextRenderer(GameBackground gameScreen)
        {
            GameScreen = gameScreen;
        }
        // shamelessly decompiled function  
        private int SjisToJis(int character)
        {
            byte b1 = (byte)(character & 0xFF);
            int b2 = ((character >> 8) & 0xFF);
            int v2;
            if ((uint)(b2 - 129) > 0x1E)
            {
                if ((uint)(b2 - 224) <= 0xF)
                    b2 -= 193;
            }
            else
            {
                b2 -= 129;
            }
            v2 = 2 * b2;
            if (b1 - 64 > 0x3E)
            {
                if (b1 - 128 > 0x1E)
                {
                    if (b1 - 159 <= 0x5D)
                    {
                        b1 -= 159;
                        ++v2;
                    }
                }
                else
                {
                    b1 -= 65;
                }
            }
            else
            {
                b1 -= 64;
            }
            int output = b1 + ((v2 + 1) << 8) + 8225;
            return output;
        }
        private int CodepointToIndex(int codepoint)
        {
            var character = CharToFullwidth(codepoint);
            
            if (character >= 0x300)
            {
                int jis = SjisToJis(character);
                int v1 = (jis >> 8) - 33;

                return (jis & 0xFF) + 94 * v1 - 33;
            }
            else return 8;
        }
        private Bitmap CharBitmap(int codepoint)
        {
            var cd = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var index = CodepointToIndex(codepoint);
            var path = cd + "\\fonts\\";
            var file = "";
            if (File.Exists($"{path}{index}.png"))
                file = $"{path}{index}.png";
            else if (File.Exists($"{path}{index:00000000}.png"))
                file = $"{path}{index:00000000}.png";
            else
                return new Bitmap(1,1);
            return new Bitmap(file);
        }

        private const float _baseGameScale = 352F;
        private const float _baseToolScale = 12F;
        public Bitmap RenderedText(string message, int xadvance, int percent, out bool exceeds)
        {
            Bitmap curBmp = new Bitmap(GameScreen.SourceImage);
            var formattedMessage = TEXT.FormatThing(message);

            float x = GameScreen.X_Origin;
            int y = GameScreen.Y_Origin;
            float xAdvanceRatio = _baseToolScale / _baseGameScale;
            exceeds = false;
            for (var i = 0; i < formattedMessage.Length; i++)
            {
                var character = formattedMessage[i];
                if (character == '<')
                {
                    var remainingMessage = new string(formattedMessage.Skip(i).Take(formattedMessage.Length - i).ToArray());
                    var endOfFormattedSect = remainingMessage.IndexOf('>');
                    if (endOfFormattedSect == -1)
                        break;
                    else
                        endOfFormattedSect += i;
                    var commands = remainingMessage.Substring(0, endOfFormattedSect - i);
                    if (commands.Contains("#n"))
                    {
                        y += 50;
                        x = GameScreen.X_Origin;
                    }
                    i = endOfFormattedSect + 1;
                    continue;
                }


                float scale = percent / 100f;
                int codePoint = character;
                var kvp = TextDecoder.CustomChars.Where(x => x.Value == character);
                if (kvp.Count() > 0) 
                    codePoint = kvp.First().Key;

                curBmp = BitmapOnBitmap(curBmp, CharBitmap(codePoint), (int)x, y, scale);
                var xAdvance = _baseGameScale * xAdvanceRatio * scale;
                x += xAdvance;

                if (GameScreen.Exceeds((int)(x - xAdvance), xadvance)) exceeds = true;
            }
            return curBmp;
        }

        private Bitmap BitmapOnBitmap(Bitmap largeBmp, Bitmap smallBmp, int x, int y, float scale)
        {
            Graphics g = Graphics.FromImage(largeBmp);
            g.CompositingMode = CompositingMode.SourceOver;

            var scaledWidth = (int)(24 * scale);
            var scaledHeight = (int)(25 * scale);

            //smallBmp.MakeTransparent();
            g.DrawImage(smallBmp, x, y, scaledWidth, scaledHeight);
            return largeBmp;
        }

    }
}
