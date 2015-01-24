namespace EWSoftware.ImageMaps.Design
{
    partial class ImageMapAreaEditorDlg
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
			if(disposing)
			{
                this.Animate(false);

                // We only dispose of the image if we loaded it
                if(imageName != null)
                    image.Dispose();

				if(components != null)
					components.Dispose();
			}

			base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageMapAreaEditorDlg));
            this.pnlImage = new EWSoftware.ImageMaps.Design.ImageMapAreaEditorDlg.BufferedPanel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblImageInfo = new System.Windows.Forms.Label();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblCoords = new System.Windows.Forms.Label();
            this.btnLeftUp = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnRightUp = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRightDown = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnLeftDown = new System.Windows.Forms.Button();
            this.btnIncRightBottom = new System.Windows.Forms.Button();
            this.btnIncBottom = new System.Windows.Forms.Button();
            this.btnIncLeftBottom = new System.Windows.Forms.Button();
            this.btnIncRight = new System.Windows.Forms.Button();
            this.btnIncAll = new System.Windows.Forms.Button();
            this.btnIncLeft = new System.Windows.Forms.Button();
            this.btnIncRightTop = new System.Windows.Forms.Button();
            this.btnIncTop = new System.Windows.Forms.Button();
            this.btnIncLeftTop = new System.Windows.Forms.Button();
            this.btnDecRightBottom = new System.Windows.Forms.Button();
            this.btnDecBottom = new System.Windows.Forms.Button();
            this.btnDecLeftBottom = new System.Windows.Forms.Button();
            this.btnDecRight = new System.Windows.Forms.Button();
            this.btnDecAll = new System.Windows.Forms.Button();
            this.btnDecLeft = new System.Windows.Forms.Button();
            this.btnDecRightTop = new System.Windows.Forms.Button();
            this.btnDecTop = new System.Windows.Forms.Button();
            this.btnDecLeftTop = new System.Windows.Forms.Button();
            this.chkShowAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // pnlImage
            // 
            this.pnlImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlImage.AutoScroll = true;
            this.pnlImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlImage.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pnlImage.Location = new System.Drawing.Point(12, 35);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(564, 424);
            this.pnlImage.TabIndex = 2;
            this.pnlImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlImage_Paint);
            this.pnlImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlImage_MouseDown);
            this.pnlImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlImage_MouseMove);
            this.pnlImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlImage_MouseUp);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(582, 389);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 32);
            this.btnOK.TabIndex = 32;
            this.btnOK.Text = "OK";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(582, 427);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 32);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Text = "Cancel";
            // 
            // lblImageInfo
            // 
            this.lblImageInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblImageInfo.AutoEllipsis = true;
            this.lblImageInfo.Location = new System.Drawing.Point(14, 9);
            this.lblImageInfo.Name = "lblImageInfo";
            this.lblImageInfo.Size = new System.Drawing.Size(527, 23);
            this.lblImageInfo.TabIndex = 0;
            this.lblImageInfo.Text = "Image: XXX Height: XXX Width: XXX";
            this.lblImageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInstructions
            // 
            this.lblInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInstructions.AutoEllipsis = true;
            this.lblInstructions.Location = new System.Drawing.Point(12, 462);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(658, 24);
            this.lblInstructions.TabIndex = 3;
            this.lblInstructions.Text = "Drawing instructions";
            this.lblInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(582, 336);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 32);
            this.btnClear.TabIndex = 31;
            this.btnClear.Text = "Cl&ear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblCoords
            // 
            this.lblCoords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCoords.Location = new System.Drawing.Point(547, 9);
            this.lblCoords.Name = "lblCoords";
            this.lblCoords.Size = new System.Drawing.Size(123, 23);
            this.lblCoords.TabIndex = 1;
            this.lblCoords.Text = "X: 0  Y: 0";
            this.lblCoords.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLeftUp
            // 
            this.btnLeftUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLeftUp.Image = ((System.Drawing.Image)(resources.GetObject("btnLeftUp.Image")));
            this.btnLeftUp.Location = new System.Drawing.Point(595, 35);
            this.btnLeftUp.Name = "btnLeftUp";
            this.btnLeftUp.Size = new System.Drawing.Size(24, 24);
            this.btnLeftUp.TabIndex = 4;
            this.btnLeftUp.Click += new System.EventHandler(this.btnMove_Click);
            this.btnLeftUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnLeftUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(619, 35);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(24, 24);
            this.btnUp.TabIndex = 5;
            this.btnUp.Click += new System.EventHandler(this.btnMove_Click);
            this.btnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnRightUp
            // 
            this.btnRightUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRightUp.Image = ((System.Drawing.Image)(resources.GetObject("btnRightUp.Image")));
            this.btnRightUp.Location = new System.Drawing.Point(643, 35);
            this.btnRightUp.Name = "btnRightUp";
            this.btnRightUp.Size = new System.Drawing.Size(24, 24);
            this.btnRightUp.TabIndex = 6;
            this.btnRightUp.Click += new System.EventHandler(this.btnMove_Click);
            this.btnRightUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnRightUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnRight
            // 
            this.btnRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRight.Image")));
            this.btnRight.Location = new System.Drawing.Point(643, 59);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(24, 24);
            this.btnRight.TabIndex = 8;
            this.btnRight.Click += new System.EventHandler(this.btnMove_Click);
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnLeft
            // 
            this.btnLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLeft.Image")));
            this.btnLeft.Location = new System.Drawing.Point(595, 59);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(24, 24);
            this.btnLeft.TabIndex = 7;
            this.btnLeft.Click += new System.EventHandler(this.btnMove_Click);
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnRightDown
            // 
            this.btnRightDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRightDown.Image = ((System.Drawing.Image)(resources.GetObject("btnRightDown.Image")));
            this.btnRightDown.Location = new System.Drawing.Point(643, 83);
            this.btnRightDown.Name = "btnRightDown";
            this.btnRightDown.Size = new System.Drawing.Size(24, 24);
            this.btnRightDown.TabIndex = 11;
            this.btnRightDown.Click += new System.EventHandler(this.btnMove_Click);
            this.btnRightDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnRightDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.Location = new System.Drawing.Point(619, 83);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(24, 24);
            this.btnDown.TabIndex = 10;
            this.btnDown.Click += new System.EventHandler(this.btnMove_Click);
            this.btnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnLeftDown
            // 
            this.btnLeftDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLeftDown.Image = ((System.Drawing.Image)(resources.GetObject("btnLeftDown.Image")));
            this.btnLeftDown.Location = new System.Drawing.Point(595, 83);
            this.btnLeftDown.Name = "btnLeftDown";
            this.btnLeftDown.Size = new System.Drawing.Size(24, 24);
            this.btnLeftDown.TabIndex = 9;
            this.btnLeftDown.Click += new System.EventHandler(this.btnMove_Click);
            this.btnLeftDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnLeftDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnIncRightBottom
            // 
            this.btnIncRightBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncRightBottom.Image = ((System.Drawing.Image)(resources.GetObject("btnIncRightBottom.Image")));
            this.btnIncRightBottom.Location = new System.Drawing.Point(643, 163);
            this.btnIncRightBottom.Name = "btnIncRightBottom";
            this.btnIncRightBottom.Size = new System.Drawing.Size(24, 24);
            this.btnIncRightBottom.TabIndex = 20;
            this.btnIncRightBottom.Click += new System.EventHandler(this.btnIncrease_Click);
            this.btnIncRightBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnIncRightBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnIncBottom
            // 
            this.btnIncBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncBottom.Image = ((System.Drawing.Image)(resources.GetObject("btnIncBottom.Image")));
            this.btnIncBottom.Location = new System.Drawing.Point(619, 163);
            this.btnIncBottom.Name = "btnIncBottom";
            this.btnIncBottom.Size = new System.Drawing.Size(24, 24);
            this.btnIncBottom.TabIndex = 19;
            this.btnIncBottom.Click += new System.EventHandler(this.btnIncrease_Click);
            this.btnIncBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnIncBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnIncLeftBottom
            // 
            this.btnIncLeftBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncLeftBottom.Image = ((System.Drawing.Image)(resources.GetObject("btnIncLeftBottom.Image")));
            this.btnIncLeftBottom.Location = new System.Drawing.Point(595, 163);
            this.btnIncLeftBottom.Name = "btnIncLeftBottom";
            this.btnIncLeftBottom.Size = new System.Drawing.Size(24, 24);
            this.btnIncLeftBottom.TabIndex = 18;
            this.btnIncLeftBottom.Click += new System.EventHandler(this.btnIncrease_Click);
            this.btnIncLeftBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnIncLeftBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnIncRight
            // 
            this.btnIncRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncRight.Image = ((System.Drawing.Image)(resources.GetObject("btnIncRight.Image")));
            this.btnIncRight.Location = new System.Drawing.Point(643, 139);
            this.btnIncRight.Name = "btnIncRight";
            this.btnIncRight.Size = new System.Drawing.Size(24, 24);
            this.btnIncRight.TabIndex = 17;
            this.btnIncRight.Click += new System.EventHandler(this.btnIncrease_Click);
            this.btnIncRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnIncRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnIncAll
            // 
            this.btnIncAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncAll.Image = ((System.Drawing.Image)(resources.GetObject("btnIncAll.Image")));
            this.btnIncAll.Location = new System.Drawing.Point(619, 139);
            this.btnIncAll.Name = "btnIncAll";
            this.btnIncAll.Size = new System.Drawing.Size(24, 24);
            this.btnIncAll.TabIndex = 16;
            this.btnIncAll.Click += new System.EventHandler(this.btnIncrease_Click);
            this.btnIncAll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnIncAll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnIncLeft
            // 
            this.btnIncLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnIncLeft.Image")));
            this.btnIncLeft.Location = new System.Drawing.Point(595, 139);
            this.btnIncLeft.Name = "btnIncLeft";
            this.btnIncLeft.Size = new System.Drawing.Size(24, 24);
            this.btnIncLeft.TabIndex = 15;
            this.btnIncLeft.Click += new System.EventHandler(this.btnIncrease_Click);
            this.btnIncLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnIncLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnIncRightTop
            // 
            this.btnIncRightTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncRightTop.Image = ((System.Drawing.Image)(resources.GetObject("btnIncRightTop.Image")));
            this.btnIncRightTop.Location = new System.Drawing.Point(643, 115);
            this.btnIncRightTop.Name = "btnIncRightTop";
            this.btnIncRightTop.Size = new System.Drawing.Size(24, 24);
            this.btnIncRightTop.TabIndex = 14;
            this.btnIncRightTop.Click += new System.EventHandler(this.btnIncrease_Click);
            this.btnIncRightTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnIncRightTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnIncTop
            // 
            this.btnIncTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncTop.Image = ((System.Drawing.Image)(resources.GetObject("btnIncTop.Image")));
            this.btnIncTop.Location = new System.Drawing.Point(619, 115);
            this.btnIncTop.Name = "btnIncTop";
            this.btnIncTop.Size = new System.Drawing.Size(24, 24);
            this.btnIncTop.TabIndex = 13;
            this.btnIncTop.Click += new System.EventHandler(this.btnIncrease_Click);
            this.btnIncTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnIncTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnIncLeftTop
            // 
            this.btnIncLeftTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncLeftTop.Image = ((System.Drawing.Image)(resources.GetObject("btnIncLeftTop.Image")));
            this.btnIncLeftTop.Location = new System.Drawing.Point(595, 115);
            this.btnIncLeftTop.Name = "btnIncLeftTop";
            this.btnIncLeftTop.Size = new System.Drawing.Size(24, 24);
            this.btnIncLeftTop.TabIndex = 12;
            this.btnIncLeftTop.Click += new System.EventHandler(this.btnIncrease_Click);
            this.btnIncLeftTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnIncLeftTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnDecRightBottom
            // 
            this.btnDecRightBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecRightBottom.Image = ((System.Drawing.Image)(resources.GetObject("btnDecRightBottom.Image")));
            this.btnDecRightBottom.Location = new System.Drawing.Point(643, 243);
            this.btnDecRightBottom.Name = "btnDecRightBottom";
            this.btnDecRightBottom.Size = new System.Drawing.Size(24, 24);
            this.btnDecRightBottom.TabIndex = 29;
            this.btnDecRightBottom.Click += new System.EventHandler(this.btnShrink_Click);
            this.btnDecRightBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnDecRightBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnDecBottom
            // 
            this.btnDecBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecBottom.Image = ((System.Drawing.Image)(resources.GetObject("btnDecBottom.Image")));
            this.btnDecBottom.Location = new System.Drawing.Point(619, 243);
            this.btnDecBottom.Name = "btnDecBottom";
            this.btnDecBottom.Size = new System.Drawing.Size(24, 24);
            this.btnDecBottom.TabIndex = 28;
            this.btnDecBottom.Click += new System.EventHandler(this.btnShrink_Click);
            this.btnDecBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnDecBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnDecLeftBottom
            // 
            this.btnDecLeftBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecLeftBottom.Image = ((System.Drawing.Image)(resources.GetObject("btnDecLeftBottom.Image")));
            this.btnDecLeftBottom.Location = new System.Drawing.Point(595, 243);
            this.btnDecLeftBottom.Name = "btnDecLeftBottom";
            this.btnDecLeftBottom.Size = new System.Drawing.Size(24, 24);
            this.btnDecLeftBottom.TabIndex = 27;
            this.btnDecLeftBottom.Click += new System.EventHandler(this.btnShrink_Click);
            this.btnDecLeftBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnDecLeftBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnDecRight
            // 
            this.btnDecRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecRight.Image = ((System.Drawing.Image)(resources.GetObject("btnDecRight.Image")));
            this.btnDecRight.Location = new System.Drawing.Point(643, 219);
            this.btnDecRight.Name = "btnDecRight";
            this.btnDecRight.Size = new System.Drawing.Size(24, 24);
            this.btnDecRight.TabIndex = 26;
            this.btnDecRight.Click += new System.EventHandler(this.btnShrink_Click);
            this.btnDecRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnDecRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnDecAll
            // 
            this.btnDecAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecAll.Image = ((System.Drawing.Image)(resources.GetObject("btnDecAll.Image")));
            this.btnDecAll.Location = new System.Drawing.Point(619, 219);
            this.btnDecAll.Name = "btnDecAll";
            this.btnDecAll.Size = new System.Drawing.Size(24, 24);
            this.btnDecAll.TabIndex = 25;
            this.btnDecAll.Click += new System.EventHandler(this.btnShrink_Click);
            this.btnDecAll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnDecAll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnDecLeft
            // 
            this.btnDecLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnDecLeft.Image")));
            this.btnDecLeft.Location = new System.Drawing.Point(595, 219);
            this.btnDecLeft.Name = "btnDecLeft";
            this.btnDecLeft.Size = new System.Drawing.Size(24, 24);
            this.btnDecLeft.TabIndex = 24;
            this.btnDecLeft.Click += new System.EventHandler(this.btnShrink_Click);
            this.btnDecLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnDecLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnDecRightTop
            // 
            this.btnDecRightTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecRightTop.Image = ((System.Drawing.Image)(resources.GetObject("btnDecRightTop.Image")));
            this.btnDecRightTop.Location = new System.Drawing.Point(643, 195);
            this.btnDecRightTop.Name = "btnDecRightTop";
            this.btnDecRightTop.Size = new System.Drawing.Size(24, 24);
            this.btnDecRightTop.TabIndex = 23;
            this.btnDecRightTop.Click += new System.EventHandler(this.btnShrink_Click);
            this.btnDecRightTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnDecRightTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnDecTop
            // 
            this.btnDecTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecTop.Image = ((System.Drawing.Image)(resources.GetObject("btnDecTop.Image")));
            this.btnDecTop.Location = new System.Drawing.Point(619, 195);
            this.btnDecTop.Name = "btnDecTop";
            this.btnDecTop.Size = new System.Drawing.Size(24, 24);
            this.btnDecTop.TabIndex = 22;
            this.btnDecTop.Click += new System.EventHandler(this.btnShrink_Click);
            this.btnDecTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnDecTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // btnDecLeftTop
            // 
            this.btnDecLeftTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecLeftTop.Image = ((System.Drawing.Image)(resources.GetObject("btnDecLeftTop.Image")));
            this.btnDecLeftTop.Location = new System.Drawing.Point(595, 195);
            this.btnDecLeftTop.Name = "btnDecLeftTop";
            this.btnDecLeftTop.Size = new System.Drawing.Size(24, 24);
            this.btnDecLeftTop.TabIndex = 21;
            this.btnDecLeftTop.Click += new System.EventHandler(this.btnShrink_Click);
            this.btnDecLeftTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStart);
            this.btnDecLeftTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdjustStop);
            // 
            // chkShowAll
            // 
            this.chkShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowAll.Location = new System.Drawing.Point(582, 292);
            this.chkShowAll.Name = "chkShowAll";
            this.chkShowAll.Size = new System.Drawing.Size(88, 24);
            this.chkShowAll.TabIndex = 30;
            this.chkShowAll.Text = "&Show All";
            this.chkShowAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkShowAll.CheckedChanged += new System.EventHandler(this.chkShowAll_CheckedChanged);
            // 
            // ImageMapAreaEditorDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(682, 495);
            this.Controls.Add(this.chkShowAll);
            this.Controls.Add(this.btnDecRightBottom);
            this.Controls.Add(this.btnDecBottom);
            this.Controls.Add(this.btnDecLeftBottom);
            this.Controls.Add(this.btnDecRight);
            this.Controls.Add(this.btnDecAll);
            this.Controls.Add(this.btnDecLeft);
            this.Controls.Add(this.btnDecRightTop);
            this.Controls.Add(this.btnDecTop);
            this.Controls.Add(this.btnDecLeftTop);
            this.Controls.Add(this.btnIncRightBottom);
            this.Controls.Add(this.btnIncBottom);
            this.Controls.Add(this.btnIncLeftBottom);
            this.Controls.Add(this.btnIncRight);
            this.Controls.Add(this.btnIncAll);
            this.Controls.Add(this.btnIncLeft);
            this.Controls.Add(this.btnIncRightTop);
            this.Controls.Add(this.btnIncTop);
            this.Controls.Add(this.btnIncLeftTop);
            this.Controls.Add(this.btnRightDown);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnLeftDown);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnRightUp);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnLeftUp);
            this.Controls.Add(this.lblCoords);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.lblImageInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(660, 520);
            this.Name = "ImageMapAreaEditorDlg";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Image Map Area Editor";
            this.Load += new System.EventHandler(this.ImageMapAreaEditorDlg_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private EWSoftware.ImageMaps.Design.ImageMapAreaEditorDlg.BufferedPanel pnlImage;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblImageInfo;
        private System.Windows.Forms.Label lblCoords;
        private System.Windows.Forms.Button btnLeftUp;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnRightUp;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRightDown;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnLeftDown;
        private System.Windows.Forms.Button btnIncRightBottom;
        private System.Windows.Forms.Button btnIncBottom;
        private System.Windows.Forms.Button btnIncLeftBottom;
        private System.Windows.Forms.Button btnIncRight;
        private System.Windows.Forms.Button btnIncAll;
        private System.Windows.Forms.Button btnIncLeft;
        private System.Windows.Forms.Button btnIncRightTop;
        private System.Windows.Forms.Button btnIncTop;
        private System.Windows.Forms.Button btnIncLeftTop;
        private System.Windows.Forms.Button btnDecRightBottom;
        private System.Windows.Forms.Button btnDecBottom;
        private System.Windows.Forms.Button btnDecLeftBottom;
        private System.Windows.Forms.Button btnDecRight;
        private System.Windows.Forms.Button btnDecAll;
        private System.Windows.Forms.Button btnDecLeft;
        private System.Windows.Forms.Button btnDecTop;
        private System.Windows.Forms.Button btnDecLeftTop;
        private System.Windows.Forms.CheckBox chkShowAll;
        private System.Windows.Forms.Button btnDecRightTop;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Button btnClear;
    }
}
