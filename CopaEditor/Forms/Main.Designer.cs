﻿namespace CopaEditor
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tStrip_Text = new System.Windows.Forms.ToolStripMenuItem();
            this.tStrip_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tStrip_replaceMcb = new System.Windows.Forms.ToolStripMenuItem();
            this.tStrip_Portrait = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tStrip_Text,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(462, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tStrip_Text
            // 
            this.tStrip_Text.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tStrip_Open});
            this.tStrip_Text.Name = "tStrip_Text";
            this.tStrip_Text.Size = new System.Drawing.Size(40, 20);
            this.tStrip_Text.Text = "Text";
            // 
            // tStrip_Open
            // 
            this.tStrip_Open.Name = "tStrip_Open";
            this.tStrip_Open.Size = new System.Drawing.Size(180, 22);
            this.tStrip_Open.Text = "Open";
            this.tStrip_Open.Click += new System.EventHandler(this.tStrip_Open_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tStrip_replaceMcb,
            this.tStrip_Portrait});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // tStrip_replaceMcb
            // 
            this.tStrip_replaceMcb.Name = "tStrip_replaceMcb";
            this.tStrip_replaceMcb.Size = new System.Drawing.Size(180, 22);
            this.tStrip_replaceMcb.Text = "Replace mcb and ui";
            // 
            // tStrip_Portrait
            // 
            this.tStrip_Portrait.Name = "tStrip_Portrait";
            this.tStrip_Portrait.Size = new System.Drawing.Size(180, 22);
            this.tStrip_Portrait.Text = "Player Portrait";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(438, 324);
            this.label1.TabIndex = 1;
            this.label1.Text = "Welcome to Copa Editor\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 367);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Copa Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tStrip_Text;
        private System.Windows.Forms.ToolStripMenuItem tStrip_Open;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tStrip_replaceMcb;
        private System.Windows.Forms.ToolStripMenuItem tStrip_Portrait;
        private System.Windows.Forms.Label label1;
    }
}

