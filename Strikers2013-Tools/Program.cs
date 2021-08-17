using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using StrikersTools.FileFormats;
using StrikersTools.Utils;

namespace StrikersTools
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                switch (args[0])
                {

                    case "-h":
                        PrintUsage();
                        break;
                    case "-u":
                        UnpackArchive(args[1]);
                        break;
                    case "-e":
                        if (args.Length > 2)
                            ExportText(args[1], args[2], Convert.ToInt32(args[3]));
                        else
                            PrintUsage();
                        break;
                    case "-r":
                        if (args.Length > 3)
                            Repack(args[1], args[2], args[3]);
                        else if (args.Length > 2)
                            ImportFiles(args[1], args[2]);
                        else
                            PrintUsage();
                        break;
                    case "-i":
                        ImportText(args[1], args[2], args[3], Convert.ToInt32(args[4]));
                        break;
                    case "-she":
                        ExportShtx(args[1]);
                        break;
                    case "-shi":
                        ConvertShtx(args[1]);
                        break;
                    case "-p":
                        var pass = Password.Encrypt(args[1]);
                        Console.WriteLine(BitConverter.ToString(pass));
                        break;
                    case "-d":
                        Decompress(args[1]);
                        break;
                    case "-c":
                        Compress(args[1]);
                        break;
                    case "-f":
                        Font.ExtractFont(args[1]);
                        break;
                    case "-fi":
                        Font.ImportLetters(args[1], args[2]);
                        break;
                    default:
                        PrintUsage();
                        break;
                }
            }
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage :");
            Console.WriteLine("\t- Unpack archive :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -u <path to .bin archive>");
            Console.WriteLine("\t- Export a text file :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -e <path to Strikers text file> <output> <accent configuration>");
            Console.WriteLine("\t- Repack to .bin archive :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -r <path to extracted bin archive> <path to .bin archive>");
            Console.WriteLine("\t- Repack to .bin archive and BLN :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -r <path to .bin archive> <path to extracted bin archive> <path to mcb1.bln>");
            Console.WriteLine("\t- Import to text file :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -i <path to original text file> <path to modified text file> <output path> <accent configuration>");
            Console.WriteLine("\t- Export SHTXFS file :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -she <path to SHTX>");
            Console.WriteLine("\t- Import SHTXFS file :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -shi <path to SHTX>");
            Console.WriteLine("\t- Encrypt password :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -p <Password>");
            Console.WriteLine("\t- Decompress file :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -d <path to file>");
            Console.WriteLine("\t- Compress file :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -c <path to file>");
            Console.WriteLine("\t- Extract font :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -f <path to font file>");
            Console.WriteLine("\t- Import letters :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -fi <path to font file> <path to letters>");
        }

        static void UnpackArchive(string path)
        {
            if (!File.Exists(path))
            {
                PrintUsage();
                return;
            }
            BIN.ExportFiles(path);
        }

        static void ImportText(string path, string txt, string output, int accentConfig)
        {
            if (!File.Exists(path) | !File.Exists(txt))
            {
                PrintUsage();
                return;
            }
            var text = new TEXT();
            text.ImportText(txt, path, output, Convert.ToInt32(accentConfig));
        }

        static void ExportText(string input, string output, int accentIndex)
        {
            if (!File.Exists(input))
            {
                PrintUsage();
                return;
            }
            var text = new TEXT();
            text.ExportText(input, output, accentIndex);
        }

        static void Repack(string binPath, string inputPath, string mcbPath)
        {
            if (Directory.Exists(inputPath) && File.Exists(binPath) && File.Exists(mcbPath))
            {
                if (!File.Exists(Path.GetDirectoryName(mcbPath) + Path.DirectorySeparatorChar + "mcb0.bln")) 
                {
                    Console.WriteLine("mcb0.bln not found");
                    return;
                }

                BLN.RepackArchiveAndBLN(inputPath, binPath, mcbPath);
            }
            else
                PrintUsage();
        }

        static void ImportFiles(string binPath, string inputPath)
        {

            if (Directory.Exists(inputPath) && File.Exists(binPath))
            {
                BIN.ImportFiles(inputPath, binPath);
            }
            else
                PrintUsage();
        }
        static void ExportShtx(string input)
        {
            SHTX.Export(input);
        }
        static void ConvertShtx(string input)
        {
            SHTX.Convert(input);
        }

        static void Decompress(string input)
        {
            var fileData = File.ReadAllBytes(input);
            var decompressedData = ShadeLz.Decompress(fileData);

            var output = File.Open(input + ".out", FileMode.Create);
            output.Write(decompressedData, 0, decompressedData.Length);
            output.Close();

        }
        static void Compress(string input)
        {
            var fileData = File.ReadAllBytes(input);
            var compressedData = ShadeLz.Compress(fileData);

            var output = File.Open(input + ".out", FileMode.Create);
            output.Write(compressedData, 0, compressedData.Length);
            output.Close();
        }
    }
}
