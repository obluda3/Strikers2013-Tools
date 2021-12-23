using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using StrikersTools.FileFormats;

namespace StrikersTools
{
    public partial class MainForm : Form
    {
        private Bitmap bitmap = new Bitmap(10,10);
        public MainForm()
        {
            InitializeComponent();  
            listBox1.Items.AddRange(Password.encryptedPasswords.Select(x => BitConverter.ToString(x.Value)).ToArray());

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Open a Strikers Text File (14/35/37.bin)";
                ofd.Filter = "Strikers text file (*.bin;*.out)|*.bin;*.out|All files (*.*)|*.*";
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
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        using (var sfd = new SaveFileDialog())
                        {
                            sfd.Title = "Save the bin file";
                            sfd.DefaultExt = ".bin";
                            sfd.Filter = "Strikers text file (*.bin)|*.bin|All files (*.*)|*.*";
                            sfd.FileName = Path.GetFileNameWithoutExtension(txtPathTxt.Text) + ".out.bin";
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                TEXT text = new TEXT();
                                text.ImportText(ofd.FileName, txtPathTxt.Text, sfd.FileName);
                                MessageBox.Show("Done !", "Done");
                            }
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
                        text.ExportText(txtPathTxt.Text, sfd.FileName);
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
            using (var cofd = new CommonOpenFileDialog())
            {
                cofd.Title = "Browse to the location of your modified files";
                cofd.IsFolderPicker = true;
                if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    txtModified.Text = cofd.FileName;
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
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtMcb.Text = ofd.FileName;
                    btnImportArc.Enabled = true;
                }
            }
        }

        private async void btnImportArc_Click(object sender, EventArgs e)
        {
            var progress = new Progress<int>(value =>
            {
                progressBar1.Value = value;
                lblProgress.Text = $"{value / 100} %";
            });

            var blnFile = new BLN(txtMcb.Text);
            await blnFile.RepackArchiveAndBLN(txtModified.Text, txtPathArc.Text, progress);
            lblProgress.Text = "Done !";
            progressBar1.Value = 0;
        }

        private async void btnExportArc_Click(object sender, EventArgs e)
        {
            var progress = new Progress<int>(value =>
            {
                progressBar1.Value = value;
                lblProgress.Text = $"{value / 100} %";
            });
            var arc = new ArchiveFile(txtPathArc.Text, true);
            await arc.ExtractFiles(progress, checkBox1.Checked);
            lblProgress.Text = "Done !";
            progressBar1.Value = 0;
        }

        private void btn_Convert(object sender, EventArgs e)
        {
            var encrypted = Password.Encrypt(textBox1.Text);
            textBox2.Text = BitConverter.ToString(encrypted);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var decrypted = Password.Decrypt(textBox1.Text);
            textBox2.Text = decrypted;
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void cmbAccents_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
