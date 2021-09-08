using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using StrikersTools.Utils;
using System.Linq;

namespace StrikersTools.FileFormats
{
    struct Mcb0Entry
    {
        public short unk1;
        public short unk2;
        public uint offset;
        public uint size;

        public Mcb0Entry(short unk1, short unk2, uint offset, uint size)
        {
            this.unk1 = unk1;
            this.unk2 = unk2;
            this.offset = offset;
            this.size = size;
        }
    }
    class Mcb1File
    {
        public int archiveId;
        public uint archiveOffset;
        public int size;

        public int index;
        public Mcb1File(BinaryReader br)
        {
            archiveId = br.ReadInt32();
            archiveOffset = br.ReadUInt32();
            size = br.ReadInt32();
        }
    }


    
    class BLN
    {
        // 0x00 => grp.bin
        // 0x01 => scn.bin
        // 0x02 => scn_sh.bin
        // 0x03 => ui.bin
        // 0x04 => dat.bin
        public static Dictionary<int, string> ArchiveNames = new Dictionary<int, string>()
        {
            [0] = "grp",
            [1] = "scn",
            [2] = "scn_sh",
            [3] = "ui",
            [4] = "dat"
        };
        public static void BrowseBln(string inputFolder)
        {
            var archives = new List<BinFileInfo>[5];
            archives[0] = BIN.GetFiles(inputFolder, "grp");
            archives[1] = BIN.GetFiles(inputFolder, "scn");
            archives[2] = BIN.GetFiles(inputFolder, "scn_sh");
            archives[3] = BIN.GetFiles(inputFolder, "ui");
            archives[4] = BIN.GetFiles(inputFolder, "dat");

            var mcb0Path = Path.GetDirectoryName(inputFolder) + Path.DirectorySeparatorChar + "mcb0.bln";
            var mcb0 = File.OpenRead(mcb0Path);
            var mcb0Entries = GetMcb0Entries(mcb0);
            mcb0.Close();

            var mcb1 = File.Open(inputFolder + "\\mcb1.bln", FileMode.Open);
            var subBlns = new List<Mcb1File>[mcb0Entries.Count];

            using (var br = new BinaryReader(mcb1))
            {
                foreach (var mcb0Entry in mcb0Entries)
                {
                    var subBln = new List<Mcb1File>();
                    br.BaseStream.Position = mcb0Entry.offset;

                    while (br.BaseStream.Position < mcb0Entry.offset + mcb0Entry.size)
                    {
                        var sample = br.ReadInt32();
                        if (sample == 0x7FFF)
                            break;
                        br.BaseStream.Position -= 4;

                        var file = new Mcb1File(br);
                        file.index = archives[file.archiveId].Where(x => x.offset == file.archiveOffset).First().index;

                        subBln.Add(file);

                        br.BaseStream.Position += file.size;
                    }
                }
            }

            // Now we can actually import files

            // Rebuilding the mcb1.bln (the most tedious part)
            mcb1 = File.Open(inputFolder + "\\mcb1.bln", FileMode.Create);
            using (var bw = new BinaryWriter(mcb1))
            {
                var i = 0;
                foreach (var subBln in subBlns)
                {
                    var mcb0Entry = mcb0Entries[i];
                    mcb0Entry.offset = (uint)bw.BaseStream.Position;

                    foreach(var file in subBln)
                    {
                        var fileInBin = archives[file.archiveId][file.index];
                        bw.Write(file.archiveId);
                        bw.Write(fileInBin.offset);
                        bw.Write(fileInBin.size);

                        bw.Write(BIN.GetFile(inputFolder, ArchiveNames[file.archiveId], (int)fileInBin.offset, (int)fileInBin.size));
                        bw.WriteAlignment(0x4000, 0);
                    }
                    // terminator
                    bw.Write(0x7Fff);

                    bw.WriteAlignment(0x800,0);
                    mcb0Entry.size = (uint)bw.BaseStream.Position - mcb0Entry.offset;
                }
            }

        }
        public static void RepackArchiveAndBLN(string inputFolder, string binPath, string blnPath)
        {
            var offsets = BIN.ImportFiles(inputFolder, binPath);
            var archiveName = Path.GetFileNameWithoutExtension(binPath);

            // https://github.com/FanTranslatorsInternational/Kuriimu2/blob/dev/plugins/Shade/plugin_shade/Archives/BlnSubSupport.cs
            // Archive index maps to the following bins for Inazuma Eleven Strikers 2013
            // 0x00 => grp.bin
            // 0x01 => scn.bin
            // 0x02 => scn_sh.bin
            // 0x03 => ui.bin
            // 0x04 => dat.bin
            // Not the best solution, but that's the only one I can think of
            int archiveIndex = 5;
            switch (archiveName.ToLower())
            {
                case "strap":
                    Console.WriteLine("strap.bin isn't copied to the BLN");
                    break;
                case "grp":
                    archiveIndex = 0;
                    break;
                case "scn":
                    archiveIndex = 1;
                    break;
                case "scn_sh":
                    archiveIndex = 2;
                    break;
                case "ui":
                    archiveIndex = 3;
                    break;
                case "dat":
                    archiveIndex = 4;
                    break;
                default:
                    Console.WriteLine("{0} is not a valid archive name, can't import it to the BLN", archiveName);
                    break;
            }

            // Get mcb0 entries offsets
            var mcb0Path = Path.GetDirectoryName(blnPath) + Path.DirectorySeparatorChar + "mcb0.bln";
            var mcb0 = File.OpenRead(mcb0Path);
            var mcb0Entries = GetMcb0Entries(mcb0);
            mcb0.Close();

            using (var mcb1 = File.Open(blnPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var binArchive = File.Open(binPath, FileMode.Open))
                {
                    using (var br = new BinaryReader(mcb1))
                    {
                        using (var bw = new BinaryWriter(mcb1))
                        {
                            foreach (var mcb0Entry in mcb0Entries)
                            {
                                br.BaseStream.Position = mcb0Entry.offset;

                                while (br.BaseStream.Position < mcb0Entry.offset + mcb0Entry.size)
                                {
                                    var sample = br.ReadInt32();
                                    if (sample == 0x7FFF)
                                        break;
                                    br.BaseStream.Position -= 4;

                                    var arcIndex = br.ReadInt32();
                                    var arcOffset = br.ReadUInt32();
                                    var size = br.ReadInt32();

                                    // Checks if it's the right archive
                                    if (arcIndex != archiveIndex)
                                    {
                                        br.BaseStream.Position += size;
                                        continue;
                                    }

                                    // Checks if it's one of the modified files
                                    if (offsets.IndexOf(arcOffset) == -1)
                                    {
                                        br.BaseStream.Position += size;
                                        continue;
                                    }

                                    // If it is, paste the new data
                                    var prevOffset = br.BaseStream.Position;

                                    binArchive.Position = arcOffset;
                                    byte[] newData = new byte[size];
                                    binArchive.Read(newData, 0, size);

                                    bw.Write(newData);
                                    br.BaseStream.Position = prevOffset + size;


                                }
                            }
                            bw.Close();
                            br.Close();
                            mcb1.Close();
                        }
                    }
                }
            }
        }

        private static List<Mcb0Entry> GetMcb0Entries(Stream mcb0)
        {
            var offsets = new List<Mcb0Entry>();
            using (var br = new BinaryReader(mcb0))
            {
                while (br.ReadInt32() != 0)
                {
                    br.BaseStream.Position -= 4;
                    var unk1 = br.ReadInt16();
                    var unk2 = br.ReadInt16();
                    var offset = br.ReadUInt32();
                    var length = br.ReadUInt32();

                    offsets.Add(new Mcb0Entry(unk1, unk2, offset, length));
                }
            }
            return offsets;
        }
    }
}
