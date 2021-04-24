using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            if(args.Length < 1) 
            {
                PrintUsage();
                Console.ReadLine();
                return;
            }
            switch (args[0])
            {
                case "-u":
                    UnpackArchive(args[1]);
                    break;
                case "-e":
                    ExportText(args[1], args[2]);
                    break;
                case "-r":
                    if (args.Length > 2)
                        ImportFiles(args[1], args[2]);
                    else
                        Repack(args[1]);
                    break;
                case "-i":
                    ImportText(args[1], args[2], args[3], Convert.ToInt32(args[4]));
                    break;
                default:
                    PrintUsage();
                    break;
            }
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage :");
            Console.WriteLine("\t- Unpack archive :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -u <path to .bin archive>");
            Console.WriteLine("\t- Export a text file :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -e <path to Strikers text file>");
            Console.WriteLine("\t- Import to a .bin archive");
            Console.WriteLine("\t\tStrikers2013Tools.exe -r <path to extracted bin archive> <path to .bin archive>");
            Console.WriteLine("\t- Repack to BLN");
            Console.WriteLine("\t\tStrikers2013Tools.exe -r <path to extracted game files>");
            Console.WriteLine("\t- Import to text file");
            Console.WriteLine("\t\tStrikers2013Tools.exe -i <path to original text file> <path to modified text file");
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
        static void ExportText(string input, string output)
        {
            if (!File.Exists(input))
            {
                PrintUsage();
                return;
            }
            var text = new TEXT();
            text.ExportText(input, output);
        }
        static void Repack(string path)
        {

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
