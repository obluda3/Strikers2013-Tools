using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using StrikersTools.Utils;

namespace StrikersTools.FileFormats
{
    class Font
    {
        private List<Letter> _letters;
        private string _fontName;
        public Font(string path)
        {
            using (var file = File.OpenRead(path))
            {
                _letters = GetLetters(file);
            }
            _fontName = path;
        }
        public void ExtractFont()
        {
            var outputFolder = Path.GetDirectoryName(Path.GetFullPath(_fontName)) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(_fontName) + "_extracted" 
                + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(outputFolder);

            foreach (var letter in _letters)
            {
                var data = ShadeLz.Decompress(letter.data);

                var pos = 0;
                var bitmap = new Bitmap(24, 25);
                for (var y = 0; y < 25; y++)
                {
                    for (var x = 0; x < 24; x++)
                    {
                        var curByte = data[pos / 2];

                        // Turns out that nibbles in A4 images need to be "doubled"
                        // For example with 0x14, when pos % 2 == 0, we don't take
                        // 4 or 0x40, but 0x44
                        var alphaData = pos % 2 == 0 ? ((curByte & 0xF) << 4) | (curByte & 0xF) : (curByte & 0xF0) | ((curByte & 0xF0) >> 4);

                        bitmap.SetPixel(x, y, Color.FromArgb(alphaData, 255, 255, 255));
                        pos += 1;
                    }
                }

                var fileName = $"{letter.index:00000000}.png";
                bitmap.Save(outputFolder + Path.DirectorySeparatorChar + fileName);
            }
        }

        public void ImportLetters(string inputFolder)
        {
            var files = Directory.GetFiles(inputFolder);
            _letters.OrderBy(x => x.index);

            foreach (var file in files)
            {
                var index = Convert.ToInt32(Path.GetFileNameWithoutExtension(file).Split('.')[0]);
                var letter = _letters[index];
                var data = PNGToA4(file);
                letter.data = ShadeLz.Compress(data, false);

                _letters[index] = letter;
            }
            for (var i = 0; i < _letters.Count; i++)
            {
                var letter = _letters[i];
                var cmpData = letter.data;
                var decData = ShadeLz.Decompress(cmpData);
                var newCmpData = ShadeLz.Compress(decData, false);

                var finalCmpData = cmpData.Length < newCmpData.Length ? cmpData : newCmpData;
                letter.data = finalCmpData;
                _letters[i] = letter;
            }
            var output = File.Open(_fontName + ".out", FileMode.Create);
            using(var bw = new BinaryWriter(output))
            {
                bw.Write(_letters.Count);

                for (var i = 0; i < _letters.Count; i++) bw.Write((int)0); // Pads with 0s, will be filled later on

                for (var i = 0; i < _letters.Count; i++)
                {
                    var letter = _letters[i];
                    if (_letters[0].data.SequenceEqual(letter.data) && letter.index != 0)
                        letter.offset = _letters[0].offset;
                    else
                    {
                        letter.offset = (int)bw.BaseStream.Position;
                        bw.Write(letter.data);
                        bw.Write((byte)0);
                    }
                    _letters[i] = letter;
                }
                
                bw.BaseStream.Position = 4;
                foreach (var letter in _letters)
                    bw.Write(letter.offset);
            }
        }

        private byte[] PNGToA4(string input)
        {
            var img = new Bitmap(input);
            var output = new byte[img.Width * img.Height / 2];

            var pos = 0;
            for(var y = 0; y < img.Height; y++)
            {
                for(var x = 0; x < img.Width; x++)
                {
                    var alpha = img.GetPixel(x, y).A;
                    alpha = (byte)(alpha & 0xF0);

                    if (pos % 2 == 0) alpha >>= 4;

                    output[pos / 2] |= alpha;
                    pos += 1;
                }
            }

            return output;
        }

        private List<Letter> GetLetters(Stream input)
        {
            var br = new BinaryReader(input);
            var entryCount = br.ReadInt32();
            var letters = new List<Letter>();
            
            // Get offsets
            for(var i = 0; i < entryCount; i++)
            {
                var offset = br.ReadInt32();
                var letter = new Letter();
                letter.offset = offset;
                letter.index = i;
                letters.Add(letter);
            }

            // Get data
            letters.OrderBy(x => x.offset);
            for(var i = 0; i < letters.Count; i++)
            {
                var letter = letters[i];
                var nextOffset = letters.FirstOrDefault(x => x.offset > letter.offset).offset;

                if(nextOffset == default(int))
                {
                    nextOffset = (int)input.Length;
                }

                var length = nextOffset - letter.offset;

                var data = new byte[length];
                input.Position = letter.offset;
                input.Read(data, 0, length);
                letter.data = data;
                letters[i] = letter;
            }

            return letters;
        }
    }

    struct Letter
    {
        public int offset;
        public byte[] data;
        public int index;
    }
}
