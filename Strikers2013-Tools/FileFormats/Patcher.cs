using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using StrikersTools.Utils;

namespace StrikersTools.FileFormats
{
    class Patcher
    {
        public static string[] archives = { "grp.bin", "scn.bin", "scn_sh.bin", "ui.bin", "dat.bin" };
        public string OriginalFolder { get; set; }
        public string ModifiedFolder { get; set; }
        public Patcher(string original, string modified)
        {
            OriginalFolder = original;
            ModifiedFolder = modified; 
        }
        public List<PatchData> CompareArchives(int ftyp)
        {
            List<PatchData> result = new List<PatchData>();

            var originalFilename = Path.Combine(OriginalFolder, archives[ftyp]);
            var newFilename = Path.Combine(ModifiedFolder, archives[ftyp]);

            var originalArc = new ArchiveFile(originalFilename, true);
            var newArc = new ArchiveFile(newFilename, true);

            for (int i = 0; i < originalArc.Files.Count; i++)
            {
                var origFile = originalArc.Files[i];
                var newFile = newArc.Files[i];

                if (!CompareData(origFile.Data, newFile.Data)) continue;

                var patchData = new PatchData();
                patchData.Ftyp = ftyp;
                patchData.OriginalOffset = (int)origFile.Offset;
                patchData.Data = newFile.Data;
                patchData.PatchedLength = (int)newFile.Size;
                patchData.FileIndex = i;
                result.Add(patchData);
            }

            return result;
        }

        public bool CompareData(byte[] source, byte[] modified)
        {
            if (source.Length != modified.Length)
                return true;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != modified[i])
                    return true;
            }
            return false;
        }

        public void BuildPatchFile(string path)
        {
            List<PatchData> patchedData = new List<PatchData>();
            for (var i = 0; i < 5; i++)
                patchedData.AddRange(CompareArchives(i));

            var file = File.Open(path, FileMode.Create);
            using (var bw = new BeBinaryWriter(file))
            {
                bw.Write(patchedData.Count);
                foreach (var patch in patchedData)
                    bw.Write(patch.OriginalOffset + patch.Ftyp);

                bw.WriteAlignment(0x2000);

                for (var i = 0; i < patchedData.Count; i++)
                    bw.Write((long)0);
               
                foreach (var patch in patchedData)
                {
                    var offset = bw.BaseStream.Position;
                    bw.Write(patch.Data);
                    patch.PatchedOffset = (int)offset;
                }
                bw.BaseStream.Position = 0x2000;

                foreach (var patch in patchedData)
                {
                    bw.Write(patch.PatchedOffset);
                    bw.Write(patch.PatchedLength);
                }
            }
        }

    }

    class PatchData
    {
        public int FileIndex;
        public int OriginalOffset;
        public int PatchedOffset;
        public int PatchedLength;
        public int Ftyp;

        public byte[] Data;
    }
}
