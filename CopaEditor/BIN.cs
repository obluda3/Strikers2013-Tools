using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace StrikersTools
{
    class BIN
    {

        public static void ExportFiles(string input)
        {
            var binfile = File.OpenRead(input);
            var folder = Path.GetDirectoryName(input) + "\\" + Path.GetFileNameWithoutExtension(input);
            Directory.CreateDirectory(folder);

            using (var br = new BinaryReader(binfile))
            {
                var fileCount = br.ReadInt32();
                var padFactor = br.ReadInt32();
                var mulFactor = br.ReadInt32();
                var shiftFactor = br.ReadInt32();
                var mask = br.ReadInt32();

                for (var i = 0; i < fileCount; i++)
                {
                    var offSize = br.ReadUInt32();
                    var bkPos = br.BaseStream.Position;
                    
                    var offset = (offSize >> shiftFactor) * padFactor;
                    var size = (offSize & mask) * mulFactor;

                    var filename = GetFileName(i, binfile);
                    var output = File.Open(folder+filename, FileMode.Create);

                    using (var bw = new BinaryWriter(output))
                    {
                        bw.Write(br.ReadBytes((int)size));
                    }

                    br.BaseStream.Position = bkPos;
                }
            }
        }

        public static string GuessExtension(Stream input)
        {
            var magicSamples = CollectMagicSamples(input);

            if (magicSamples.Contains(0x55AA382D))
                return "arc";

            if (magicSamples.Contains(0x52415344))
                return "rasd";

            if (magicSamples.Contains(0x53485458))
                return "shtx";

            if (magicSamples.Contains(0x53534144))
                return "ssad";

            if (magicSamples.Contains(0x434d504b))
                return "cpmk";

            if (magicSamples.Contains(StringToUInt32("bres")))
                return "brres";

            return "bin";
        }

        private static uint StringToUInt32(string text)
        {
            return BufferToUInt32(Encoding.UTF8.GetBytes(text));
        }
        private static uint BufferToUInt32(byte[] buffer)
        {
            return (uint)((buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3]);
        }

        private static IList<uint> CollectMagicSamples(Stream input)
        {
            var bkPos = input.Position;

            input.Position = bkPos;
            var magic1 = PeekUInt32(input);
            input.Position = bkPos + 1;
            var magic2 = PeekUInt32(input);
            input.Position = bkPos + 2;
            var magic3 = PeekUInt32(input);

            input.Position = bkPos + 12;
            var magic4 = PeekUInt32(input);
            input.Position = bkPos + 13;
            var magic5 = PeekUInt32(input);
            input.Position = bkPos + 14;
            var magic6 = PeekUInt32(input);

            return new[] { magic1, magic2, magic3, magic4, magic5, magic6 };
        }

        private static uint PeekUInt32(Stream input)
        {
            var bkPos = input.Position;

            var buffer = new byte[4];
            input.Read(buffer, 0, 4);

            input.Position = bkPos;

            return BufferToUInt32(buffer);
        }   
        private static string GetFileName(int index, Stream input)
        {
            var extension = GuessExtension(input);

            return $"{index:00000000}.{extension}";
        }


    }
}
