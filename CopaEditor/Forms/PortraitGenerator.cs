using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Text;

namespace CopaEditor
{
    public partial class PortraitGenerator : Form
    {
        public PortraitGenerator()
        {
            InitializeComponent();
        }
        Bitmap portrait;
        private void selectFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filename = "test.png";
            using (var ofd = new OpenFileDialog())
            {
                ofd.DefaultExt = ".png";
                ofd.Filter = "Portrait (.png)|*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filename = ofd.FileName;
                }
            }
            portrait = new Bitmap(filename);
            using (Graphics g = Graphics.FromImage(portrait))
            {
                g.Clip = new Region(new Rectangle(121, 113, 71, 15));
                g.Clear(Color.FromArgb(0, Color.White));
            }
            pictureBox1.Image = portrait;


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            using (Graphics g = Graphics.FromImage(portrait))
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
                var font = new FontFamily("Verdana");
                var fontsize = 9;
                p.AddString(
                    text,
                    font,
                    (int)FontStyle.Regular,
                    g.DpiY * fontsize / 72,
                    new Point(154, 120),
                    strFormat);
                Pen outlinePen = new Pen(Brushes.Black);
                outlinePen.Width = 2.5F;
                g.DrawPath(outlinePen, p);
                g.FillPath(Brushes.White, p);
            }
            pictureBox1.Image = portrait;
        }
    }
}
