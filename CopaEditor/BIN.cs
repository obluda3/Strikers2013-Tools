using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using StrikersTools.Utils;

namespace StrikersTools
{
    class BIN
    {

        public static void ExportFiles(string input)
        {
            var binfile = File.OpenRead(input);
            var folder = Path.GetDirectoryName(input) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(input);
            Directory.CreateDirectory(folder);

            using (var br = new BinaryReader(binfile))
            {
                // Reads header
                var fileCount = br.ReadInt32();
                var padFactor = br.ReadInt32();
                var mulFactor = br.ReadInt32();
                var shiftFactor = br.ReadInt32();
                var mask = br.ReadInt32();

                for (var i = 0; i < fileCount; i++)
                {
                    // Get offset and size
                    uint size;
                    var offset = GetOffsetAndSize(padFactor, mulFactor, shiftFactor, mask, i, binfile, out size);

                    // Create output file
                    var filename = GetFileName(i, binfile);
                    var output = File.Open(folder+"\\"+filename, FileMode.Create);

                    // Write data to output file
                    br.BaseStream.Position = offset;
                    using (var bw = new BinaryWriter(output))
                    {
                        bw.Write(br.ReadBytes((int)size));
                    }

                }
            }
        }

        public static List<uint> ImportFiles(string inputFolder, string binPath)
        {
            var binfile = File.Open(binPath, FileMode.Open,FileAccess.ReadWrite);
            var archiveOffsets = new List<uint>;

            using (var br = new BinaryReader(binfile))
            {
                using (var bw = new BinaryWriter(binfile))
                {
                    // Reads header
                    var fileCount = br.ReadInt32();
                    var padFactor = br.ReadInt32();
                    var mulFactor = br.ReadInt32();
                    var shiftFactor = br.ReadInt32();
                    var mask = br.ReadInt32();

                    var files = Directory.GetFiles(inputFolder);

                    foreach(var file in files)
                    {
                        // Gets the index of the file in the archive
                        var index = Convert.ToInt32(Path.GetFileNameWithoutExtension(file).Split('.')[0]);

                        // Can't add files yet
                        if(index >= fileCount)
                        {
                            Console.WriteLine("{0} skipped : index {1} over the file count limit ({2})",Path.GetFileName(file),index,fileCount);
                            continue;
                        }

                        var fileStream = File.OpenRead(file);

                        uint originalSize;
                        var offset = GetOffsetAndSize(padFactor, mulFactor, shiftFactor, mask, index, binfile, out originalSize);
                        var size = fileStream.Length;
                        archiveOffsets.Add((uint)offset);

                        // Doesn't support changing the file size
                        if (size > originalSize)
                        {
                            Console.WriteLine("{0} skipped : size {1} over original size of {2}", Path.GetFileName(file), size, originalSize);
                            continue;
                        }

                        Console.WriteLine("File {0} :\n\t- Size : {1}\n\t- Offset : {2}\n\t- Index : {3}",Path.GetFileName(file),size,offset,index);
                        // Write the modified data
                        binfile.Position = offset;
                        using (var _br = new BinaryReader(fileStream))
                        {
                            bw.Write(_br.ReadBytes((int)size));
                        }

                        // Pads to original size
                        var padSize = originalSize - size;
                        bw.PadWith(0, padSize);

                    }
                }
            }
            return archiveOffsets;
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
            return (uint)((buffer[3] << 24) | (buffer[2] << 16) | (buffer[1] << 8) | buffer[0]);
        }

        private static IList<uint> CollectMagicSamples(Stream input)
        {
            var bkPos = input.Position;

            using (var br = new BinaryReader(input))
            {
                input.Position = bkPos;
                var magic1 = br.PeekUInt32();
                input.Position = bkPos + 1;
                var magic2 = br.PeekUInt32();
                input.Position = bkPos + 2;
                var magic3 = br.PeekUInt32();

                input.Position = bkPos + 12;
                var magic4 = br.PeekUInt32();
                input.Position = bkPos + 13;
                var magic5 = br.PeekUInt32();
                input.Position = bkPos + 14;
                var magic6 = br.PeekUInt32();

                return new[] { magic1, magic2, magic3, magic4, magic5, magic6 };
            }
        }

        private static string GetFileName(int index, Stream input)
        {
            var extension = GuessExtension(input);

            return $"{index:00000000}.{extension}";
        }

        private static long GetOffsetAndSize(int padFactor, int mulFactor, int shiftFactor, int mask, int index, Stream input, out uint size)
        {
            input.Position = 20 + index * 4;

            using (var br = new BinaryReader(input))
            {
                var offSize = br.ReadUInt32();

                var offset = (offSize >> shiftFactor) * padFactor;
                size = (uint)((offSize & mask) * mulFactor);

                return offset;
            }
        }
    }
}
