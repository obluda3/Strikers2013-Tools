using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using StrikersTools.FileFormats;

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
            Console.WriteLine("\t- Repack to .bin archive");
            Console.WriteLine("\t\tStrikers2013Tools.exe -r <path to extracted bin archive> <path to .bin archive>");
            Console.WriteLine("\t- Repack to .bin archive and BLN");
            Console.WriteLine("\t\tStrikers2013Tools.exe -r <path to extracted bin archive> <path to .bin archive> <path to mcb1.bln>");
            Console.WriteLine("\t- Import to text file");
            Console.WriteLine("\t\tStrikers2013Tools.exe -i <path to original text file> <path to modified text file> <output path> <accent configuration>");
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
    }
}
