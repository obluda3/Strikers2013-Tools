using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using StrikersTools.Utils;
using System.Linq;

namespace StrikersTools.FileFormats
{
    struct BinFileInfo 
    {
        public uint size;
        public uint paddedSize;
        public uint trueSize;
        public long offset;
        public long oldOffset;
        public int index;
        public bool modified;
        public string modifiedFilePath;
        public BinFileInfo(BinaryReader br, int shiftFactor, int padFactor, int mulFactor, int mask, int i)
        {
            var offSize = br.ReadUInt32();

            offset = (offSize >> shiftFactor) * padFactor;
            size = (uint)((offSize & mask) * mulFactor);
            paddedSize = size;
            trueSize = size;

            oldOffset = offset;
            index = i;
            modified = false;
            modifiedFilePath = "";
        }
    }
    class BIN
    {
        // https://github.com/FanTranslatorsInternational/Kuriimu2/blob/dev/plugins/Shade/plugin_shade/Archives/BlnSubSupport.cs
        // 0x00 => grp.bin
        // 0x01 => scn.bin
        // 0x02 => scn_sh.bin
        // 0x03 => ui.bin
        // 0x04 => dat.bin
        public static string[] ArchiveNames = { "grp.bin", "scn.bin", "scn_sh.bin", "ui.bin", "dat.bin" };
        public static void ExportFiles(string input)
        {
            var binfile = File.OpenRead(input);
            var folder = Path.GetDirectoryName(input) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(input);
            Directory.CreateDirectory(folder);

            using (var br = new BinaryReader(binfile))
            {
                var files = GetFiles(br);

                foreach (var file in files)
                {
                    // Create output file
                    var filename = GetFileName(file.index, binfile);
                    var output = File.Open(folder+"\\"+filename, FileMode.Create);

                    // Write data to output file
                    br.BaseStream.Position = file.offset;
                    using (var bw = new BinaryWriter(output))
                    {
                        bw.Write(br.ReadBytes((int)file.size));
                    }

                }
            }
        }

        public static BinFileInfo[] ImportFiles(string inputFolder, string binPath)
        {
            BinFileInfo[] result;
            var binfile = File.Open(binPath, FileMode.Open);
            var tempFile = File.OpenWrite(binPath + ".tmp");
            using (var br = new BinaryReader(binfile))
            {
                using (var bw = new BinaryWriter(tempFile))
                {
                    // Reads header
                    var fileCount = br.ReadInt32();
                    var padFactor = br.ReadInt32();
                    var mulFactor = br.ReadInt32();
                    var shiftFactor = br.ReadInt32();
                    var mask = br.ReadInt32();

                    var files = Directory.GetFiles(inputFolder);

                    // Get file info
                    var fileTable = new BinFileInfo[fileCount];
                    for (var i = 0; i < fileCount; i++)
                        fileTable[i] = new BinFileInfo(br, shiftFactor, padFactor, mulFactor, mask, i);

                    // Get modified files's filenames
                    foreach (var file in files)
                    {
                        try
                        {
                            var index = Convert.ToInt32(Path.GetFileNameWithoutExtension(file).Split('.')[0]);
                            fileTable[index].modified = true;
                            fileTable[index].modifiedFilePath = file;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Not a valid filename: ", file);
                            continue;
                        }
                    }

                    // Write header
                    bw.Write(fileCount);
                    bw.Write(padFactor);
                    bw.Write(mulFactor);
                    bw.Write(shiftFactor);
                    bw.Write(mask);

                    // Placeholder for the file table
                    for(var i = 0; i < fileCount; i++) bw.Write(0);

                    bw.WriteAlignment(padFactor);
                    br.SeekAlignment(padFactor);
                    for (var i = 0; i < fileCount; i++)
                    {
                        var file = fileTable[i];
                        // Update offset
                        var newOffset = bw.BaseStream.Position;

                        if (file.modified)
                        {
                            Console.WriteLine("File {0}: Index: {1}", Path.GetFileName(file.modifiedFilePath), file.index);
                            var data = File.ReadAllBytes(file.modifiedFilePath);
                            bw.Write(data);
                            file.size = (uint)(padFactor * ((data.Length + padFactor - 1) / padFactor)); // Align to 0x10 bytes        
                        }
                        else
                        {
                            br.BaseStream.Position = file.offset;

                            // For some reason, some files do not have the correct size
                            uint size = 0;
                            if(br.ReadUInt32() == 0xA755AAFC) // Gets the size from the compression header
                            {
                                br.ReadUInt32();
                                size = br.ReadUInt32();
                                br.BaseStream.Position -= 8;
                            }
                            br.BaseStream.Position -= 4;
                            file.trueSize = Math.Max(size, file.size);

                            var fileData = br.ReadBytes((int)file.trueSize);
                            bw.Write(fileData);
                        }

                        file.paddedSize = (uint)(padFactor * ((file.size + padFactor - 1) / padFactor));

                        bw.WriteAlignment(padFactor);
                        file.offset = newOffset;

                        fileTable[i] = file;
                    }

                    bw.BaseStream.Position = 0x14; // After the header
                    foreach(var fileInfo in fileTable)
                    {
                        uint offsize = 0;
                        offsize |= (uint)(fileInfo.offset / padFactor) << shiftFactor;
                        offsize |= (uint)(fileInfo.size / mulFactor);

                        bw.Write(offsize);
                    }

                    result = fileTable;
                }
            }
            File.Move(binPath,binPath + ".old");
            File.Move(binPath + ".tmp", binPath);
            
            return result;
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
            var br = new BinaryReader(input);

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

        private static string GetFileName(int index, Stream input)
        {
            var extension = GuessExtension(input);

            return $"{index:00000000}.{extension}";
        }

        public static List<BinFileInfo> GetFiles(string folder, string name)
        {
            var file = File.OpenRead(folder + "\\" + name + ".bin");
            var br = new BinaryReader(file);

            var output = GetFiles(br);
            br.BaseStream.Close();
            return output;
        }

        private static List<BinFileInfo> GetFiles(BinaryReader br)
        {
            var fileCount = br.ReadInt32();
            var padFactor = br.ReadInt32();
            var mulFactor = br.ReadInt32();
            var shiftFactor = br.ReadInt32();
            var mask = br.ReadInt32();

            var files = new List<BinFileInfo>();
            for (var i = 0; i < fileCount; i++)
                files.Add(new BinFileInfo(br, shiftFactor, padFactor, mulFactor, mask, i));
            return files;
        }
    }
}
