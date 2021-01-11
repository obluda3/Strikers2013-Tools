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

            byte[] data = File.ReadAllBytes(fileName);

            int nom = 0;

            for (int i =0; i < data.Length; i++) {

                if (data[i] == 0 && data[i + 1] == 0 && data[i + 2] == 0 && data[i + 3] == 0) {

                    byte[] taille = { data[i + 11]), data[i + 10]), data[i + 9]), data[i + 8]) };
                    String[] octet = { BitConverter.ToString(data[i + 11]),  };
                
                }
            
            
            }

        
        }
    }
}
