namespace superXBR
{
    partial class xBRForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblScale = new System.Windows.Forms.Label();
            this.picSrc = new System.Windows.Forms.PictureBox();
            this.picDst = new System.Windows.Forms.PictureBox();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.lblCost = new System.Windows.Forms.Label();
            this.btnDo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picSrc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDst)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.Location = new System.Drawing.Point(16, 13);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(105, 52);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblScale
            // 
            this.lblScale.AutoSize = true;
            this.lblScale.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScale.Location = new System.Drawing.Point(203, 13);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(47, 18);
            this.lblScale.TabIndex = 1;
            this.lblScale.Text = "Scale";
            // 
            // picSrc
            // 
            this.picSrc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSrc.Location = new System.Drawing.Point(16, 99);
            this.picSrc.Name = "picSrc";
            this.picSrc.Size = new System.Drawing.Size(300, 300);
            this.picSrc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSrc.TabIndex = 2;
            this.picSrc.TabStop = false;
            // 
            // picDst
            // 
            this.picDst.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picDst.Location = new System.Drawing.Point(349, 99);
            this.picDst.Name = "picDst";
            this.picDst.Size = new System.Drawing.Size(300, 300);
            this.picDst.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDst.TabIndex = 3;
            this.picDst.TabStop = false;
            // 
            // txtScale
            // 
            this.txtScale.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScale.Location = new System.Drawing.Point(174, 43);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(106, 26);
            this.txtScale.TabIndex = 4;
            this.txtScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblCost
            // 
            this.lblCost.AutoSize = true;
            this.lblCost.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCost.Location = new System.Drawing.Point(481, 30);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(88, 18);
            this.lblCost.TabIndex = 5;
            this.lblCost.Text = "Cost time:";
            // 
            // btnDo
            // 
            this.btnDo.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDo.Location = new System.Drawing.Point(349, 13);
            this.btnDo.Name = "btnDo";
            this.btnDo.Size = new System.Drawing.Size(105, 52);
            this.btnDo.TabIndex = 0;
            this.btnDo.Text = "Do";
            this.btnDo.UseVisualStyleBackColor = true;
            this.btnDo.Click += new System.EventHandler(this.btnDo_Click);
            // 
            // xBRForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 432);
            this.Controls.Add(this.lblCost);
            this.Controls.Add(this.txtScale);
            this.Controls.Add(this.picDst);
            this.Controls.Add(this.picSrc);
            this.Controls.Add(this.lblScale);
            this.Controls.Add(this.btnDo);
            this.Controls.Add(this.btnLoad);
            this.Name = "xBRForm";
            this.Text = "Super-xBR";
            ((System.ComponentModel.ISupportInitialize)(this.picSrc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDst)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblScale;
        private System.Windows.Forms.PictureBox picSrc;
        private System.Windows.Forms.PictureBox picDst;
        private System.Windows.Forms.TextBox txtScale;
        private System.Windows.Forms.Label lblCost;
        private System.Windows.Forms.Button btnDo;
    }
}

