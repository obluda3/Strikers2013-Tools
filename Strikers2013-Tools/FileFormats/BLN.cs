using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using StrikersTools.Utils;
using System.Linq;

namespace StrikersTools.FileFormats
{
    struct Mcb0Entry
    {
        public int id;
        public uint offset;
        public uint size;

        public Mcb0Entry(int id, uint offset, uint size)
        {
            this.id = id;
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
        // strap.bin is a lie but it's a hack!
        public static string[] ArchiveNames = { "grp.bin", "scn.bin", "scn_sh.bin", "ui.bin", "dat.bin", "strap.bin" };
        private const int MCB0ENTRYLENGTH = 0xC;
        private string RootFolder;
        private List<Mcb0Entry> Entries = new List<Mcb0Entry>();
        private byte[] UnkData;

        public BLN(string blnPath)
        {
            RootFolder = Path.GetDirectoryName(blnPath) + Path.DirectorySeparatorChar;

            var mcb0 = File.OpenRead(RootFolder + "mcb0.bln");
            using (var br = new BinaryReader(mcb0))
            {
                while (br.PeekUInt32() != 0)
                {
                    var id = br.ReadInt32();
                    var offset = br.ReadUInt32();
                    var size = br.ReadUInt32();

                    var mcb0Entry = new Mcb0Entry(id, offset, size);
                    Entries.Add(mcb0Entry);
                }
                UnkData = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
            }
        }
        public void Locate(int subBlnIndex)
        {
            var entry = Entries[subBlnIndex];

            var archives = new List<List<ArchiveFileInfo>>();

            foreach (var archive in ArchiveNames)
            {
                var archiveData = new ArchiveFile(RootFolder + archive, false).Files;
                archiveData.ForEach(x => x.Data = Array.Empty<byte>()); // frees some memory

                archives.Add(archiveData);
            }

            var mcb1 = File.OpenRead(RootFolder + "mcb1.bln");
            using(var br = new BinaryReader(mcb1))
            {
                br.BaseStream.Position = entry.offset;
                var i = 0;
                while (br.PeekUInt32() != 0x7fff && br.BaseStream.Position < entry.offset + entry.size)
                {
                    var arcIndex = br.ReadInt32();
                    var arcOffset = br.ReadInt32();
                    var size = br.ReadInt32();

                    var fileIndex = archives[arcIndex].FirstOrDefault(x => x.Offset == arcOffset).Index;
                    Console.WriteLine($"{subBlnIndex:00000000}\\{i:00000000}.bin\t{ArchiveNames[arcIndex]}\\" +
                        $"{fileIndex:00000000}.bin");
                    br.BaseStream.Position += size;
                    i++;
                }
            }
        }

        public void LocateAll()
        {
            for (var i = 0; i < Entries.Count; i++) Locate(i);
        }

        public async Task SyncWithBin(ArchiveFile binFile, int archiveIndex, IProgress<int> progress)
        {
            await Task.Run(() =>
            {
                var fileTable = binFile.Files;
                var blnPath = RootFolder + "mcb1.bln";

                var mcb1 = File.Open(blnPath, FileMode.Open, FileAccess.ReadWrite);
                var tempMcb1 = File.Open(blnPath + ".tmp", FileMode.Create);

                using (var br = new BinaryReader(mcb1))
                {
                    using (var bw = new BinaryWriter(tempMcb1))
                    {
                        for (var i = 0; i < Entries.Count; i++)
                        {
                            var mcb0Entry = Entries[i];
                            var newOffset = (uint)bw.BaseStream.Position;

                            br.BaseStream.Position = mcb0Entry.offset;
                            while (br.BaseStream.Position < mcb0Entry.offset + mcb0Entry.size)
                            {
                                if (bw.BaseStream.Position % 4 != 0)
                                    Console.WriteLine($"paniiiique {i}");
                                if (br.PeekUInt32() == 0x7FFF)
                                {
                                    break;
                                }

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
                                var writtenSize = fileInfo.Size > 0 ? (int)fileInfo.Size : size; // hack
                                bw.Write(writtenSize);
                                var backupPos = bw.BaseStream.Position;
                                if (fileInfo.Modified)
                                    Console.WriteLine("{0} found in BLN Sub: {1}", fileInfo.Path, i);
                                br.BaseStream.Position += size;
                                bw.Write(fileInfo.Data);
                                var writtenCount = bw.BaseStream.Position - backupPos;
                                bw.PadWith(0, (fileInfo.Size - writtenCount));
                            }
                            bw.Write(0x7FFF);
                            bw.WriteAlignment(0x800);
                            var newSize = (uint)bw.BaseStream.Position - newOffset;
                            mcb0Entry.offset = newOffset;
                            mcb0Entry.size = newSize;

                            Entries[i] = mcb0Entry;
                            progress.Report(10000 * (i + 2) / (Entries.Count + 1));
                        }
                        br.Close();
                        bw.Close();
                        tempMcb1.Close();
                        mcb1.Close();
                    }
                }
                File.Delete(blnPath);
                File.Move(blnPath + ".tmp", blnPath);
                var mcb0Path = RootFolder + "mcb0.bln";
                var mcb0 = File.Open(mcb0Path, FileMode.Create);
                using (var bw = new BinaryWriter(mcb0))
                {
                    foreach (var entry in Entries)
                    {
                        bw.Write(entry.id);
                        bw.Write(entry.offset);
                        bw.Write(entry.size);
                    }
                    bw.Write(UnkData);
                }
            }
            );
        }
        public async Task RepackArchiveAndBLN(string inputFolder, string binPath, IProgress<int> progress)
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

                var binFile = new ArchiveFile(binPath, true);
                binFile.ImportFiles(inputFolder);

                await binFile.Save(binPath);

                progress.Report(10000 / (Entries.Count + 1));

                await SyncWithBin(binFile, archiveIndex, progress);
            });

        }

    }
}
