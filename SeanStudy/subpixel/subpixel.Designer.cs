namespace subpixel
{
    partial class subpixel
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
            this.btnLoad = new System.Windows.Forms.Button();
            this.picSrc = new System.Windows.Forms.PictureBox();
            this.picDst = new System.Windows.Forms.PictureBox();
            this.lblSrcCenter = new System.Windows.Forms.Label();
            this.lblDstCenter = new System.Windows.Forms.Label();
            this.lblDiff = new System.Windows.Forms.Label();
            this.txtSigma = new System.Windows.Forms.TextBox();
            this.lblSigma = new System.Windows.Forms.Label();
            this.lblCannyH = new System.Windows.Forms.Label();
            this.txtCannyH = new System.Windows.Forms.TextBox();
            this.lblCannyL = new System.Windows.Forms.Label();
            this.txtCannyL = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picSrc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDst)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(26, 22);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(65, 41);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // picSrc
            // 
            this.picSrc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSrc.Location = new System.Drawing.Point(26, 162);
            this.picSrc.Name = "picSrc";
            this.picSrc.Size = new System.Drawing.Size(300, 300);
            this.picSrc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSrc.TabIndex = 1;
            this.picSrc.TabStop = false;
            // 
            // picDst
            // 
            this.picDst.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picDst.Location = new System.Drawing.Point(469, 162);
            this.picDst.Name = "picDst";
            this.picDst.Size = new System.Drawing.Size(300, 300);
            this.picDst.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDst.TabIndex = 1;
            this.picDst.TabStop = false;
            // 
            // lblSrcCenter
            // 
            this.lblSrcCenter.AutoSize = true;
            this.lblSrcCenter.Location = new System.Drawing.Point(23, 121);
            this.lblSrcCenter.Name = "lblSrcCenter";
            this.lblSrcCenter.Size = new System.Drawing.Size(186, 17);
            this.lblSrcCenter.TabIndex = 2;
            this.lblSrcCenter.Text = "Center point of FindContour:";
            // 
            // lblDstCenter
            // 
            this.lblDstCenter.AutoSize = true;
            this.lblDstCenter.Location = new System.Drawing.Point(466, 121);
            this.lblDstCenter.Name = "lblDstCenter";
            this.lblDstCenter.Size = new System.Drawing.Size(152, 17);
            this.lblDstCenter.TabIndex = 2;
            this.lblDstCenter.Text = "Center point of SubPix:";
            // 
            // lblDiff
            // 
            this.lblDiff.AutoSize = true;
            this.lblDiff.Location = new System.Drawing.Point(466, 46);
            this.lblDiff.Name = "lblDiff";
            this.lblDiff.Size = new System.Drawing.Size(77, 17);
            this.lblDiff.TabIndex = 2;
            this.lblDiff.Text = "Difference:";
            // 
            // txtSigma
            // 
            this.txtSigma.Location = new System.Drawing.Point(226, 13);
            this.txtSigma.Name = "txtSigma";
            this.txtSigma.Size = new System.Drawing.Size(100, 22);
            this.txtSigma.TabIndex = 3;
            this.txtSigma.Text = "0.0";
            this.txtSigma.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSigma
            // 
            this.lblSigma.AutoSize = true;
            this.lblSigma.Location = new System.Drawing.Point(156, 16);
            this.lblSigma.Name = "lblSigma";
            this.lblSigma.Size = new System.Drawing.Size(51, 17);
            this.lblSigma.TabIndex = 4;
            this.lblSigma.Text = "Sigma:";
            // 
            // lblCannyH
            // 
            this.lblCannyH.AutoSize = true;
            this.lblCannyH.Location = new System.Drawing.Point(118, 52);
            this.lblCannyH.Name = "lblCannyH";
            this.lblCannyH.Size = new System.Drawing.Size(89, 17);
            this.lblCannyH.TabIndex = 6;
            this.lblCannyH.Text = "Canny_High:";
            // 
            // txtCannyH
            // 
            this.txtCannyH.Location = new System.Drawing.Point(226, 47);
            this.txtCannyH.Name = "txtCannyH";
            this.txtCannyH.Size = new System.Drawing.Size(100, 22);
            this.txtCannyH.TabIndex = 5;
            this.txtCannyH.Text = "300";
            this.txtCannyH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblCannyL
            // 
            this.lblCannyL.AutoSize = true;
            this.lblCannyL.Location = new System.Drawing.Point(122, 86);
            this.lblCannyL.Name = "lblCannyL";
            this.lblCannyL.Size = new System.Drawing.Size(85, 17);
            this.lblCannyL.TabIndex = 8;
            this.lblCannyL.Text = "Canny_Low:";
            // 
            // txtCannyL
            // 
            this.txtCannyL.Location = new System.Drawing.Point(226, 86);
            this.txtCannyL.Name = "txtCannyL";
            this.txtCannyL.Size = new System.Drawing.Size(100, 22);
            this.txtCannyL.TabIndex = 7;
            this.txtCannyL.Text = "100";
            this.txtCannyL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // subpixel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 527);
            this.Controls.Add(this.lblCannyL);
            this.Controls.Add(this.txtCannyL);
            this.Controls.Add(this.lblCannyH);
            this.Controls.Add(this.txtCannyH);
            this.Controls.Add(this.lblSigma);
            this.Controls.Add(this.txtSigma);
            this.Controls.Add(this.lblDiff);
            this.Controls.Add(this.lblDstCenter);
            this.Controls.Add(this.lblSrcCenter);
            this.Controls.Add(this.picDst);
            this.Controls.Add(this.picSrc);
            this.Controls.Add(this.btnLoad);
            this.Name = "subpixel";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picSrc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDst)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.PictureBox picSrc;
        private System.Windows.Forms.PictureBox picDst;
        private System.Windows.Forms.Label lblSrcCenter;
        private System.Windows.Forms.Label lblDstCenter;
        private System.Windows.Forms.Label lblDiff;
        private System.Windows.Forms.TextBox txtSigma;
        private System.Windows.Forms.Label lblSigma;
        private System.Windows.Forms.Label lblCannyH;
        private System.Windows.Forms.TextBox txtCannyH;
        private System.Windows.Forms.Label lblCannyL;
        private System.Windows.Forms.TextBox txtCannyL;
    }
}

