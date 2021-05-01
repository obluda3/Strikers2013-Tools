using System;
using System.Collections.Generic;
using StrikersTools.Dictionaries;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Be.IO;

namespace StrikersTools.FileFormats
{
    class TEXT
    {
        private int unkCount;
        private uint sectNumber, offsetPointer, entryCount, nextSectOffset;
        private uint[] pointers;
        private byte[] unk1, unk2, unk3;
        private Encoding sjis = Encoding.GetEncoding("sjis");
        private Encoding utf8 = Encoding.GetEncoding("utf-8", new EncoderExceptionFallback(), new DecoderExceptionFallback());


        public void ExportText(string path, string output, int accentIndex)
        {
            parseTextFile(path);
            var file = File.Open(path, FileMode.Open);

            using (var ber = new BeBinaryReader(file, Encoding.GetEncoding("sjis")))
            {
                var textFile = File.Open(output, FileMode.Create);
                using (var txtFile = new StreamWriter(textFile))
                {
                    for (var i = 0; i < pointers.Length; i++)
                    {
                        var length = 0;
                        if (i + 1 == pointers.Length)
                            length = 0;
                        else if (pointers[i] == 0)
                            length = 0;
                        else
                        {
                            length = (int)pointers[i + 1] - (int)pointers[i];
                            var j = i + 1;
                            while (length < 0) // Takes into account blank pointers
                            {
                                length = (int)pointers[j] - (int)pointers[i];
                                j++;
                                if (j > pointers.Length - 1) 
                                {
                                    length = 4;
                                    break; 
                                }
                            }
                        }

                        ber.BaseStream.Position = pointers[i];
                        var entry = ber.ReadBytes(length);
                        var entryString = "";
                        entryString = sjis.GetString(entry);
                        entryString = ReplaceAccentsEx(entryString, accentIndex);
                        if (i == pointers.Length - 1)
                            txtFile.Write(entryString);
                        else
                            txtFile.WriteLine(entryString);
                    }
                }

            }
        }

        public void ImportText(string input, string orig, string output, int accentIndex)
        {
            parseTextFile(orig);
            var lines = File.ReadAllLines(input);
            var file = File.Open(output, FileMode.Create);
            int entriesLength = 0;
            using (var bw = new BeBinaryWriter(file))
            {
                bw.Write(sectNumber);
                bw.Write(nextSectOffset);
                bw.Write(unk1);
                bw.Write(offsetPointer);
                bw.Write(entryCount);
                bw.Write(unk2);
                bw.Write(pointers[0]);
                var pointersPos = bw.BaseStream.Position;

                // Pad the pointers with zero, they will be replaced later
                for (var i = 1; i < lines.Length+1; i++)
                {
                    bw.Write((int)0);
                }

                // It is useful for later, if it isn't done
                // the algo will not work for files other than
                // 14.bin
                uint previous = 0;
                foreach (var pointer in pointers)
                {
                    if (pointer != 0)
                    {
                        previous = pointer;
                        break;
                    }
                }

                var j = 0;
                foreach(var line in lines)
                {
                    if (line == "@")
                        Console.WriteLine("tkt");

                    var entrystr = ReplaceAccentsIn(line, accentIndex);

                    var entry = sjis.GetBytes(entrystr);
                    if (entry.Length > 0)
                    {
                        var padSize = 4 - ((entry.Length) % 4); // Every entry is padded to a 4 byte alignment
                        bw.Write(entry);

                        for (var _i = 0; _i < padSize; _i++)
                            bw.Write((byte)0);

                        entriesLength += entry.Length + padSize;
                        uint current = (uint)previous + (uint)entry.Length + (uint)padSize;
                        if(previous != 0)
                            pointers[j] = previous;
                        previous = current;
                    }
                    else
                    {   
                        pointers[j] = 0;
                    }
                    j++;
                }
                bw.Write(unk3);
                bw.BaseStream.Position = pointersPos - 4;
                foreach (var pointer in pointers)
                    bw.Write(pointer);
                bw.BaseStream.Position = 4;
                nextSectOffset =(uint)(unk1.Length + 4 + 4 + unk2.Length + pointers.Length * 4 + entriesLength + 4+ 4); // Basically, trust me dude but all these numbers are correct
                bw.Write(nextSectOffset);

            }
        }

        private void parseTextFile(string input)
        {
            var file = File.OpenRead(input);
            using (var ber = new BeBinaryReader(file, Encoding.GetEncoding("sjis")))
            {
                sectNumber = ber.ReadUInt32();
                nextSectOffset = ber.ReadUInt32();

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
                var unk1Length = unkCount * 4;
                unk1 = ber.ReadBytes(unk1Length);

                offsetPointer = ber.ReadUInt32();
                entryCount = ber.ReadUInt32();
                
                var unk2Length = (int)offsetPointer - (int)ber.BaseStream.Position;
                unk2 = ber.ReadBytes(unk2Length);

                pointers = new uint[entryCount];
                for (var i = 0; i < entryCount; i++)
                {
                    pointers[i] = ber.ReadUInt32();
                }
                if (pointers[entryCount - 1] != 0)
                    ber.BaseStream.Position = pointers[entryCount - 1];
                else
                    ber.BaseStream.Position = pointers[entryCount - 2] + 4; // After the text
                var unk3Length = (int)ber.BaseStream.Length - (int)ber.BaseStream.Position;
                unk3 = ber.ReadBytes(unk3Length);
            }
        }

        private string ReplaceAccentsIn(string s, int accentIndex)
        {
            s = s.Replace("{returnline}","\n");

            var customEncoding = GetCustomEncoding(accentIndex);

            var output = new StringBuilder(s);
            foreach (var kvp in customEncoding)
                output.Replace(kvp.Key, kvp.Value);

            return output.ToString();
        }
        private string ReplaceAccentsEx(string s, int accentIndex)
        {
            s.Replace("\0", string.Empty);
            s.Replace("\n", "{returnline}");

            var customEncoding = GetCustomEncoding(accentIndex);

            var output = new StringBuilder(s);
            foreach (var kvp in customEncoding)
                output.Replace(kvp.Value, kvp.Key);

            return output.ToString();
        }

        private Dictionary<string,string> GetCustomEncoding(int accentIndex)
        {
            switch (accentIndex)
            {
                default:
                    return new Dictionary<string,string>();
                case 0:
                    return SpecialChars.FrenchAccents;
            }
        }
    }
}
