//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageMapEventsForm.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/11/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft C#
//
// This form is used to demonstrate the handling of the various Image Map Windows Forms control events and the
// owner draw capabilities.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 12/27/2004  EFW  Created the code
// 07/08/2006  EFW  Updated for use with .NET 2.0
//===============================================================================================================

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;

using EWSoftware.ImageMaps;
using EWSoftware.ImageMaps.Windows.Forms;

namespace ImageMapWinForms
{
	/// <summary>
    /// This form is used to demonstrate the handling of the various Image Map Windows Forms control events and
    /// the owner draw capabilities.
	/// </summary>
	public partial class ImageMapEventsForm : System.Windows.Forms.Form
	{
        #region Main program entry point
        //=====================================================================

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ImageMapEventsForm());
        }
        #endregion

        #region Private data members
        //=====================================================================

        private Font buttonFont, hyperlinkFont;
        private ImageAreaRectangle areaOwnerDrawOnOff, areaVisitWebSite;
        private StringFormat sfFormat;

		private Image imgFiller;

		private ImageAttributes iaNormal, iaDisabled;
		private ColorMatrix cmNormal, cmDisabled;
        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
		public ImageMapEventsForm()
		{
			InitializeComponent();

            // This is used to format text for various image areas
            sfFormat = new StringFormat();
            sfFormat.Alignment = StringAlignment.Center;
            sfFormat.LineAlignment = StringAlignment.Center;
            sfFormat.HotkeyPrefix = HotkeyPrefix.Show;

            // A couple of fonts for some of the image areas
            buttonFont = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
            hyperlinkFont = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Underline);

            // Get the image for the "buttons" and define the attributes used to given them their various draw
            // state effects.
			imgFiller = new Bitmap(GetType(), "ImageMapEventsForm.Filler.png");

			// Image attributes used to lighten normal buttons
			cmNormal = new ColorMatrix();
			cmNormal.Matrix33 = 0.5f;

			iaNormal = new ImageAttributes();
			iaNormal.SetColorMatrix(cmNormal, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

			// Image attributes that lighten and desaturate disabled buttons
			cmDisabled = new ColorMatrix();
			cmDisabled.Matrix00 =  cmDisabled.Matrix01 =  cmDisabled.Matrix02 =  cmDisabled.Matrix10 = 
			    cmDisabled.Matrix11 = cmDisabled.Matrix12 = cmDisabled.Matrix20 = cmDisabled.Matrix21 = 
			    cmDisabled.Matrix22 = 0.33f;
			cmDisabled.Matrix33 = 0.5f;

			iaDisabled = new ImageAttributes();
			iaDisabled.SetColorMatrix(cmDisabled, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            // Hook up the event handlers.  Since they are not accessible in the designer, we must do it
            // manually.  Hang on to the references as we use them fairly often in here.
            areaOwnerDrawOnOff = (ImageAreaRectangle)imMap.Areas[0];
            areaOwnerDrawOnOff.Click += areaOwnerDrawOnOff_Click;
            areaOwnerDrawOnOff.DrawImage += OwnerDrawOnOff_DrawImage;

            areaVisitWebSite = (ImageAreaRectangle)imMap.Areas[3];
            areaVisitWebSite.DrawImage += VisitWebSite_DrawImage;
            areaVisitWebSite.DoubleClick += VisitWebSite_DoubleClick;

            // The same draw event handler is used for the three "buttons".  The image map Click event handles
            // the area click events.
            ImageAreaBase a = (ImageAreaBase)imMap.Areas[4];
            a.DrawImage += Button_DrawImage;

            a = (ImageAreaBase)imMap.Areas[5];
            a.DrawImage += Button_DrawImage;

            a = (ImageAreaBase)imMap.Areas[6];
            a.DrawImage += Button_DrawImage;

            // These two just demonstrate mouse and focus event handling by the image areas
            a = (ImageAreaBase)imMap.Areas[1];
            a.DrawImage += MouseArea_DrawImage;
            a.MouseEnter += MouseArea_MouseEnter;
            a.MouseDown += MouseArea_MouseDown;
            a.MouseUp += MouseArea_MouseUp;
            a.MouseLeave += MouseArea_MouseLeave;

            // These two tend to obscure the other mouse events.
            // You can uncomment them to see them in action.
//            a.MouseMove += MouseArea_MouseMove;
//            a.MouseHover += MouseArea_MouseHover;

            a = (ImageAreaBase)imMap.Areas[2];
            a.DrawImage += FocusArea_DrawImage;
            a.Enter += FocusArea_Enter;
            a.Leave+= FocusArea_Leave;
        }
        #endregion

        #region Event handlers
        //=====================================================================

        /// <summary>
        /// The image map can handle click events or the areas can handle their own events.  In this demo, the
        /// image map handles the click event for areas 1, 2, 5, and 6.  Area 0 has it's own Click handler and
        /// area 3 only responds to double-clicks with its own DoubleClick handler.
        /// </summary>
        /// <param name="sender">The sender of the event (the image map)</param>
        /// <param name="e">The event arguments</param>
        private void imMap_Click(object sender, ImageMapClickEventArgs e)
        {
            // We'll handle the click for image area 0 (toggle the image map's owner status)
            switch(imMap.FocusedArea)
            {
                case 5:     // Show the interactive property demo
                    using(ImageMapPropertyForm dlg = new ImageMapPropertyForm())
                    {
                        dlg.ShowDialog();
                    }
                    break;

                case 6:     // Exit the application
                    this.Close();
                    break;

                default:
                    if(imMap.FocusedArea != 0 && imMap.FocusedArea != 3)
                        MessageBox.Show(String.Format(CultureInfo.CurrentUICulture, "You clicked area " +
                            "#{0} ({1}) at point {2}, {3}", e.AreaIndex + 1, imMap.Areas[e.AreaIndex].ToolTip,
                            e.XCoordinate, e.YCoordinate), "Image Map Clicked", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    break;
            }
        }

        // This is used in the help file
        #region Owner drawn image map help example

        /// <summary>
        /// When the image map is owner drawn, you can draw just the background
        /// or you can draw the image areas too.
        /// </summary>
        /// <param name="sender">The sender of the event (the image map)</param>
        /// <param name="e">The event arguments</param>
        private void imMap_DrawImage(object sender, DrawImageEventArgs e)
        {
            // For this demo, we'll just draw a gradient fill background.
            // You could get the image using the ImageMap.Image property too.
            // Offset the drawing rectangle by the image offset in the event
            // arguments.  This indicates the true position of the image when
            // centered or scrolled.
            Rectangle r = new Rectangle(e.ImageOffset.X, e.ImageOffset.Y,
                imMap.ImageMapWidth, imMap.ImageMapHeight);

            using(LinearGradientBrush lgBrush = new LinearGradientBrush(r,
              Color.White, Color.SteelBlue, LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(lgBrush, r);
            }

            e.Graphics.DrawString("All items seen on this form are drawn by " +
                "the image map or image area DrawImage event handlers.  " +
                "Even with owner draw disabled for the image map, image " +
                "areas can still be set to owner drawn.", imMap.Font,
                Brushes.Black, new Rectangle(r.X + 250, r.Y + 10, 325, 100),
                sfFormat);

            // The image map can draw areas too.  In this case, we defer to
            // the event handlers as they'll take over if owner drawing of the
            // image map is turned off.
            this.OwnerDrawOnOff_DrawImage(sender, e);
            this.VisitWebSite_DrawImage(sender, e);
        }
        #endregion

        /// <summary>
        /// Draw the "Owner Draw" toggle image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image map or image area in this demo)</param>
        /// <param name="e">The event arguments</param>
        private void OwnerDrawOnOff_DrawImage(object sender, DrawImageEventArgs e)
        {
            Rectangle r = areaOwnerDrawOnOff.Rectangle;
            r.Inflate(-2, -2);

            // Offset the area rectangle by the draw event offset
            r.Offset(e.ImageOffset.X, e.ImageOffset.Y);

            if(sender is ImageMap)
            {
                ControlPaint.DrawCheckBox(e.Graphics, r.X + 5, r.Y, r.Height, r.Height, ButtonState.Checked);

                r.X += 5 + r.Height;
                r.Width -= 5 + r.Height;

                // Use the image map's font
                e.Graphics.DrawString("&Image map owner draw ON", imMap.Font, Brushes.Black, r, sfFormat);
            }
            else
            {
                ControlPaint.DrawCheckBox(e.Graphics, r.X + 5, r.Y, r.Height, r.Height, ButtonState.Normal);

                r.X += 5 + r.Height;
                r.Width -= 5 + r.Height;
                e.Graphics.DrawString("&Image map owner draw OFF", imMap.Font, Brushes.Black, r, sfFormat);

                // Tell the image map to draw the focus rectangle if this is the focused area.  It's false by
                // default if the area is drawing itself.
                e.DrawFocus = true;
            }
        }

        /// <summary>
        /// The click event handler for the owner draw on/off image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void areaOwnerDrawOnOff_Click(object sender, ImageMapClickEventArgs e)
        {
            // Turn owner draw on/off for the image map
            imMap.OwnerDraw = !imMap.OwnerDraw;

            // Disable owner drawing of the two areas when the image map is owner drawn.  It draws them in that
            // case.
            areaOwnerDrawOnOff.OwnerDraw = areaVisitWebSite.OwnerDraw = !imMap.OwnerDraw;
        }

        /// <summary>
        /// Draw the "Visit web site" image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image map or image area in this demo)</param>
        /// <param name="e">The event arguments</param>
        private void VisitWebSite_DrawImage(object sender, DrawImageEventArgs e)
        {
            Font f = imMap.Font;  // Use image map font if not focused

            ImageMap m = (sender as ImageMap);

            if(m != null)
            {
                // If this area is focused or contains the mouse, underline the hyperlink text
                if(m.FocusedArea == 3 || m.MouseArea == 3)
                {
                    f = hyperlinkFont;

                    // Tell the image map not to draw the focus.  It's true by default in the image map's owner
                    // draw event.
                    if(m.FocusedArea == 3)
                        e.DrawFocus = false;
                }
            }
            else
            {
                // If the image area is drawing itself, use the event arguments to determine the draw state.  If
                // hot lighting or focused, use the underlined font.
                if(e.DrawState == DrawState.HotLight || e.DrawState == DrawState.Focus)
                    f = hyperlinkFont;
            }

            Rectangle r = areaVisitWebSite.Rectangle;

            // Offset the area rectangle by the draw event offset
            r.Offset(e.ImageOffset.X, e.ImageOffset.Y);

            e.Graphics.DrawString("https://github.com/EWSoftware/ImageMaps", f, Brushes.Blue, r, sfFormat);

            r.Width = (int)e.Graphics.MeasureString("Double-click to go to ", imMap.Font).Width + 4;
            r.X = r.X - r.Width + 10;
            e.Graphics.DrawString("&Double-click to go to ", imMap.Font, Brushes.Black, r, sfFormat);
        }

        /// <summary>
        /// When the "Visit web site" area is double-clicked, open the web page
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void VisitWebSite_DoubleClick(object sender, ImageMapClickEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/EWSoftware/ImageMaps");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to start web browser for URL", "URL Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                // Log the exception to the debugger for the developer
                System.Diagnostics.Debug.Write(ex.ToString());
            }
        }

        // This is used in the help file
        #region Owner drawn area help example

        /// <summary>
        /// Draw the "button" image areas
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void Button_DrawImage(object sender, DrawImageEventArgs e)
        {
            ImageAttributes ia;
            Graphics g = e.Graphics;

            // All are ellipse image areas
            ImageAreaEllipse a = (ImageAreaEllipse)sender;
            Rectangle r = a.Ellipse;

            // Offset the area rectangle by the draw event offset
            r.Offset(e.ImageOffset.X, e.ImageOffset.Y);

            if(!a.Enabled)
                ia = iaDisabled;
            else
                ia = iaNormal;

            using(TextureBrush tb = new TextureBrush(imgFiller,
              new Rectangle(0, 0, imgFiller.Width, imgFiller.Height), ia))
            {
                tb.WrapMode = WrapMode.Tile;

                // Translate the brush coordinates to account for the offset
                using(Matrix m = new Matrix())
                {
                    m.Translate((float)r.X, (float)r.Y);
                    tb.Transform = m;

                    // If the area is focused or hot lighted, give it a glow effect
                    if(e.DrawState == DrawState.Focus || e.DrawState == DrawState.HotLight)
                    {
                        using(GraphicsPath pth = new GraphicsPath())
                        {
                            pth.AddEllipse(r);

                            using(PathGradientBrush pgb = new PathGradientBrush(pth))
                            {
                                pgb.CenterColor = Color.LightSteelBlue;

                                if(e.DrawState == DrawState.Focus)
                                    pgb.SurroundColors = new Color[] { Color.Yellow };
                                else
                                    pgb.SurroundColors = new Color[] { Color.Blue };

                                pgb.FocusScales = new PointF(0.8f, 0.8f);

                                g.FillEllipse(pgb, r);
                            }
                        }
                    }

                    // Draw the filler
                    g.FillEllipse(tb, r);
                    g.DrawString((string)a.Tag, buttonFont, Brushes.Black, r, sfFormat);
                }
            }
        }
        #endregion

        /// <summary>
        /// Draw the mouse events image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void MouseArea_DrawImage(object sender, DrawImageEventArgs e)
        {
            Graphics g = e.Graphics;
            ImageAreaCircle a = (ImageAreaCircle)sender;
            int nShineOffset = a.Radius / 2;

            Rectangle r = new Rectangle(a.CenterPoint.X - a.Radius, a.CenterPoint.Y - a.Radius, a.Radius * 2,
                a.Radius * 2);

            r.Inflate(-2, -2);

            // Offset the area rectangle by the draw event offset
            r.Offset(e.ImageOffset.X, e.ImageOffset.Y);

            using(GraphicsPath pth = new GraphicsPath())
            {
                pth.AddEllipse(r);

                using(PathGradientBrush pgb = new PathGradientBrush(pth))
                {
                    pgb.CenterColor = Color.White;
                    pgb.SurroundColors = new Color [] { Color.BurlyWood };
                    Point shine = new Point(a.CenterPoint.X - nShineOffset, a.CenterPoint.Y - nShineOffset);
                    shine.Offset(e.ImageOffset.X, e.ImageOffset.Y);
                    pgb.CenterPoint = shine;

                    g.FillEllipse(pgb, r);
                    g.DrawString((string)a.Tag, buttonFont, Brushes.Black, r, sfFormat);
                }
            }

            // We'll let the image map draw the focus frame when needed
            e.DrawFocus = true;
        }

        /// <summary>
        /// Handle the mouse enter event for the mouse image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void MouseArea_MouseEnter(object sender, EventArgs e)
        {
            ImageAreaCircle a = (ImageAreaCircle)sender;
            a.Tag = "Mouse Entered Area";
        }

        /// <summary>
        /// Handle the mouse move event for the mouse image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void MouseArea_MouseMove(object sender, MouseEventArgs e)
        {
            ImageAreaCircle a = (ImageAreaCircle)sender;
            a.Tag = String.Format(CultureInfo.CurrentUICulture, "Mouse Move ({0},{1})", e.X, e.Y);
            imMap.Invalidate();
            imMap.Update();
        }

        /// <summary>
        /// Handle the mouse hover event for the mouse image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void MouseArea_MouseHover(object sender, EventArgs e)
        {
            ImageAreaCircle a = (ImageAreaCircle)sender;
            a.Tag = "Mouse Hovering";
            imMap.Invalidate();
            imMap.Update();
        }

        /// <summary>
        /// Handle the mouse down event for the mouse image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void MouseArea_MouseDown(object sender, MouseEventArgs e)
        {
            ImageAreaCircle a = (ImageAreaCircle)sender;
            a.Tag = "Mouse Button Down";
            imMap.Invalidate();
            imMap.Update();
        }

        /// <summary>
        /// Handle the mouse up event for the mouse image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void MouseArea_MouseUp(object sender, MouseEventArgs e)
        {
            ImageAreaCircle a = (ImageAreaCircle)sender;
            a.Tag = "Mouse Button Up";
            imMap.Invalidate();
            imMap.Update();
        }

        /// <summary>
        /// Handle the mouse leave event for the mouse image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void MouseArea_MouseLeave(object sender, EventArgs e)
        {
            ImageAreaCircle a = (ImageAreaCircle)sender;
            a.Tag = "Mouse Left Area";
        }

        /// <summary>
        /// Draw the focus enter/leave image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void FocusArea_DrawImage(object sender, DrawImageEventArgs e)
        {
            Graphics g = e.Graphics;
            ImageAreaPolygon a = (ImageAreaPolygon)sender;
            Rectangle r = a.BoundingRectangle;

            // Offset the area rectangle by the draw event offset
            r.Offset(e.ImageOffset.X, e.ImageOffset.Y);

            Point[] pts = new Point[a.Points.Count];
            a.Points.CopyTo(pts, 0);

            using(PathGradientBrush pgb = new PathGradientBrush(pts))
            {
                // Translate the brush coordinates to account for the offset
                using(Matrix m = new Matrix())
                {
                    m.Translate((float)e.ImageOffset.X, (float)e.ImageOffset.Y);
                    pgb.Transform = m;

                    if(e.DrawState == DrawState.Focus)
                        pgb.SurroundColors = new Color[] { Color.DarkOrange };
                    else
                        pgb.SurroundColors = new Color[] { Color.Navy };

                    pgb.CenterColor = Color.Gray;
                    g.FillRectangle(pgb, r);

                    r.Y += 20;
                    g.DrawString((string)a.Tag, buttonFont, Brushes.Yellow, r, sfFormat);
                }
            }
        }

        /// <summary>
        /// Handle the enter event for the focus image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void FocusArea_Enter(object sender, EventArgs e)
        {
            ImageAreaPolygon a = (ImageAreaPolygon)sender;
            a.Tag = "Area Got Focus";
        }

        /// <summary>
        /// Handle the leave event for the focus image area
        /// </summary>
        /// <param name="sender">The sender of the event (the image area)</param>
        /// <param name="e">The event arguments</param>
        private void FocusArea_Leave(object sender, EventArgs e)
        {
            ImageAreaPolygon a = (ImageAreaPolygon)sender;
            a.Tag = "Area Lost Focus";
        }
        #endregion
    }
}
