using System;
using System.Data;
using System.Drawing;
using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using StrikersTools.Utils;
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
                    ofd.Filter = "Text file (*.txt)|*.txt|KUP file (*.kup)|*.kup|All files (*.*)|*.*";
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
                                TEXT text = new TEXT(txtPathTxt.Text);
                                if (ofd.FileName.EndsWith(".kup")) text.GetFromKUP(ofd.FileName);
                                else text.GetEntriesFromTXT(ofd.FileName);
                                text.Save(sfd.FileName);
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
                    sfd.Title = "Save";
                    sfd.DefaultExt = ".txt";
                    sfd.Filter = "Text file (*.txt)|*.txt|KUP File (*.kup)|*.kup|All files (*.*)|*.*";
                    sfd.FileName = Path.GetFileNameWithoutExtension(txtPathTxt.Text) + ".txt";
                    if(sfd.ShowDialog() == DialogResult.OK)
                    {
                        var text = new TEXT(txtPathTxt.Text);
                        if (sfd.FileName.EndsWith(".kup")) File.WriteAllText(sfd.FileName, text.ToKUP().ToString());
                        else text.ExportText(Path.GetFullPath(sfd.FileName));
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

        private void button3_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Open a shtx file";
                ofd.Filter = "SHTX File (*.shtx)|*.shtx|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox4.Text = ofd.FileName;
                    button4.Enabled = button5.Enabled = true;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Save";
                sfd.DefaultExt = ".txt";
                sfd.Filter = "PNG File (*.png)|*.png|All files (*.*)|*.*";
                sfd.FileName = Path.GetFileNameWithoutExtension(textBox4.Text) + ".png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SHTX.Export(textBox4.Text, sfd.FileName);
                    MessageBox.Show("Done !", "Done");
                }
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Open the png file to inject";
                ofd.Filter = "PNG File (*.png)|*.png|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (var sfd = new SaveFileDialog())
                    {
                        sfd.Title = "Save the shtx file";
                        sfd.DefaultExt = ".shtx";
                        sfd.Filter = "SHTX File (*.shtx)|*.shtx|All files (*.*)|*.*";
                        sfd.FileName = textBox4.Text;
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            SHTX.Convert(ofd.FileName, textBox4.Text, sfd.FileName);
                            MessageBox.Show("Done !", "Done");
                        }
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using(var ofd = new OpenFileDialog())
            {
                ofd.Title = "Open the file to compress/decompress";
                ofd.Filter = "All files (*.*)|*.*";
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox3.Text = ofd.FileName;
                    btnDecompress.Enabled = true;
                    btnCompress.Enabled = true;
                }
            }
        }

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            var decData = ShadeLz.Decompress(File.ReadAllBytes(textBox3.Text));
            var output = File.Open(textBox3.Text + ".out", FileMode.Create);
            output.Write(decData, 0, decData.Length);
            MessageBox.Show("Done !", "Done");
        }

        private void btnCompress_Click(object sender, EventArgs e)
        {
            var cmpData = ShadeLz.Compress(File.ReadAllBytes(textBox3.Text), !chkHeader.Checked);
            var output = File.Open(textBox3.Text + ".out", FileMode.Create);
            output.Write(cmpData, 0, cmpData.Length);
            MessageBox.Show("Done !", "Done");
        }
    }
}
