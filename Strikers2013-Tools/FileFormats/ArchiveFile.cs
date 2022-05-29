using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using StrikersTools.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace StrikersTools.FileFormats
{
    class ArchiveFileInfo 
    {
        public uint Size { get; set; }
        public byte[] Data { get; set; }
        public long Offset { get; set; }
        public long OldOffset { get; set; }
        public int Index { get; set; }
        public bool Modified { get; set; } = false;
        public string Path { get; set; } = "";

        public ArchiveFileInfo(uint size, long offset, byte[] data, int index)
        {
            Size = size;
            Offset = offset;
            OldOffset = offset;
            Index = index;
            Data = data;
        }
        public ArchiveFileInfo(uint size, long offset, int index)
        {
            Size = size;
            Offset = offset;
            OldOffset = offset;
            Index = index;
        }
    }
    class ArchiveFile
    {
        // https://github.com/FanTranslatorsInternational/Kuriimu2/blob/dev/plugins/Shade/plugin_shade/Archives/BlnSubSupport.cs
        // 0x00 => grp.bin
        // 0x01 => scn.bin
        // 0x02 => scn_sh.bin
        // 0x03 => ui.bin
        // 0x04 => dat.bin
        public static string[] ArchiveNames = { "grp.bin", "scn.bin", "scn_sh.bin", "ui.bin", "dat.bin", "strap.bin" };
        public int ArchiveIndex { get; private set; }
        private string fileName;

        private int fileCount;
        private int padFactor;
        private int multFactor;
        private int shiftFactor;
        private int mask;

        public List<ArchiveFileInfo> Files { get; private set; }

        public ArchiveFile(string filename, bool getData) 
        {
            fileName = filename;
            var archiveFileName = Path.GetFileName(filename);
            var archiveIndex = -1;

            for (var i = 0; i < ArchiveNames.Length; i++)
            {
                if (archiveFileName.Contains(ArchiveNames[i]))
                {
                    archiveIndex = i;
                    break;
                }
            }
            ArchiveIndex = archiveIndex;

            var fileStream = File.OpenRead(filename);
            using (var br = new BinaryReader(fileStream)) // Parses header
            {
                fileCount = br.ReadInt32();
                padFactor = br.ReadInt32();
                multFactor = br.ReadInt32();
                shiftFactor = br.ReadInt32();
                mask = br.ReadInt32();

                Files = GetFileInfos(br, getData);
            }
        }

        private List<ArchiveFileInfo> GetFileInfos(BinaryReader br, bool getData)
        {
            var files = new List<ArchiveFileInfo>();

            for (var i = 0; i < fileCount; i++)
            {
                br.BaseStream.Position = 0x14 + 4 * i;

                var offSize = br.ReadUInt32();
                var offset = (offSize >> shiftFactor) * padFactor;
                var size = (uint)((offSize & mask) * multFactor);

                br.BaseStream.Position = offset;
                var paddedSize = (uint) ((size + padFactor - 1) & ~(padFactor - 1));
                ArchiveFileInfo afi;
                if(paddedSize == 0 || size == 0)
                {
                    size = paddedSize = (uint)padFactor; // hack
                }
                if (getData)
                {
                    var data = br.ReadBytes((int)paddedSize);

                    afi = new ArchiveFileInfo(paddedSize, offset, data, i);
                }
                else
                {
                    afi = new ArchiveFileInfo(paddedSize, offset, i);
                }
                files.Add(afi);
            }
            return files;
        }

        public byte[] ExtractFile(int index)
        {
            byte[] output = new byte[0];
            try
            {
                output = ShadeLz.Decompress(Files[index].Data);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine($"File could not be decompressed");
            }
            return output;
        }

        public async Task ExtractFiles(IProgress<int> progress)
        {
            await Task.Run(() =>
           {
               var folder = Path.GetDirectoryName(fileName) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fileName) + Path.DirectorySeparatorChar;
               Directory.CreateDirectory(folder);
               var decompressedFolder = folder + Path.DirectorySeparatorChar + "dec\\";
               Directory.CreateDirectory(decompressedFolder);

               foreach (var file in Files)
               {
                   // Create output file
                   var filename = GetFileName(file.Index);
                   var extension = Path.GetExtension(filename);
                   var outputPath = Path.GetFileNameWithoutExtension(filename) + ".bin";
                   var output = File.Open(folder + outputPath, FileMode.Create);
                   output.Write(file.Data, 0, file.Data.Length);
                   output.Close();
                   try
                   {
                        var decompressedData = ShadeLz.Decompress(file.Data);
                        var outPath = decompressedFolder + Path.GetFileNameWithoutExtension(filename) + "_dec" + extension;
                        var outDecompressed = File.Open(outPath, FileMode.Create);
                        outDecompressed.Write(decompressedData, 0, decompressedData.Length);
                        outDecompressed.Close();

                        var shtxMagic = new List<byte> { 0x53, 0x48, 0x54, 0x58 };
                        if (decompressedData.Take(4).SequenceEqual(shtxMagic))
                        {
                           var bitmap = SHTX.Export(outPath);
                           bitmap.Save(outPath + ".png");
                        }
                   }
                   catch(Exception)
                   {
                        Console.WriteLine($"File {filename} could not be decompressed");
                   }
                    
                   progress.Report(file.Index * 100 / Files.Count);
               }
           });
        }

        public void ImportFile(int index, byte[] data, bool isDecompressed)
        {
            var fileInfo = Files[index];

            var fileData = data;
            if(isDecompressed)
            {
                Console.WriteLine($"Compressing {fileInfo.Path}...");
                var needsHeader = !fileName.Contains("dat");
                fileData = ShadeLz.Compress(data, needsHeader);
            }

            fileInfo.Data = fileData.Padded(padFactor);
            fileInfo.Modified = true;
            fileInfo.Size = (uint)((fileData.Length + padFactor - 1) & ~(padFactor - 1));

            Files[index] = fileInfo;
        }
        public void ImportFile(int index, string path)
        {
            var data = File.ReadAllBytes(path);
            var fileInfo = Files[index];
            var isDecompressed = Path.GetExtension(path).EndsWith(".dec");
            
            fileInfo.Path = path;
            ImportFile(index, data, isDecompressed);
        }

        public void ImportFiles(string inputFolder)
        {
            var files = Directory.GetFiles(inputFolder);
            Console.WriteLine($"Importing {files.Length} files...");
            foreach (var file in files)
            {
                int index;
                try
                {
                    index = Convert.ToInt32(Path.GetFileNameWithoutExtension(file).Split('.')[0].Split('_')[0]);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Not a valid filename: {file}");
                    continue;
                }

                ImportFile(index, file);
            }
        }

        public async Task Save(string path)
        {
            var file = File.Open(path, FileMode.Create);
            using (var bw = new BinaryWriter(file))
            {
                bw.Write(fileCount);
                bw.Write(padFactor);
                bw.Write(multFactor);
                bw.Write(shiftFactor);
                bw.Write(mask);

                bw.BaseStream.Position += fileCount * 4;

                bw.WriteAlignment(padFactor);

                foreach(var fileInfo in Files)
                {
                    fileInfo.OldOffset = fileInfo.Offset;
                    fileInfo.Offset = bw.BaseStream.Position;

                    bw.Write(fileInfo.Data);

                    var padSize = fileInfo.Size - fileInfo.Data.Length;
                    bw.PadWith(0, padSize);
                    bw.WriteAlignment(padFactor);
                }

                bw.BaseStream.Position = 0x14;
                foreach(var fileInfo in Files)
                {
                    bw.Write(GetOffSizeFromOffsetAndSize((uint)fileInfo.Offset, fileInfo.Size));
                }
            }
        }
        private string GetFileName(int index)
        {
            var extension = GuessExtension(index);

            return $"{index:00000000}.{extension}";
        }

        private uint GetOffSizeFromOffsetAndSize(uint offset, uint size)
        {
            uint offsize = 0;
            offsize |= (uint)(offset / padFactor) << shiftFactor;
            offsize |= (uint)(size / multFactor);
            return offsize;
        }

        private string GuessExtension(int index)
        {
            var magicSamples = CollectMagicSamples(index);

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

            if (magicSamples.Contains(0x62726573))
                return "brres";

            return "bin";
        }


        private IList<uint> CollectMagicSamples(int index)
        {
            var dataStream = new MemoryStream(Files[index].Data);

            using (var br = new BeBinaryReader(dataStream))
            {
                var bkPos = br.BaseStream.Position;
                br.BaseStream.Position = bkPos;
                var magic1 = br.PeekUInt32();
                br.BaseStream.Position = bkPos + 1;
                var magic2 = br.PeekUInt32();
                br.BaseStream.Position = bkPos + 2;
                var magic3 = br.PeekUInt32();

                br.BaseStream.Position = bkPos + 12;
                var magic4 = br.PeekUInt32();
                br.BaseStream.Position = bkPos + 13;
                var magic5 = br.PeekUInt32();
                br.BaseStream.Position = bkPos + 14;
                var magic6 = br.PeekUInt32();

                return new[] { magic1, magic2, magic3, magic4, magic5, magic6 };
            }
        }
    }
}
