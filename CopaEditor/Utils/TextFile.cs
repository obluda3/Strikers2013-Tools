using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Be.IO;

namespace CopaEditor.Utils
{
    class TextFile
    {
        public string fileName;
        private int unkCount;
        private uint fileType, sectNumber, offsetPointer,entryNumber,sectLength; // 14, 35 etc...
        private uint[] pointers;
        private Encoding sjis = Encoding.GetEncoding("sjis", new EncoderExceptionFallback(),new DecoderExceptionFallback());
        private Encoding utf8 = Encoding.GetEncoding("utf-8", new EncoderExceptionFallback(), new DecoderExceptionFallback());

        public void ExportText(string input, string output) 
        {
            var file = File.Open(input, FileMode.Open);
            using (var ber = new BeBinaryReader(file, Encoding.GetEncoding("sjis")))
            {
                sectNumber = ber.ReadUInt32();
                sectLength = ber.ReadUInt32();

                // Takes into account all text files that way
                unkCount = 0;
                switch (sectNumber)
                {
                    case 1:
                        unkCount = 1;
                        break;
                    case 2:
                        unkCount = 3;
                        break;
                    case 3:
                        unkCount = 1;
                        break;
                }
                ber.BaseStream.Position += unkCount * 4;

                // Gets the the position of the first pointer and the number of entries in the text files
                offsetPointer = ber.ReadUInt32();
                entryNumber = ber.ReadUInt32();
                ber.BaseStream.Position = offsetPointer;

                pointers = new uint[entryNumber];
                for (var i = 0; i < entryNumber; i++)
                {
                    pointers[i] = ber.ReadUInt32();
                }

                var textFile = File.Open(output, FileMode.Create);
                using (var txtFile = new StreamWriter(textFile))
                {
                    for (var i = 0; i < pointers.Length; i++)
                    {
                        var length = 0;
                        if (i+1==pointers.Length)
                            length = 4;
                        else if (pointers[i] == 0)
                            length = 0;
                        else
                        {
                            length = (int)pointers[i + 1] - (int)pointers[i];
                            var j = i+1;
                            while (length < 0) // Takes into account blank pointers
                            {
                                length = (int)pointers[j] - (int)pointers[i];
                                j++;
                                if (j > pointers.Length - 1) { length = 4; break; }
                            }
                        }
                        ber.BaseStream.Position = pointers[i];
                        var entry = ber.ReadBytes(length);
                        Console.WriteLine(ber.BaseStream.Position);
                        var entryString = "";
                        try
                        {
                            entryString = sjis.GetString(entry);

                        }
                        catch(EncoderFallbackException e)
                        {
                            Console.WriteLine(e);
                        }
                        entryString = entryString.Replace("\0", string.Empty);
                        txtFile.WriteLine(entryString);
                    }
                }

            }
        }
        public void ImportText(string input, string orig) { }
    }
}
