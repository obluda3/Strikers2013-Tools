using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Text;
using System.IO;

namespace CopaEditor
{
    public partial class PortraitGenerator : Form
    {
        public PortraitGenerator()
        {
            InitializeComponent();
        }
        Bitmap portrait;
        string filename;
        private void selectFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = "test.png";
            using (var ofd = new OpenFileDialog())
            {
                ofd.DefaultExt = ".png";
                ofd.Filter = "Portrait (.png)|*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filename = ofd.FileName;
                    portrait = new Bitmap(filename);
                    using (Graphics g = Graphics.FromImage(portrait))
                    {
                        g.Clip = new Region(new Rectangle(121, 113, 71, 15));
                        g.Clear(Color.FromArgb(0, Color.White));
                    }
                    pictureBox1.Image = portrait;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            updateImage(text, portrait);
            pictureBox1.Image = portrait;
        }
        
        private void updateImage(string text, Bitmap image)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                g.InterpolationMode = InterpolationMode.High;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.Clip = new Region(new Rectangle(121, 113, 71, 15));
                g.Clear(Color.FromArgb(0, Color.White));
                StringFormat strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Center;
                strFormat.LineAlignment = StringAlignment.Center;
                GraphicsPath p = new GraphicsPath();
                var font = new FontFamily("Arial");
                var fontsize = 10;
                p.AddString(
                    text,
                    font,
                    (int)FontStyle.Bold,
                    g.DpiY * fontsize / 72,
                    new Point(154, 120),
                    strFormat);
                Pen outlinePen = new Pen(Brushes.Black);
                outlinePen.Width = 2.5F;
                g.DrawPath(outlinePen, p);
                g.FillPath(Brushes.White, p);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            portrait.Save(filename);
        }

        private void batchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folder = "";
            string file = "";
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    folder = fbd.SelectedPath;
                    var fileEntries = Directory.GetFiles(folder);
                    using (var ofd = new OpenFileDialog())
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            file = ofd.FileName;
                            var playerNames = File.ReadAllLines(file);
                            if (playerNames.Length != fileEntries.Length)
                                MessageBox.Show("Not the same number of lines and files", "Error");
                            else
                            {
                                var i = 0;
                                foreach (var filename in fileEntries)
                                {
                                    var portraitFile = new Bitmap(filename);
                                    updateImage(playerNames[i], portraitFile);
                                    portraitFile.Save(filename + "_out.png");
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PortraitGenerator_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
