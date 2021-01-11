namespace CopaEditor
{
    partial class FileType
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
            this.rdi_14 = new System.Windows.Forms.RadioButton();
            this.rdi_37 = new System.Windows.Forms.RadioButton();
            this.rdi_2 = new System.Windows.Forms.RadioButton();
            this.btn_Ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rdi_14
            // 
            this.rdi_14.AutoSize = true;
            this.rdi_14.Checked = true;
            this.rdi_14.Location = new System.Drawing.Point(137, 84);
            this.rdi_14.Name = "rdi_14";
            this.rdi_14.Size = new System.Drawing.Size(54, 17);
            this.rdi_14.TabIndex = 0;
            this.rdi_14.TabStop = true;
            this.rdi_14.Text = "14.bin";
            this.rdi_14.UseVisualStyleBackColor = true;
            // 
            // rdi_37
            // 
            this.rdi_37.AutoSize = true;
            this.rdi_37.Location = new System.Drawing.Point(77, 84);
            this.rdi_37.Name = "rdi_37";
            this.rdi_37.Size = new System.Drawing.Size(54, 17);
            this.rdi_37.TabIndex = 1;
            this.rdi_37.Text = "37.bin";
            this.rdi_37.UseVisualStyleBackColor = true;
            // 
            // rdi_2
            // 
            this.rdi_2.AutoSize = true;
            this.rdi_2.Location = new System.Drawing.Point(23, 84);
            this.rdi_2.Name = "rdi_2";
            this.rdi_2.Size = new System.Drawing.Size(48, 17);
            this.rdi_2.TabIndex = 2;
            this.rdi_2.Text = "2.bin";
            this.rdi_2.UseVisualStyleBackColor = true;
            // 
            // btn_Ok
            // 
            this.btn_Ok.Location = new System.Drawing.Point(68, 156);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(75, 23);
            this.btn_Ok.TabIndex = 3;
            this.btn_Ok.Text = "OK";
            this.btn_Ok.UseVisualStyleBackColor = true;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // FileType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 191);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.rdi_2);
            this.Controls.Add(this.rdi_37);
            this.Controls.Add(this.rdi_14);
            this.Name = "FileType";
            this.Text = "Select the type";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdi_14;
        private System.Windows.Forms.RadioButton rdi_37;
        private System.Windows.Forms.RadioButton rdi_2;
        private System.Windows.Forms.Button btn_Ok;
    }
}