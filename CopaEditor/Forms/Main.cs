using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CopaEditor.Utils;

namespace CopaEditor
{
    public partial class Form1 : Form
    {
        TextFile _textFile;
        BinFiles _binFile;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void tStrip_Open_Click(object sender, EventArgs e)
        {
            _textFile = new TextFile();

            using(var ofd = new OpenFileDialog())
            {
                ofd.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _textFile.fileName = ofd.FileName;
                }
            }
            using (var ftf = new FileType())
            {
                var result = ftf.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _textFile.fileType = ftf.format;
                }
            }
            this.lbl_selected.Text = "The current selected file is "+_textFile.fileName+" of filetype "+_textFile.fileType.ToString()+".bin";
            this.btn_Export.Enabled = true;
            this.btn_Import.Enabled = true;

        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            string output = "";
            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = _textFile.fileType.ToString();
                sfd.DefaultExt = ".txt";
                sfd.Filter = "Text documents (.txt)|*.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    output = sfd.FileName;
                }
                _textFile.ExportText(output);
            }
        }
        

        private void btn_Import_Click(object sender, EventArgs e)
        {
            string input = "";
            using (var ofd = new OpenFileDialog())
            {
                ofd.FileName = _textFile.fileType.ToString();
                ofd.DefaultExt = ".txt";
                ofd.Filter = "Text documents (.txt)|*.txt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    input = ofd.FileName;
                }
            }
            _textFile.ImportText(input);
        }

        private void tStrip_Portrait_Click(object sender, EventArgs e)
        {
            PortraitGenerator portraitGenerator = new PortraitGenerator();
            portraitGenerator.Show();

        }

        private void extractFilesFrombinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _binFile = new BinFiles();

            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using(var fbd = new FolderBrowserDialog())
                    {
                        if (fbd.ShowDialog() == DialogResult.OK)
                        {
                            _binFile.ExportFiles(ofd.FileName, fbd.SelectedPath);
                        }
                    }
                }
            }
        }
    }
}
