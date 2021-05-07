using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using StrikersTools.FileFormats;

namespace StrikersTools
{
    public partial class MainForm : Form
    {
        private bool importBln = false;
        private Bitmap bitmap = new Bitmap(10,10);
        public MainForm()
        {
            InitializeComponent();  
            cmbAccents.Items.AddRange(new String[] { "French", "German", });
            cmbAccents.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Open a Strikers Text File (14/35/37.bin)";
                ofd.Filter = "Strikers text file (*.bin)|*.bin|All files (*.*)|*.*";
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPathTxt.Text = ofd.FileName;
                    btnTxtExport.Enabled = true;
                    btnTxtImport.Enabled = true;
                }
            }
        }

        private void btnTxtImport_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtPathTxt.Text))
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Title = "Open the txt file to inject";
                    ofd.DefaultExt = ".txt";
                    ofd.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
                    using (var sfd = new SaveFileDialog())
                    {
                        sfd.Title = "Save the bin file";
                        sfd.DefaultExt = ".bin";
                        sfd.Filter = "Strikers text file (*.bin)|*.bin|All files (*.*)|*.*";
                        sfd.FileName = Path.GetFileNameWithoutExtension(txtPathTxt.Text) + ".out";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            TEXT text = new TEXT();
                            text.ImportText(ofd.FileName, txtPathTxt.Text, sfd.FileName, cmbAccents.SelectedIndex);
                            MessageBox.Show("Done !", "Done");
                        }
                    }
                }
            }
        }

        private void btnTxtExport_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtPathTxt.Text))
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Title = "Save the txt file";
                    sfd.DefaultExt = ".txt";
                    sfd.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
                    sfd.FileName = Path.GetFileNameWithoutExtension(txtPathTxt.Text) + ".txt";
                    if(sfd.ShowDialog() == DialogResult.OK)
                    {
                        TEXT text = new TEXT();
                        text.ExportText(txtPathTxt.Text, sfd.FileName, cmbAccents.SelectedIndex);
                        MessageBox.Show("Done !", "Done");
                    }
                }
            }
        }

        private void btnBrowseArc_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Open a Strikers bin archive";
                ofd.Filter = "Strikers bin archive (*.bin)|*.bin|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPathArc.Text = ofd.FileName;
                    btnModified.Enabled = true;
                    btnExportArc.Enabled = true;
                }
            }
        }

        private void btnModified_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Browse to the location of your modified files";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtModified.Text = fbd.SelectedPath;
                    btnImportArc.Enabled = true;
                }
            }
        }

        private void btnMcb1_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Open the mcb1.bln";
                ofd.Filter = "mcb1.bln (*.bln)|*.bln|All files (*.*)|*.*";
                ofd.FileName = "mcb1.bln";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtMcb.Text = ofd.FileName;
                    btnImportArc.Enabled = true;
                }
            }
        }

        private void btnImportArc_Click(object sender, EventArgs e)
        {
            BLN.RepackArchiveAndBLN(txtModified.Text, txtPathArc.Text, txtMcb.Text);
            MessageBox.Show("Done !", "Done");
        }

        private void btnExportArc_Click(object sender, EventArgs e)
        {
            BIN.ExportFiles(txtPathArc.Text);
            MessageBox.Show("Done !", "Done");
        }


    }
}
