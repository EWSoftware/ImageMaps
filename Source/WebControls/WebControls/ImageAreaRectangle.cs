//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaRectangle.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/03/2023
// Note    : Copyright 2004-2023, Eric Woodruff, All rights reserved
//
// This file contains the rectangle image area class
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/01/2004  EFW  Created the code
//===============================================================================================================

// Ignore Spelling: rect

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace EWSoftware.ImageMaps.Web.Controls
{
    /// <summary>
    /// This is the rectangle image area class
    /// </summary>
    /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex1']/*" />
    public class ImageAreaRectangle : ImageAreaBase
    {
        #region Properties
        //=====================================================================

        /// <summary>
        /// This is overridden to provide the shape type
        /// </summary>
        /// <value>It always returns <see cref="ImageAreaShape.Rectangle"/></value>.
        public override ImageAreaShape Shape
        {
            get { return ImageAreaShape.Rectangle; }
        }

        /// <summary>
        /// This is overridden to specify the HTML shape name
        /// </summary>
        /// <value>It always returns "<c>rect</c>"</value>.
        /// <seealso cref="Shape"/>
        public override string ShapeText
        {
            get { return "rect"; }
        }

        /// <summary>
        /// This is overridden to get or set the coordinate values in string form
        /// </summary>
        /// <remarks>The coordinates are expected to be numeric and in the order left, top, right, bottom with
        /// each value separated by commas.</remarks>
        /// <exception cref="ArgumentException">This is thrown if the number of coordinates given is not exactly
        /// equal to four or if non-numeric values are found.</exception>
        /// <seealso cref="Rectangle"/>
        [DefaultValue("0, 0, 0, 0"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Description("The rectangle area's upper left and lower right coordinates")]
        public override string Coordinates
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3}", this.Rectangle.X,
                    this.Rectangle.Y, this.Rectangle.X + this.Rectangle.Width,
                    this.Rectangle.Y + this.Rectangle.Height);
            }
            set
            {
                if(value == null || value.Length == 0)
                {
                    this.Rectangle = new Rectangle(0, 0, 0, 0);
                    return;
                }

                string[] coordinates = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if(coordinates.Length != 4)
                    throw new ArgumentException("There must be exactly four rectangle coordinates");

                try
                {
                    int x1 = Convert.ToInt32(coordinates[0], CultureInfo.InvariantCulture),
                        y1 = Convert.ToInt32(coordinates[1], CultureInfo.InvariantCulture),
                        x2 = Convert.ToInt32(coordinates[2], CultureInfo.InvariantCulture),
                        y2 = Convert.ToInt32(coordinates[3], CultureInfo.InvariantCulture);

                    this.Rectangle = new Rectangle(x1, y1, x2 - x1, y2 - y1);
                }
                catch(FormatException ex)
                {
                    throw new ArgumentException("The rectangle coordinates must be numeric", ex);
                }
            }
        }

        /// <summary>
        /// This is used to get or set the rectangle's area
        /// </summary>
        /// <seealso cref="Coordinates"/>
        [Category("Appearance"), DefaultValue(typeof(Rectangle), "0, 0, 0, 0"), Bindable(true),
          RefreshProperties(RefreshProperties.Repaint), Description("The area rectangle for the image map")]
        public Rectangle Rectangle
        {
            get { return (Rectangle)this.ViewState["Rectangle"]; }
            set { this.ViewState["Rectangle"] = value; }
        }
        #endregion

        #region Constructors
        //=====================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <overloads>There are four overloads for the constructor</overloads>
        public ImageAreaRectangle()
        {
            this.Rectangle = new Rectangle(0, 0, 0, 0);
        }

        /// <summary>
        /// Constructor.  This version takes the rectangle coordinates.
        /// </summary>
        /// <param name="rect">The rectangle coordinates</param>
        public ImageAreaRectangle(Rectangle rect) : this(rect, null, null)
        {
        }

        /// <summary>
        /// Constructor.  This version takes the rectangle coordinates and the URL to which to navigate when
        /// clicked.
        /// </summary>
        /// <param name="rect">The rectangle coordinates</param>
        /// <param name="url">The URL to which to navigate when clicked</param>
        public ImageAreaRectangle(Rectangle rect, string url) : this(rect, url, null)
        {
        }

        /// <summary>
        /// Constructor.  This version takes the rectangle coordinates, the URL to which to navigate when
        /// clicked, and a tool tip.
        /// </summary>
        /// <param name="rect">The rectangle coordinates</param>
        /// <param name="url">The URL to which to navigate when clicked</param>
        /// <param name="toolTip">The tool tip to show when the mouse hovers over the area</param>
        public ImageAreaRectangle(Rectangle rect, string url, string toolTip) : base(url, toolTip)
        {
            this.Rectangle = rect;
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
            Rectangle r = this.Rectangle;

            UnsafeNativeMethods.DrawReversibleRectangle(g, new Point(r.X, r.Y),
                new Point(r.X + r.Width, r.Y + r.Height), offset);
        }
        #endregion
    }
}
