using System;
using System.IO;
using System.Windows.Forms;

namespace CopaEditor.Utils
{
    class BinFiles
    {

        public void ExportFiles(string input, string outputFolder)
        {
            var binfile = File.OpenRead(input);
            using (var br = new BinaryReader(binfile))
            {
                var fileCount = br.ReadInt32();
                var alignment = br.ReadInt32();
                br.BaseStream.Position+=12;
                Console.WriteLine("File ", input);
                Console.WriteLine(format:"It contains {0} files that are aligned to {1}", fileCount, BitConverter.ToString(BitConverter.GetBytes(alignment)));
                int blockSize;
                switch (alignment)
                {
                    case 0x4000:
                        // All the files except grp.bin have to be multiplied my align/4, this tries to check whether it's /4 or / 2
                        var bkpos = br.BaseStream.Position;
                        br.BaseStream.Position += ((fileCount - 1) * 4) + 2;
                        var entry = br.ReadUInt16();
                        blockSize = alignment / 2;
                        if (entry * blockSize > br.BaseStream.Length)
                        {
                            blockSize = alignment / 4;
                        }
                        br.BaseStream.Position = bkpos;
                        break;
                    default:
                        blockSize = alignment / 2;
                        break;
                }
                Console.WriteLine(format:"The files need to be multiplied by {0} in order to get their real offset", BitConverter.ToString(BitConverter.GetBytes(blockSize)));

                for (var i = 0; i < fileCount; i++)
                {
                    var unk = br.ReadUInt16();
                    var offset = br.ReadUInt16() * blockSize;
                    var bkPos = br.BaseStream.Position;
                    if(offset % alignment != 0)
                    {
                        Console.WriteLine(format: "fake file {0} at {1}",br.BaseStream.Position,offset);
                        continue;
                    }

                    br.BaseStream.Position += 2;
                    int length;
                    if (i == fileCount - 1)
                        length = (int)br.BaseStream.Length - offset;
                    else
                    {
                        var next = br.ReadUInt16() * blockSize;
                        length = next - offset;
                    }

                    br.BaseStream.Position = offset;
                    var output = File.Open(outputFolder + "\\\\"+Path.GetFileNameWithoutExtension(input) + i.ToString() + ".bin", FileMode.Create);
                    using (var bw = new BinaryWriter(output))
                    {
                        bw.Write(br.ReadBytes(length));
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
