namespace DomDocumentTest
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbRecompositionEventDetails = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.webBrowser1);
            this.panel1.Location = new System.Drawing.Point(39, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1726, 1296);
            this.panel1.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1726, 1296);
            this.webBrowser1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tbRecompositionEventDetails);
            this.panel2.Location = new System.Drawing.Point(1840, 60);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(568, 1238);
            this.panel2.TabIndex = 1;
            // 
            // tbRecompositionEventDetails
            // 
            this.tbRecompositionEventDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRecompositionEventDetails.Location = new System.Drawing.Point(0, 0);
            this.tbRecompositionEventDetails.Multiline = true;
            this.tbRecompositionEventDetails.Name = "tbRecompositionEventDetails";
            this.tbRecompositionEventDetails.Size = new System.Drawing.Size(568, 1238);
            this.tbRecompositionEventDetails.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1835, 1320);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Recomposition Exceptions";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2462, 1422);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbRecompositionEventDetails;
        private System.Windows.Forms.Label label1;
        
    }
}

