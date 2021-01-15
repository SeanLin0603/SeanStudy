namespace camera
{
    partial class CameraCalibrationView
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
            this.filepath = new System.Windows.Forms.TextBox();
            this.btnbrowse = new System.Windows.Forms.Button();
            this.srcpic = new System.Windows.Forms.PictureBox();
            this.dstpic = new System.Windows.Forms.PictureBox();
            this.btnfindcorner = new System.Windows.Forms.Button();
            this.setwidth = new System.Windows.Forms.NumericUpDown();
            this.setheight = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cameraMat00 = new System.Windows.Forms.Label();
            this.btngetmatrix = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cameraMat10 = new System.Windows.Forms.Label();
            this.cameraMat20 = new System.Windows.Forms.Label();
            this.cameraMat01 = new System.Windows.Forms.Label();
            this.cameraMat11 = new System.Windows.Forms.Label();
            this.cameraMat21 = new System.Windows.Forms.Label();
            this.cameraMat02 = new System.Windows.Forms.Label();
            this.cameraMat12 = new System.Windows.Forms.Label();
            this.cameraMat22 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.coef0 = new System.Windows.Forms.Label();
            this.coef1 = new System.Windows.Forms.Label();
            this.coef2 = new System.Windows.Forms.Label();
            this.coef3 = new System.Windows.Forms.Label();
            this.coef4 = new System.Windows.Forms.Label();
            this.btnundistortion = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDTV = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbltotalpts = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblsumoffset = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblaveoffset = new System.Windows.Forms.Label();
            this.lblmaxindex = new System.Windows.Forms.Label();
            this.lblminindex = new System.Windows.Forms.Label();
            this.lblmaxoffset = new System.Windows.Forms.Label();
            this.lblminoffset = new System.Windows.Forms.Label();
            this.btnSaveMat = new System.Windows.Forms.Button();
            this.btnLoadMat = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.srcpic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dstpic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setwidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setheight)).BeginInit();
            this.SuspendLayout();
            // 
            // filepath
            // 
            this.filepath.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filepath.Location = new System.Drawing.Point(9, 28);
            this.filepath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.filepath.Name = "filepath";
            this.filepath.Size = new System.Drawing.Size(235, 28);
            this.filepath.TabIndex = 0;
            // 
            // btnbrowse
            // 
            this.btnbrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnbrowse.Location = new System.Drawing.Point(248, 17);
            this.btnbrowse.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnbrowse.Name = "btnbrowse";
            this.btnbrowse.Size = new System.Drawing.Size(80, 25);
            this.btnbrowse.TabIndex = 1;
            this.btnbrowse.Text = "Browse";
            this.btnbrowse.UseVisualStyleBackColor = true;
            this.btnbrowse.Click += new System.EventHandler(this.btnbrowse_Click);
            // 
            // srcpic
            // 
            this.srcpic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.srcpic.Location = new System.Drawing.Point(9, 79);
            this.srcpic.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.srcpic.Name = "srcpic";
            this.srcpic.Size = new System.Drawing.Size(451, 488);
            this.srcpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.srcpic.TabIndex = 2;
            this.srcpic.TabStop = false;
            // 
            // dstpic
            // 
            this.dstpic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dstpic.Location = new System.Drawing.Point(464, 79);
            this.dstpic.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dstpic.Name = "dstpic";
            this.dstpic.Size = new System.Drawing.Size(451, 488);
            this.dstpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.dstpic.TabIndex = 2;
            this.dstpic.TabStop = false;
            // 
            // btnfindcorner
            // 
            this.btnfindcorner.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnfindcorner.Location = new System.Drawing.Point(248, 46);
            this.btnfindcorner.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnfindcorner.Name = "btnfindcorner";
            this.btnfindcorner.Size = new System.Drawing.Size(80, 25);
            this.btnfindcorner.TabIndex = 1;
            this.btnfindcorner.Text = "FindCorner";
            this.btnfindcorner.UseVisualStyleBackColor = true;
            this.btnfindcorner.Click += new System.EventHandler(this.btnfindcorner_Click);
            // 
            // setwidth
            // 
            this.setwidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setwidth.Location = new System.Drawing.Point(344, 40);
            this.setwidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.setwidth.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.setwidth.Name = "setwidth";
            this.setwidth.Size = new System.Drawing.Size(56, 28);
            this.setwidth.TabIndex = 4;
            this.setwidth.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // setheight
            // 
            this.setheight.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setheight.Location = new System.Drawing.Point(442, 40);
            this.setheight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.setheight.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.setheight.Name = "setheight";
            this.setheight.Size = new System.Drawing.Size(56, 28);
            this.setheight.TabIndex = 4;
            this.setheight.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(332, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "CornerWidth";
            // 
            // cameraMat00
            // 
            this.cameraMat00.AutoSize = true;
            this.cameraMat00.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cameraMat00.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cameraMat00.Location = new System.Drawing.Point(927, 46);
            this.cameraMat00.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cameraMat00.Name = "cameraMat00";
            this.cameraMat00.Size = new System.Drawing.Size(20, 22);
            this.cameraMat00.TabIndex = 5;
            this.cameraMat00.Text = "0";
            // 
            // btngetmatrix
            // 
            this.btngetmatrix.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btngetmatrix.Location = new System.Drawing.Point(530, 21);
            this.btngetmatrix.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btngetmatrix.Name = "btngetmatrix";
            this.btngetmatrix.Size = new System.Drawing.Size(80, 46);
            this.btngetmatrix.TabIndex = 1;
            this.btngetmatrix.Text = "GetMatrix";
            this.btngetmatrix.UseVisualStyleBackColor = true;
            this.btngetmatrix.Click += new System.EventHandler(this.btngetmatrix_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(922, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 24);
            this.label4.TabIndex = 5;
            this.label4.Text = "Camera Matrix";
            // 
            // cameraMat10
            // 
            this.cameraMat10.AutoSize = true;
            this.cameraMat10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cameraMat10.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cameraMat10.Location = new System.Drawing.Point(927, 81);
            this.cameraMat10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cameraMat10.Name = "cameraMat10";
            this.cameraMat10.Size = new System.Drawing.Size(20, 22);
            this.cameraMat10.TabIndex = 5;
            this.cameraMat10.Text = "0";
            // 
            // cameraMat20
            // 
            this.cameraMat20.AutoSize = true;
            this.cameraMat20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cameraMat20.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cameraMat20.Location = new System.Drawing.Point(927, 117);
            this.cameraMat20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cameraMat20.Name = "cameraMat20";
            this.cameraMat20.Size = new System.Drawing.Size(20, 22);
            this.cameraMat20.TabIndex = 5;
            this.cameraMat20.Text = "0";
            // 
            // cameraMat01
            // 
            this.cameraMat01.AutoSize = true;
            this.cameraMat01.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cameraMat01.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cameraMat01.Location = new System.Drawing.Point(1002, 46);
            this.cameraMat01.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cameraMat01.Name = "cameraMat01";
            this.cameraMat01.Size = new System.Drawing.Size(20, 22);
            this.cameraMat01.TabIndex = 5;
            this.cameraMat01.Text = "0";
            // 
            // cameraMat11
            // 
            this.cameraMat11.AutoSize = true;
            this.cameraMat11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cameraMat11.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cameraMat11.Location = new System.Drawing.Point(1002, 81);
            this.cameraMat11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cameraMat11.Name = "cameraMat11";
            this.cameraMat11.Size = new System.Drawing.Size(20, 22);
            this.cameraMat11.TabIndex = 5;
            this.cameraMat11.Text = "0";
            // 
            // cameraMat21
            // 
            this.cameraMat21.AutoSize = true;
            this.cameraMat21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cameraMat21.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cameraMat21.Location = new System.Drawing.Point(1002, 117);
            this.cameraMat21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cameraMat21.Name = "cameraMat21";
            this.cameraMat21.Size = new System.Drawing.Size(20, 22);
            this.cameraMat21.TabIndex = 5;
            this.cameraMat21.Text = "0";
            // 
            // cameraMat02
            // 
            this.cameraMat02.AutoSize = true;
            this.cameraMat02.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cameraMat02.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cameraMat02.Location = new System.Drawing.Point(1077, 46);
            this.cameraMat02.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cameraMat02.Name = "cameraMat02";
            this.cameraMat02.Size = new System.Drawing.Size(20, 22);
            this.cameraMat02.TabIndex = 5;
            this.cameraMat02.Text = "0";
            // 
            // cameraMat12
            // 
            this.cameraMat12.AutoSize = true;
            this.cameraMat12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cameraMat12.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cameraMat12.Location = new System.Drawing.Point(1077, 81);
            this.cameraMat12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cameraMat12.Name = "cameraMat12";
            this.cameraMat12.Size = new System.Drawing.Size(20, 22);
            this.cameraMat12.TabIndex = 5;
            this.cameraMat12.Text = "0";
            // 
            // cameraMat22
            // 
            this.cameraMat22.AutoSize = true;
            this.cameraMat22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cameraMat22.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cameraMat22.Location = new System.Drawing.Point(1077, 117);
            this.cameraMat22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cameraMat22.Name = "cameraMat22";
            this.cameraMat22.Size = new System.Drawing.Size(20, 22);
            this.cameraMat22.TabIndex = 5;
            this.cameraMat22.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(922, 147);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(185, 24);
            this.label12.TabIndex = 5;
            this.label12.Text = "Distortion coefficients";
            // 
            // coef0
            // 
            this.coef0.AutoSize = true;
            this.coef0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.coef0.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coef0.Location = new System.Drawing.Point(927, 183);
            this.coef0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.coef0.Name = "coef0";
            this.coef0.Size = new System.Drawing.Size(20, 22);
            this.coef0.TabIndex = 5;
            this.coef0.Text = "0";
            // 
            // coef1
            // 
            this.coef1.AutoSize = true;
            this.coef1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.coef1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coef1.Location = new System.Drawing.Point(927, 215);
            this.coef1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.coef1.Name = "coef1";
            this.coef1.Size = new System.Drawing.Size(20, 22);
            this.coef1.TabIndex = 5;
            this.coef1.Text = "0";
            // 
            // coef2
            // 
            this.coef2.AutoSize = true;
            this.coef2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.coef2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coef2.Location = new System.Drawing.Point(927, 248);
            this.coef2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.coef2.Name = "coef2";
            this.coef2.Size = new System.Drawing.Size(20, 22);
            this.coef2.TabIndex = 5;
            this.coef2.Text = "0";
            // 
            // coef3
            // 
            this.coef3.AutoSize = true;
            this.coef3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.coef3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coef3.Location = new System.Drawing.Point(927, 280);
            this.coef3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.coef3.Name = "coef3";
            this.coef3.Size = new System.Drawing.Size(20, 22);
            this.coef3.TabIndex = 5;
            this.coef3.Text = "0";
            // 
            // coef4
            // 
            this.coef4.AutoSize = true;
            this.coef4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.coef4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coef4.Location = new System.Drawing.Point(927, 313);
            this.coef4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.coef4.Name = "coef4";
            this.coef4.Size = new System.Drawing.Size(20, 22);
            this.coef4.TabIndex = 5;
            this.coef4.Text = "0";
            // 
            // btnundistortion
            // 
            this.btnundistortion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnundistortion.Location = new System.Drawing.Point(806, 20);
            this.btnundistortion.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnundistortion.Name = "btnundistortion";
            this.btnundistortion.Size = new System.Drawing.Size(100, 46);
            this.btnundistortion.TabIndex = 1;
            this.btnundistortion.Text = "Undistortion";
            this.btnundistortion.UseVisualStyleBackColor = true;
            this.btnundistortion.Click += new System.EventHandler(this.btnundistortion_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label3.Location = new System.Drawing.Point(920, 7);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(276, 141);
            this.label3.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.label5.Location = new System.Drawing.Point(920, 147);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(276, 199);
            this.label5.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label6.Location = new System.Drawing.Point(920, 348);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(276, 53);
            this.label6.TabIndex = 6;
            // 
            // lblDTV
            // 
            this.lblDTV.AutoSize = true;
            this.lblDTV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblDTV.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDTV.Location = new System.Drawing.Point(1052, 366);
            this.lblDTV.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDTV.Name = "lblDTV";
            this.lblDTV.Size = new System.Drawing.Size(20, 24);
            this.lblDTV.TabIndex = 5;
            this.lblDTV.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(927, 366);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 20);
            this.label8.TabIndex = 5;
            this.label8.Text = "TV Distortion";
            // 
            // lbltotalpts
            // 
            this.lbltotalpts.AutoSize = true;
            this.lbltotalpts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lbltotalpts.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltotalpts.Location = new System.Drawing.Point(1120, 403);
            this.lbltotalpts.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbltotalpts.Name = "lbltotalpts";
            this.lbltotalpts.Size = new System.Drawing.Size(20, 24);
            this.lbltotalpts.TabIndex = 5;
            this.lbltotalpts.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(922, 403);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 24);
            this.label11.TabIndex = 5;
            this.label11.Text = "Statistic";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label7.Location = new System.Drawing.Point(920, 401);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(276, 202);
            this.label7.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(423, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "CornerHeight";
            // 
            // lblsumoffset
            // 
            this.lblsumoffset.AutoSize = true;
            this.lblsumoffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblsumoffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsumoffset.Location = new System.Drawing.Point(1034, 442);
            this.lblsumoffset.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblsumoffset.Name = "lblsumoffset";
            this.lblsumoffset.Size = new System.Drawing.Size(20, 24);
            this.lblsumoffset.TabIndex = 5;
            this.lblsumoffset.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(1000, 403);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(106, 24);
            this.label14.TabIndex = 7;
            this.label14.Text = "Total points";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(927, 442);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 24);
            this.label10.TabIndex = 8;
            this.label10.Text = "Sum offset";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(927, 477);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(96, 24);
            this.label15.TabIndex = 9;
            this.label15.Text = "Ave. offset";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(927, 514);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(140, 24);
            this.label16.TabIndex = 10;
            this.label16.Text = "Max offset point";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(927, 555);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(135, 24);
            this.label17.TabIndex = 11;
            this.label17.Text = "Min offset point";
            // 
            // lblaveoffset
            // 
            this.lblaveoffset.AutoSize = true;
            this.lblaveoffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblaveoffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblaveoffset.Location = new System.Drawing.Point(1034, 477);
            this.lblaveoffset.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblaveoffset.Name = "lblaveoffset";
            this.lblaveoffset.Size = new System.Drawing.Size(20, 24);
            this.lblaveoffset.TabIndex = 12;
            this.lblaveoffset.Text = "0";
            // 
            // lblmaxindex
            // 
            this.lblmaxindex.AutoSize = true;
            this.lblmaxindex.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblmaxindex.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmaxindex.Location = new System.Drawing.Point(1065, 514);
            this.lblmaxindex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblmaxindex.Name = "lblmaxindex";
            this.lblmaxindex.Size = new System.Drawing.Size(20, 24);
            this.lblmaxindex.TabIndex = 13;
            this.lblmaxindex.Text = "0";
            // 
            // lblminindex
            // 
            this.lblminindex.AutoSize = true;
            this.lblminindex.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblminindex.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblminindex.Location = new System.Drawing.Point(1065, 555);
            this.lblminindex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblminindex.Name = "lblminindex";
            this.lblminindex.Size = new System.Drawing.Size(20, 24);
            this.lblminindex.TabIndex = 14;
            this.lblminindex.Text = "0";
            // 
            // lblmaxoffset
            // 
            this.lblmaxoffset.AutoSize = true;
            this.lblmaxoffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblmaxoffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmaxoffset.Location = new System.Drawing.Point(1097, 514);
            this.lblmaxoffset.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblmaxoffset.Name = "lblmaxoffset";
            this.lblmaxoffset.Size = new System.Drawing.Size(20, 24);
            this.lblmaxoffset.TabIndex = 15;
            this.lblmaxoffset.Text = "0";
            // 
            // lblminoffset
            // 
            this.lblminoffset.AutoSize = true;
            this.lblminoffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblminoffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblminoffset.Location = new System.Drawing.Point(1097, 555);
            this.lblminoffset.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblminoffset.Name = "lblminoffset";
            this.lblminoffset.Size = new System.Drawing.Size(20, 24);
            this.lblminoffset.TabIndex = 16;
            this.lblminoffset.Text = "0";
            // 
            // btnSaveMat
            // 
            this.btnSaveMat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnSaveMat.Location = new System.Drawing.Point(614, 22);
            this.btnSaveMat.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveMat.Name = "btnSaveMat";
            this.btnSaveMat.Size = new System.Drawing.Size(80, 46);
            this.btnSaveMat.TabIndex = 1;
            this.btnSaveMat.Text = "SaveMatrix";
            this.btnSaveMat.UseVisualStyleBackColor = true;
            this.btnSaveMat.Click += new System.EventHandler(this.btnSaveMat_Click);
            // 
            // btnLoadMat
            // 
            this.btnLoadMat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnLoadMat.Location = new System.Drawing.Point(698, 21);
            this.btnLoadMat.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoadMat.Name = "btnLoadMat";
            this.btnLoadMat.Size = new System.Drawing.Size(80, 46);
            this.btnLoadMat.TabIndex = 1;
            this.btnLoadMat.Text = "LoadMatrix";
            this.btnLoadMat.UseVisualStyleBackColor = true;
            this.btnLoadMat.Click += new System.EventHandler(this.btnLoadMat_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblStatus.Location = new System.Drawing.Point(11, 586);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(52, 17);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Status:";
            // 
            // CameraCalibrationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1218, 618);
            this.Controls.Add(this.lblminoffset);
            this.Controls.Add(this.lblmaxoffset);
            this.Controls.Add(this.lblminindex);
            this.Controls.Add(this.lblmaxindex);
            this.Controls.Add(this.lblaveoffset);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cameraMat22);
            this.Controls.Add(this.cameraMat12);
            this.Controls.Add(this.coef4);
            this.Controls.Add(this.coef3);
            this.Controls.Add(this.coef2);
            this.Controls.Add(this.cameraMat02);
            this.Controls.Add(this.cameraMat21);
            this.Controls.Add(this.coef1);
            this.Controls.Add(this.cameraMat11);
            this.Controls.Add(this.cameraMat01);
            this.Controls.Add(this.cameraMat20);
            this.Controls.Add(this.coef0);
            this.Controls.Add(this.cameraMat10);
            this.Controls.Add(this.lbltotalpts);
            this.Controls.Add(this.lblDTV);
            this.Controls.Add(this.cameraMat00);
            this.Controls.Add(this.lblsumoffset);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.setheight);
            this.Controls.Add(this.setwidth);
            this.Controls.Add(this.dstpic);
            this.Controls.Add(this.srcpic);
            this.Controls.Add(this.btnundistortion);
            this.Controls.Add(this.btnLoadMat);
            this.Controls.Add(this.btnSaveMat);
            this.Controls.Add(this.btngetmatrix);
            this.Controls.Add(this.btnfindcorner);
            this.Controls.Add(this.btnbrowse);
            this.Controls.Add(this.filepath);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "CameraCalibrationView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.srcpic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dstpic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setwidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setheight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox filepath;
        private System.Windows.Forms.Button btnbrowse;
        private System.Windows.Forms.PictureBox srcpic;
        private System.Windows.Forms.PictureBox dstpic;
        private System.Windows.Forms.Button btnfindcorner;
        private System.Windows.Forms.NumericUpDown setwidth;
        private System.Windows.Forms.NumericUpDown setheight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label cameraMat00;
        private System.Windows.Forms.Button btngetmatrix;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label cameraMat10;
        private System.Windows.Forms.Label cameraMat20;
        private System.Windows.Forms.Label cameraMat01;
        private System.Windows.Forms.Label cameraMat11;
        private System.Windows.Forms.Label cameraMat21;
        private System.Windows.Forms.Label cameraMat02;
        private System.Windows.Forms.Label cameraMat12;
        private System.Windows.Forms.Label cameraMat22;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label coef0;
        private System.Windows.Forms.Label coef1;
        private System.Windows.Forms.Label coef2;
        private System.Windows.Forms.Label coef3;
        private System.Windows.Forms.Label coef4;
        private System.Windows.Forms.Button btnundistortion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblDTV;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbltotalpts;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblsumoffset;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblaveoffset;
        private System.Windows.Forms.Label lblmaxindex;
        private System.Windows.Forms.Label lblminindex;
        private System.Windows.Forms.Label lblmaxoffset;
        private System.Windows.Forms.Label lblminoffset;
        private System.Windows.Forms.Button btnSaveMat;
        private System.Windows.Forms.Button btnLoadMat;
        private System.Windows.Forms.Label lblStatus;
    }
}

