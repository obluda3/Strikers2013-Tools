using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopaEditor
{
    public partial class FileType : Form
    {
        public FileType()
        {
            InitializeComponent();

        }
        public int format { get; set; }
        private void btn_Ok_Click(object sender, EventArgs e)
        {
            if (rdi_14.Checked) format = 14;
            else if (rdi_37.Checked) format = 37;
            else if (rdi_2.Checked) format = 2;
            this.DialogResult = DialogResult.OK;
            this.Close();

        }
    }
}
