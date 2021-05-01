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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnTxtImport = new System.Windows.Forms.Button();
            this.btnTxtExport = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbAccents = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtPathTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnMcb1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMcb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnModified = new System.Windows.Forms.Button();
            this.txtModified = new System.Windows.Forms.TextBox();
            this.btnImportArc = new System.Windows.Forms.Button();
            this.btnExportArc = new System.Windows.Forms.Button();
            this.btnBrowseArc = new System.Windows.Forms.Button();
            this.txtPathArc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnOpenImage = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(607, 470);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnTxtImport);
            this.tabPage1.Controls.Add(this.btnTxtExport);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.cmbAccents);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.txtPathTxt);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(599, 444);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Text";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnTxtImport
            // 
            this.btnTxtImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTxtImport.Enabled = false;
            this.btnTxtImport.Location = new System.Drawing.Point(518, 415);
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
            this.btnTxtExport.Location = new System.Drawing.Point(9, 415);
            this.btnTxtExport.Name = "btnTxtExport";
            this.btnTxtExport.Size = new System.Drawing.Size(75, 23);
            this.btnTxtExport.TabIndex = 5;
            this.btnTxtExport.Tag = "";
            this.btnTxtExport.Text = "Export";
            this.btnTxtExport.UseVisualStyleBackColor = true;
            this.btnTxtExport.Click += new System.EventHandler(this.btnTxtExport_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Accent configuration";
            // 
            // cmbAccents
            // 
            this.cmbAccents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAccents.FormattingEnabled = true;
            this.cmbAccents.Location = new System.Drawing.Point(117, 36);
            this.cmbAccents.Name = "cmbAccents";
            this.cmbAccents.Size = new System.Drawing.Size(445, 21);
            this.cmbAccents.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(568, 10);
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
            this.txtPathTxt.Size = new System.Drawing.Size(445, 20);
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnMcb1);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtMcb);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.btnModified);
            this.tabPage2.Controls.Add(this.txtModified);
            this.tabPage2.Controls.Add(this.btnImportArc);
            this.tabPage2.Controls.Add(this.btnExportArc);
            this.tabPage2.Controls.Add(this.btnBrowseArc);
            this.tabPage2.Controls.Add(this.txtPathArc);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(599, 444);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Archive";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnMcb1
            // 
            this.btnMcb1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMcb1.Location = new System.Drawing.Point(568, 62);
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
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "mcb1 path (if importing)";
            // 
            // txtMcb
            // 
            this.txtMcb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMcb.Enabled = false;
            this.txtMcb.Location = new System.Drawing.Point(139, 62);
            this.txtMcb.Name = "txtMcb";
            this.txtMcb.Size = new System.Drawing.Size(419, 20);
            this.txtMcb.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Modified files (if importing)";
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
            this.btnModified.Location = new System.Drawing.Point(568, 35);
            this.btnModified.Name = "btnModified";
            this.btnModified.Size = new System.Drawing.Size(25, 19);
            this.btnModified.TabIndex = 12;
            this.btnModified.Text = "...";
            this.btnModified.UseVisualStyleBackColor = true;
            this.btnModified.Click += new System.EventHandler(this.btnModified_Click);
            // 
            // txtModified
            // 
            this.txtModified.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModified.Enabled = false;
            this.txtModified.Location = new System.Drawing.Point(139, 36);
            this.txtModified.Name = "txtModified";
            this.txtModified.Size = new System.Drawing.Size(419, 20);
            this.txtModified.TabIndex = 11;
            // 
            // btnImportArc
            // 
            this.btnImportArc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportArc.Enabled = false;
            this.btnImportArc.Location = new System.Drawing.Point(518, 415);
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
            this.btnExportArc.Location = new System.Drawing.Point(9, 415);
            this.btnExportArc.Name = "btnExportArc";
            this.btnExportArc.Size = new System.Drawing.Size(75, 23);
            this.btnExportArc.TabIndex = 7;
            this.btnExportArc.Tag = "";
            this.btnExportArc.Text = "Export";
            this.btnExportArc.UseVisualStyleBackColor = true;
            this.btnExportArc.Click += new System.EventHandler(this.btnExportArc_Click);
            // 
            // btnBrowseArc
            // 
            this.btnBrowseArc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseArc.Location = new System.Drawing.Point(568, 10);
            this.btnBrowseArc.Name = "btnBrowseArc";
            this.btnBrowseArc.Size = new System.Drawing.Size(25, 19);
            this.btnBrowseArc.TabIndex = 5;
            this.btnBrowseArc.Text = "...";
            this.btnBrowseArc.UseVisualStyleBackColor = true;
            this.btnBrowseArc.Click += new System.EventHandler(this.btnBrowseArc_Click);
            // 
            // txtPathArc
            // 
            this.txtPathArc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPathArc.Enabled = false;
            this.txtPathArc.Location = new System.Drawing.Point(139, 10);
            this.txtPathArc.Name = "txtPathArc";
            this.txtPathArc.Size = new System.Drawing.Size(419, 20);
            this.txtPathArc.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "File";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.comboBox1);
            this.tabPage3.Controls.Add(this.btnOpenImage);
            this.tabPage3.Controls.Add(this.textBox1);
            this.tabPage3.Controls.Add(this.pictureBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(599, 444);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Text Renderer";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(592, 329);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(4, 340);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(314, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnOpenImage
            // 
            this.btnOpenImage.Location = new System.Drawing.Point(521, 338);
            this.btnOpenImage.Name = "btnOpenImage";
            this.btnOpenImage.Size = new System.Drawing.Size(75, 23);
            this.btnOpenImage.TabIndex = 2;
            this.btnOpenImage.Text = "Open";
            this.btnOpenImage.UseVisualStyleBackColor = true;
            this.btnOpenImage.Click += new System.EventHandler(this.btnOpenImage_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(324, 340);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(191, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 495);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(376, 375);
            this.Name = "MainForm";
            this.Text = "Strikers2013-Tools";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbAccents;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPathTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnTxtImport;
        private System.Windows.Forms.Button btnTxtExport;
        private System.Windows.Forms.Button btnImportArc;
        private System.Windows.Forms.Button btnExportArc;
        private System.Windows.Forms.Button btnBrowseArc;
        private System.Windows.Forms.TextBox txtPathArc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnMcb1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMcb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnModified;
        private System.Windows.Forms.TextBox txtModified;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnOpenImage;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}