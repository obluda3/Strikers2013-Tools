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
        private uint fileType, sectNumber, offsetPointer, entryCount, sectLength; // 14, 35 etc...
        private uint[] pointers;
        private byte[] unk1, unk2, unk3;
        private Encoding sjis = Encoding.GetEncoding("sjis");
        private Encoding utf8 = Encoding.GetEncoding("utf-8", new EncoderExceptionFallback(), new DecoderExceptionFallback());


        public void ExportText(string output)
        {
            parseTextFile(fileName);
            var file = File.Open(fileName, FileMode.Open);

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
                                if (j > pointers.Length - 1) { length = 4; break; }
                            }
                        }

                        ber.BaseStream.Position = pointers[i];
                        var entry = ber.ReadBytes(length);
                        var entryString = "";
                        entryString = sjis.GetString(entry);
                        entryString = entryString.Replace("\0", string.Empty);
                        txtFile.WriteLine(entryString);
                    }
                }

            }
        }

        public void ImportText(string input)
            // not working properly and i don't want to fix it 
        {
            parseTextFile(fileName);
            var lines = File.ReadAllLines(input);
            var file = File.Open(input + ".bin", FileMode.Create);
            int entriesLength = 0;
            using (var bw = new BeBinaryWriter(file))
            {
                bw.Write(sectNumber);
                bw.Write(sectLength);
                bw.Write(unk1);
                bw.Write(offsetPointer);
                bw.Write(entryCount);
                bw.Write(unk2);
                bw.Write(pointers[0]);
                var pointersPos = bw.BaseStream.Position;
                for (var i = 1; i < lines.Length; i++)
                {
                    /*if (lines.Length > 0)
                    {
                        var previous = pointers[i - 1];
                        var j = i;
                        while (previous == 0)
                        {
                            j -= 1;
                            previous = pointers[j];
                        }
                        var remainder = 0;
                        var entryLength = 4 * (Math.DivRem(lines[i].Length + 1, 4, out remainder) + 1);
                        pointers[i] = previous + (uint)entryLength;
                        if(i == 7575) { 
                            Console.WriteLine("cc"); }
                    }
                    else
                    {
                        pointers[i] = 0;
                    }*/
                    bw.Write((int)0);
                }
                var previous = pointers[0];
                var j = 0;
                foreach(var line in lines)
                {

                    var entry = sjis.GetBytes(line);
                    if (entry.Length > 0)
                    {
                        var padSize = 4 - ((entry.Length) % 4);
                        bw.Write(entry);
                        for (var _i = 0; _i < padSize; _i++)
                            bw.Write((byte)0);
                        entriesLength += entry.Length + padSize;
                        uint current = (uint)previous + (uint)entry.Length + (uint)padSize;
                        pointers[j] = previous;
                        previous = current;
                    }
                    else
                    {
                    }
                    j++;
                }
                bw.Write(unk3);
                bw.BaseStream.Position = pointersPos - 4;
                foreach (var pointer in pointers)
                    bw.Write(pointer);
                bw.BaseStream.Position = 4;
                sectLength =(uint)(unk1.Length + 4 + 4 + unk2.Length + pointers.Length * 4 + entriesLength + 4+ 4);
                bw.Write(sectLength);

            }
        }

        private void parseTextFile(string input)
        {
            var file = File.OpenRead(input);
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
                var unk1Length = unkCount * 4;
                unk1 = ber.ReadBytes(unk1Length);

                // Gets the the position of the first pointer and the number of entries in the text files
                offsetPointer = ber.ReadUInt32();
                entryCount = ber.ReadUInt32();
                
                var unk2Length = (int)offsetPointer - (int)ber.BaseStream.Position;
                unk2 = ber.ReadBytes(unk2Length);

                pointers = new uint[entryCount];
                for (var i = 0; i < entryCount; i++)
                {
                    pointers[i] = ber.ReadUInt32();
                }

                ber.BaseStream.Position = pointers[entryCount - 2] + 4; // After the text
                var unk3Length = (int)ber.BaseStream.Length - (int)ber.BaseStream.Position;
                unk3 = ber.ReadBytes(unk3Length);
            }
        }

        private string ReplaceAccents(string s, Dictionary<string,string> customEncoding)
        {
            var output = new StringBuilder(s);
            foreach (var kvp in customEncoding)
                output.Replace(kvp.Key, kvp.Value);

            return output.ToString();
        }
    }
}
