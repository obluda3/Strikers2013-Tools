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
        public Form1()
        {
            InitializeComponent();
        }

        private void tStrip_Open_Click(object sender, EventArgs e)
        {
            var _textFile = new TextFile();
            using(var ofd = new OpenFileDialog())
            {
                ofd.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _textFile.fileName = 
                }
            }
        }
    }
}
