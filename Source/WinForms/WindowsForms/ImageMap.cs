//===============================================================================================================
// System  : Image Map Control Library
// File    : FormImageMap.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/10/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived UserControl class that can be used to display an image map on a Windows form (an
// image with hyperlink hot spots).
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/09/2004  EFW  Created the code
// 12/23/2004  EFW  Added support for forwarding various event to the active image area.  Added new ellipse
//                  image area.  Added support for owner drawing.
// 06/28/2006  EFW  Reworked code for use with .NET 2.0
//===============================================================================================================

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;

using EWSoftware.ImageMaps.Design.Windows.Forms;

namespace EWSoftware.ImageMaps.Windows.Forms
{
    /// <summary>
    /// This is a derived <see cref="System.Windows.Forms.UserControl" /> class that can be used to display an
    /// image map (an image with clickable hot spots).
    /// </summary>
    /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex3']/*" />
	[DefaultProperty("Image"), DefaultEvent("Click"), Designer(typeof(ImageMapDesigner))]
	public partial class ImageMap : System.Windows.Forms.UserControl, IImageMap
	{
        #region Private data members
        //=====================================================================

        // This is static and is shared by all image maps to display them in a disabled state
        private static ImageAttributes iaDisabled;

        // The image and the image area stuff
        private Graphics gPanel;
		private GraphicsPath pathData;

        private Image image;
        private ImageAreaCollection imageAreas;

        private int imageMapWidth, imageMapHeight, activeArea, mouseArea;
        private bool sizeToImage, centerImage, currentlyAnimating, ownerDrawn;

        // The cursors to use when the mouse is over a defined image area and when not over an image area
        private Cursor areaCursor, nonAreaCursor;

        #endregion

        #region Properties
        //=====================================================================

        /// <summary>
        /// This is used to get or set the image to display in the image map control
        /// </summary>
        [DefaultValue(null), Category("Appearance"), RefreshProperties(RefreshProperties.All),
          Description("The image to display")]
        public virtual Image Image
        {
            get { return image; }
            set
            {
                this.Animate(false);

                // NOTE: Do not dispose of the old image.  If it's a project resource, it will cause a crash
                // later if re-used as the project won't expect it to have been disposed.
                image = value;

                this.Animate();

                if(!sizeToImage)
                {
                    base.AutoScrollMinSize = new Size(this.ImageMapWidth, this.ImageMapHeight);
                    this.RefreshAreaInformation();
                }
                else    // The code to size it is in the property so just set it
                    this.SizeToImage = true;
            }
        }

        /// <summary>
        /// This is used to get or set the cursor shown when the mouse is not over a defined image area
        /// </summary>
        /// <value>The default is <see cref="Cursors.Default"/></value>
        /// <seealso cref="ImageAreaCursor"/>
		[Category("Appearance"), Bindable(true),
          Description("The cursor shown when the mouse is not over a defined image area")]
        public override Cursor Cursor
        {
            get { return base.Cursor; }
            set
            {
                nonAreaCursor = (value == null) ? Cursors.Default : value;
                base.Cursor = value;
            }
        }

        /// <summary>
        /// This property is used to get or set the cursor shown when the mouse is over a defined image area
        /// </summary>
        /// <value>The default is <see cref="Cursors.Hand"/></value>
        /// <seealso cref="Cursor"/>
		[Category("Appearance"), Bindable(true),
          Description("The cursor shown when the mouse is over a defined image area")]
        public Cursor ImageAreaCursor
        {
            get { return areaCursor; }
            set { areaCursor = (value == null) ? Cursors.Hand : value; }
        }

        /// <summary>
        /// This property is used to get or set whether or not the control will resize itself to show the entire
        /// image if possible if the image is larger than its current size or shrink its size to fit the image if
        /// the image is smaller than its current size.
        /// </summary>
        /// <value>If true, the control will resize itself to display the entire image if possible.  An attempt
        /// is made to keep the control within the bounds of the parent control.  If false (the default), it will
        /// keep its current size and will enable scroll bars when necessary if the image is too large to fit in
        /// the display area.
        /// </value>
        /// <seealso cref="CenterImage"/>
		[Category("Appearance"), Bindable(true), DefaultValue(false), RefreshProperties(RefreshProperties.Repaint),
          Description("Specify whether or not the control resizes itself to show the entire image")]
        public bool SizeToImage
        {
            get { return sizeToImage; }
            set
            {
                int width, height, borderWidth;

                sizeToImage = value;

                if(value && image != null)
                {
                    imageMapWidth = imageMapHeight = 0;

                    // Account for the border style and position
                    switch(this.BorderStyle)
                    {
                        case BorderStyle.Fixed3D:
                            borderWidth = 4;
                            break;

                        case BorderStyle.FixedSingle:
                            borderWidth = 2;
                            break;

                        default:
                            borderWidth = 0;
                            break;
                    }

                    width = image.Width + borderWidth;
                    height = image.Height + borderWidth;

                    // Keep it within the parent
                    if(this.Left + width > this.Parent.ClientSize.Width)
                        width = this.Parent.ClientSize.Width - this.Left;

                    if(this.Top + height > this.Parent.ClientSize.Height)
                        height = this.Parent.ClientSize.Height - this.Top;

                    this.Size = new Size(width, height);
                    base.AutoScrollMinSize = new Size(this.ImageMapWidth, this.ImageMapHeight);
                }

                this.RefreshAreaInformation();
            }
        }

        /// <summary>
        /// This property is used to get or set whether or not the control will center the image in the client
        /// area.
        /// </summary>
        /// <value>If true (the default), the control will center the image if it is smaller than the height
        /// and/or width of the client area.  If false, it is drawn in the upper left corner.</value>
        /// <seealso cref="SizeToImage"/>
		[Category("Appearance"), Bindable(true), DefaultValue(true), RefreshProperties(RefreshProperties.Repaint),
          Description("Specify whether or not the control will center the image in its client area")]
        public bool CenterImage
        {
            get { return centerImage; }
            set
            {
                centerImage = value;
                this.RefreshAreaInformation();
            }
        }

        /// <summary>
        /// This property is used to get or set the index of the area that has the focus
        /// </summary>
        /// <value>Returns -1 if no area has the focus.  Setting the area to an index outside the bounds of the
        /// collection will clear the focus.</value>
        [Bindable(false), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Description("get or set the focused image area")]
        public int FocusedArea
        {
            get { return activeArea; }
            set { this.Focus(value, false); }
        }

        /// <summary>
        /// This read-only property is used to get the index of the area that the mouse is over
        /// </summary>
        /// <value>Returns -1 if the mouse is not over an image area.  Note that this area can be different than
        /// the focused area index returned by the <see cref="FocusedArea"/> property.</value>
        [Bindable(false), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Description("Get the image area that contains the mouse")]
        public int MouseArea
        {
            get { return mouseArea; }
        }

        /// <summary>
        /// This is used to turn owner draw mode on and off
        /// </summary>
        /// <value>When true, the control will fire the <see cref="DrawImage"/> event to allow you to draw the
        /// image map and the individual image areas.  Note that image areas can still be set to owner drawn even
        /// if the image map is not.</value>
        /// <seealso cref="ImageAreaBase.DrawImage"/>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex5']/*" />
		[Category("Behavior"), DefaultValue(false), Bindable(true), Description("Turn owner draw mode on or off")]
        public bool OwnerDraw
        {
            get { return ownerDrawn; }
            set
            {
                ownerDrawn = value;
                this.RefreshAreaInformation();
            }
        }
        #endregion

        #region Private designer methods
        //=====================================================================
        
        // These are used because the default values for these properties don't work with the DefaultValue
        // attribute.

        /// <summary>
        /// The designer uses this to determine whether or not to serialize changes to the ImageAreaCursor
        /// property.
        /// </summary>
        /// <returns>True if it should serialize the property, false if not</returns>
        private bool ShouldSerializeImageAreaCursor()
        {
            return (areaCursor != Cursors.Hand);
        }

        /// <summary>
        /// Reset the image area cursor
        /// </summary>
        private void ResetImageAreaCursor()
        {
            areaCursor = Cursors.Hand;
        }

        /// <summary>
        /// The designer uses this to determine whether or not to serialize changes to the Areas property
        /// </summary>
        /// <returns>True if it should serialize the property, false if not</returns>
        private bool ShouldSerializeAreas()
        {
            return this.Areas.Count > 0;
        }

        /// <summary>
        /// Reset the areas collection
        /// </summary>
        private void ResetAreas()
        {
            this.Areas.Clear();
        }

        /// <summary>
        /// The designer uses this to determine whether or not to serialize the image width property
        /// </summary>
        /// <returns>True if it should serialize the property, false if not</returns>
        private bool ShouldSerializeImageMapWidth()
        {
            if(image == null)
                return (imageMapWidth != 0);

            return (image.Width != this.ImageMapWidth);
        }

        /// <summary>
        /// Reset the image width property
        /// </summary>
        private void ResetImageMapWidth()
        {
            this.ImageMapWidth = 0;
        }

        /// <summary>
        /// The designer uses this to determine whether or not to serialize the image height property
        /// </summary>
        /// <returns>True if it should serialize the property, false if not</returns>
        private bool ShouldSerializeImageMapHeight()
        {
            if(image == null)
                return (imageMapHeight != 0);

            return (image.Height != this.ImageMapHeight);
        }

        /// <summary>
        /// Reset the image height property
        /// </summary>
        private void ResetImageMapHeight()
        {
            this.ImageMapHeight = 0;
        }
        #endregion

        #region Hidden properties and events
        //=====================================================================

        // These properties and events do not apply so they are hidden

        /// <summary>
        /// Image maps do not use this property so it is hidden.  It always returns null.
        /// </summary>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Image BackgroundImage
        {
            get { return null; }
        }

        /// <summary>
        /// Image maps do not use this property so it is hidden.  It always returns the base image layout.
        /// </summary>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
        }

        /// <summary>
        /// Image maps do not use this property so it is hidden.  It always returns false.
        /// </summary>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AllowDrop
        {
            get { return false; }
        }

        /// <summary>
        /// Image maps do not use this property so it is hidden.  It always returns true.
        /// </summary>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoScroll
        {
            get { return true; }
        }

        /// <summary>
        /// Image maps do not use this property so it is hidden.  It always returns the base margin.
        /// </summary>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size AutoScrollMargin
        {
            get { return base.AutoScrollMargin; }
        }

        /// <summary>
        /// Image maps do not use this property so it is hidden.  It always returns the base size.
        /// </summary>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size AutoScrollMinSize
        {
            get { return base.AutoScrollMinSize; }
        }

        /// <summary>
        /// Image maps do not use this property so it is hidden.  It always returns the value of the
        /// <see cref="SizeToImage"/> property.
        /// </summary>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoSize
        {
            get { return this.SizeToImage; }
        }

        /// <summary>
        /// Image maps do not use this property so it is hidden.  It always returns the base auto-size mode.
        /// </summary>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new AutoSizeMode AutoSizeMode
        {
            get { return base.AutoSizeMode; }
        }

#pragma warning disable 0067
        /// <summary>
        /// Image maps do not use the background image so this event is hidden.
        /// </summary>
        /// <exclude/>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler BackgroundImageChanged;

        /// <summary>
        /// Image maps do not use the default paint event so it is hidden
        /// </summary>
        /// <exclude/>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event PaintEventHandler Paint;

#pragma warning restore 0067
        #endregion

        #region IImageMap interface implementation
        //=====================================================================

        /// <summary>
        /// This is used to get or set the tool tip text displayed for all undefined areas of the image
        /// </summary>
        [Bindable(true), Description("Tool tip text for the image"), Category("Appearance"), DefaultValue("")]
        public string ToolTip { get; set; }

        /// <summary>
		/// Gets or sets the width of the image in the image map control in pixels
        /// </summary>
        /// <seealso cref="ImageMapHeight"/>
		[Bindable(true), Category("Layout"), RefreshProperties(RefreshProperties.All),
          Description("Specify the image map width in pixels")]
        public int ImageMapWidth
        {
            get
            {
                if(image != null && imageMapWidth == 0)
                    return image.Width;

                return imageMapWidth;
            }
            set
            {
                if(image == null || (value != 0 && image.Width != value))
                {
                    imageMapWidth = value;
                    base.AutoScrollMinSize = new Size(value, base.AutoScrollMinSize.Height);
                }
                else
                {
                    imageMapWidth = 0;     // Matches image width
                    base.AutoScrollMinSize = new Size(image.Width, base.AutoScrollMinSize.Height);
                }

                this.RefreshAreaInformation();
            }
        }

        /// <summary>
		/// Gets or sets the height of the image in the image map control in pixels
        /// </summary>
        /// <seealso cref="ImageMapWidth"/>
		[Bindable(true), Category("Layout"), RefreshProperties(RefreshProperties.All),
          Description("Specify the image map height in pixels")]
        public int ImageMapHeight
        {
            get
            {
                if(image != null && imageMapHeight == 0)
                    return image.Height;

                return imageMapHeight;
            }
            set
            {
                if(image == null || (value != 0 && image.Height != value))
                {
                    imageMapHeight = value;
                    base.AutoScrollMinSize = new Size(base.AutoScrollMinSize.Width, value);
                }
                else
                {
                    imageMapHeight = 0;    // Matches image height
                    base.AutoScrollMinSize = new Size(base.AutoScrollMinSize.Width, image.Height);
                }

                this.RefreshAreaInformation();
            }
        }

        /// <summary>
        /// This returns the collection of areas defined for the image map
        /// </summary>
		[Bindable(true), Category("Layout"), MergableProperty(false),
		  DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
          Description("The collection of areas that represent hot spots on the image map"),
          Editor(typeof(ImageAreaCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ImageAreaCollection Areas
        {
            get
            {
                if(imageAreas == null)
                {
                    imageAreas = new ImageAreaCollection(this);
                    imageAreas.ImageAreaChanged += this.ImageAreaChanged;
                }

                return imageAreas;
            }
        }

        /// <summary>
        /// This event is raised when a hot spot on the image map is clicked
        /// </summary>
		[Category("Action"), Description("Fires when a hot spot on the image map is clicked")]
        public new event EventHandler<ImageMapClickEventArgs> Click;

        /// <summary>
        /// This raises the image map <see cref="Click"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnClick(ImageMapClickEventArgs e)
        {
            var handler = Click;

            if(handler != null)
                handler(this, e);
        }
        #endregion

        #region Other Events
        //=====================================================================
        
        // Other events that are not part of the IImageMap interface.  These are used only by the Windows Forms
        // image map control.

        /// <summary>
        /// This event is raised when a hot spot on the image map is double-clicked
        /// </summary>
		[Category("Action"), Description("Fires when a hot spot on the image map is double-clicked")]
        public new event EventHandler<ImageMapClickEventArgs> DoubleClick;

        /// <summary>
        /// This raises the image map <see cref="DoubleClick"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnDoubleClick(ImageMapClickEventArgs e)
        {
            var handler = DoubleClick;

            if(handler != null)
                handler(this, e);
        }

        /// <summary>
        /// This event is raised when owner draw mode is enabled and the image map needs to be drawn
        /// </summary>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex5']/*" />
		[Category("Action"), Description("Fires when owner draw mode is on and the image map needs to be drawn")]
        public event EventHandler<DrawImageEventArgs> DrawImage;

        /// <summary>
        /// This raises the image map <see cref="DrawImage"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex5']/*" />
        protected virtual void OnDrawImage(DrawImageEventArgs e)
        {
            var handler = DrawImage;

            if(handler != null)
                handler(this, e);
            else
            {
                e.Graphics.FillRectangle(Brushes.White, this.ClientRectangle);
                e.Graphics.DrawString("ImageMap: DrawImageEvent not implemented", base.Font, Brushes.Black, 0, 0);
            }
        }
        #endregion

        #region Private class methods
        //=====================================================================

        /// <summary>
        /// This is used to calculate the image offset for drawing and image area hit testing
        /// </summary>
        /// <returns>The offset as a point</returns>
        private Point CalculateImageOffset()
        {
            int offsetX, offsetY;

            Size s = this.ClientSize;

            if(!centerImage || this.ImageMapWidth > s.Width)
                offsetX = this.AutoScrollPosition.X;
            else
                offsetX = (s.Width - this.ImageMapWidth) / 2;

            if(!centerImage || this.ImageMapHeight > s.Height)
                offsetY = this.AutoScrollPosition.Y;
            else
                offsetY = (s.Height - this.ImageMapHeight) / 2;

            return new Point(offsetX, offsetY);
        }

        /// <summary>
        /// This is used to fill the graphics path with the image area shapes
        /// </summary>
        /// <remarks>The path data is used as a cache of sorts to help the image map quickly determine whether or
        /// not an area is active.  Empty areas are ignored.</remarks>
        private void SetImageAreaPaths()
        {
            pathData.Reset();
            pathData.FillMode = FillMode.Winding;

            foreach(ImageAreaBase a in this.Areas)
            {
                if(pathData.PointCount > 0)
                    pathData.SetMarkers();

                ImageAreaRectangle r = a as ImageAreaRectangle;

                if(r != null)
                {
                    if(r.Rectangle.Width > 0 && r.Rectangle.Height > 0)
                        pathData.AddRectangle(r.Rectangle);
                }
                else
                {
                    ImageAreaCircle c = a as ImageAreaCircle;

                    if(c != null)
                    {
                        if(c.Radius > 0)
                            pathData.AddEllipse(c.CenterPoint.X - c.Radius, c.CenterPoint.Y - c.Radius,
                                c.Radius * 2, c.Radius * 2);
                    }
                    else
                    {
                        ImageAreaEllipse e = a as ImageAreaEllipse;

                        if(e != null)
                        {
                            if(e.Ellipse.Width > 0 && e.Ellipse.Height > 0)
                                pathData.AddEllipse(e.Ellipse);
                        }
                        else
                        {
                            ImageAreaPolygon p = (ImageAreaPolygon)a;
                            int count = p.Points.Count;

                            if(count > 2)
                            {
                                Point[] pts = new Point[count];
                                p.Points.CopyTo(pts, 0);
                                pathData.AddPolygon(pts);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This is used to get the image area at the specified point (if any)
        /// </summary>
        /// <returns>The image area index if the point is in an image area or negative one (-1) if it is not in
        /// any of the image areas.</returns>
		private int ImageAreaAtPoint(Point pt)
		{
            int idx;

            if(pt.X > this.ImageMapWidth || pt.Y > this.ImageMapHeight)
                return -1;

            // Set the image area paths if they need recalculating
            if(pathData.PointCount == 0)
                this.SetImageAreaPaths();

			using(GraphicsPath p = new GraphicsPath())
			    using(GraphicsPathIterator i = new GraphicsPathIterator(pathData))
                {
			        i.Rewind();

			        for(idx = 0; idx < i.SubpathCount; idx++)
			        {
				        i.NextMarker(p);

				        if(p.IsVisible(pt, gPanel) && this.Areas[idx].Enabled)
				            break;
			        }

                    return (idx < i.SubpathCount) ? idx : -1;
                }
		}

        /// <summary>
        /// This is used to locate the first or last selectable image area in tab index order
        /// </summary>
        /// <param name="firstArea">True if searching for the first selectable area, false if searching for the
        /// last selectable area.</param>
        /// <returns>Returns a valid index into the <see cref="Areas"/> collection if an area is found or -1 if
        /// not.</returns>
        /// <remarks>A selectable area is defined as one that is enabled, uses the <c>FireEvent</c> action, and
        /// has a non-zero tab index value.</remarks>
        private int FirstOrLastSelectableArea(bool firstArea)
        {
            ImageAreaBase a;
            int position, firstTabIdx, firstTabPos = -1;

            if(this.Areas.Count == 0)
                return -1;

            position = (firstArea) ? 0 : this.Areas.Count - 1;

            if(firstArea)
            {
                firstTabIdx = Int32.MaxValue;

                while(position < this.Areas.Count)
                {
                    a = (ImageAreaBase)this.Areas[position];

                    // Remember the position if it is next in the tab order
                    if(a.Enabled && a.TabIndex > 0 && a.Action == AreaClickAction.FireEvent &&
                      a.TabIndex < firstTabIdx)
                    {
                        firstTabPos = position;
                        firstTabIdx = a.TabIndex;
                    }

                    position++;
                }
            }
            else
            {
                firstTabIdx = 0;

                while(position > -1)
                {
                    a = (ImageAreaBase)this.Areas[position];

                    // Remember the position if it is next in the tab order
                    if(a.Enabled && a.TabIndex > 0 && a.Action == AreaClickAction.FireEvent &&
                      a.TabIndex > firstTabIdx)
                    {
                        firstTabPos = position;
                        firstTabIdx = a.TabIndex;
                    }

                    position--;
                }
            }

            return firstTabPos;
        }

        /// <summary>
        /// This is used to locate the next selectable image area moving forward or backwards from the specified
        /// position in tab index order.
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="forward">True if searching forward, false if searching backwards through the tab index
        /// order.</param>
        /// <returns>Returns a valid index into the <see cref="Areas"/> collection if an area is found or -1 if
        /// not.</returns>
        /// <remarks>A selectable area is defined as one that is enabled, uses the FireEvent action, and has a
        /// non-zero tab index value.</remarks>
        private int NextSelectableArea(int start, bool forward)
        {
            ImageAreaBase a;
            int position, tabIndex, firstTabIdx, firstTabPos = -1;

            if(this.Areas.Count == 0)
                return -1;

            // Get the starting position's tab index
            if(start < 0 || start >= this.Areas.Count)
                tabIndex = (forward) ? 0 : Int32.MaxValue;
            else
                tabIndex = this.Areas[start].TabIndex;

            position = (forward) ? 0 : this.Areas.Count - 1;

            if(forward)
            {
                firstTabIdx = Int32.MaxValue;

                while(position < this.Areas.Count)
                {
                    a = (ImageAreaBase)this.Areas[position];

                    // Remember the position if it is next in the tab order
                    if(a.Enabled && a.TabIndex > 0 && a.Action == AreaClickAction.FireEvent &&
                      a.TabIndex > tabIndex && a.TabIndex < firstTabIdx)
                    {
                        firstTabPos = position;
                        firstTabIdx = a.TabIndex;
                    }

                    position++;
                }
            }
            else
            {
                firstTabIdx = 0;

                while(position > -1)
                {
                    a = (ImageAreaBase)this.Areas[position];

                    // Remember the position if it is next in the tab order
                    if(a.Enabled && a.TabIndex > 0 && a.Action == AreaClickAction.FireEvent &&
                      a.TabIndex < tabIndex && a.TabIndex > firstTabIdx)
                    {
                        firstTabPos = position;
                        firstTabIdx = a.TabIndex;
                    }

                    position--;
                }
            }

            return firstTabPos;
        }

        /// <summary>
        /// This is used to scroll the control to ensure that the selected image area is in view
        /// </summary>
        private void EnsureAreaVisible()
        {
            ImageAreaCircle c;
            Rectangle r;
            Point p1, p2;
            Size s;
            int radius;

            if(activeArea >= this.Areas.Count)
                activeArea = -1;

            if(activeArea == -1)
                return;

            ImageAreaBase a = (ImageAreaBase)this.Areas[activeArea];

            // Figure out the minimum and maximum coordinates of the shape
            switch(a.Shape)
            {
                case ImageAreaShape.Rectangle:
                    r = ((ImageAreaRectangle)a).Rectangle;
                    break;

                case ImageAreaShape.Circle:
                    c = (ImageAreaCircle)a;
                    p1 = c.CenterPoint;
                    radius = c.Radius;
                    r = new Rectangle(p1.X - radius, p1.Y - radius, radius * 2, radius * 2);
                    break;

                case ImageAreaShape.Ellipse:
                    r = ((ImageAreaEllipse)a).Ellipse;
                    break;

                default:
                    p1 = new Point(Int32.MaxValue, Int32.MaxValue);
                    p2 = new Point(Int32.MinValue, Int32.MinValue);

                    foreach(Point pt in ((ImageAreaPolygon)a).Points)
                    {
                        if(pt.X < p1.X)
                            p1.X = pt.X;

                        if(pt.Y < p1.Y)
                            p1.Y = pt.Y;

                        if(pt.X > p2.X)
                            p2.X = pt.X;

                        if(pt.Y > p2.Y)
                            p2.Y = pt.Y;
                    }

                    r = new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
                    break;
            }

            s = this.ClientSize;

            if(centerImage && this.ImageMapWidth < s.Width)
                r.X += (s.Width - this.ImageMapWidth) / 2;

            if(centerImage && this.ImageMapHeight < s.Height)
                r.Y += (s.Height - this.ImageMapHeight) / 2;

            p1 = new Point(this.AutoScrollPosition.X * -1, this.AutoScrollPosition.Y * -1);

            if(r.X < p1.X)
                p1.X -= p1.X - r.X;

            if(r.Y < p1.Y)
                p1.Y -= p1.Y - r.Y;

            if(r.X + r.Width > p1.X + s.Width)
                p1.X = r.X + r.Width - s.Width;

            if(r.Y + r.Height > p1.Y + s.Height)
                p1.Y = r.Y + r.Height - s.Height;

            if(this.AutoScrollPosition.X * -1 != p1.X || this.AutoScrollPosition.Y * -1 != p1.Y)
                this.AutoScrollPosition = p1;
        }

        /// <summary>
        /// This is used to enable image animation if supported
        /// </summary>
        private void Animate()
        {
            this.Animate(base.Visible && base.Enabled && this.Parent != null);
        }

        /// <summary>
        /// This is used to enable or disable image animation if supported
        /// </summary>
        /// <param name="enableAnimation">True to enable animation, false to disable it</param>
        private void Animate(bool enableAnimation)
        {
            if(enableAnimation == currentlyAnimating)
                return;

            currentlyAnimating = enableAnimation;

            if(enableAnimation)
            {
                if(image == null || !ImageAnimator.CanAnimate(image))
                    currentlyAnimating = false;
                else
                    ImageAnimator.Animate(image, this.OnFrameChanged);
            }
            else
                if(image != null)
                    ImageAnimator.StopAnimate(image, this.OnFrameChanged);
        }

        /// <summary>
        /// This is called to raise one of the mouse click events.  They are all similar and use the same code.
        /// </summary>
        /// <param name="areaEvent">The image area mouse event to raise</param>
        /// <param name="clickLocation">The mouse cursor click location</param>
        /// <param name="args">The event arguments</param>
        private void RaiseMouseClickEvent(ImageAreaEvent areaEvent, Point clickLocation, object args)
        {
            ImageMapClickEventArgs ce = null;

            Point offset = CalculateImageOffset();

            int nArea = this.ImageAreaAtPoint(new Point(clickLocation.X - offset.X, clickLocation.Y - offset.Y));

            if(nArea != -1)
            {
                ImageAreaBase a = (ImageAreaBase)this.Areas[nArea];

                if(a.Action == AreaClickAction.FireEvent)
                {
                    if(activeArea != nArea)
                        this.Focus(nArea, true);

                    // Create ImageMapClickEventArgs if the passed arguments parameter is null
                    if(args == null)
                        ce = new ImageMapClickEventArgs(nArea, clickLocation.X - offset.X,
                            clickLocation.Y - offset.Y);

                    switch(areaEvent)
                    {
                        case ImageAreaEvent.Click:
                            a.RaiseEvent(areaEvent, ce);
                            this.OnClick(ce);
                            break;

                        case ImageAreaEvent.DoubleClick:
                            a.RaiseEvent(areaEvent, ce);
                            this.OnDoubleClick(ce);
                            break;

                        case ImageAreaEvent.MouseDown:
                        case ImageAreaEvent.MouseUp:
                            a.RaiseEvent(areaEvent, args);
                            break;

                        default:
                            throw new NotImplementedException("Unknown event used for RaiseMouseClickEvent");

                    }
                }
            }
        }
        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        public ImageMap()
		{
            // Set the value of the double-buffering style bits to true
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

			InitializeComponent();

			gPanel = Graphics.FromHwnd(this.Handle);

            activeArea = mouseArea = -1;
            areaCursor = Cursors.Hand;
            nonAreaCursor = Cursors.Default;
            centerImage = true;
			pathData = new GraphicsPath();

            // Create the disabled image attributes on first use
            if(iaDisabled == null)
            {
                float[][] afColorMatrix = new float[5][];

                afColorMatrix[0] = new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f };
                afColorMatrix[1] = new float[5] { 0.2577f, 0.2577f, 0.2577f, 0f, 0f };
                afColorMatrix[2] = new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f };
                afColorMatrix[3] = new float[5] { 0f, 0f, 0f, 1f, 0f };
                afColorMatrix[4] = new float[5] { 0.38f, 0.38f, 0.38f, 0f, 1f};

                iaDisabled = new ImageAttributes();
                iaDisabled.ClearColorKey();
                iaDisabled.SetColorMatrix(new ColorMatrix(afColorMatrix));
            }
		}
        #endregion

        #region Helper methods
        //=====================================================================

        /// <summary>
        /// This method is used to notify the image map that its image area collection has been modified in some
        /// way and that it should refresh its internal area markers.
        /// </summary>
        /// <remarks>The image areas make every effort to notify the image map of changes that may affect their
        /// visual presentation.  However, there may be cases where this does not or cannot happen.  In such
        /// cases, this can be called manually to tell the image map to refresh its cached image area
        /// information.  Once such case is when modifying the points collection of a polygon image area.  The
        /// point collection is not equipped to notify the image area that it has changed.  If you make changes
        /// to it manually, this method must be called to notify the image map of the changes.</remarks>
        public void RefreshAreaInformation()
        {
            if(pathData.PointCount > 0)
            {
                this.SetImageAreaPaths();
                this.Invalidate();
                this.Update();
            }
        }

        /// <summary>
        /// This method can be used to set the focus to a specific image area
        /// </summary>
        /// <param name="area">The image area to which focus is given</param>
        /// <param name="setFocus">Specify true to make the image map the focused control, false to leave the
        /// focus alone.</param>
        /// <remarks>If the image map does not contain the image area specified, the focus is cleared.</remarks>
        /// <overloads>There are two overloads for this method.</overloads>
        public void Focus(IImageArea area, bool setFocus)
        {
            this.Focus(this.Areas.IndexOf(area), setFocus);
        }

        /// <summary>
        /// This method can be used to set the focus to a specific image area index
        /// </summary>
        /// <param name="area">The index of the image area to which focus is given</param>
        /// <param name="setFocus">Specify true to make the image map the focused control, false to leave the
        /// focus alone.</param>
        /// <remarks>If the index is outside the bounds of the area collection, the focus is cleared.</remarks>
        public void Focus(int area, bool setFocus)
        {
            int  priorArea = activeArea;
            bool isFocused = this.Focused;

            if(!isFocused && setFocus)
                this.Focus();

            if(area < 0 || area >= this.Areas.Count)
                activeArea = -1;
            else
                activeArea = area;

            this.EnsureAreaVisible();

            // Raise the Leave event for the prior area if necessary
            if(priorArea != -1 && isFocused && priorArea != activeArea)
                ((ImageAreaBase)this.Areas[priorArea]).RaiseEvent(ImageAreaEvent.Leave, EventArgs.Empty);

            // Raise the Enter event for the new area if necessary
            if(activeArea != -1 && this.Focused && activeArea != priorArea)
                ((ImageAreaBase)this.Areas[activeArea]).RaiseEvent(ImageAreaEvent.Enter, EventArgs.Empty);

            this.Invalidate();
        }
        #endregion

        #region Event handlers
        //=====================================================================

        /// <summary>
        /// This is called whenever an image area property changes that will affect its visual presentation in
        /// the image map such as its size or position.
        /// </summary>
        /// <remarks>When called, this will refresh the cached image area information</remarks>
        private void ImageAreaChanged(object sender, EventArgs e)
        {
            bool tabStop = false;

            this.RefreshAreaInformation();

            // See if TabStop needs to be turned on or off
            foreach(ImageAreaBase a in this.Areas)
                if(a.Enabled && a.TabIndex != 0 && a.Action == AreaClickAction.FireEvent)
                {
                    tabStop = true;
                    break;
                }

            if(base.TabStop != tabStop)
                base.TabStop = tabStop;

            if(activeArea >= this.Areas.Count)
                this.Focus(-1, false);
        }

        /// <summary>
        /// This event is used to redraw an animated image
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event parameters</param>
        private void OnFrameChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        #endregion

        #region Method overrides
        //=====================================================================

        /// <summary>
        /// This is overridden to adjust the control layout when the control style changes
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnStyleChanged(EventArgs e)
        {
            base.OnStyleChanged(e);

            // Resize to fit image if necessary.  The code to do it is in the property so we'll just set it to
            // true again.
            if(sizeToImage)
                this.SizeToImage = true;
        }

        /// <summary>
        /// This is overridden to enable or disable animation when the control's enabled state changes
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            this.Animate();
        }

        /// <summary>
        /// This is overridden to enable or disable animation when the control's parent changes
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            this.Animate();
        }

        /// <summary>
        /// This is overridden to enable or disable animation when the control's visible state changes
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            this.Animate();
        }

        /// <summary>
        /// This is overridden to repaint the control and select the appropriate image area as the active one
        /// when the control gains focus.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnGotFocus(EventArgs e)
        {
            int area = activeArea;

            base.OnGotFocus(e);

            // Find the first enabled area if there is one.  If tabbed into, find the first one from the start.
            // If Shift+Tabbed into, find the first one from the end.
            if(area == -1 || area >= this.Areas.Count)
                area = this.FirstOrLastSelectableArea((Control.ModifierKeys != Keys.Shift));

            if(area != activeArea)
                this.Focus(area, false);

            this.Invalidate();
        }

        /// <summary>
        /// This is overridden to repaint the control when it loses focus
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.Invalidate();
        }

        /// <summary>
        /// This is overridden to invalidate the display when resized
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        /// <summary>
        /// This is overridden to invalidate the client area so that the control is redrawn correctly when
        /// scrolled.
        /// </summary>
        /// <param name="m">The message</param>
        protected override void WndProc(ref Message m)
        {
            if(m.Msg == 0x0114 || m.Msg == 0x0115)
                this.Invalidate();

            base.WndProc(ref m);
        }

        /// <summary>
        /// This is overridden to fire the image map click event if an image area has the focus and the Space bar
        /// or Enter key is pressed.
        /// </summary>
        /// <param name="charCode">The character code</param>
        /// <returns>True if handled, false if not</returns>
        protected override bool ProcessDialogChar(char charCode)
        {
            if(activeArea != -1 && (charCode == '\r' || charCode == ' '))
            {
                ImageAreaBase a = (ImageAreaBase)this.Areas[activeArea];
                ImageMapClickEventArgs ce = new ImageMapClickEventArgs(activeArea, -1, -1);

                a.RaiseEvent(ImageAreaEvent.Click, ce);
                this.OnClick(ce);
                return true;
            }

            return base.ProcessDialogChar(charCode);
        }

        /// <summary>
        /// This is overridden to handle arrow keys in the same manner as a Tab key press
        /// </summary>
        /// <param name="keyData">The key to process</param>
        /// <returns>True if the key was processed, false if not</returns>
        /// <remarks>The up and left arrows act like Shift+Tab.  The right and down arrows act like Tab.</remarks>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            bool result;

            switch((keyData & Keys.KeyCode))
            {
                case Keys.Left:
                case Keys.Up:
                    result = this.ProcessTabKey(false);

                    if(!result && this.Parent != null)
                        result = this.Parent.SelectNextControl(this, false, false, false, true);
                    break;

                case Keys.Right:
                case Keys.Down:
                    result = this.ProcessTabKey(true);

                    if(!result && this.Parent != null)
                        result = this.Parent.SelectNextControl(this, true, false, false, true);
                    break;

                default:
                    result = base.ProcessDialogKey(keyData);
                    break;
            }

            return result;
        }

        /// <summary>
        /// This is overridden to handle processing of image area access keys
        /// </summary>
        /// <param name="charCode">The character code</param>
        /// <returns>True if handled, false if not</returns>
        protected override bool ProcessMnemonic(char charCode)
        {
            ImageAreaBase a;
            bool result = false;

            for(int idx = 0; idx < this.Areas.Count; idx++)
            {
                a = (ImageAreaBase)this.Areas[idx];

                if(a.Enabled && a.Action == AreaClickAction.FireEvent && a.AccessKey != null &&
                  a.AccessKey[0] == Char.ToUpper(charCode, CultureInfo.InvariantCulture))
                {
                    result = true;
                    this.Focus(idx, true);

                    ImageMapClickEventArgs ce = new ImageMapClickEventArgs(idx, -1, -1);

                    a.RaiseEvent(ImageAreaEvent.Click, ce);
                    this.OnClick(ce);
                }
            }

            if(!result)
                result = base.ProcessMnemonic(charCode);

            return result;
        }

        /// <summary>
        /// This is overridden to handle tabbing through the image areas
        /// </summary>
        /// <param name="forward">True if tabbing forward through the areas or false if tabbing backwards through
        /// them.</param>
        /// <returns>True if the next image area is selected or false if there is not one to go to (i.e. the
        /// start or end of the image areas is reached).</returns>
        protected override bool ProcessTabKey(bool forward)
        {
            int area = this.NextSelectableArea(activeArea, forward);

            this.Focus(area, false);

            if(area == -1)
                return base.ProcessTabKey(forward);

            return true;
        }

        /// <summary>
        /// This is overridden to display the image map
        /// </summary>
        /// <remarks>It fires the <see cref="DrawImage"/> event for the image map if it is owner drawn and the
        /// <see cref="ImageAreaBase.DrawImage"/> event for any owner drawn image areas.</remarks>
        /// <param name="e">The event arguments</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            ImageAreaBase a;
            bool drawFrame = true, designMode = this.DesignMode, focused = this.Focused, enabled = this.Enabled;

            Graphics g = e.Graphics;
            Point p = CalculateImageOffset();

            DrawImageEventArgs drawArgs = new DrawImageEventArgs(g, DrawState.Normal, p, true);

            // Set the image area paths if they need recalculating
            if(pathData.PointCount == 0)
                this.SetImageAreaPaths();

            // Image animation is supported even for owner drawn image maps.  The owner draw event can get the
            // image and display it using the Image property.
            if(image != null)
            {
                this.Animate();
                ImageAnimator.UpdateFrames();
            }

            if(!ownerDrawn || designMode)
            {
                // The width and height properties are used in case custom values have been specified.
                if(image != null)
                    if(this.Enabled)
                        g.DrawImage(image, p.X, p.Y, this.ImageMapWidth, this.ImageMapHeight);
                    else
                        g.DrawImage(image, new Rectangle(p.X, p.Y, this.ImageMapWidth, this.ImageMapHeight),
                            0, 0, image.Width, image.Height, GraphicsUnit.Pixel, iaDisabled);
            }
            else
            {
                // Set the drawing state
                if(!enabled)
                    drawArgs.DrawState = DrawState.Disabled;
                else
                    if(focused)
                        drawArgs.DrawState = DrawState.Focus;

                OnDrawImage(drawArgs);

                // If the owner draw event handled the focus frame, we won't draw one on non-owner drawn image
                // areas.
                drawFrame = drawArgs.DrawFocus;
            }

            // Raise the DrawImage event for all owner drawn image areas.  Image areas can be owner draw even if
            // the image map is not.
            if(!designMode)
                for(int idx = 0; idx < this.Areas.Count; idx++)
                {
                    // Assume no focus frame on owner drawn image areas
                    drawArgs.DrawFocus = false;
                    drawArgs.DrawState = DrawState.Normal;

                    a = (ImageAreaBase)this.Areas[idx];

                    if(a.OwnerDraw)
                    {
                        // Set the image area's drawing state
                        if(!enabled  || !a.Enabled)
                            drawArgs.DrawState = DrawState.Disabled;
                        else
                            if(focused && activeArea == idx)
                                drawArgs.DrawState = DrawState.Focus;
                            else
                                if(mouseArea == idx)
                                    drawArgs.DrawState = DrawState.HotLight;

                        a.RaiseEvent(ImageAreaEvent.DrawImage, drawArgs);

                        // Draw focus frame?
                        if(focused && idx == activeArea && drawArgs.DrawFocus)
                            a.DrawFocusFrame(g, p);
                    }
                    else
                    {
                        // If we have the focus and this is the active area, draw a focus frame around it unless
                        // overridden by the image map owner draw event.
                        if(focused && idx == activeArea && drawFrame)
                            a.DrawFocusFrame(g, p);
                    }
                }
        }

        /// <summary>
        /// This is overridden to raise the image map click event when a defined image area is clicked
        /// </summary>
        /// <param name="e">The event arguments</param>
        /// <overloads>There are two overloads for this method.</overloads>
        protected override void OnClick(EventArgs e)
        {
            this.RaiseMouseClickEvent(ImageAreaEvent.Click, this.PointToClient(Cursor.Position), null);
            base.OnClick(e);
        }

        /// <summary>
        /// This is overridden to raise the image map double-click event when a defined image area is
        /// double-clicked.
        /// </summary>
        /// <param name="e">The event arguments</param>
        /// <overloads>There are two overloads for this method.</overloads>
        protected override void OnDoubleClick(EventArgs e)
        {
            this.RaiseMouseClickEvent(ImageAreaEvent.DoubleClick, this.PointToClient(Cursor.Position), null);
            base.OnDoubleClick(e);
        }

        /// <summary>
        /// This is overridden to raise the event in the image area under the mouse if there is one
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.RaiseMouseClickEvent(ImageAreaEvent.MouseDown, new Point(e.X, e.Y), e);
            base.OnMouseDown(e);
        }

        /// <summary>
        /// This is overridden to raise the event in the image area under the mouse if there is one
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // MouseUp fires after Click.  If the click event disposes of us, don't fire the MouseUp event.
            if(pathData != null)
                this.RaiseMouseClickEvent(ImageAreaEvent.MouseUp, new Point(e.X, e.Y), e);

            base.OnMouseUp(e);
        }

        /// <summary>
        /// This is overridden to raise the event in the image area under the mouse if there is one
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnMouseHover(EventArgs e)
        {
            if(mouseArea != -1)
                ((ImageAreaBase)this.Areas[mouseArea]).RaiseEvent(ImageAreaEvent.MouseHover, e);

            base.OnMouseHover(e);
        }

        /// <summary>
        /// This is overridden to adjust the display as the mouse moves in and out of defined areas on the image
        /// map and to fire the appropriate image area mouse events.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            ImageAreaBase a = null;

            Point p = CalculateImageOffset();

            int area = this.ImageAreaAtPoint(new Point(e.X - p.X, e.Y - p.Y));

            if(area != -1)
                a = (ImageAreaBase)this.Areas[area];

            if(area != mouseArea)
            {
                // Raise the MouseLeave event for the old area
                if(mouseArea != -1)
                    ((ImageAreaBase)this.Areas[mouseArea]).RaiseEvent(ImageAreaEvent.MouseLeave, EventArgs.Empty);

                if(area > -1)
                {
                    mouseArea = area;

                    // If the action is none, use the normal cursor
                    if(a.Action == AreaClickAction.None)
                        base.Cursor = nonAreaCursor;
                    else
                        base.Cursor = areaCursor;

                    toolTip.SetToolTip(this, this.Areas[area].ToolTip);

                    // Raise the MouseEnter event for the area
                    a.RaiseEvent(ImageAreaEvent.MouseEnter, EventArgs.Empty);
                }
                else
                {
                    mouseArea = -1;
                    base.Cursor = nonAreaCursor;
                    toolTip.SetToolTip(this, this.ToolTip);
                }

                this.Invalidate();
            }

            // Raise the MouseMove event if over an area
            if(area != -1)
                a.RaiseEvent(ImageAreaEvent.MouseMove, e);

            base.OnMouseMove(e);

            // We also need to reset mouse hover notification so that it occurs whenever the mouse stops moving
            UnsafeNativeMethods.ResetMouseHover(this.Handle);
        }
        #endregion
    }
}
