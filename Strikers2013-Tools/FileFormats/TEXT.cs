using System;
using System.Collections.Generic;
using StrikersTools.Dictionaries;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using StrikersTools.Utils;

namespace StrikersTools.FileFormats
{
    /// Needs a major rewrite
    /// This was before I managed to really understand the format
    /// check https://www.github.com/Obluda3/strikers2013-re-notes
    /// for more info.
    /// With a rewrite, we might be able to extract the other sections as well
    /// 
    struct TextEntry
    {
        public string normal;
        public string formatted;
        public int index;
    }
    class TEXT
    {
        private uint END_OF_TEXTOFF, POINTERS_OFF;
        private uint sectNumber, offsetPointer, entryCount, endOfText;
        private TextEntry[] Entries = new TextEntry[0];
        private uint begSec2, endSec2 = 0;
        private uint[] pointers;
        private byte[] unk1, unk2, unk3, unk4, unk5;

        public TEXT(string path)
        {
            var file = File.OpenRead(path);
            using (var ber = new BeBinaryReader(file))
            {
                sectNumber = ber.ReadUInt32();

                // Takes into account all text files that way
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
                Entries = new TextEntry[pointers.Length];
                for (var i = 0; i < pointers.Length; i++)
                {
                    ber.BaseStream.Position = pointers[i];
                    string entryString = "";
                    if (pointers[i] != 0)
                    {
                        var length = 0;
                        while (ber.ReadByte() != 0)
                        {
                            length += 1;
                        }

                        ber.BaseStream.Position = pointers[i];
                        var entryBytes = ber.ReadBytes(length);
                        entryString = TextDecoder.Decode(entryBytes);
                    }
                    var entry = new TextEntry();
                    entry.index = i;
                    entry.normal = entryString;
                    entry.formatted = FormatThing(entryString);
                    Entries[i] = entry;
                }
            }
        }

        public void ExportText(string output)
        {
            var textFile = File.Open(output, FileMode.Create);
            using (var txtFile = new StreamWriter(textFile))
            {
                for (var i = 0; i < pointers.Length; i++)
                {
                    var entryString = Entries[i].normal;
                    if (i == pointers.Length - 1)
                        txtFile.Write(entryString);
                    else
                        txtFile.WriteLine(entryString);
                }
            }
        }

        // absolute madness, this should be illegal
        public XElement ToKUP()
        {
            return new XElement("kup", new XElement("pointerTables"), new XElement("stringBounds"), 
                new XElement("entries", Entries.Select(x => new
            {
                nameAttribute = new XAttribute("name", x.index.ToString("text_0000")),
                offsetAttribute = new XAttribute("offset", 0),
                relocatableAttribute = new XAttribute("relocatable", false),
                max_lengthAttribute = new XAttribute("max_length", 0),
                originalElement = new XElement("original", x.formatted),
                editedElement = new XElement("edited", x.formatted),
                subEntries = new XElement("subEntries")
            }).Select(x => new XElement("entry", new XAttribute(x.nameAttribute), 
            new XAttribute(x.offsetAttribute), new XAttribute(x.relocatableAttribute), 
            new XAttribute(x.max_lengthAttribute), new XElement(x.originalElement), 
            new XElement(x.editedElement), new XElement(x.subEntries))
            )));;
        }

        public void GetFromKUP(string kupPath)
        {
            var xelement = XElement.Parse(File.ReadAllText(kupPath));
            var entries = xelement.Elements("entries");
            for(var i = 0; i < entries.Count(); i++)
            {
                var entry = entries.ElementAt(i);
                var text = entry.Element("entry").Element("edited").ToString().Replace("<edited>", "").Replace("</edited>", "").Replace("<", "").Replace(">","");
                var entryBase = Entries[i];
                entryBase.normal = text;
                Entries[i] = entryBase;
            }
        }

        public void GetEntriesFromTXT(string input)
        {
            var lines = File.ReadAllLines(input);
            for(var i = 0; i < lines.Length; i++)
            {
                var entry = Entries[i];
                entry.normal = lines[i];
                Entries[i] = entry;
            }
        }
        public void Save(string output)
        {
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
                for (var i = 1; i < Entries.Length+1; i++)
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
                foreach(var line in Entries)
                {
                    var entry = TextDecoder.Encode(line.normal);
                    if (entry.Length > 0)
                    {
                        var padSize = 4 - ((entry.Length) % 4); // Every entry is padded to a 4 byte alignment
                        bw.Write(entry);

                        bw.PadWith(0, padSize);

                        entriesLength += entry.Length + padSize;
                        uint current = previous + (uint)entry.Length + (uint)padSize;
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

        // It works!
        public static string FormatThing(string input)
        {
            var ParseNumber = "xXcCqQuUsSiIBbzZxXyYpPr";
            var Digits = "0123456789-";
            var output = "";
            for(var i = 0; i < input.Length; i++)
            {
                if(input[i] == '#')
                {
                    output += "<#";
                    while(++i < input.Length)
                    {
                        var curChar = input[i];
                        output += curChar;

                        var parseNumber = false;
                        if (curChar == 'R' || curChar == 'o')
                        {
                            if (i + 1 < input.Length)
                            {
                                curChar = input[++i];
                                output += curChar;
                            }
                        }

                        if (curChar == '=')
                        {
                            break;
                        }

                        var endOfCommand = false;
                        if (ParseNumber.Contains(curChar))
                        {
                            var newPos = i + 1;
                             
                            while (newPos < input.Length)
                            {
                                var newChar = input[newPos];
                                if(Digits.Contains(newChar))
                                {
                                    output += newChar;
                                    newPos++;
                                }
                                else
                                {
                                    if(newChar != '#')
                                    {
                                        endOfCommand = true;
                                    }
                                    else
                                    {
                                        output += '#';
                                        newPos++;
                                    }           
                                    break;
                                }
                                if (newPos == input.Length)
                                {
                                    endOfCommand = true;
                                    break;
                                }
                            }
                            i = newPos - 1;
                        }
                        else
                        {
                            if(i + 1 < input.Length)
                            {
                                if (input[i + 1] != '#')
                                {
                                    endOfCommand = true;
                                }
                                else
                                {
                                    output += '#';
                                    i+= 1;
                                }
                            }
                            else
                            {
                                endOfCommand = true;
                            }
                        }
                        if (endOfCommand)
                        {
                            break;
                        }
                    }
                    output += ">";
                    var endCommand = i;
                }
                else
                {
                    output += input[i];
                }
            }
            return output;
        }


    }
}
