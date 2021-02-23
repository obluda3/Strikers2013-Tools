using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Text;
using System.IO;

namespace CopaEditor
{
    // Needs a lot of cleanup but i'm lazy af...
    public partial class PortraitGenerator : Form
    {
        public PortraitGenerator()
        {
            InitializeComponent();
        }
        Bitmap portrait;
        private string path;
        private void selectFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.DefaultExt = ".png";
                ofd.Filter = "Portrait (.png)|*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    path = ofd.FileName;
                    portrait = new Bitmap(path);
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
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.Clip = new Region(new Rectangle(121, 113, 71, 15));
                g.Clear(Color.FromArgb(0, Color.White));
                StringFormat strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Center;
                strFormat.LineAlignment = StringAlignment.Center;
                GraphicsPath p = new GraphicsPath();
                var font = new FontFamily("Arial");
                var fontsize = 9;
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
            using (var sfd = new SaveFileDialog())
            {
                sfd.DefaultExt = ".png";
                sfd.Filter = "Image (.png)|*.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    portrait.Save(sfd.FileName);
                }
            }
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
                    foreach (var filename in fileEntries)
                    {
                        var portraitFile = new Bitmap(filename);
                        using (Graphics g = Graphics.FromImage(portraitFile))
                        {
                            g.Clip = new Region(new Rectangle(121, 113, 71, 15));
                            g.Clear(Color.FromArgb(0, Color.White));
                        }
                        portraitFile.Save(@"C:\Users\PCHMD\Desktop\Emulateurs\Dolphin-x64\strikers\mcb\extracted\batchPortrait\ouy\" + Path.GetFileName(filename));
                       
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
