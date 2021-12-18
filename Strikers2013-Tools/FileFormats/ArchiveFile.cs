using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using StrikersTools.Utils;
using System.Linq;

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
    }
    class ArchiveFile
    {
        // https://github.com/FanTranslatorsInternational/Kuriimu2/blob/dev/plugins/Shade/plugin_shade/Archives/BlnSubSupport.cs
        // 0x00 => grp.bin
        // 0x01 => scn.bin
        // 0x02 => scn_sh.bin
        // 0x03 => ui.bin
        // 0x04 => dat.bin
        public static string[] ArchiveNames = { "grp.bin", "scn.bin", "scn_sh.bin", "ui.bin", "dat.bin" };

        private string FileName;

        private int FileCount;
        private int PadFactor;
        private int MultFactor;
        private int ShiftFactor;
        private int Mask;

        public List<ArchiveFileInfo> Files { get; private set; }

        public ArchiveFile(string filename) 
        {
            FileName = filename;

            var fileStream = File.OpenRead(filename);
            using (var br = new BinaryReader(fileStream)) // Parses header
            {
                FileCount = br.ReadInt32();
                PadFactor = br.ReadInt32();
                MultFactor = br.ReadInt32();
                ShiftFactor = br.ReadInt32();
                Mask = br.ReadInt32();

                Files = GetFileInfos(br);
            }
        }

        private List<ArchiveFileInfo> GetFileInfos(BinaryReader br)
        {
            var files = new List<ArchiveFileInfo>();

            for (var i = 0; i < FileCount; i++)
            {
                br.BaseStream.Position = 0x14 + 4 * i;

                var offSize = br.ReadUInt32();
                var offset = (offSize >> ShiftFactor) * PadFactor;
                var size = (uint)((offSize & Mask) * MultFactor);

                br.BaseStream.Position = offset;
                var paddedSize = (uint) ((size + PadFactor - 1) & ~(PadFactor - 1));
                var data = br.ReadBytes((int)paddedSize);

                var afi = new ArchiveFileInfo(size, offset, data, i);
                files.Add(afi);
            }
            return files;
        }
        public void ExtractFiles(IProgress<int> progress)
        {
            var folder = Path.GetDirectoryName(FileName) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(FileName) + Path.DirectorySeparatorChar; 
            Directory.CreateDirectory(folder);

            foreach (var file in Files)
            {
                // Create output file
                var filename = GetFileName(file.Index);
                var output = File.Open(folder + filename, FileMode.Create);

                output.Write(file.Data, 0, file.Data.Length);
                progress.Report(file.Index * 10000 / Files.Count);
            }
        }

        public void ImportFile(int index, string path)
        {
            var data = File.ReadAllBytes(path);
            var fileInfo = Files[index];
            fileInfo.Data = data;
            fileInfo.Modified = true;
            fileInfo.Path = path;
            fileInfo.Size = (uint) ((data.Length + PadFactor - 1) & ~(PadFactor-1));

            Files[index] = fileInfo;
        }

        public void ImportFiles(string inputFolder)
        {
            var files = Directory.GetFiles(inputFolder);
            foreach (var file in files)
            {
                int index = -1;
                try
                {
                    index = Convert.ToInt32(Path.GetFileNameWithoutExtension(file).Split('.')[0]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Not a valid filename: ", file);
                    continue;
                }

                ImportFile(index, file);
            }
        }

        public void Save(string path)
        {
            var file = File.Open(path, FileMode.Create);
            using (var bw = new BinaryWriter(file))
            {
                bw.Write(FileCount);
                bw.Write(PadFactor);
                bw.Write(MultFactor);
                bw.Write(ShiftFactor);
                bw.Write(Mask);

                for (var i = 0; i < FileCount; i++) bw.Write(0); // file table, filled later

                bw.WriteAlignment(PadFactor);

                foreach(var fileInfo in Files)
                {
                    fileInfo.OldOffset = fileInfo.Offset;
                    fileInfo.Offset = bw.BaseStream.Position;

                    bw.Write(fileInfo.Data);

                    var padSize = fileInfo.Size - fileInfo.Data.Length;
                    bw.PadWith(0, padSize);
                    bw.WriteAlignment(PadFactor);
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
            offsize |= (uint)(offset / PadFactor) << ShiftFactor;
            offsize |= (uint)(size / MultFactor);
            return offsize;
        }


        /*
        public static ArchiveFileInfo[] ImportFiles(string inputFolder, string binPath)
        {
            ArchiveFileInfo[] result;
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
                    var fileTable = new ArchiveFileInfo[fileCount];
                    for (var i = 0; i < fileCount; i++)
                        fileTable[i] = new ArchiveFileInfo(br, shiftFactor, padFactor, mulFactor, mask, i);

                    // Get modified files's filenames
                    foreach (var file in files)
                    {
                        try
                        {
                            var index = Convert.ToInt32(Path.GetFileNameWithoutExtension(file).Split('.')[0]);
                            fileTable[index].Modified = true;
                            fileTable[index].Path = file;
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

                        if (file.Modified)
                        {
                            Console.WriteLine("File {0}: Index: {1}", Path.GetFileName(file.Path), file.Index);
                            var data = File.ReadAllBytes(file.Path);
                            bw.Write(data);
                            file.Size = (uint)(padFactor * ((data.Length + padFactor - 1) / padFactor)); // Align to 0x10 bytes        
                        }
                        else
                        {
                            br.BaseStream.Position = file.Offset;

                            // For some reason, some files do not have the correct size
                            uint size = 0;
                            if(br.ReadUInt32() == 0xA755AAFC) // Gets the size from the compression header
                            {
                                br.ReadUInt32();
                                size = br.ReadUInt32();
                                br.BaseStream.Position -= 8;
                            }
                            br.BaseStream.Position -= 4;
                            file.TrueSize = Math.Max(size, file.Size);

                            var fileData = br.ReadBytes((int)file.TrueSize);
                            bw.Write(fileData);
                        }

                        file.PaddedSize = (uint)(padFactor * ((file.Size + padFactor - 1) / padFactor));

                        bw.WriteAlignment(padFactor);
                        file.Offset = newOffset;

                        fileTable[i] = file;
                    }

                    bw.BaseStream.Position = 0x14; // After the header
                    foreach(var fileInfo in fileTable)
                    {
                        uint offsize = 0;
                        offsize |= (uint)(fileInfo.Offset / padFactor) << shiftFactor;
                        offsize |= (uint)(fileInfo.Size / mulFactor);

                        bw.Write(offsize);
                    }

                    result = fileTable;
                }
            }
            File.Move(binPath, binPath.Replace(".bin", ".old"));
            File.Move(binPath + ".tmp", binPath);
            
            return result;
        }*/

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
