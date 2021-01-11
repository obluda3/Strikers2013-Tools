using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopaEditor.Utils
{
    class BinFiles
    {
        public string fileName;

        public void ExportFiles(string output) {

            var binfile = File.OpenRead(output); // ou input est le chemin obtenu à l'aide du openfiledialog
            using (var br = new BinaryReader(binfile))
            {
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    var sample = br.readUInt32();
                    if (sample == 0xA755AAFC)
                    {
                        var uncompSize = br.ReadInt32();
                        var compSize = br.ReadInt32();
                    // Ici on a récupéré la taille du fichier, suffit juste de retourner de 8 octet en arrière et de copier le fichier dans un array de byte, puis dans un fichier de destination
                    }
                }
            }
        }
    }
}
