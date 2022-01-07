using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using StrikersTools.Utils;
using nQuant;

namespace StrikersTools.FileFormats
{
    class SHTX
    {

        public static void Export(string input, string output)
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
                        textureDataLength = width * height * 4;
                        break;
                    case 0x3446:
                        colorPalette = new Color[16];
                        var colorList = new List<Color>();
                        for (var i = 0; i < 256; i++)
                        {
                            colorList.Add(Int32ToColor(br.ReadInt32()));
                        }
                        colorPalette = colorList.Take(16).ToArray();
                        textureDataLength = width * height / 2;
                        break;
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
                image.Save(output);
            }
        }

        public static void Convert(string input, string original, string outputPath)
        {
            var output = new MemoryStream();
            var quantizer = new WuQuantizer();
            var magic = "";
            byte[] unkData = new byte[2];
            using(var originalFile = File.OpenRead(original))
            {
                originalFile.Position += 4;
                byte[] magicData = new byte[2];
                originalFile.Read(magicData, 0, 2);
                magic = Encoding.ASCII.GetString(magicData);

                if(magic == "F4")
                {
                    originalFile.Position = 12 + 16 * 4;
                    var len = (256 - 16) * 4;
                    unkData = new byte[(256 - 16) * 4];
                    originalFile.Read(unkData, 0, len);
                }
            }
            using (var bitmap = new Bitmap(input))
            {
                if(magic == "FS" || magic == "F4") 
                {
                    var paletteLength = 0;
                    if (magic == "FS") paletteLength = 256;
                    else paletteLength = 16;
                    var quantizedImage = quantizer.QuantizeImage(bitmap, maxColors: paletteLength);
                    using (var quantized = new Bitmap(quantizedImage))
                    {
                        var palette = quantizedImage.Palette.Entries.ToList().Select(x => x.ToArgb()).ToList();

                        for (var i = palette.Count; i < paletteLength; i++) palette.Add(0);

                        var bw = new BinaryWriter(output);
                        bw.Write(Encoding.ASCII.GetBytes("SHTX"));
                        bw.Write(Encoding.ASCII.GetBytes(magic));
                        bw.Write((short)quantized.Width);
                        bw.Write((short)quantized.Height);
                        bw.Write((short)0);
                        palette.ForEach(x => bw.Write(x));
                        if (magic == "F4") bw.Write(unkData);

                        if (magic == "FS")
                        {
                            for (var y = 0; y < quantized.Height; y++)
                            {
                                for (var x = 0; x < quantized.Width; x++)
                                {
                                    var color = quantized.GetPixel(x, y).ToArgb();
                                    bw.Write((byte)palette.IndexOf(color));
                                }
                            }
                        }
                        else
                        {
                            byte bufferedByte = 0;
                            var pos = 0;
                            for (var y = 0; y < quantized.Height; y++)
                            {
                                for (var x = 0; x < quantized.Width; x++)
                                {
                                    var color = quantized.GetPixel(x, y).ToArgb();
                                    var index = palette.IndexOf(color);
                                    if(pos == 0)
                                    {
                                        bufferedByte |= (byte)(index << 4);
                                        pos++;
                                    }
                                    else
                                    {
                                        bufferedByte |= (byte)index;
                                        pos = 0;
                                        bw.Write(bufferedByte);
                                        bufferedByte = 0;
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                else
                {
                    var bw = new BinaryWriter(output);
                    bw.Write(Encoding.ASCII.GetBytes("SHTXFF"));
                    bw.Write((short)bitmap.Width);
                    bw.Write((short)bitmap.Height);
                    bw.Write((short)0);
                    for (var y = 0; y < bitmap.Height; y++)
                    {
                        for (var x = 0; x < bitmap.Width; x++)
                        {
                            var pixel = bitmap.GetPixel(x, y).ToArgb();
                            bw.Write(pixel);
                        }
                    }
                }
            }
            var outputStream = File.Open(outputPath, FileMode.Create);
            output.Position = 0;
            output.CopyTo(outputStream);
            output.Close();
            outputStream.Close();
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
            else if(palette.Length == 256)
            {
                var pos = 0;
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var indexByte = textureData[pos];

                        var color = palette[indexByte];
                        bitmap.SetPixel(x, y, color);
                        pos++;
                    }
                }
            }
            else
            {
                var pos = 0;
                var index = 0;
                for(var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        index = textureData[pos/2];
                        index = pos % 2 != 0 ? index & 0xF : (index & 0xF0) >> 4;
                        var color = palette[index];
                        bitmap.SetPixel(x, y, color);
                        pos++;
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
