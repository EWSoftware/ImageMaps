//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageMapAreaEditorDlg.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/03/2023
// Note    : Copyright 2004-2023, Eric Woodruff, All rights reserved
//
// This file contains the form class used to edit image map areas in a more user-friendly format
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/04/2004  EFW  Created the code
// 12/21/2004  EFW  Added position and size adjustment controls and support for the ellipse image area
// 06/28/2006  EFW  Reworked code for use with .NET 2.0
//===============================================================================================================

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace EWSoftware.ImageMaps.Design
{
	/// <summary>
	/// This is the dialog used to edit image map areas in a more user-friendly format
	/// </summary>
	internal partial class ImageMapAreaEditorDlg : System.Windows.Forms.Form
	{
        #region Buffered panel control
        //=====================================================================

        /// <summary>
        /// This class is derived from <see cref="Panel"/> and uses double-buffering to prevent flickering during
        /// redraw.
        /// </summary>
        [ToolboxItem(false)]
        internal class BufferedPanel : System.Windows.Forms.Panel
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public BufferedPanel()
            {
                this.DoubleBuffered = true;
                this.ResizeRedraw = true;
            }
        }
        #endregion

        #region Private data members
        //=====================================================================

        // The coordinate points collection
        private readonly PointCollection points;

        // The image to display and its height and width
        private string imageName;
        private int imageHeight, imageWidth;

        // The index of the area in the collection being edited
        private int areaIndex;

        // The shape of the area (determined by the number of coordinates)
        private ImageAreaShape shape;

        // The radius for circles
        private int radius;

        // The image and supporting stuff
        private Image image;
        private bool currentlyAnimating;

        // This flag indicted whether or not an image area is being selected
        private bool isSelecting;
        private Point origin;       // The origin of the selection operation

        // Timer and active control for the adjustment buttons
        private Timer timer;
        private Button adjustButton;
        private bool suppressClick;

        #endregion

        #region Properties
        //=====================================================================

        /// <summary>
        /// This read-only property is used to get a reference to the image area collection based on the control
        /// being edited.
        /// </summary>
#if IMAGEMAPWEB
        private static ImageAreaCollection Areas => Web.ImageAreaCollectionEditor.Areas;
#else
        private static ImageAreaCollection Areas => Windows.Forms.ImageAreaCollectionEditor.Areas;
#endif

        /// <summary>
        /// This is used to get or set the actual shape of the image area
        /// </summary>
        public ImageAreaShape Shape
        {
            get => shape;
            set => shape = value;
        }

        /// <summary>
        /// This property is used to get or set the area's coordinates on the image
        /// </summary>
        /// <value>The shape is determined from the number of coordinates.  Three is a circle, four is a
        /// rectangle or an ellipse, more than four is a polygon.  Since we can't tell the difference between
        /// a rectangle and ellipse, it's up to the designer to set the shape correctly for the ellipse image
        /// area.</value>
        /// <exception name="ArgumentException">This is thrown if any of the coordinate values in the string are
        /// non-numeric.</exception>
        public string Coordinates
        {
            get
            {
                StringBuilder sb = new StringBuilder(256);

                foreach(Point p in points)
                {
                    if(sb.Length > 0)
                        sb.Append(", ");

                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}, {1}", p.X, p.Y);
                }

                // If it's a circle, append the radius
                if(shape == ImageAreaShape.Circle && sb.Length > 0)
                    sb.AppendFormat(CultureInfo.InvariantCulture, ", {0}", radius);

                return sb.ToString();
            }
            set
            {
                int idx;

                points.Clear();

                string[] coordinates = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    // If the length is three, assume it's a circle
                    if(coordinates.Length == 3)
                    {
                        radius = Convert.ToInt32(coordinates[2], CultureInfo.InvariantCulture);

                        // Don't add the point if it has no size
                        if(radius != 0)
                            points.Add(new Point(Convert.ToInt32(coordinates[0], CultureInfo.InvariantCulture),
                                Convert.ToInt32(coordinates[1], CultureInfo.InvariantCulture)));

                        shape = ImageAreaShape.Circle;
                    }
                    else
                    {
                        if(coordinates.Length > 1)
                            for(idx = 0; idx < coordinates.Length; idx += 2)
                                points.Add(new Point(Convert.ToInt32(coordinates[idx], CultureInfo.InvariantCulture),
                                    Convert.ToInt32(coordinates[idx + 1], CultureInfo.InvariantCulture)));

                        // Could be an ellipse too if it's four.  The designer will have to set it for us.
                        if(points.Count == 2)
                            shape = ImageAreaShape.Rectangle;
                        else
                            shape = ImageAreaShape.Polygon;

                        // Clear the points if all are zero (no size)
                        for(idx = 0; idx < points.Count; idx++)
                            if(points[idx].X != 0 || points[idx].Y != 0)
                                break;

                        if(idx >= points.Count)
                            points.Clear();
                    }
                }
                catch(FormatException ex)
                {
                    throw new ArgumentException("The coordinates must be numeric", ex);
                }
            }
        }

        /// <summary>
        /// This property is used to get or set the image filename to display
        /// </summary>
        /// <remarks>Set this property or the <see cref="Image"/> property.  If this is set, the
        /// <see cref="Image"/> property is set to null.</remarks>
        public string ImageFilename
        {
            get => imageName;
            set
            {
                imageName = value;
                image = null;
            }
        }

        /// <summary>
        /// This property is used to get or set the image to display
        /// </summary>
        /// <remarks>Set this property or the <see cref="ImageFilename"/> property.  If this is set, the
        /// <see cref="ImageFilename"/> property is set to null.</remarks>
        public Image Image
        {
            get => image;
            set
            {
                image = value;
                imageName = null;
            }
        }

        /// <summary>
        /// This property is used to get or set the image height
        /// </summary>
        public int ImageHeight
        {
            get => imageHeight;
            set => imageHeight = value;
        }

        /// <summary>
        /// This property is used to get or set the image width
        /// </summary>
        public int ImageWidth
        {
            get => imageWidth;
            set => imageWidth = value;
        }
        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageMapAreaEditorDlg()
		{
			InitializeComponent();

            points = new PointCollection();
            areaIndex = -1;
        }
        #endregion

        #region Helper methods
        //=====================================================================

        /// <summary>
        /// This is used to enable image animation if supported
        /// </summary>
        private void Animate()
        {
            this.Animate(base.Visible && base.Enabled);
        }

        /// <summary>
        /// This is used to enable or disable image animation if supported
        /// </summary>
        /// <param name="enableAnimation">True to enable animation, false to disable it</param>
        private void Animate(bool enableAnimation)
        {
            if(enableAnimation != currentlyAnimating)
            {
                currentlyAnimating = enableAnimation;

                if(enableAnimation)
                {
                    if(image == null || !ImageAnimator.CanAnimate(image))
                        currentlyAnimating = false;
                    else
                        ImageAnimator.Animate(image, this.OnFrameChanged);
                }
                else
                {
                    if(image != null)
                        ImageAnimator.StopAnimate(image, this.OnFrameChanged);
                }
            }
        }

        /// <summary>
        /// Validate a single point to make sure it is within the image's boundaries
        /// </summary>
        private Point ValidatePoint(Point p)
        {
            if(p.X < 0)
                p.X = 0;

            if(p.X > imageWidth)
                p.X = imageWidth;

            if(p.Y < 0)
                p.Y = 0;

            if(p.Y > imageHeight)
                p.Y = imageHeight;

            return p;
        }

        /// <summary>
        /// Validate the radius of the circle area after it has changed to keep it within the bounds of the image
        /// </summary>
        private void ValidateRadius()
        {
            if(radius < 0)
                radius *= -1;

            if(points[0].X - radius < 0)
                radius += (points[0].X - radius);

            if(points[0].X + radius > imageWidth)
                radius -= (points[0].X + radius - imageWidth);

            if(points[0].Y - radius < 0)
                radius += (points[0].Y - radius);

            if(points[0].Y + radius > imageHeight)
                radius -= (points[0].Y + radius - imageHeight);
        }
        #endregion

        #region Method overrides and event handlers
        //=====================================================================

        /// <summary>
        /// This is overridden to repaint the control when resized
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            pnlImage.Invalidate();
        }

        /// <summary>
        /// This is overridden to repaint the control when resized
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            pnlImage.Invalidate();
        }

        /// <summary>
        /// On load, set the information labels and load the image
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void ImageMapAreaEditorDlg_Load(object sender, EventArgs e)
        {
            try
            {
                this.Animate(false);

                if(image == null)
                {
                    // If there is no image at all, create a temporary image
                    if(imageName == null)
                    {
                        // Set a name so that the image is disposed of on close
                        imageName = "BLANK";

                        if(this.ImageHeight == 0)
                            this.ImageHeight = 300;

                        if(this.ImageWidth == 0)
                            this.ImageWidth = 500;

                        Bitmap bm = new Bitmap(this.ImageWidth, this.ImageHeight);

                        using(Graphics g = Graphics.FromImage(bm))
                        {
                            g.FillRectangle(Brushes.White, 0, 0, this.ImageWidth, this.ImageHeight);
                        }

                        image = (Image)bm;
                    }
                    else
                        image = Image.FromFile(imageName);

                    if(this.ImageWidth == 0)
                        this.ImageWidth = image.Width;

                    if(this.ImageHeight == 0)
                        this.ImageHeight = image.Height;
                }

                this.Animate();

                pnlImage.AutoScrollMinSize = new Size(imageWidth, imageHeight);
            }
            catch(FileNotFoundException ex)
            {
                MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Unable to load image file: {0}",
                    ex.ToString()), "Error Loading Image", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnCancel.PerformClick();
            }

            lblImageInfo.Text = String.Format(CultureInfo.InvariantCulture, "Image: {0}  Height: {1}  Width: {2}",
                Path.GetFileName(imageName), imageHeight, imageWidth);

            switch(shape)
            {
                case ImageAreaShape.Rectangle:
                    lblInstructions.Text = "Click and drag to define rectangle area";
                    break;

                case ImageAreaShape.Circle:
                    lblInstructions.Text = "Click and drag to define circle area";
                    break;

                case ImageAreaShape.Ellipse:
                    lblInstructions.Text = "Click and drag to define ellipse area";
                    break;

                default:
                    lblInstructions.Text = "Click points to define the polygon";

                    // Can't shrink or increase the dimensions of a polygon
                    btnIncLeftTop.Visible = btnIncTop.Visible = btnIncRightTop.Visible = btnIncLeft.Visible =
                      btnIncAll.Visible = btnIncRight.Visible = btnIncLeftBottom.Visible = btnIncBottom.Visible =
                      btnIncRightBottom.Visible = btnDecLeftTop.Visible = btnDecTop.Visible =
                      btnDecRightTop.Visible = btnDecLeft.Visible = btnDecAll.Visible = btnDecRight.Visible =
                      btnDecLeftBottom.Visible = btnDecBottom.Visible = btnDecRightBottom.Visible = false;
                    break;
            }

            // Locate the image area in the collection being edited based on the shape and coordinates.  We use
            // this to know which area not to draw when the "Show All" option is on.  We turn that on
            // automatically if the image map is a Windows Forms image map control and it is set to owner drawn.
            ImageAreaCollection ic =  ImageMapAreaEditorDlg.Areas;

#if !IMAGEMAPWEB
            EWSoftware.ImageMaps.Windows.Forms.ImageMap im =
                (ic.ImageMapControl as EWSoftware.ImageMaps.Windows.Forms.ImageMap);

            if(im != null && im.OwnerDraw)
                chkShowAll.Checked = true;
#endif
            string coordinates = this.Coordinates;
            int idx = 0;

            foreach(IImageArea a in ic)
            {
                if(a.Shape == shape && a.Coordinates == coordinates)
                    areaIndex = idx;

                idx++;
            }
        }

        /// <summary>
        /// Clear the current image area selection
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            points.Clear();
            pnlImage.Invalidate();
            pnlImage.Update();
        }

        /// <summary>
        /// This is used to paint the image and the image area
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void pnlImage_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Point ptOffset = new Point(pnlImage.AutoScrollPosition.X, pnlImage.AutoScrollPosition.Y);

            if(image != null)
            {
                this.Animate();
                ImageAnimator.UpdateFrames();

                g.DrawImage(image, ptOffset.X, ptOffset.Y, imageWidth, imageHeight);
            }

            // Draw the other areas for reference if requested
            if(chkShowAll.Checked)
            {
                ImageAreaCollection ic = ImageMapAreaEditorDlg.Areas;

                for(int nIdx = 0; nIdx < ic.Count; nIdx++)
                    if(nIdx != areaIndex)
                        ic[nIdx].DrawFocusFrame(g, ptOffset);
            }

            if(points.Count > 0)
                switch(shape)
                {
                    case ImageAreaShape.Rectangle:
                        UnsafeNativeMethods.DrawReversibleRectangle(g, points[0], points[1], ptOffset);
                        break;

                    case ImageAreaShape.Circle:
                        UnsafeNativeMethods.DrawReversibleCircle(g, points[0], radius, ptOffset);
                        break;

                    case ImageAreaShape.Ellipse:
                        UnsafeNativeMethods.DrawReversibleEllipse(g, points[0], points[1], ptOffset);
                        break;

                    default:
                        // Note that the polygon isn't closed in the designer
                        UnsafeNativeMethods.DrawReversiblePolygon(g, points, false, ptOffset);
                        break;
                }
        }

        /// <summary>
        /// On left mouse click, clear the current point collection and begin selecting a new image area
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void pnlImage_MouseDown(object sender, MouseEventArgs e)
        {
            Point p, p1, p2;

            // Ignore if not left mouse button
            if(e.Button != MouseButtons.Left)
                return;

            p = new Point(e.X, e.Y);
            p.Offset(pnlImage.AutoScrollPosition.X * -1, pnlImage.AutoScrollPosition.Y * -1);

            isSelecting = true;

            // Starting a new selection or adjusting the old one?
            if(points.Count == 0 && shape != ImageAreaShape.Polygon)
            {
                // Add the first point
                points.Add(p);

                // Determine what to do next based on the shape
                if(shape == ImageAreaShape.Rectangle || shape == ImageAreaShape.Ellipse)
                    points.Add(new Point(p.X, p.Y));
                else
                    radius = 0;

                origin = p;
            }
            else    // Extend or adjust the existing one
                if(shape != ImageAreaShape.Rectangle && shape != ImageAreaShape.Ellipse)
                    pnlImage_MouseMove(sender, e);
                else
                {
                    // Use the nearest corner as the drag point
                    p1 = points[0];
                    p2 = points[1];

                    if(p.X >= p1.X && p.Y >= p1.Y)
                    {
                        origin = p1;
                        p2 = p;
                    }
                    else
                        if(p.X < p1.X && p.Y < p1.Y)
                        {
                            p1 = p;
                            origin = p2;
                        }
                        else
                            if(p.X < p1.X && p.Y >= p1.Y)
                            {
                                origin.X = p2.X;
                                origin.Y = p1.Y;
                                p1.X = p.X;
                                p2.Y = p.Y;
                            }
                            else
                            {
                                origin.X = p1.X;
                                origin.Y = p2.Y;
                                p2.X = p.X;
                                p1.Y = p.Y;
                            }

                    points[0] = ValidatePoint(p1);
                    points[1] = ValidatePoint(p2);
                }

            pnlImage.Invalidate();
            pnlImage.Update();
        }

        /// <summary>
        /// When the mouse button is released, set the new image area
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void pnlImage_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                isSelecting = false;

                // Add the point if it's a polygon
                if(shape == ImageAreaShape.Polygon)
                {
                    Point p = new Point(e.X, e.Y);
                    p.Offset(pnlImage.AutoScrollPosition.X * -1, pnlImage.AutoScrollPosition.Y * -1);

                    // Ignore it if outside the image
                    if(p.X >= 0 && p.X < imageWidth && p.Y >= 0 && p.Y < imageHeight)
                        points.Add(p);
                }

                pnlImage.Invalidate();
                pnlImage.Update();
            }
        }

        /// <summary>
        /// Resize the image area as the mouse moves
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void pnlImage_MouseMove(object sender, MouseEventArgs e)
        {
            Point p, p1, p2;
            int diffX, diffY;

            p = new Point(e.X, e.Y);
            p.Offset(pnlImage.AutoScrollPosition.X * -1, pnlImage.AutoScrollPosition.Y * -1);

            lblCoords.Text = String.Format(CultureInfo.InvariantCulture, "X: {0}  Y: {1}", p.X, p.Y);

            // Ignore if not selecting or if the shape is a polygon.  Polygons are drawn by clicking points so
            // there's nothing to do here for them.
            if(!isSelecting || shape == ImageAreaShape.Polygon)
                return;

            // Adjust the circle's radius or the rectangle's dimensions.  
            if(shape == ImageAreaShape.Circle)
            {
                // Use the greater of the X or Y difference
                diffX = p.X - points[0].X;
                diffY = p.Y - points[0].Y;

                if(diffX < 0)
                    diffX *= -1;

                if(diffY < 0)
                    diffY *= -1;

                if(diffX > diffY)
                    radius = diffX;
                else
                    radius = diffY;

                // Keep it within the bounds of the image
                ValidateRadius();
            }
            else
            {
                // Points are value types so we have to get a copy to modify them
                p1 = points[0];
                p2 = points[1];

                // Flip the point coordinates if necessary
                if(origin.X > p.X)
                {
                    p1.X = p.X;
                    p2.X = origin.X;
                }
                else
                    p2.X = p.X;

                if(origin.Y > p.Y)
                {
                    p1.Y = p.Y;
                    p2.Y = origin.Y;
                }
                else
                    p2.Y = p.Y;

                points[0] = ValidatePoint(p1);
                points[1] = ValidatePoint(p2);
            }

            pnlImage.Invalidate();
            pnlImage.Update();
        }

        /// <summary>
        /// This event is used to redraw an animated image
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event parameters</param>
        private void OnFrameChanged(object sender, EventArgs e)
        {
            pnlImage.Invalidate();
        }

        /// <summary>
        /// Turn drawing of other image areas on and off
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            pnlImage.Invalidate();
            pnlImage.Update();
        }

        /// <summary>
        /// Set up the timer so that we can hold down the adjustment buttons to fire repeated adjustment events
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event parameters</param>
        private void btnAdjustStart(object sender, MouseEventArgs e)
        {
            if(timer == null)
            {
                timer = new Timer();
                timer.Tick += this.AdjustmentTick;
            }

            suppressClick = false;
            adjustButton = (Button)sender;
            timer.Interval = 250;
            timer.Start();
        }

        /// <summary>
        /// Stop the timer when the mouse button is released.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event parameters</param>
        private void btnAdjustStop(object sender, MouseEventArgs e)
        {
            // Suppress the final click event if a timer tick occurred
            if(timer.Interval == 20)
                suppressClick = true;

            timer.Stop();
            timer.Dispose();
            timer = null;
        }

        /// <summary>
        /// Fire the adjustment event
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event parameters</param>
        private void AdjustmentTick(object sender, EventArgs e)
        {
            // Reduce the interval after the first tick so that it goes faster
            timer.Interval = 20;

            if(adjustButton == btnLeftUp || adjustButton == btnUp || adjustButton ==  btnRightUp ||
              adjustButton == btnLeft || adjustButton == btnRight || adjustButton == btnLeftDown ||
              adjustButton == btnDown || adjustButton == btnRightDown)
            {
                btnMove_Click(adjustButton, e);
            }
            else if(adjustButton == btnIncLeftTop || adjustButton == btnIncTop ||
              adjustButton == btnIncRightTop || adjustButton == btnIncLeft || adjustButton == btnIncAll ||
              adjustButton ==  btnIncRight || adjustButton == btnIncLeftBottom || adjustButton == btnIncBottom ||
              adjustButton == btnIncRightBottom)
            {
                btnIncrease_Click(adjustButton, e);
            }
            else if(adjustButton == btnDecLeftTop || adjustButton == btnDecTop ||
              adjustButton == btnDecRightTop || adjustButton == btnDecLeft || adjustButton == btnDecAll ||
              adjustButton ==  btnDecRight || adjustButton == btnDecLeftBottom || adjustButton == btnDecBottom ||
              adjustButton == btnDecRightBottom)
            {
                btnShrink_Click(adjustButton, e);
            }
        }

        /// <summary>
        /// Move the image area
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event parameters</param>
        private void btnMove_Click(object sender, EventArgs e)
        {
            Point p1, p2;
            int idx, left = 0, top = 0, right = 0, bottom = 0;

            if(suppressClick || points.Count == 0)
                return;

            // Figure out the adjustment based on the button clicked
            if(sender == btnLeftUp)
            {
                left = top = right = bottom = -1;
            }
            else if(sender == btnUp)
            {
                top = bottom = -1;
            }
            else if(sender == btnRightUp)
            {
                top = bottom = -1;
                left = right = 1;
            }
            else if(sender == btnLeft)
            {
                left = right = -1;
            }
            else if(sender == btnRight)
            {
                left = right = 1;
            }
            else if(sender == btnLeftDown)
            {
                left = right = -1;
                top = bottom = 1;
            }
            else if(sender == btnDown)
            {
                top = bottom = 1;
            }
            else if(sender == btnRightDown)
            {
                left = top = right = bottom = 1;
            }

            switch(shape)
            {
                case ImageAreaShape.Rectangle:
                case ImageAreaShape.Ellipse:
                    p1 = points[0];
                    p2 = points[1];

                    p1.X += left;
                    p1.Y += top;
                    p2.X += right;
                    p2.Y += bottom;

                    points[0] = ValidatePoint(p1);
                    points[1] = ValidatePoint(p2);
                    break;

                case ImageAreaShape.Circle:
                    p1 = points[0];

                    p1.X += left;
                    p1.Y += top;
                    p1.X += right;
                    p1.Y += bottom;

                    points[0] = ValidatePoint(p1);
                    ValidateRadius();
                    break;

                default:
                    // Polygon.  Don't move it if any one point goes out of bounds.  This is a two pass
                    // operation.
                    for(idx = 0; idx < points.Count; idx++)
                    {
                        p1 = points[idx];
                        p1.X += left;
                        p1.Y += top;
                        p1.X += right;
                        p1.Y += bottom;

                        if(p1 != ValidatePoint(p1))
                            break;
                    }

                    if(idx >= points.Count)
                        for(idx = 0; idx < points.Count; idx++)
                        {
                            p1 = points[idx];
                            p1.X += left;
                            p1.Y += top;
                            p1.X += right;
                            p1.Y += bottom;
                            points[idx] = p1;
                        }
                    break;
            }

            pnlImage.Invalidate();
            pnlImage.Update();
        }

        /// <summary>
        /// Increase the image area
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event parameters</param>
        private void btnIncrease_Click(object sender, EventArgs e)
        {
            Point p1, p2;
            int left = 0, top = 0, right = 0, bottom = 0;

            if(suppressClick || points.Count == 0)
                return;

            // Figure out the adjustment based on the button clicked
            if(sender == btnIncLeftTop)
            {
                left = top = -1;
            }
            else if(sender == btnIncTop)
            {
                top = -1;
            }
            else if(sender == btnIncRightTop)
            {
                top = -1;
                right = 1;
            }
            else if(sender == btnIncLeft)
            {
                left = -1;
            }
            else if(sender == btnIncRight)
            {
                right = 1;
            }
            else if(sender == btnIncLeftBottom)
            {
                left = -1;
                bottom = 1;
            }
            else if(sender == btnIncBottom)
            {
                bottom = 1;
            }
            else if(sender == btnIncRightBottom)
            {
                bottom = right = 1;
            }
            else if(sender == btnIncAll)
            {
                left = top = -1;
                right = bottom = 1;
            }

            switch(shape)
            {
                case ImageAreaShape.Rectangle:
                case ImageAreaShape.Ellipse:
                    p1 = points[0];
                    p2 = points[1];

                    p1.X += left;
                    p1.Y += top;
                    p2.X += right;
                    p2.Y += bottom;

                    points[0] = ValidatePoint(p1);
                    points[1] = ValidatePoint(p2);
                    break;

                case ImageAreaShape.Circle:
                    p1 = points[0];
                    radius += (left * -1) + (top * -1) + right + bottom;

                    points[0] = ValidatePoint(p1);
                    ValidateRadius();
                    break;

                default:
                    break;  // Polygon's can't be grown
            }

            pnlImage.Invalidate();
            pnlImage.Update();
        }

        /// <summary>
        /// Shrink the image area
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event parameters</param>
        private void btnShrink_Click(object sender, EventArgs e)
        {
            Point topLeft, bottomRight, p1, p2;
            int left = 0, top = 0, right = 0, bottom = 0;

            if(suppressClick || points.Count == 0)
                return;

            // Figure out the adjustment based on the button clicked
            if(sender == btnDecLeftTop)
            {
                left = top = 1;
            }
            else if(sender == btnDecTop)
            {
                top = 1;
            }
            else if(sender == btnDecRightTop)
            {
                top = 1;
                right = -1;
            }
            else if(sender == btnDecLeft)
            {
                left = 1;
            }
            else if(sender == btnDecRight)
            {
                right = -1;
            }
            else if(sender == btnDecLeftBottom)
            {
                left = 1;
                bottom = -1;
            }
            else if(sender == btnDecBottom)
            {
                bottom = -1;
            }
            else if(sender == btnDecRightBottom)
            {
                bottom = right = -1;
            }
            else if(sender == btnDecAll)
            {
                left = top = 1;
                right = bottom = -1;
            }

            switch(shape)
            {
                case ImageAreaShape.Rectangle:
                case ImageAreaShape.Ellipse:
                    topLeft = points[0];
                    bottomRight = points[1];

                    topLeft.X += left;
                    topLeft.Y += top;
                    bottomRight.X += right;
                    bottomRight.Y += bottom;

                    // Flip the point coordinates if necessary
                    p1 = new Point();
                    p2 = new Point();

                    if(topLeft.X > bottomRight.X)
                    {
                        p1.X = bottomRight.X;
                        p2.X = topLeft.X;
                    }
                    else
                    {
                        p1.X = topLeft.X;
                        p2.X = bottomRight.X;
                    }

                    if(topLeft.Y > bottomRight.Y)
                    {
                        p1.Y = bottomRight.Y;
                        p2.Y = topLeft.Y;
                    }
                    else
                    {
                        p1.Y = topLeft.Y;
                        p2.Y = bottomRight.Y;
                    }

                    points[0] = ValidatePoint(p1);
                    points[1] = ValidatePoint(p2);
                    break;

                case ImageAreaShape.Circle:
                    p1 = points[0];
                    radius += (left * -1) + (top * -1) + right + bottom;

                    points[0] = ValidatePoint(p1);
                    ValidateRadius();
                    break;

                default:
                    break;  // Polygon's can't be shrunk
            }

            pnlImage.Invalidate();
            pnlImage.Update();
        }
        #endregion
    }
}
