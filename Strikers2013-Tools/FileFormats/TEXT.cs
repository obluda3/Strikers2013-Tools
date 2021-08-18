using System;
using System.Collections.Generic;
using StrikersTools.Dictionaries;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using StrikersTools.Utils;

namespace StrikersTools.FileFormats
{
    class TEXT
    {
        private int unkCount;
        private uint END_OF_TEXTOFF, POINTERS_OFF;
        private uint sectNumber, offsetPointer, entryCount, endOfText;
        private uint begSec2, endSec2 = 0;
        private uint[] pointers;
        private byte[] unk1, unk2, unk3, unk4, unk5;

        public void ExportText(string path, string output)
        {
            parseTextFile(path);
            var file = File.Open(path, FileMode.Open);

            using (var ber = new BeBinaryReader(file))
            {
                var textFile = File.Open(output, FileMode.Create);
                using (var txtFile = new StreamWriter(textFile))
                {
                    for (var i = 0; i < pointers.Length; i++)
                    {
                        if(i == pointers.Length - 5)
                        {
                            Console.WriteLine("deb");
                        }
                        /*
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
                        }*/
                        ber.BaseStream.Position = pointers[i];
                        var length = 0;
                        while(ber.ReadByte() != 0)
                        {
                            length += 1;
                        }

                        ber.BaseStream.Position = pointers[i];
                        var entry = ber.ReadBytes(length);
                        var entryString = TextDecoder.Decode(entry);
                        if (i == pointers.Length - 1)
                            txtFile.Write(entryString);
                        else
                            txtFile.WriteLine(entryString);
                    }
                }

            }
        }

        public void ImportText(string input, string orig, string output)
        {
            parseTextFile(orig);
            var lines = File.ReadAllLines(input);
            var file = File.Open(output, FileMode.Create);
            int entriesLength = 0;
            using (var bw = new BeBinaryWriter(file))
            {
                bw.Write(sectNumber);
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
                bw.BaseStream.Position = pointers.First(x => x != 0);
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
                    var entry = TextDecoder.Encode(line);
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
                endOfText = (uint)bw.BaseStream.Position;
                bw.Write(unk3);
                begSec2 = (uint)bw.BaseStream.Position;
                bw.Write(unk4);
                endSec2 = (uint)bw.BaseStream.Position;
                bw.Write(unk5);
                bw.BaseStream.Position = pointersPos - 4;

                foreach (var pointer in pointers)
                    bw.Write(pointer);

                bw.BaseStream.Position = END_OF_TEXTOFF;
                bw.Write(endOfText);

                if(sectNumber == 3)
                {
                    bw.BaseStream.Position = 0x1c;
                    bw.Write(begSec2);
                    bw.BaseStream.Position = 4;
                    bw.Write(endSec2);
                }


            }
        }

        private void parseTextFile(string input)
        {
            var file = File.OpenRead(input);
            using (var ber = new BeBinaryReader(file))
            {
                sectNumber = ber.ReadUInt32();

                // Takes into account all text files that way
                unkCount = 0;
                switch (sectNumber)
                {
                    case 1:
                        POINTERS_OFF = 12;
                        END_OF_TEXTOFF = 4;
                        break;
                    case 2:
                        POINTERS_OFF = 0x14;
                        END_OF_TEXTOFF = 4;
                        break;
                    case 3:
                        POINTERS_OFF = 12;
                        END_OF_TEXTOFF = 0x14;
                        ber.BaseStream.Position = 0x1c;
                        begSec2 = ber.ReadUInt32();
                        ber.BaseStream.Position = 4;
                        endSec2 = ber.ReadUInt32();
                        ber.BaseStream.Position -= 4;
                        break;
                }
                unk1 = ber.ReadBytes((int)POINTERS_OFF - (int)ber.BaseStream.Position);

                ber.BaseStream.Position = END_OF_TEXTOFF;
                endOfText = ber.ReadUInt32();

                ber.BaseStream.Position = POINTERS_OFF;
                offsetPointer = ber.ReadUInt32();
                entryCount = ber.ReadUInt32();
                
                var unk2Length = (int)offsetPointer - (int)ber.BaseStream.Position;
                unk2 = ber.ReadBytes(unk2Length);

                pointers = new uint[entryCount];
                for (var i = 0; i < entryCount; i++)
                {
                    pointers[i] = ber.ReadUInt32();
                }
                /*
                if (pointers[entryCount - 1] != 0)
                    ber.BaseStream.Position = pointers[entryCount - 1];
                else
                    ber.BaseStream.Position = pointers[entryCount - 2] + 4; // After the text
                */
                ber.BaseStream.Position = endOfText;

                int unk3Length = 0;
                int unk4Length = 0;
                int unk5Length = 0;

                if (sectNumber != 3)
                    unk3Length = (int)ber.BaseStream.Length - (int)ber.BaseStream.Position;
                else
                {
                    unk3Length = (int)begSec2 - (int)endOfText;
                    unk4Length = (int)endSec2 - (int)begSec2;
                    unk5Length = (int)ber.BaseStream.Length - (int)endSec2;
                }
                unk3 = ber.ReadBytes(unk3Length);
                unk4 = ber.ReadBytes(unk4Length);
                unk5 = ber.ReadBytes(unk5Length);
            }
        }

    }
}
