namespace EMD
{
    partial class EMDForm
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
            this.picImg1 = new System.Windows.Forms.PictureBox();
            this.picImg2 = new System.Windows.Forms.PictureBox();
            this.lblEMD = new System.Windows.Forms.Label();
            this.lblCorrel = new System.Windows.Forms.Label();
            this.lblChisqr = new System.Windows.Forms.Label();
            this.lblIntersect = new System.Windows.Forms.Label();
            this.lblBhattacharyya = new System.Windows.Forms.Label();
            this.lblChisqrAlt = new System.Windows.Forms.Label();
            this.btnCalc = new System.Windows.Forms.Button();
            this.picHist1 = new System.Windows.Forms.PictureBox();
            this.picHist2 = new System.Windows.Forms.PictureBox();
            this.txtBinSize = new System.Windows.Forms.TextBox();
            this.lblBinSize = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picImg1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImg2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHist1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHist2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadPic1
            // 
            this.btnLoadPic1.Location = new System.Drawing.Point(11, 11);
            this.btnLoadPic1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoadPic1.Name = "btnLoadPic1";
            this.btnLoadPic1.Size = new System.Drawing.Size(65, 35);
            this.btnLoadPic1.TabIndex = 0;
            this.btnLoadPic1.Text = "Load1";
            this.btnLoadPic1.UseVisualStyleBackColor = true;
            this.btnLoadPic1.Click += new System.EventHandler(this.btnLoadPic1_Click);
            // 
            // btnLoadPic2
            // 
            this.btnLoadPic2.Location = new System.Drawing.Point(509, 9);
            this.btnLoadPic2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoadPic2.Name = "btnLoadPic2";
            this.btnLoadPic2.Size = new System.Drawing.Size(65, 35);
            this.btnLoadPic2.TabIndex = 0;
            this.btnLoadPic2.Text = "Load2";
            this.btnLoadPic2.UseVisualStyleBackColor = true;
            this.btnLoadPic2.Click += new System.EventHandler(this.btnLoadPic2_Click);
            // 
            // picImg1
            // 
            this.picImg1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picImg1.Location = new System.Drawing.Point(11, 51);
            this.picImg1.Name = "picImg1";
            this.picImg1.Size = new System.Drawing.Size(226, 244);
            this.picImg1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImg1.TabIndex = 1;
            this.picImg1.TabStop = false;
            // 
            // picImg2
            // 
            this.picImg2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picImg2.Location = new System.Drawing.Point(509, 51);
            this.picImg2.Name = "picImg2";
            this.picImg2.Size = new System.Drawing.Size(226, 244);
            this.picImg2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImg2.TabIndex = 1;
            this.picImg2.TabStop = false;
            // 
            // lblEMD
            // 
            this.lblEMD.AutoSize = true;
            this.lblEMD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEMD.Location = new System.Drawing.Point(10, 470);
            this.lblEMD.Name = "lblEMD";
            this.lblEMD.Size = new System.Drawing.Size(44, 16);
            this.lblEMD.TabIndex = 2;
            this.lblEMD.Text = "EMD: ";
            // 
            // lblCorrel
            // 
            this.lblCorrel.AutoSize = true;
            this.lblCorrel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorrel.Location = new System.Drawing.Point(10, 320);
            this.lblCorrel.Name = "lblCorrel";
            this.lblCorrel.Size = new System.Drawing.Size(79, 16);
            this.lblCorrel.TabIndex = 2;
            this.lblCorrel.Text = "Correlation: ";
            // 
            // lblChisqr
            // 
            this.lblChisqr.AutoSize = true;
            this.lblChisqr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChisqr.Location = new System.Drawing.Point(10, 350);
            this.lblChisqr.Name = "lblChisqr";
            this.lblChisqr.Size = new System.Drawing.Size(81, 16);
            this.lblChisqr.TabIndex = 3;
            this.lblChisqr.Text = "Chi-Square: ";
            // 
            // lblIntersect
            // 
            this.lblIntersect.AutoSize = true;
            this.lblIntersect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIntersect.Location = new System.Drawing.Point(10, 380);
            this.lblIntersect.Name = "lblIntersect";
            this.lblIntersect.Size = new System.Drawing.Size(82, 16);
            this.lblIntersect.TabIndex = 3;
            this.lblIntersect.Text = "Intersection: ";
            // 
            // lblBhattacharyya
            // 
            this.lblBhattacharyya.AutoSize = true;
            this.lblBhattacharyya.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBhattacharyya.Location = new System.Drawing.Point(10, 410);
            this.lblBhattacharyya.Name = "lblBhattacharyya";
            this.lblBhattacharyya.Size = new System.Drawing.Size(100, 16);
            this.lblBhattacharyya.TabIndex = 3;
            this.lblBhattacharyya.Text = "Bhattacharyya: ";
            // 
            // lblChisqrAlt
            // 
            this.lblChisqrAlt.AutoSize = true;
            this.lblChisqrAlt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChisqrAlt.Location = new System.Drawing.Point(10, 440);
            this.lblChisqrAlt.Name = "lblChisqrAlt";
            this.lblChisqrAlt.Size = new System.Drawing.Size(147, 16);
            this.lblChisqrAlt.TabIndex = 3;
            this.lblChisqrAlt.Text = "Alternative Chi-Square: ";
            // 
            // btnCalc
            // 
            this.btnCalc.Location = new System.Drawing.Point(341, 51);
            this.btnCalc.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(65, 35);
            this.btnCalc.TabIndex = 0;
            this.btnCalc.Text = "Calc";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // picHist1
            // 
            this.picHist1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picHist1.Location = new System.Drawing.Point(251, 173);
            this.picHist1.Name = "picHist1";
            this.picHist1.Size = new System.Drawing.Size(114, 123);
            this.picHist1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHist1.TabIndex = 1;
            this.picHist1.TabStop = false;
            // 
            // picHist2
            // 
            this.picHist2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picHist2.Location = new System.Drawing.Point(378, 173);
            this.picHist2.Name = "picHist2";
            this.picHist2.Size = new System.Drawing.Size(114, 123);
            this.picHist2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHist2.TabIndex = 1;
            this.picHist2.TabStop = false;
            // 
            // txtBinSize
            // 
            this.txtBinSize.Location = new System.Drawing.Point(378, 118);
            this.txtBinSize.Name = "txtBinSize";
            this.txtBinSize.Size = new System.Drawing.Size(100, 20);
            this.txtBinSize.TabIndex = 4;
            this.txtBinSize.Text = "256";
            this.txtBinSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblBinSize
            // 
            this.lblBinSize.AutoSize = true;
            this.lblBinSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBinSize.Location = new System.Drawing.Point(294, 119);
            this.lblBinSize.Name = "lblBinSize";
            this.lblBinSize.Size = new System.Drawing.Size(62, 16);
            this.lblBinSize.TabIndex = 5;
            this.lblBinSize.Text = "BinSize : ";
            // 
            // EMDForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 518);
            this.Controls.Add(this.lblBinSize);
            this.Controls.Add(this.txtBinSize);
            this.Controls.Add(this.lblChisqrAlt);
            this.Controls.Add(this.lblBhattacharyya);
            this.Controls.Add(this.lblIntersect);
            this.Controls.Add(this.lblChisqr);
            this.Controls.Add(this.lblCorrel);
            this.Controls.Add(this.lblEMD);
            this.Controls.Add(this.picImg2);
            this.Controls.Add(this.picHist2);
            this.Controls.Add(this.picHist1);
            this.Controls.Add(this.picImg1);
            this.Controls.Add(this.btnLoadPic2);
            this.Controls.Add(this.btnCalc);
            this.Controls.Add(this.btnLoadPic1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "EMDForm";
            this.Text = "Earth Mover\'s Distance";
            ((System.ComponentModel.ISupportInitialize)(this.picImg1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImg2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHist1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHist2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadPic1;
        private System.Windows.Forms.Button btnLoadPic2;
        private System.Windows.Forms.PictureBox picImg1;
        private System.Windows.Forms.PictureBox picImg2;
        private System.Windows.Forms.Label lblEMD;
        private System.Windows.Forms.Label lblCorrel;
        private System.Windows.Forms.Label lblChisqr;
        private System.Windows.Forms.Label lblIntersect;
        private System.Windows.Forms.Label lblBhattacharyya;
        private System.Windows.Forms.Label lblChisqrAlt;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.PictureBox picHist1;
        private System.Windows.Forms.PictureBox picHist2;
        private System.Windows.Forms.TextBox txtBinSize;
        private System.Windows.Forms.Label lblBinSize;
    }
}

