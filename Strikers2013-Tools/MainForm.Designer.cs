namespace StrikersTools
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnMcb1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMcb = new System.Windows.Forms.TextBox();
            this.txtModified = new System.Windows.Forms.TextBox();
            this.txtPathArc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnModified = new System.Windows.Forms.Button();
            this.btnImportArc = new System.Windows.Forms.Button();
            this.btnExportArc = new System.Windows.Forms.Button();
            this.btnBrowseArc = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnTxtImport = new System.Windows.Forms.Button();
            this.btnTxtExport = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtPathTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnCompress = new System.Windows.Forms.Button();
            this.btnDecompress = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkHeader = new System.Windows.Forms.CheckBox();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox1);
            this.tabPage2.Controls.Add(this.lblProgress);
            this.tabPage2.Controls.Add(this.progressBar1);
            this.tabPage2.Controls.Add(this.btnMcb1);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtMcb);
            this.tabPage2.Controls.Add(this.txtModified);
            this.tabPage2.Controls.Add(this.txtPathArc);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.btnModified);
            this.tabPage2.Controls.Add(this.btnImportArc);
            this.tabPage2.Controls.Add(this.btnExportArc);
            this.tabPage2.Controls.Add(this.btnBrowseArc);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(784, 285);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Archive";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 257);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(94, 17);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Decompress ?";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(376, 254);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 19;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(139, 228);
            this.progressBar1.Maximum = 10000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(509, 23);
            this.progressBar1.TabIndex = 18;
            // 
            // btnMcb1
            // 
            this.btnMcb1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMcb1.Location = new System.Drawing.Point(753, 39);
            this.btnMcb1.Name = "btnMcb1";
            this.btnMcb1.Size = new System.Drawing.Size(25, 19);
            this.btnMcb1.TabIndex = 17;
            this.btnMcb1.Text = "...";
            this.btnMcb1.UseVisualStyleBackColor = true;
            this.btnMcb1.Click += new System.EventHandler(this.btnMcb1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Modified files";
            // 
            // txtMcb
            // 
            this.txtMcb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMcb.Enabled = false;
            this.txtMcb.Location = new System.Drawing.Point(139, 36);
            this.txtMcb.Name = "txtMcb";
            this.txtMcb.Size = new System.Drawing.Size(576, 20);
            this.txtMcb.TabIndex = 15;
            // 
            // txtModified
            // 
            this.txtModified.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModified.Enabled = false;
            this.txtModified.Location = new System.Drawing.Point(139, 62);
            this.txtModified.Name = "txtModified";
            this.txtModified.Size = new System.Drawing.Size(576, 20);
            this.txtModified.TabIndex = 11;
            // 
            // txtPathArc
            // 
            this.txtPathArc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPathArc.Enabled = false;
            this.txtPathArc.Location = new System.Drawing.Point(139, 10);
            this.txtPathArc.Name = "txtPathArc";
            this.txtPathArc.Size = new System.Drawing.Size(576, 20);
            this.txtPathArc.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "mcb1.bln";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 13;
            // 
            // btnModified
            // 
            this.btnModified.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModified.Location = new System.Drawing.Point(753, 65);
            this.btnModified.Name = "btnModified";
            this.btnModified.Size = new System.Drawing.Size(25, 19);
            this.btnModified.TabIndex = 12;
            this.btnModified.Text = "...";
            this.btnModified.UseVisualStyleBackColor = true;
            this.btnModified.Click += new System.EventHandler(this.btnModified_Click);
            // 
            // btnImportArc
            // 
            this.btnImportArc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportArc.Enabled = false;
            this.btnImportArc.Location = new System.Drawing.Point(703, 228);
            this.btnImportArc.Name = "btnImportArc";
            this.btnImportArc.Size = new System.Drawing.Size(75, 23);
            this.btnImportArc.TabIndex = 8;
            this.btnImportArc.Text = "Import";
            this.btnImportArc.UseVisualStyleBackColor = true;
            this.btnImportArc.Click += new System.EventHandler(this.btnImportArc_Click);
            // 
            // btnExportArc
            // 
            this.btnExportArc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportArc.Enabled = false;
            this.btnExportArc.Location = new System.Drawing.Point(9, 228);
            this.btnExportArc.Name = "btnExportArc";
            this.btnExportArc.Size = new System.Drawing.Size(75, 23);
            this.btnExportArc.TabIndex = 7;
            this.btnExportArc.Tag = "";
            this.btnExportArc.Text = "Extract";
            this.btnExportArc.UseVisualStyleBackColor = true;
            this.btnExportArc.Click += new System.EventHandler(this.btnExportArc_Click);
            // 
            // btnBrowseArc
            // 
            this.btnBrowseArc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseArc.Location = new System.Drawing.Point(753, 10);
            this.btnBrowseArc.Name = "btnBrowseArc";
            this.btnBrowseArc.Size = new System.Drawing.Size(25, 19);
            this.btnBrowseArc.TabIndex = 5;
            this.btnBrowseArc.Text = "...";
            this.btnBrowseArc.UseVisualStyleBackColor = true;
            this.btnBrowseArc.Click += new System.EventHandler(this.btnBrowseArc_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Archive file (ui/grp/...)";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnTxtImport);
            this.tabPage1.Controls.Add(this.btnTxtExport);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.txtPathTxt);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(784, 285);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Text";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnTxtImport
            // 
            this.btnTxtImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTxtImport.Enabled = false;
            this.btnTxtImport.Location = new System.Drawing.Point(703, 228);
            this.btnTxtImport.Name = "btnTxtImport";
            this.btnTxtImport.Size = new System.Drawing.Size(75, 23);
            this.btnTxtImport.TabIndex = 6;
            this.btnTxtImport.Text = "Import";
            this.btnTxtImport.UseVisualStyleBackColor = true;
            this.btnTxtImport.Click += new System.EventHandler(this.btnTxtImport_Click);
            // 
            // btnTxtExport
            // 
            this.btnTxtExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTxtExport.Enabled = false;
            this.btnTxtExport.Location = new System.Drawing.Point(9, 228);
            this.btnTxtExport.Name = "btnTxtExport";
            this.btnTxtExport.Size = new System.Drawing.Size(75, 23);
            this.btnTxtExport.TabIndex = 5;
            this.btnTxtExport.Tag = "";
            this.btnTxtExport.Text = "Export";
            this.btnTxtExport.UseVisualStyleBackColor = true;
            this.btnTxtExport.Click += new System.EventHandler(this.btnTxtExport_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(753, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 19);
            this.button1.TabIndex = 2;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtPathTxt
            // 
            this.txtPathTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPathTxt.Enabled = false;
            this.txtPathTxt.Location = new System.Drawing.Point(117, 10);
            this.txtPathTxt.Name = "txtPathTxt";
            this.txtPathTxt.Size = new System.Drawing.Size(598, 20);
            this.txtPathTxt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(12, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(792, 311);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.button2);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.textBox2);
            this.tabPage3.Controls.Add(this.btnConvert);
            this.tabPage3.Controls.Add(this.textBox1);
            this.tabPage3.Controls.Add(this.listBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(784, 285);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Password";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Input";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Output";
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button2.Location = new System.Drawing.Point(244, 49);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(171, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Decrypt";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(418, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Default Passwords";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(24, 96);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(391, 20);
            this.textBox2.TabIndex = 3;
            // 
            // btnConvert
            // 
            this.btnConvert.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnConvert.Location = new System.Drawing.Point(24, 49);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(171, 23);
            this.btnConvert.TabIndex = 2;
            this.btnConvert.Text = "Encrypt";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btn_Convert);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(24, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(391, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(421, 29);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(361, 238);
            this.listBox1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button5);
            this.tabPage4.Controls.Add(this.button4);
            this.tabPage4.Controls.Add(this.button3);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.textBox4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(784, 285);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Texture";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(139, 10);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(576, 20);
            this.textBox4.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Input";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(753, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(25, 19);
            this.button3.TabIndex = 13;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(9, 228);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 14;
            this.button4.Tag = "";
            this.button4.Text = "Export";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(703, 228);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 15;
            this.button5.Tag = "";
            this.button5.Text = "Import png";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.chkHeader);
            this.tabPage5.Controls.Add(this.btnCompress);
            this.tabPage5.Controls.Add(this.btnDecompress);
            this.tabPage5.Controls.Add(this.button8);
            this.tabPage5.Controls.Add(this.textBox3);
            this.tabPage5.Controls.Add(this.label10);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(784, 285);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Compression";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btnCompress
            // 
            this.btnCompress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompress.Enabled = false;
            this.btnCompress.Location = new System.Drawing.Point(703, 228);
            this.btnCompress.Name = "btnCompress";
            this.btnCompress.Size = new System.Drawing.Size(75, 23);
            this.btnCompress.TabIndex = 11;
            this.btnCompress.Enabled = false;
            this.btnCompress.Text = "Compress";
            this.btnCompress.UseVisualStyleBackColor = true;
            this.btnCompress.Click += new System.EventHandler(this.btnCompress_Click);
            // 
            // btnDecompress
            // 
            this.btnDecompress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDecompress.Enabled = false;
            this.btnDecompress.Location = new System.Drawing.Point(9, 228);
            this.btnDecompress.Name = "btnDecompress";
            this.btnDecompress.Size = new System.Drawing.Size(75, 23);
            this.btnDecompress.TabIndex = 10;
            this.btnDecompress.Tag = "";
            this.btnDecompress.Enabled = false;
            this.btnDecompress.Text = "Decompress";
            this.btnDecompress.UseVisualStyleBackColor = true;
            this.btnDecompress.Click += new System.EventHandler(this.btnDecompress_Click);
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Location = new System.Drawing.Point(753, 10);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(25, 19);
            this.button8.TabIndex = 9;
            this.button8.Text = "...";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(117, 10);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(598, 20);
            this.textBox3.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "File";
            // 
            // chkHeader
            // 
            this.chkHeader.AutoSize = true;
            this.chkHeader.Location = new System.Drawing.Point(670, 257);
            this.chkHeader.Name = "chkHeader";
            this.chkHeader.Size = new System.Drawing.Size(108, 17);
            this.chkHeader.TabIndex = 12;
            this.chkHeader.Text = "Remove header?";
            this.chkHeader.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 336);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(376, 375);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Strikers2013-Tools";
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnMcb1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMcb;
        private System.Windows.Forms.TextBox txtModified;
        private System.Windows.Forms.TextBox txtPathArc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnModified;
        private System.Windows.Forms.Button btnImportArc;
        private System.Windows.Forms.Button btnExportArc;
        private System.Windows.Forms.Button btnBrowseArc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnTxtImport;
        private System.Windows.Forms.Button btnTxtExport;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPathTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button btnCompress;
        private System.Windows.Forms.Button btnDecompress;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkHeader;
    }
}