namespace ImageMapWinForms
{
    partial class ImageMapEventsForm
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
                if(buttonFont != null)
                    buttonFont.Dispose();

                if(hyperlinkFont != null)
                    hyperlinkFont.Dispose();

                if(sfFormat != null)
                    sfFormat.Dispose();

                if(imgFiller != null)
                    imgFiller.Dispose();

                if(iaNormal != null)
                    iaNormal.Dispose();

                if(iaDisabled != null)
                    iaDisabled.Dispose();

                if(components != null)
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
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle imageAreaRectangle1 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle();
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaCircle imageAreaCircle1 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaCircle();
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaPolygon imageAreaPolygon1 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaPolygon();
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle imageAreaRectangle2 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle();
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse imageAreaEllipse1 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse();
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse imageAreaEllipse2 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse();
            EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse imageAreaEllipse3 = new EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageMapEventsForm));
            this.imMap = new EWSoftware.ImageMaps.Windows.Forms.ImageMap();
            this.SuspendLayout();
            // 
            // imMap
            // 
            imageAreaRectangle1.AccessKey = "I";
            imageAreaRectangle1.Rectangle = new System.Drawing.Rectangle(14, 47, 208, 21);
            imageAreaRectangle1.TabIndex = 1;
            imageAreaRectangle1.ToolTip = "Turn image map owner draw on/off";
            imageAreaCircle1.CenterPoint = new System.Drawing.Point(107, 167);
            imageAreaCircle1.OwnerDraw = true;
            imageAreaCircle1.Radius = 71;
            imageAreaCircle1.TabIndex = 2;
            imageAreaCircle1.Tag = "Mouse event demo";
            imageAreaCircle1.ToolTip = "Demonstrate mouse events";
            imageAreaPolygon1.OwnerDraw = true;
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(310, 114));
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(210, 214));
            imageAreaPolygon1.Points.Add(new System.Drawing.Point(410, 214));
            imageAreaPolygon1.TabIndex = 3;
            imageAreaPolygon1.Tag = "Focus demo";
            imageAreaPolygon1.ToolTip = "Demonstrate focus enter/leave";
            imageAreaRectangle2.AccessKey = "D";
            imageAreaRectangle2.Rectangle = new System.Drawing.Rectangle(130, 272, 312, 21);
            imageAreaRectangle2.TabIndex = 4;
            imageAreaRectangle2.ToolTip = "Double-click to visit the web site";
            imageAreaEllipse1.Ellipse = new System.Drawing.Rectangle(453, 161, 131, 30);
            imageAreaEllipse1.Enabled = false;
            imageAreaEllipse1.OwnerDraw = true;
            imageAreaEllipse1.TabIndex = 5;
            imageAreaEllipse1.Tag = "Disabled";
            imageAreaEllipse1.ToolTip = "Disabled owner drawn area";
            imageAreaEllipse2.AccessKey = "P";
            imageAreaEllipse2.Ellipse = new System.Drawing.Rectangle(453, 208, 131, 30);
            imageAreaEllipse2.OwnerDraw = true;
            imageAreaEllipse2.TabIndex = 6;
            imageAreaEllipse2.Tag = "&Properties";
            imageAreaEllipse2.ToolTip = "Interactive property demo";
            imageAreaEllipse3.AccessKey = "X";
            imageAreaEllipse3.Ellipse = new System.Drawing.Rectangle(453, 252, 131, 30);
            imageAreaEllipse3.OwnerDraw = true;
            imageAreaEllipse3.TabIndex = 7;
            imageAreaEllipse3.Tag = "E&xit";
            imageAreaEllipse3.ToolTip = "Exit this demo";
            this.imMap.Areas.Add(imageAreaRectangle1);
            this.imMap.Areas.Add(imageAreaCircle1);
            this.imMap.Areas.Add(imageAreaPolygon1);
            this.imMap.Areas.Add(imageAreaRectangle2);
            this.imMap.Areas.Add(imageAreaEllipse1);
            this.imMap.Areas.Add(imageAreaEllipse2);
            this.imMap.Areas.Add(imageAreaEllipse3);
            this.imMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imMap.ImageMapHeight = 300;
            this.imMap.ImageMapWidth = 600;
            this.imMap.Location = new System.Drawing.Point(0, 0);
            this.imMap.Name = "imMap";
            this.imMap.OwnerDraw = true;
            this.imMap.Size = new System.Drawing.Size(600, 300);
            this.imMap.TabIndex = 0;
            this.imMap.ToolTip = null;
            this.imMap.Click += new System.EventHandler<EWSoftware.ImageMaps.ImageMapClickEventArgs>(this.imMap_Click);
            this.imMap.DrawImage += new System.EventHandler<EWSoftware.ImageMaps.Windows.Forms.DrawImageEventArgs>(this.imMap_DrawImage);
            // 
            // ImageMapEventsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.Controls.Add(this.imMap);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "ImageMapEventsForm";
            this.Text = "EWSoftware Image Map Control Demo";
            this.ResumeLayout(false);

        }

        #endregion

        private EWSoftware.ImageMaps.Windows.Forms.ImageMap imMap;
    }
}
