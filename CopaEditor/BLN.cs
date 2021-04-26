using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using StrikersTools.Utils;

namespace StrikersTools
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
    
    class BLN
    {
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

            var mcb1 = File.Open(blnPath, FileMode.Open, FileAccess.ReadWrite);
            var binArchive = File.Open(binPath, FileMode.Open, FileAccess.Read);
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
