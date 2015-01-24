namespace ImageMapWinForms
{
    partial class ImageMapPropertyForm
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
            if(disposing && (components != null))
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
            this.components = new System.ComponentModel.Container();
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle imageAreaRectangle1 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle();
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaCircle imageAreaCircle1 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaCircle();
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaPolygon imageAreaPolygon1 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaPolygon();
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle imageAreaRectangle2 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle();
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse imageAreaEllipse1 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageMapPropertyForm));
            this.imMap = new EWSoftware.ImageMaps.Windows.Forms.ImageMap();
            this.pgImageMapProps = new System.Windows.Forms.PropertyGrid();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.ttTips = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // imMap
            // 
            this.imMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            imageAreaRectangle1.AccessKey = "R";
            imageAreaRectangle1.Rectangle = new System.Drawing.Rectangle(16, 16, 150, 94);
            imageAreaRectangle1.TabIndex = 3;
            imageAreaRectangle1.ToolTip = "A rectangle area";
            imageAreaCircle1.AccessKey = "C";
            imageAreaCircle1.CenterPoint = new System.Drawing.Point(444, 88);
            imageAreaCircle1.Radius = 60;
            imageAreaCircle1.TabIndex = 1;
            imageAreaCircle1.ToolTip = "A circle area";
            imageAreaPolygon1.AccessKey = "P";
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(42, 186));
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(110, 286));
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(144, 250));
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(138, 230));
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(160, 216));
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(190, 214));
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(152, 168));
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(112, 172));
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(70, 154));
            imageAreaPolygon1.TabIndex = 2;
            imageAreaPolygon1.ToolTip = "A polygon area";
            imageAreaRectangle2.Action = EWSoftware.ImageMaps.Windows.Forms.AreaClickAction.None;
            imageAreaRectangle2.Rectangle = new System.Drawing.Rectangle(316, 176, 206, 118);
            imageAreaRectangle2.ToolTip = "A tool tip only area";
            imageAreaEllipse1.AccessKey = "E";
            imageAreaEllipse1.Ellipse = new System.Drawing.Rectangle(199, 88, 128, 58);
            imageAreaEllipse1.TabIndex = 4;
            imageAreaEllipse1.ToolTip = "An ellipse area";
            this.imMap.Areas.Add(imageAreaRectangle1);
            this.imMap.Areas.Add(imageAreaCircle1);
            this.imMap.Areas.Add(imageAreaPolygon1);
            this.imMap.Areas.Add(imageAreaRectangle2);
            this.imMap.Areas.Add(imageAreaEllipse1);
            this.imMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imMap.Image = ((System.Drawing.Image)(resources.GetObject("imMap.Image")));
            this.imMap.Location = new System.Drawing.Point(12, 12);
            this.imMap.Name = "imMap";
            this.imMap.Size = new System.Drawing.Size(574, 451);
            this.imMap.TabIndex = 0;
            this.imMap.ToolTip = "A test image";
            this.imMap.Click += new System.EventHandler<EWSoftware.ImageMaps.ImageMapClickEventArgs>(this.imMap_Click);
            // 
            // pgImageMapProps
            // 
            this.pgImageMapProps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgImageMapProps.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.pgImageMapProps.Location = new System.Drawing.Point(592, 12);
            this.pgImageMapProps.Name = "pgImageMapProps";
            this.pgImageMapProps.Size = new System.Drawing.Size(288, 451);
            this.pgImageMapProps.TabIndex = 1;
            this.pgImageMapProps.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgImageMapProps_PropertyValueChanged);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(12, 473);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 32);
            this.btnHelp.TabIndex = 2;
            this.btnHelp.Text = "&Help";
            this.ttTips.SetToolTip(this.btnHelp, "Open ImageMap help file");
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(805, 473);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 32);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "E&xit";
            this.ttTips.SetToolTip(this.btnExit, "Exit the demo");
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(139, 466);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(614, 46);
            this.label1.TabIndex = 4;
            this.label1.Text = "Use the property grid on the right to modify the image map properties and to acce" +
    "ss the image area collection editor and coordinate editor.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ImageMapPropertyForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(892, 517);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.pgImageMapProps);
            this.Controls.Add(this.imMap);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "ImageMapPropertyForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Windows Forms Image Map Demo";
            this.ResumeLayout(false);

        }

        #endregion

        private EWSoftware.ImageMaps.Windows.Forms.ImageMap imMap;
        private System.Windows.Forms.PropertyGrid pgImageMapProps;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ToolTip ttTips;
        private System.Windows.Forms.Label label1;
    }
}
