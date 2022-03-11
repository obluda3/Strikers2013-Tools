using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using StrikersTools.Utils;
using System.Linq;

namespace StrikersTools.FileFormats
{
    class Mcb0Entry
    {
        public int ID { get; }
        public uint Offset { get; set; }
        public uint Size { get; set; }
        public readonly int Index;

        public List<Mcb1File> Files { get; set; }
        
        public Mcb0Entry(int id, uint offset, uint size, int index)
        {
            ID = id;
            Offset = offset;
            Size = size;
            Index = index;
        }
        public Mcb0Entry(BinaryReader brMcb0, BinaryReader brMcb1, int index)
        {
            ID = brMcb0.ReadInt32();
            Offset = brMcb0.ReadUInt32();
            Size = brMcb0.ReadUInt32();

            Files = new List<Mcb1File>();

            brMcb1.BaseStream.Position = Offset;
            for(var i = 0;  brMcb1.BaseStream.Position < Offset + Size && brMcb1.PeekUInt32() != 0x7fff; i++)
            {
                var mcb1File = new Mcb1File(brMcb1, i);
                brMcb1.BaseStream.Position += mcb1File.Size;
                Files.Add(mcb1File);
            }
            Index = index;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(Offset);
            bw.Write(Size);
        }
    }

    class Mcb1File
    {
        public int ArchiveIndex { get; set; }
        public int ArchiveOffset { get; set; }
        public int Size { get; set; }
         
        public readonly long OriginalOffset;
        public byte[] NewData { get; set; }
        public readonly int Index;

        public void Write(BinaryWriter bw, BinaryReader mcb1Reader)
        {
            bw.Write(ArchiveIndex);
            bw.Write(ArchiveOffset);
            bw.Write(Size);

            if (NewData != null)
                bw.Write(NewData);
            else
            {
                mcb1Reader.BaseStream.Position = OriginalOffset + 12;
                byte[] fileData = mcb1Reader.ReadBytes(Size);
                bw.Write(fileData);
            }
        }
        public Mcb1File(BinaryReader br, int index)
        {
            var offset = br.BaseStream.Position;
            OriginalOffset = offset;
            ArchiveIndex = br.ReadInt32();
            ArchiveOffset = br.ReadInt32();
            Size = br.ReadInt32();
            Index = index;
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
        private string RootFolder;
        private List<Mcb0Entry> Entries = new List<Mcb0Entry>();
        private byte[] UnkData;

        public BLN(string blnPath)
        {
            RootFolder = Path.GetDirectoryName(blnPath) + Path.DirectorySeparatorChar;

            var mcb0 = File.OpenRead(RootFolder + "mcb0.bln");
            var mcb1 = File.OpenRead(RootFolder + "mcb1.bln");
            using (var br = new BinaryReader(mcb0))
            using (var brMcb1 = new BinaryReader(mcb1))
            {
                for (var i = 0; br.PeekUInt32() != 0; i++)
                {
                    var mcb0Entry = new Mcb0Entry(br, brMcb1, i);
                    Entries.Add(mcb0Entry);
                }
                UnkData = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
            }
        }

        public async void Save(IProgress<int> progress)
        {
            await Task.Run(() =>
            {
                var blnPath = RootFolder + "mcb1.bln";

                var mcb1 = File.Open(blnPath, FileMode.Open, FileAccess.Read);
                var tempMcb1 = File.Open(blnPath + ".tmp", FileMode.Create);

                using (var br = new BinaryReader(mcb1))
                using (var bw = new BinaryWriter(tempMcb1))
                {
                    for (var i = 0; i < Entries.Count; i++)
                    {
                        var entry = Entries[i];
                        var newOffset = (uint)bw.BaseStream.Position;
                        var subBlnFiles = entry.Files;
                        foreach (var file in subBlnFiles)
                        {
                            file.Write(bw, br);
                        }
                        bw.Write(0x7FFF);
                        bw.WriteAlignment(0x800);
                        var newSize = (uint)bw.BaseStream.Position - newOffset;
                        entry.Offset = newOffset;
                        entry.Size = newSize;
                        progress.Report(100 * (i + 1) / (Entries.Count));
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
                        entry.Write(bw);
                    }
                    bw.Write(UnkData);
                }
            });
        }

        public void UpdateBlnReferences(ArchiveFile binFile)
        {
            var fileTable = binFile.Files;

            foreach (var entry in Entries) 
            { 
                var files = entry.Files;
                var correctFiles = files.Where(x => x.ArchiveIndex == binFile.ArchiveIndex);
                foreach (var file in correctFiles)
                {
                    var fileInfo = fileTable.FirstOrDefault(x => x.OldOffset == file.ArchiveOffset);
                    if (fileInfo != null)
                    {
                        var newOffset = fileInfo.Offset;
                        var newSize = fileInfo.Size > 0 ? (int)fileInfo.Size : file.Size; // hack
                        file.ArchiveOffset = (int)newOffset;
                        file.Size = newSize;

                        if (fileInfo.Modified)
                        {
                            Console.WriteLine($"{fileInfo.Path} found in BLN Sub: {entry.Index:00000000}\\{file.Index:00000000}.bin");
                            file.NewData = fileInfo.Data;
                        }
                    }
                }
            }
        }
           
        public void Locate(int subBlnIndex)
        {
            var entry = Entries[subBlnIndex];

            var archives = new List<List<ArchiveFileInfo>>();

            foreach (var archive in ArchiveNames)
            {
                var archiveData = new ArchiveFile(RootFolder + archive, false).Files;
                archives.Add(archiveData);
            }

            foreach (var file in entry.Files)
            {
                var fileIndex = archives[file.ArchiveIndex].FirstOrDefault(x => x.Offset == file.ArchiveOffset).Index;
                Console.WriteLine($"{subBlnIndex:00000000}\\{file.Index:00000000}.bin\t{ArchiveNames[file.ArchiveIndex]}\\" +
                    $"{fileIndex:00000000}.bin");
            }
        }

        public void LocateAll()
        {
            for (var i = 0; i < Entries.Count; i++) Locate(i);
        }


    }
}
