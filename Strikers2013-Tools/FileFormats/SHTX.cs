using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using Be.IO;
using nQuant;

namespace StrikersTools.FileFormats
{
    class SHTX
    {

        public static void Export(string input)
        {
            byte[] textureData;
            int textureDataLength = 0;

            var file = File.Open(input, FileMode.Open);
            using (var br = new BinaryReader(file))
            {
                var magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "SHTX")
                {
                    Console.WriteLine("{0} : Not a valid SHTX file", input);
                }

                var format = br.ReadInt16();
                var width = br.ReadInt16();
                var height = br.ReadInt16();
                var unk = br.ReadInt16();
                Color[] colorPalette = new Color[1];
                switch (format)
                {
                    case 0x4646:
                        Console.WriteLine("SHTXFF not supported");
                        break;
                    case 0x3446:
                        Console.WriteLine("SHTXF4 not supported");
                        return;
                    default:
                        colorPalette = new Color[256];
                        for (var i = 0; i < 256; i++)
                        {
                            colorPalette[i] = Int32ToColor(br.ReadInt32());
                        }
                        textureDataLength = width * height;
                        break;
                }

                textureData = br.ReadBytes(textureDataLength);

                var image = DecodeImage(width, height, colorPalette, textureData);
                image.Save(Path.GetFileNameWithoutExtension(input)+".png");
            }
        }

        public static void Convert(string input)
        {
            var output = File.Open(Path.GetDirectoryName(input) + "\\" + Path.GetFileNameWithoutExtension(input) + ".shtx", FileMode.Create);
            var quantizer = new WuQuantizer();
            using (var bitmap = new Bitmap(input))
            {
                using (var quantized = new Bitmap(quantizer.QuantizeImage(bitmap)))
                {
                    var colorList = new List<int>();
                    for(var y = 0; y < quantized.Height; y++)
                    {
                        for (var x = 0; x < quantized.Width; x++)
                        {
                            var color = quantized.GetPixel(x, y).ToArgb();
                            if(colorList.IndexOf(color) == -1)
                            {
                                colorList.Add(color);
                            }
                        }
                    }

                    var palette = new int[256];
                    colorList.Take(256).ToList().CopyTo(palette);

                    using(var bw = new BinaryWriter(output))
                    {
                        bw.Write(Encoding.ASCII.GetBytes("SHTXFS"));
                        bw.Write((short)quantized.Width);
                        bw.Write((short)quantized.Height);
                        bw.Write((short)0);
                        palette.ToList().ForEach(x => bw.Write(x));

                        for (var y = 0; y < quantized.Height; y++)
                        {
                            for (var x = 0; x < quantized.Width; x++)
                            {
                                var color = quantized.GetPixel(x, y).ToArgb();
                                bw.Write((byte)colorList.IndexOf(color));
                            }
                        }
                    }


                }
            }
        }
        private static Bitmap DecodeImage(short width, short height, Color[] palette, byte[] textureData)
        {
            var bitmap = new Bitmap(width, height);
            if (palette.Length < 16)
            {
                var pos = 0;
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {

                        var color = Int32ToColor(BitConverter.ToInt32(textureData, pos));
                        bitmap.SetPixel(x, y, color);
                        pos += 4;
                    }
                }
            }
            else
            {
                var pos = 0;
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var indexByte = textureData[pos];

                        var color = palette[indexByte];
                        bitmap.SetPixel(x, y, color);
                        pos += 1;
                    }
                }
            }
            return bitmap;

        } 

        private static Color Int32ToColor(int color)
        {
            var reversedBytes = BitConverter.GetBytes(color);
            var reversedColor = BitConverter.ToInt32(reversedBytes, 0);
            return Color.FromArgb(reversedColor);
        }
        private static Color Int32ToColorPalette(int color)
        {
            var reversedBytes = BitConverter.GetBytes(color).Reverse().ToArray();
            var reversedColor = BitConverter.ToInt32(reversedBytes, 0);
            return Color.FromArgb(reversedColor);
        }
    }
}
