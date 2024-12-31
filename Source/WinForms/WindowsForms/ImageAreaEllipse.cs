//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaEllipse.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 12/31/2024
// Note    : Copyright 2004-2024, Eric Woodruff, All rights reserved
//
// This file contains the ellipse image area class
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 12/23/2004  EFW  Created the code
//===============================================================================================================

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

using EWSoftware.ImageMaps.Design.Windows.Forms;

namespace EWSoftware.ImageMaps.Windows.Forms
{
    /// <summary>
    /// This is the ellipse image area class
    /// </summary>
    /// <remarks>Although it can be perfectly circular, the ellipse uses a rectangular area to define its
    /// coordinates and thus can be stretched vertically or horizontally.</remarks>
    /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex3']/*" />
    [TypeConverter(typeof(ImageAreaTypeConverter))]
    public class ImageAreaEllipse : ImageAreaBase
    {
        #region Private data members
        //=====================================================================

        private Rectangle r;

        #endregion

        #region Properties
        //=====================================================================

        /// <summary>
        /// This is overridden to provide the shape type.
        /// </summary>
        /// <value>It always returns <see cref="ImageAreaShape.Ellipse"/></value>.
        public override ImageAreaShape Shape => ImageAreaShape.Ellipse;

        /// <summary>
        /// This is overridden to get or set the coordinate values in string form
        /// </summary>
        /// <remarks>The coordinates are expected to be numeric and in the order left, top, right, bottom with
        /// each value separated by commas.</remarks>
        /// <exception cref="ArgumentException">This is thrown if the number of coordinates given is not exactly
        /// equal to four or if non-numeric values are found.</exception>
        /// <seealso cref="Ellipse"/>
        [DefaultValue("0, 0, 0, 0"), Description("The ellipse area's upper left and lower right coordinates"),
          Editor(typeof(EllipseCoordinateEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public override string Coordinates
        {
            get => String.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3}", this.Ellipse.X,
                    this.Ellipse.Y, this.Ellipse.X + this.Ellipse.Width, this.Ellipse.Y + this.Ellipse.Height);
            set
            {
                if(value == null || value.Length == 0)
                {
                    this.Ellipse = new Rectangle(0, 0, 0, 0);
                    return;
                }

                string[] coordinates = value.Split([','], StringSplitOptions.RemoveEmptyEntries);

                if(coordinates.Length != 4)
                    throw new ArgumentException("There must be exactly four ellipse coordinates");

                try
                {
                    int x1 = Convert.ToInt32(coordinates[0], CultureInfo.InvariantCulture),
                        y1 = Convert.ToInt32(coordinates[1], CultureInfo.InvariantCulture),
                        x2 = Convert.ToInt32(coordinates[2], CultureInfo.InvariantCulture),
                        y2 = Convert.ToInt32(coordinates[3], CultureInfo.InvariantCulture);

                    this.Ellipse = new Rectangle(x1, y1, x2 - x1, y2 - y1);
                }
                catch(FormatException ex)
                {
                    throw new ArgumentException("The ellipse coordinates must be numeric", ex);
                }
            }
        }

        /// <summary>
        /// This is used to get or set the ellipse's area
        /// </summary>
        /// <seealso cref="Coordinates"/>
        [Category("Appearance"), DefaultValue(typeof(Rectangle), "0, 0, 0, 0"), Bindable(true),
          RefreshProperties(RefreshProperties.Repaint), Description("The ellipse area for the image map")]
        public Rectangle Ellipse
        {
            get => r;
            set
            {
                if(r != value)
                {
                    r = value;
                    OnImageAreaChanged(EventArgs.Empty);
                }
            }
        }
        #endregion

        #region Constructors
        //=====================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <overloads>There are three overloads for the constructor</overloads>
        public ImageAreaEllipse()
        {
            r = new Rectangle(0, 0, 0, 0);
        }

        /// <summary>
        /// Constructor.  This version takes the ellipse coordinates.
        /// </summary>
        /// <param name="rect">The ellipse coordinates</param>
        public ImageAreaEllipse(Rectangle rect) : this(rect, null)
        {
        }

        /// <summary>
        /// Constructor.  This version takes the ellipse coordinates and a tool tip.
        /// </summary>
        /// <param name="rect">The ellipse coordinates</param>
        /// <param name="toolTip">The tool tip to show when the mouse hovers over the area</param>
        public ImageAreaEllipse(Rectangle rect, string? toolTip) : base(toolTip)
        {
            r = rect;
        }
        #endregion

        #region Methods
        //=====================================================================

        /// <summary>
        /// This is overridden to get the shape to draw a focus frame around itself
        /// </summary>
        /// <param name="g">The graphics object to use for drawing</param>
        /// <param name="offset">A point used to offset the image area coordinates (i.e. if the image is
        /// scrolled).</param>
        public override void DrawFocusFrame(Graphics g, Point offset)
        {
            if(g != null)
            {
                UnsafeNativeMethods.DrawReversibleEllipse(g, new Point(r.X, r.Y),
                    new Point(r.X + r.Width, r.Y + r.Height), offset);
            }
        }
        #endregion
    }
}
