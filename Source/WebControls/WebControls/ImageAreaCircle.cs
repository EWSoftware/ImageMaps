//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaCircle.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 12/31/2024
// Note    : Copyright 2004-2024, Eric Woodruff, All rights reserved
//
// This file contains the circle image area class
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

// Ignore Spelling: circ

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace EWSoftware.ImageMaps.Web.Controls
{
    /// <summary>
    /// This is the circle image area class
    /// </summary>
    /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex1']/*" />
    public class ImageAreaCircle : ImageAreaBase
    {
        #region Properties
        //=====================================================================

        /// <summary>
        /// This is overridden to provide the shape type
        /// </summary>
        /// <value>It always returns <see cref="ImageAreaShape.Circle"/></value>.
        public override ImageAreaShape Shape => ImageAreaShape.Circle;

        /// <summary>
        /// This is overridden to specify the HTML shape name
        /// </summary>
        /// <value>It always returns "<c>circ</c>"</value>.
        /// <seealso cref="Shape"/>
        public override string ShapeText => "circ";

        /// <summary>
        /// This is overridden to get or set the coordinate values in string form
        /// </summary>
        /// <remarks>The coordinates are expected to be numeric and in the order X, Y, and radius with each value
        /// separated by commas.</remarks>
        /// <exception cref="ArgumentException">This is thrown if the number of coordinates given is not exactly
        /// equal to three or if non-numeric values are found.</exception>
        /// <seealso cref="CenterPoint"/>
        /// <seealso cref="Radius"/>
        [DefaultValue("0, 0, 0"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Description("The circle area's center point and radius")]
        public override string Coordinates
        {
            get => String.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", this.CenterPoint.X,
                    this.CenterPoint.Y, this.Radius);
            set
            {
                if(value == null || value.Length == 0)
                {
                    this.CenterPoint = new Point(0, 0);
                    this.Radius = 0;
                    return;
                }

                string[] strCoords = value.Split([','], StringSplitOptions.RemoveEmptyEntries);

                if(strCoords.Length != 3)
                    throw new ArgumentException("There must be exactly two coordinates and a radius value");

                try
                {
                    int x1 = Convert.ToInt32(strCoords[0], CultureInfo.InvariantCulture),
                        y1 = Convert.ToInt32(strCoords[1], CultureInfo.InvariantCulture),
                        r = Convert.ToInt32(strCoords[2], CultureInfo.InvariantCulture);

                    this.CenterPoint = new Point(x1, y1);
                    this.Radius = r;
                }
                catch(FormatException ex)
                {
                    throw new ArgumentException("The coordinates and radius must be numeric", ex);
                }
            }
        }

        /// <summary>
        /// This is used to get or set the circle's center point
        /// </summary>
        /// <seealso cref="Coordinates"/>
        /// <seealso cref="Radius"/>
        [Category("Appearance"), DefaultValue(typeof(Point), "0, 0"), Bindable(true),
          RefreshProperties(RefreshProperties.Repaint), Description("The center point for the area on the image map")]
        public Point CenterPoint
        {
            get => (Point)this.ViewState["CenterPoint"];
            set => this.ViewState["CenterPoint"] = value;
        }

        /// <summary>
        /// This is used to get or set the circle's radius
        /// </summary>
        /// <seealso cref="Coordinates"/>
        /// <seealso cref="CenterPoint"/>
        [Category("Appearance"), DefaultValue(0), Bindable(true), RefreshProperties(RefreshProperties.Repaint),
          Description("The radius of the area on the image map")]
        public int Radius
        {
            get => (int)this.ViewState["Radius"];
            set => this.ViewState["Radius"] = value;
        }
        #endregion

        #region Constructors
        //=====================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <overloads>There are four overloads for the constructor</overloads>
        public ImageAreaCircle()
        {
            this.CenterPoint = new Point(0, 0);
            this.Radius = 0;
        }

        /// <summary>
        /// Constructor.  This version takes the center point and radius.
        /// </summary>
        /// <param name="center">The center point</param>
        /// <param name="radius">The radius of the circle</param>
        public ImageAreaCircle(Point center, int radius) : this(center, radius, null, null)
        {
        }

        /// <summary>
        /// Constructor.  This version takes the center point, radius, and the URL to which to navigate when
        /// clicked.
        /// </summary>
        /// <param name="center">The center point</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="url">The URL to which to navigate when clicked</param>
        public ImageAreaCircle(Point center, int radius, string? url) : this(center, radius, url, null)
        {
        }

        /// <summary>
        /// Constructor.  This version takes the center point, radius, the URL to which to navigate when clicked,
        /// and a tool tip.
        /// </summary>
        /// <param name="center">The center point</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="url">The URL to which to navigate when clicked</param>
        /// <param name="toolTip">The tool tip to show when the mouse
        /// hovers over the area</param>
        public ImageAreaCircle(Point center, int radius, string? url, string? toolTip) : base(url, toolTip)
        {
            this.CenterPoint = center;
            this.Radius = radius;
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
            UnsafeNativeMethods.DrawReversibleCircle(g, this.CenterPoint, this.Radius, offset);
        }
        #endregion
    }
}
