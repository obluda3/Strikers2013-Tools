﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
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
            else if (args.Length < 2) PrintUsage();
            else
            {

                switch (args[0])
                {
                    case "-u":
                        UnpackArchive(args[1]);
                        break;
                    case "-e":
                        ExportText(args[1], args[2]);
                        break;
                    case "-r":
                        if (args.Length > 3)
                            Repack(args[1], args[2], args[3]);
                        else
                            PrintUsage();
                        break;
                    case "-i":
                        ImportText(args[1], args[2], args[3]);
                        break;
                    case "-she":
                        ExportShtx(args[1], args[2]);
                        break;
                    case "-shi":
                        ConvertShtx(args[1], args[2], args[3]);
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
                        var font = new Font(args[1]);
                        font.ExtractFont();
                        break;
                    case "-fi":
                        var fontFile = new Font(args[1]);
                        fontFile.ImportLetters(args[2]);
                        break;
                    case "-l":
                        var bln = new BLN(args[1]);
                        if (args[2] == "all" || args[2] == "full") Locate(bln, -1);
                        else Locate(bln, Convert.ToInt32(args[2]));
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
            Console.WriteLine("\t\tStrikers2013Tools.exe -e <path to Strikers text file> <output>");
            Console.WriteLine("\t- Repack to .bin archive and BLN :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -r <path to .bin archive> <path to modified files> <path to mcb1.bln>");
            Console.WriteLine("\t- Get file locations from BLN Sub :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -l <path to .bln> <BLN Sub index>");
            Console.WriteLine("\t- Get file locations from BLN :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -l <path to .bln> <all/-1/full>");
            Console.WriteLine("\t- Import to text file :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -i <path to original text file> <path to modified text file> <output path>");
            Console.WriteLine("\t- Export SHTX file :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -she <path to SHTX> <output>");
            Console.WriteLine("\t- Convert to SHTX :");
            Console.WriteLine("\t\tStrikers2013Tools.exe -shi <path to png> <path to shtx> <output>");
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

        static async void UnpackArchive(string path)
        {
            if (!File.Exists(path))
            {
                PrintUsage();
                return;
            }
            var progress = new Progress<int>();
            var arc = new ArchiveFile(Path.GetFullPath(path), true);
            await arc.ExtractFiles(progress);
        }

        static void ImportText(string path, string txt, string output)
        {
            var filePath = Path.GetFullPath(path);
            var txtPath = Path.GetFullPath(txt);
            var outPath = Path.GetFullPath(output);
            if (!File.Exists(filePath) | !File.Exists(txtPath))
            {
                PrintUsage();
                return;
            }
            var text = new TEXT(filePath);
            if (txt.EndsWith(".kup")) text.GetFromKUP(txt);
            else text.GetEntriesFromTXT(txt);
            text.Save(outPath);
        }

        static void Locate(BLN bln, int index)
        {
            if (index != -1)
                bln.Locate(index);
            else
                bln.LocateAll();
        }
        static void ExportText(string input, string output)
        {
            var inputPath = Path.GetFullPath(input);
            if (!File.Exists(inputPath))
            {
                PrintUsage();
                return;
            }
            var text = new TEXT(Path.GetFullPath(inputPath));
            if(output.EndsWith(".kup")) File.WriteAllText(output, text.ToKUP().ToString());
            else text.ExportText(Path.GetFullPath(output));

        }

        static async void Repack(string bin, string input, string mcb)
        {
            var binPath = Path.GetFullPath(bin);
            var inputPath = Path.GetFullPath(input);
            var mcbPath = Path.GetFullPath(mcb);
            if (Directory.Exists(inputPath) && File.Exists(binPath) && File.Exists(mcbPath))
            {
                if (!File.Exists(Path.GetDirectoryName(mcbPath) + Path.DirectorySeparatorChar + "mcb0.bln")) 
                {
                    Console.WriteLine("mcb0.bln not found");
                    return;
                }
                var progress = new Progress<int>();
                var bln = new BLN(mcbPath);
            }
            else
               PrintUsage();
        }

        static async void ImportFiles(string binPath, string inputPath, string destPath)
        {
            string inputFolder = Path.GetFullPath(inputPath);
            string actualBinPath = Path.GetFullPath(binPath);
            if (Directory.Exists(inputFolder) && File.Exists(actualBinPath))
            {
                var arc = new ArchiveFile(actualBinPath, true);
                arc.ImportFiles(inputFolder);
                await arc.Save(Path.GetFullPath(destPath));
            }
            else
                PrintUsage();
        }
        static void ExportShtx(string input, string output)
        {
            var bitmap = SHTX.Export(input);
            bitmap.Save(output);
        }
        static void ConvertShtx(string input, string original, string output)
        {
            var data = SHTX.Convert(input, original);
            using (var file = File.OpenWrite(output))
                file.Write(data, 0, data.Length);
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
            var timer = new Stopwatch();
            timer.Start();
            var compressedData = ShadeLz.Compress(fileData, false);
            timer.Stop();
            var output = File.Open(input + ".out", FileMode.Create);
            output.Write(compressedData, 0, compressedData.Length);
            output.Close();
            
            float sizeKB = fileData.Length / 1024F;
            float elapsedMs = timer.ElapsedMilliseconds;

            var avgSpeed = sizeKB / (elapsedMs / 1000);

            Console.WriteLine($"Avg {avgSpeed:0.0} KB/s");
        }
    }
}
