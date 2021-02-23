using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using CopaEditor.Utils;
using System.Diagnostics;

namespace CopaEditor
{
    public partial class Form1 : Form
    {
        TextFile _textFile;
        BinFiles _binFile = new BinFiles();
        public Form1()
        {
            InitializeComponent();
        }

        private void tStrip_Open_Click(object sender, EventArgs e)
        {
            _textFile = new TextFile();

            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _textFile.fileName = ofd.FileName;
                }
            }
            /*using (var ftf = new FileType())
            {
                var result = ftf.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _textFile.fileType = ftf.format;
                }
            }*/
            this.lbl_selected.Text = "The current selected file is " + _textFile.fileName;
            this.btn_Export.Enabled = true;
            this.btn_Import.Enabled = true;




        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            string output = "";
            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = _textFile.fileName + ".txt";
                sfd.DefaultExt = ".txt";
                sfd.Filter = "Text documents (.txt)|*.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    output = sfd.FileName;

                    _textFile.ExportText(output);
                }
            }
        }


        private void btn_Import_Click(object sender, EventArgs e)
        {
            string input = "";
            using (var ofd = new OpenFileDialog())
            {
                ofd.FileName = _textFile.fileName + ".txt";
                ofd.DefaultExt = ".txt";
                ofd.Filter = "Text documents (.txt)|*.txt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    input = ofd.FileName;
                    _textFile.ImportText(input);
                }

               
            }
        }
        

        private void tStrip_Portrait_Click(object sender, EventArgs e)
        {
            PortraitGenerator portraitGenerator = new PortraitGenerator();
            portraitGenerator.Show();

        }

        private void extractFilesFrombinToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (var fbd = new FolderBrowserDialog())
                    {
                        if (fbd.ShowDialog() == DialogResult.OK)
                        {
                            _binFile.ExportFiles(ofd.FileName, fbd.SelectedPath);
                        }
                    }
                }
            }
        }

        private void tStrip_replaceMcb_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "mcb1.bln (*.bln)|*.bln|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var mcb = ofd.FileName;
                    using (var ofd2 = new OpenFileDialog())
                    {
                        ofd2.Filter = "ui.bin (*.bin)|*.bin|All files (*.*)|*.*";
                        if (ofd2.ShowDialog() == DialogResult.OK)
                        {
                            var ui = ofd2.FileName;
                            using (var fbd = new FolderBrowserDialog())
                            {
                                fbd.Description = "Select the folder where the old files are located";
                                if (fbd.ShowDialog() == DialogResult.OK)
                                {
                                    var oldfolder = fbd.SelectedPath;
                                    using (var fbd2 = new FolderBrowserDialog())
                                    {
                                        fbd2.Description = "Select the folder where the new files are located";
                                        if (fbd2.ShowDialog() == DialogResult.OK)
                                        {
                                            var newfolder = fbd2.SelectedPath;
                                            _binFile.BatchReplace(mcb, ui, oldfolder, newfolder);
                                            MessageBox.Show("Done");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void matchSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the folder where the old files are located";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    var oldfolder = fbd.SelectedPath;
                    using (var fbd2 = new FolderBrowserDialog())
                    {
                        fbd2.Description = "Select the folder where the new files are located";
                        fbd2.SelectedPath = oldfolder;
                        if (fbd2.ShowDialog() == DialogResult.OK)
                        {
                            var newfolder = fbd2.SelectedPath;

                            var oldFiles = Directory.GetFiles(oldfolder);
                            var newFiles = Directory.GetFiles(newfolder);

                            Console.WriteLine(oldFiles.Length);
                            Console.WriteLine(newFiles.Length);

                            if (oldFiles.Length == newFiles.Length)
                            {
                                for (var i = 0; i < oldFiles.Length; i++)
                                {
                                    var oldfile = File.Open(oldFiles[i], FileMode.Open);
                                    var newfile = File.Open(newFiles[i], FileMode.Open);

                                    var oldlength = oldfile.Length;
                                    var newlength = newfile.Length;

                                    if (newlength > oldlength) {
                                        Console.WriteLine("Error");
                                        Console.WriteLine(newfile.Name); }

                                    var padlength = oldlength - newlength;

                                    using (var bw = new BinaryWriter(newfile))
                                    {
                                        bw.BaseStream.Position = newfile.Length;
                                        for (var j = 0; j < padlength; j++)
                                            bw.Write((byte)0);
                                    }
                                }
                                MessageBox.Show("Done");
                            }
                            else {
                                MessageBox.Show("# les problèmes");
                            }

                        }
                    }
                }
            }
        }

        private void renamingToolStripMenuItem_Click(object sender, EventArgs e) // it makes absolutely no sense, it's just for my personal usage
        {
            string ext;

            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the folder where the files are located";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string newfolder =" ";
                    using (var fbd2 = new FolderBrowserDialog())
                    {
                        fbd2.Description = "select the output";
                        if (fbd2.ShowDialog() == DialogResult.OK)
                        {
                            newfolder = fbd2.SelectedPath;
                        }
                    }
                    var oldfolder = fbd.SelectedPath;
                    var files = Directory.GetFiles(oldfolder);
                    var index = 0;
                    for (var i = 0;i < files.Length; i++)
                    {
                        ext = ".from";
                        var newfilename = newfolder + "\\" + index + ext;
                        File.Move(files[i], newfilename);
                        index +=1;


                    }



                }
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
