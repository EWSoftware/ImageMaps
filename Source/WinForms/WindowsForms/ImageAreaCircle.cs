//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaCircle.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/09/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
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
// 07/09/2004  EFW  Created the code
//===============================================================================================================

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

using EWSoftware.ImageMaps.Design.Windows.Forms;

namespace EWSoftware.ImageMaps.Windows.Forms
{
    /// <summary>
    /// This is the circle image area class
    /// </summary>
    /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex3']/*" />
    [TypeConverter(typeof(ImageAreaTypeConverter))]
    public class ImageAreaCircle : ImageAreaBase
    {
        #region Private data members
        //=====================================================================

        private Point centerPoint;
        private int radius;

        #endregion

        #region Properties
        //=====================================================================

        /// <summary>
        /// This is overridden to provide the shape type
        /// </summary>
        /// <value>It always returns <see cref="ImageAreaShape.Circle"/></value>.
        public override ImageAreaShape Shape
        {
            get { return ImageAreaShape.Circle; }
        }

        /// <summary>
        /// This is overridden to get or set the coordinate values in string form
        /// </summary>
        /// <remarks>The coordinates are expected to be numeric and in the order X, Y, and radius with each value
        /// separated by commas.</remarks>
        /// <exception cref="ArgumentException">This is thrown if the number of coordinates given is not exactly
        /// equal to three or if non-numeric values are found.</exception>
        /// <seealso cref="CenterPoint"/>
        /// <seealso cref="Radius"/>
        [DefaultValue("0, 0, 0"), Description("The circle area's center point and radius")]
        public override string Coordinates
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", this.CenterPoint.X,
                    this.CenterPoint.Y, this.Radius);
            }
            set
            {
                if(value == null || value.Length == 0)
                {
                    if(centerPoint.X != 0 || centerPoint.Y != 0 || radius != 0)
                    {
                        centerPoint = new Point(0, 0);
                        radius = 0;
                        OnImageAreaChanged(EventArgs.Empty);
                    }

                    return;
                }

                string[] coordinates = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if(coordinates.Length != 3)
                    throw new ArgumentException("There must be exactly two coordinates and a radius value");

                try
                {
                    int x1 = Convert.ToInt32(coordinates[0], CultureInfo.InvariantCulture),
                        y1 = Convert.ToInt32(coordinates[1], CultureInfo.InvariantCulture),
                        r = Convert.ToInt32(coordinates[2], CultureInfo.InvariantCulture);

                    if(centerPoint.X != x1 || centerPoint.Y != y1 || radius != r)
                    {
                        centerPoint = new Point(x1, y1);
                        radius = r;
                        OnImageAreaChanged(EventArgs.Empty);
                    }
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
            get { return centerPoint; }
            set
            {
                if(centerPoint != value)
                {
                    centerPoint = value;
                    OnImageAreaChanged(EventArgs.Empty);
                }
            }
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
            get { return radius; }
            set
            {
                if(radius != value)
                {
                    radius = value;
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
        public ImageAreaCircle()
        {
            centerPoint = new Point(0, 0);
        }

        /// <summary>
        /// Constructor.  This version takes the center point and radius.
        /// </summary>
        /// <param name="centerPoint">The center point</param>
        /// <param name="radius">The radius of the circle</param>
        public ImageAreaCircle(Point centerPoint, int radius) : this(centerPoint, radius, null)
        {
        }

        /// <summary>
        /// Constructor.  This version takes the center point, radius, and a tool tip.
        /// </summary>
        /// <param name="centerPoint">The center point</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="toolTip">The tool tip to show when the mouse hovers over the area</param>
        public ImageAreaCircle(Point centerPoint, int radius, string toolTip) : base(toolTip)
        {
            this.centerPoint = centerPoint;
            this.radius = radius;
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
            UnsafeNativeMethods.DrawReversibleCircle(g, centerPoint, radius, offset);
        }
        #endregion
    }
}
