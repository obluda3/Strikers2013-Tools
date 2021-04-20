using System;
using System.IO;
using System.Windows.Forms;

namespace CopaEditor
{
    class BinFiles
    {

        public void ExportFiles(string input, string outputFolder)
        {
            var binfile = File.OpenRead(input);
            using (var br = new BinaryReader(binfile))
            {
                var fileCount = br.ReadInt32();
                var padFactor = br.ReadInt32();
                var mulFactor = br.ReadInt32();
                var shiftFactor = br.ReadInt32();
                var mask = br.ReadInt32();

                for (var i = 0; i < fileCount; i++)
                {
                    var offSize = br.ReadInt32();
                    var bkPos = br.BaseStream.Position;
                    
                    var offset = (offSize >> shiftFactor) * padFactor;
                    var size = (offSize & mask) * mulFactor;

                    var output = File.Open(outputFolder + "\\\\"+Path.GetFileNameWithoutExtension(input) + i.ToString() + ".bin", FileMode.Create);
                    using (var bw = new BinaryWriter(output))
                    {
                        bw.Write(br.ReadBytes(size));
                    }
                    br.BaseStream.Position = bkPos;
                }
                MessageBox.Show("Done");
            }
        }

        public void BatchReplace(string mcbInput, string uiInput, string oldFolder, string newFolder)
        {
            
        }


    }
}
