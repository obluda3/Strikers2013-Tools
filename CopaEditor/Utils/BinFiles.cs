using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopaEditor.Utils
{
    class BinFiles
    {

        public void ExportFiles(string input, string outputFolder)
        {
            var index = 0;
            var binfile = File.Open(input, FileMode.Open);
            using (var br = new BinaryReader(binfile))
            {
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    var sample = br.ReadUInt32();
                    if (sample == 0xA755AAFC)
                    {
                        // Recupere la taille du fichier
                        var uncompSize = br.ReadInt32();
                        var compSize = br.ReadInt32();
                        var padsize = compSize % 4;
                        compSize = compSize + (4 - padsize);
                        
                        // Retourne en arrière et copie le fichier dans un fichier de destination
                        br.BaseStream.Position -= 12;
                        
                        var file = br.ReadBytes(compSize);
                        Console.WriteLine("");
                        var output = File.Open(outputFolder + "\\\\" + index.ToString() +".bin", FileMode.Create);
                        using (var bw = new BinaryWriter(output))
                            bw.Write(file);
                        index += 1;
                    }
                }
                MessageBox.Show("Done");


            }
        }
    }
}
