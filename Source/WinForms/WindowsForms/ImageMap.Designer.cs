namespace EWSoftware.ImageMaps.Windows.Forms
{
	partial class ImageMap
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

                // Dispose of our stuff too
                if(gPanel != null)
                    gPanel.Dispose();

                if(pathData != null)
                {
                    pathData.Dispose();
                    pathData = null;
                }

                // NOTE: Do not dispose of the old image.  If it's a project resource, it will cause a crash
                // later if re-used as the project won't expect it to have been disposed.

				if(components != null)
					components.Dispose();
			}

			base.Dispose(disposing);
        }

		#region Component Designer generated code

		// <summary>
		// Required method for Designer support - do not modify
		// the contents of this method with the code editor.
		// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // ImageMap
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.Name = "ImageMap";
            this.Size = new System.Drawing.Size(200, 112);
            this.ResumeLayout(false);

        }
		#endregion

        private System.Windows.Forms.ToolTip toolTip;
    }
}
