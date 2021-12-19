using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

    class BLN
    {
        // https://github.com/FanTranslatorsInternational/Kuriimu2/blob/dev/plugins/Shade/plugin_shade/Archives/BlnSubSupport.cs
        // 0x00 => grp.bin
        // 0x01 => scn.bin
        // 0x02 => scn_sh.bin
        // 0x03 => ui.bin
        // 0x04 => dat.bin
        public static string[] ArchiveNames = { "grp.bin", "scn.bin", "scn_sh.bin", "ui.bin", "dat.bin" };
        private const int MCB0ENTRYLENGTH = 0xC;
        public static async Task RepackArchiveAndBLN(string inputFolder, string binPath, string blnPath, IProgress<int> progress)
        {
            await Task.Run(async () => 
            {
                var archiveFileName = Path.GetFileName(binPath);
                var archiveIndex = -1;

                for (var i = 0; i < ArchiveNames.Length; i++)
                {
                    if (archiveFileName.Contains(ArchiveNames[i]))
                    {
                        archiveIndex = i;
                        break;
                    }
                }
                if (archiveIndex < 0)
                    return;

                var binFile = new ArchiveFile(binPath);
                binFile.ImportFiles(inputFolder);

                await binFile.Save(binPath);

                var fileTable = binFile.Files;

                // Get mcb0 entries offsets
                var mcb0Path = Path.GetDirectoryName(blnPath) + Path.DirectorySeparatorChar + "mcb0.bln";
                var mcb0 = File.OpenRead(mcb0Path);
                var mcb0Entries = GetMcb0Entries(mcb0);
                progress.Report(10000 / (mcb0Entries.Count + 1));

                // Saves the unknown data at the end of the mcb0
                mcb0.Position = MCB0ENTRYLENGTH * mcb0Entries.Count;
                var unkMcb0 = new byte[mcb0.Length - mcb0.Position];
                mcb0.Read(unkMcb0, 0, unkMcb0.Length);
                mcb0.Close();

                var mcb1 = File.Open(blnPath, FileMode.Open, FileAccess.ReadWrite);
                var tempMcb1 = File.Open(blnPath + ".tmp", FileMode.Create);

                using (var br = new BinaryReader(mcb1))
                {
                    using (var bw = new BinaryWriter(tempMcb1))
                    {
                        for (var i = 0; i < mcb0Entries.Count; i++)
                        {
                            var mcb0Entry = mcb0Entries[i];
                            var newOffset = (uint)bw.BaseStream.Position;

                            br.BaseStream.Position = mcb0Entry.offset;
                            while (br.BaseStream.Position < mcb0Entry.offset + mcb0Entry.size)
                            {
                                if (bw.BaseStream.Position % 4 != 0) 
                                    Console.WriteLine("paniiiique");
                                var sample = br.ReadInt32();
                                if (sample == 0x7FFF)
                                {
                                    break;
                                }

                                br.BaseStream.Position -= 4;

                                var arcIndex = br.ReadInt32();
                                var arcOffset = br.ReadUInt32();
                                var size = br.ReadInt32();

                                bw.Write(arcIndex);

                                // Checks if it's the right archive
                                if (arcIndex != archiveIndex)
                                {
                                    bw.Write(arcOffset);
                                    bw.Write(size);
                                    bw.Write(br.ReadBytes(size));
                                    continue;
                                }

                                // If it is, gets the fileInfo of the file
                                var fileInfo = fileTable.FirstOrDefault(x => x.OldOffset == arcOffset);
                                if (fileInfo.Offset == 0)
                                {
                                    Console.WriteLine("Invalid archive offset in SubBLN {0}: {1}\n\r", i, arcOffset);
                                    return;
                                }
                                bw.Write((uint)fileInfo.Offset);
                                bw.Write(fileInfo.Size);
                                var backupPos = bw.BaseStream.Position;
                                if (fileInfo.Modified)
                                {
                                    Console.WriteLine("{0} found in BLN Sub: {1}", fileInfo.Path, i);
                                    bw.Write(fileInfo.Data);
                                    br.BaseStream.Position += size;
                                }
                                else
                                {
                                    bw.Write(br.ReadBytes(size));
                                }
                                var writtenCount = bw.BaseStream.Position - backupPos;
                                bw.PadWith(0, (fileInfo.Size - writtenCount));
                            }
                            bw.Write(0x7FFF);
                            bw.WriteAlignment(0x800);
                            var newSize = (uint)bw.BaseStream.Position - newOffset;
                            mcb0Entry.offset = newOffset;
                            mcb0Entry.size = newSize;

                            mcb0Entries[i] = mcb0Entry;
                            progress.Report(10000 * (i + 2) / (mcb0Entries.Count + 1));
                        }
                        br.Close();
                        bw.Close();
                        tempMcb1.Close();
                        mcb1.Close();
                    }
                }
                File.Delete(blnPath);
                File.Move(blnPath + ".tmp", blnPath);
                mcb0 = File.Open(mcb0Path, FileMode.Create);
                using (var bw = new BinaryWriter(mcb0))
                {
                    foreach (var mcb0entry in mcb0Entries)
                    {
                        bw.Write(mcb0entry.unk1);
                        bw.Write(mcb0entry.unk2);
                        bw.Write(mcb0entry.offset);
                        bw.Write(mcb0entry.size);
                    }
                    bw.Write(unkMcb0);
                }
            });

        }
        /*
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
        }*/
        private static List<Mcb0Entry> GetMcb0Entries(Stream mcb0)
        {
            var entries = new List<Mcb0Entry>();
            var br = new BinaryReader(mcb0);
            while (br.ReadInt32() != 0)
            {
                br.BaseStream.Position -= 4;
                var unk1 = br.ReadInt16();
                var unk2 = br.ReadInt16();
                var offset = br.ReadUInt32();
                var length = br.ReadUInt32();

                entries.Add(new Mcb0Entry(unk1, unk2, offset, length));
            }
            return entries;
        }
    }
}
