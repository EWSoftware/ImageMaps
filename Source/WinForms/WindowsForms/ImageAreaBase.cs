//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaBase.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 12/31/2024
// Note    : Copyright 2004-2024, Eric Woodruff, All rights reserved
//
// This file contains the abstract image area base class
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/09/2004  EFW  Created the code
// 12/23/2004  EFW  Added support for forwarding various event to the image areas.  Added new ellipse area.
//                  Added support for owner drawing.
// 06/28/2006  EFW  Reworked code for use with .NET 2.0
//===============================================================================================================

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

using EWSoftware.ImageMaps.Design.Windows.Forms;

namespace EWSoftware.ImageMaps.Windows.Forms
{
    /// <summary>
    /// This is the abstract base class for the image area types of the Windows Forms <see cref="ImageMap"/>
    /// control.
    /// </summary>
    /// <seealso cref="ImageAreaRectangle"/>
    /// <seealso cref="ImageAreaCircle"/>
    /// <seealso cref="ImageAreaEllipse"/>
    /// <seealso cref="ImageAreaPolygon"/>
    /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex3']/*" />
    public abstract class ImageAreaBase : IImageArea
    {
        #region Private data members
        //=====================================================================

        private bool enabled;           // True if enabled, false if not
        private bool ownerDrawn;        // True if owner drawn, false if not
        private int tabIndex;           // The tab index of area within map
        private string? toolTip;         // The tool tip for the image area
        private char accessKey;         // The access key

        #endregion

        #region Properties
        //=====================================================================

        /// <summary>
        /// This must be overridden to provide the image area shape
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public abstract ImageAreaShape Shape
        {
            get;
        }

        /// <summary>
        /// This must be overridden to provide the image area coordinates
        /// </summary>
        /// <remarks>This is not really used by the Windows Forms image map but is used in the designer to access
        /// the coordinate editor dialog box like the web server control.</remarks>
        [Category("Appearance"), Bindable(true), MergableProperty(false), Description("The area's coordinates"),
          RefreshProperties(RefreshProperties.Repaint),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Editor(typeof(ImageAreaCoordinateEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public abstract string Coordinates
        {
            get;
            set;
        }

        /// <summary>
        /// This is used to get or set the access key (a shortcut key or mnemonic) for the image area
        /// </summary>
        /// <value>The value should be a single alphanumeric character.  If the area is enabled and its
        /// <see cref="Action"/> is set to <c>FireEvent</c>, pressing the <strong>Alt</strong> key plus the
        /// access key will give the area the keyboard focus.</value>
        /// <exception cref="ArgumentOutOfRangeException">The specified access key is not a single character
        /// string.</exception>
        [Category("Behavior"), Bindable(true), DefaultValue(null), MergableProperty(false),
          Description("The access key for the area")]
        public string? AccessKey
        {
            get
            {
                if(accessKey == Char.MinValue)
                    return null;

                return accessKey.ToString();
            }
            set
            {
                if(value != null && value.Length > 1)
                    throw new ArgumentOutOfRangeException(nameof(value));

                if(String.IsNullOrEmpty(value))
                    accessKey = Char.MinValue;
                else
                    accessKey = Char.ToUpperInvariant(value![0]);
            }
        }

        /// <summary>
        /// This defines the action to take when the area is clicked
        /// </summary>
        /// <value>The default is to fire the <see cref="ImageMap.Click"/> event</value>
        [Category("Behavior"), DefaultValue(AreaClickAction.FireEvent), Bindable(true),
          Description("The action to take when the area is clicked")]
        public AreaClickAction Action { get; set; }

        /// <summary>
        /// This is used to enable or disable the area
        /// </summary>
        [Category("Behavior"), DefaultValue(true), Bindable(true), Description("Enable or disable the area")]
        public bool Enabled
        {
            get => enabled;
            set
            {
                if(enabled != value)
                {
                    enabled = value;
                    OnImageAreaChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// This is used to turn owner draw mode on and off
        /// </summary>
        /// <value>When true, the control will fire the <see cref="DrawImage"/> event to allow you to draw the
        /// image map and the individual image areas.  Note that image areas can still be set to owner drawn even
        /// if the image map is not.</value>
        /// <seealso cref="ImageMap.DrawImage"/>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex4']/*" />
        [Category("Behavior"), DefaultValue(false), Bindable(true), Description("Turn owner draw mode on or off")]
        public bool OwnerDraw
        {
            get => ownerDrawn;
            set
            {
                if(ownerDrawn != value)
                {
                    ownerDrawn = value;
                    OnImageAreaChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// This is used to get or set the tab index of the area
        /// </summary>
        /// <value>Tab index values within the image map are independent of tab index values in the containing
        /// control.  A value of zero excludes an area from the tab order of the image map.  Only image areas
        /// that are enabled and have an <see cref="Action"/> value of <c>FireEvent</c> can receive the focus.
        /// If no image areas have a tab index value, the image map will not participate in its parent's tab
        /// order.</value>
        /// <exception cref="ArgumentOutOfRangeException">This is thrown if the tab index is less than zero</exception>
        [Category("Behavior"), DefaultValue(0), Bindable(true),
          Description("The tab index of the area within the image map")]
        public int TabIndex
        {
            get => tabIndex;
            set
            {
                if(value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "TabIndex must be greater than or equal to zero");

                tabIndex = value;
            }
        }

        /// <summary>
        /// This is used to get or set the tool tip to display when the mouse hovers over the area
        /// </summary>
        [Category("Appearance"), DefaultValue(null), Bindable(true),
          Description("The tool tip to display when the mouse hovers over the area")]
        public string? ToolTip
        {
            get => toolTip;
            set
            {
                if(!String.IsNullOrWhiteSpace(value))
                    toolTip = value;
                else
                    toolTip = null;
            }
        }

        /// <summary>
        /// This is used to get or set an object that contains additional user-defined data for the image area
        /// </summary>
        [Category("Data"), DefaultValue(null), Bindable(true), TypeConverter(typeof(StringConverter)),
          Description("User-defined data associated with the control")]
        public object? Tag { get; set; }

        #endregion

        #region Events
        //=====================================================================

        /// <summary>
        /// This event is raised when an image area property changes that affects its visual presentation in the
        /// image map control such as its position or enabled state.
        /// </summary>
        [Category("Action"), Description("Fires when a property changes that affects its visual presentation")]
        public event EventHandler? ImageAreaChanged;

        /// <summary>
        /// This raises the ImageAreaChanged event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnImageAreaChanged(EventArgs e)
        {
            ImageAreaChanged?.Invoke(this, e);
        }

        /// <summary>
        /// This event is raised when the image area is clicked
        /// </summary>
        [Category("Action"), Description("Fires when the image area is clicked")]
        public event EventHandler<ImageMapClickEventArgs>? Click;

        /// <summary>
        /// This raises the image area <see cref="Click"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnClick(ImageMapClickEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        /// <summary>
        /// This event is raised when the image area is double-clicked
        /// </summary>
        [Category("Action"), Description("Fires when the image area is double-clicked")]
        public event EventHandler<ImageMapClickEventArgs>? DoubleClick;

        /// <summary>
        /// This raises the image area <see cref="DoubleClick"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnDoubleClick(ImageMapClickEventArgs e)
        {
            DoubleClick?.Invoke(this, e);
        }

        /// <summary>
        /// This event is raised when a mouse button is pressed while the mouse is inside the image area
        /// </summary>
        [Category("Mouse"), Description("Fires when a mouse button is pressed when the mouse is in the image area")]
        public event MouseEventHandler? MouseDown;

        /// <summary>
        /// This raises the image area <see cref="MouseDown"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        /// <summary>
        /// This event is raised when a mouse button is released while the mouse is inside the image area
        /// </summary>
        [Category("Mouse"), Description("Fires when a mouse button is released when the mouse is in the image area")]
        public event MouseEventHandler? MouseUp;

        /// <summary>
        /// This raises the image area <see cref="MouseUp"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            MouseUp?.Invoke(this, e);
        }

        /// <summary>
        /// This event is raised when the mouse enters the image area
        /// </summary>
        [Category("Mouse"), Description("Fires when the mouse enters the image area")]
        public event EventHandler? MouseEnter;

        /// <summary>
        /// This raises the image area <see cref="MouseEnter"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnMouseEnter(EventArgs e)
        {
            MouseEnter?.Invoke(this, e);
        }

        /// <summary>
        /// This event is raised when the mouse leaves the image area
        /// </summary>
        [Category("Mouse"), Description("Fires when the mouse leaves the image area")]
        public event EventHandler? MouseLeave;

        /// <summary>
        /// This raises the image area <see cref="MouseLeave"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnMouseLeave(EventArgs e)
        {
            MouseLeave?.Invoke(this, e);
        }

        /// <summary>
        /// This event is raised when the mouse remains stationary inside the image area for an amount of time
        /// </summary>
        /// <remarks>This event will fire each time the mouse becomes stationary after moving within the bounds
        /// of an image area.</remarks>
        [Category("Mouse"),
          Description("Fires when the mouse remains stationary inside the image area for an amount of time")]
        public event EventHandler? MouseHover;

        /// <summary>
        /// This raises the image area <see cref="MouseHover"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnMouseHover(EventArgs e)
        {
            MouseHover?.Invoke(this, e);
        }

        /// <summary>
        /// This event is raised when the mouse moves in the image area
        /// </summary>
        [Category("Mouse"), Description("Fires when the mouse moves in the image area")]
        public event MouseEventHandler? MouseMove;

        /// <summary>
        /// This raises the image area <see cref="MouseMove"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            MouseMove?.Invoke(this, e);
        }

        /// <summary>
        /// This event is raised when the image area gains the focus
        /// </summary>
        [Category("Focus"), Description("Fires when the image area gains the focus")]
        public event EventHandler? Enter;

        /// <summary>
        /// This raises the image area <see cref="Enter"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnEnter(EventArgs e)
        {
            Enter?.Invoke(this, e);
        }

        /// <summary>
        /// This event is raised when the image area loses the focus
        /// </summary>
        [Category("Focus"), Description("Fires when the image area loses the focus")]
        public event EventHandler? Leave;

        /// <summary>
        /// This raises the image area <see cref="Leave"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnLeave(EventArgs e)
        {
            Leave?.Invoke(this, e);
        }

        /// <summary>
        /// This event is raised when owner draw mode is enabled and the image area needs to be drawn
        /// </summary>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex4']/*" />
        [Category("Action"), Description("Fires when owner draw mode is on and the image area needs to be drawn")]
        public event EventHandler<DrawImageEventArgs>? DrawImage;

        /// <summary>
        /// This raises the image area <see cref="DrawImage"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex4']/*" />
        protected virtual void OnDrawImage(DrawImageEventArgs e)
        {
            ImageAreaCircle c;
            var handler = DrawImage;

            if(e == null)
                throw new ArgumentNullException(nameof(e));

            if(handler != null)
                handler(this, e);
            else
            {
                Rectangle r;

                switch(this.Shape)
                {
                    case ImageAreaShape.Rectangle:
                        r = ((ImageAreaRectangle)this).Rectangle;
                        break;

                    case ImageAreaShape.Ellipse:
                        r = ((ImageAreaEllipse)this).Ellipse;
                        break;

                    case ImageAreaShape.Circle:
                        c = (ImageAreaCircle)this;
                        r = new Rectangle(c.CenterPoint, new Size(0, 0));
                        r.Width = r.Height = c.Radius * 2;
                        r.X -= r.Width / 2;
                        r.Y -= r.Height / 2;
                        break;

                    default:    // Polygon, use first point and a fixed size
                        r = new Rectangle(((ImageAreaPolygon)this).Points[0], new Size(100, 100));
                        break;
                }

                // Add the offset
                r.X += e.ImageOffset.X;
                r.Y += e.ImageOffset.Y;

                using Font font = new("Microsoft Sans Serif", 7.8f);

                e.Graphics.FillRectangle(Brushes.Beige, r);
                e.Graphics.DrawString("ImageArea: DrawImageEvent not implemented", font, Brushes.Black,
                    r.X, r.Y);
            }
        }
        #endregion

        #region Constructors
        //=====================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <overloads>There are two overloads for the constructor</overloads>
        protected ImageAreaBase()
        {
            enabled = true;
        }

        /// <summary>
        /// This version of the constructor takes a tool tip
        /// </summary>
        /// <param name="toolTip">The tool tip to associate with the image area</param>
        protected ImageAreaBase(string? toolTip) : this()
        {
            this.ToolTip = toolTip;
        }
        #endregion

        #region Methods
        //=====================================================================

        /// <summary>
        /// This must be overridden to get the shape to draw a focus frame around itself
        /// </summary>
        /// <param name="g">The graphics object to use for drawing.</param>
        /// <param name="offset">A point used to offset the image area coordinates (i.e. if the image is
        /// scrolled).</param>
        public abstract void DrawFocusFrame(Graphics g, Point offset);

        /// <summary>
        /// This is overridden to allow proper comparison of image area objects.
        /// </summary>
        /// <param name="obj">The object to which this instance is compared</param>
        /// <returns>Returns true if the object equals this instance, false if it does not</returns>
        public override bool Equals(object? obj)
        {
            return obj is ImageAreaBase a &&
                (this.Shape == a.Shape && this.Coordinates == a.Coordinates &&
                this.AccessKey == a.AccessKey && this.Action == a.Action && this.Enabled == a.Enabled &&
                this.TabIndex == a.TabIndex && this.ToolTip == a.ToolTip &&
                ((this.Tag == null && a.Tag == null) || (this.Tag != null && this.Tag.Equals(a.Tag))));
        }

        /// <summary>
        /// Get a hash code for the image area object
        /// </summary>
        /// <returns>Returns the hash code for the image area's string form</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Convert the image area instance to its string description
        /// </summary>
        /// <returns>Returns the description of the image area.</returns>
        public override string ToString()
        {
            StringBuilder sb = new(80);

            if(this.ToolTip != null)
            {
                sb.Append(this.ToolTip);
                sb.Append(", ");
            }

            sb.AppendFormat(CultureInfo.InvariantCulture, "{0} at {1}", this.Shape.ToString(), this.Coordinates);

            return sb.ToString();
        }

        /// <summary>
        /// This is used to fire an event on the image area
        /// </summary>
        /// <param name="areaEvent">The event to fire</param>
        /// <param name="args">The event arguments</param>
        public void RaiseEvent(ImageAreaEvent areaEvent, object args)
        {
            // DrawImage always fires.  The image map sets all of the necessary event argument parameters.
            if(areaEvent == ImageAreaEvent.DrawImage)
            {
                OnDrawImage((DrawImageEventArgs)args);
                return;
            }

            // No other events are fired if disabled
            if(enabled)
            {
                switch(areaEvent)
                {
                    case ImageAreaEvent.Click:
                        this.OnClick((ImageMapClickEventArgs)args);
                        break;

                    case ImageAreaEvent.DoubleClick:
                        this.OnDoubleClick((ImageMapClickEventArgs)args);
                        break;

                    case ImageAreaEvent.MouseDown:
                        this.OnMouseDown((MouseEventArgs)args);
                        break;

                    case ImageAreaEvent.MouseUp:
                        this.OnMouseUp((MouseEventArgs)args);
                        break;

                    case ImageAreaEvent.MouseEnter:
                        this.OnMouseEnter((EventArgs)args);
                        break;

                    case ImageAreaEvent.MouseLeave:
                        this.OnMouseLeave((EventArgs)args);
                        break;

                    case ImageAreaEvent.MouseHover:
                        this.OnMouseHover((EventArgs)args);
                        break;

                    case ImageAreaEvent.MouseMove:
                        this.OnMouseMove((MouseEventArgs)args);
                        break;

                    case ImageAreaEvent.Enter:
                        this.OnEnter((EventArgs)args);
                        break;

                    case ImageAreaEvent.Leave:
                        this.OnLeave((EventArgs)args);
                        break;
                }
            }
        }
        #endregion
    }
}
