namespace EMD
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
            this.btnLoadPic1 = new System.Windows.Forms.Button();
            this.btnLoadPic2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadPic1
            // 
            this.btnLoadPic1.Location = new System.Drawing.Point(37, 39);
            this.btnLoadPic1.Name = "btnLoadPic1";
            this.btnLoadPic1.Size = new System.Drawing.Size(195, 113);
            this.btnLoadPic1.TabIndex = 0;
            this.btnLoadPic1.Text = "Load1";
            this.btnLoadPic1.UseVisualStyleBackColor = true;
            this.btnLoadPic1.Click += new System.EventHandler(this.btnLoadPic1_Click);
            // 
            // btnLoadPic2
            // 
            this.btnLoadPic2.Location = new System.Drawing.Point(37, 200);
            this.btnLoadPic2.Name = "btnLoadPic2";
            this.btnLoadPic2.Size = new System.Drawing.Size(195, 113);
            this.btnLoadPic2.TabIndex = 0;
            this.btnLoadPic2.Text = "Load2";
            this.btnLoadPic2.UseVisualStyleBackColor = true;
            this.btnLoadPic2.Click += new System.EventHandler(this.btnLoadPic2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnLoadPic2);
            this.Controls.Add(this.btnLoadPic1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadPic1;
        private System.Windows.Forms.Button btnLoadPic2;
    }
}

