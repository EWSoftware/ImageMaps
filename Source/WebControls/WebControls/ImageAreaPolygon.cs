//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaPolygon.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 12/31/2024
// Note    : Copyright 2004-2024, Eric Woodruff, All rights reserved
//
// This file contains the polygon image area class
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

// Ignore Spelling: poly

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace EWSoftware.ImageMaps.Web.Controls
{
    /// <summary>
	/// This is the polygon image area class
	/// </summary>
    /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex1']/*" />
	public class ImageAreaPolygon : ImageAreaBase
	{
        #region Properties
        //=====================================================================

        /// <summary>
        /// This is overridden to provide the shape type
        /// </summary>
        /// <value>It always returns <see cref="ImageAreaShape.Polygon"/></value>.
        public override ImageAreaShape Shape => ImageAreaShape.Polygon;

        /// <summary>
        /// This is overridden to specify the HTML shape name
        /// </summary>
        /// <value>It always returns "<c>poly</c>"</value>.
        /// <seealso cref="Shape"/>
        public override string ShapeText => "poly";

        /// <summary>
        /// This is overridden to get or set the coordinate values in string form
        /// </summary>
        /// <remarks>The coordinates are expected to be numeric and in X,Y pairs with each value separated by
        /// commas.  There must be at least three pairs of coordinates to form the polygon.</remarks>
        /// <exception cref="ArgumentException">This is thrown if the number of coordinates given is not exactly
        /// a multiple of two, there are less than three pairs of coordinates, or if non-numeric values are
        /// found.</exception>
        /// <seealso cref="Points"/>
        [DefaultValue(""), Description("The polygon area's coordinates")]
        public override string Coordinates
        {
            get
            {
                PointCollection pts = this.Points;
                StringBuilder sb = new(256);

                foreach(Point p in pts)
                {
                    if(sb.Length > 0)
                        sb.Append(", ");

                    sb.AppendFormat("{0}, {1}", p.X, p.Y);
                }

                return sb.ToString();
            }
            set
            {
                PointCollection pts = this.Points;
                pts.Clear();

                if(value == null || value.Length == 0)
                    return;

                string[] coordinates = value.Split([','], StringSplitOptions.RemoveEmptyEntries);

                if(coordinates.Length < 6)
                    throw new ArgumentException("There must be at least three pairs of polygon coordinates");

                if((coordinates.Length % 2) != 0)
                    throw new ArgumentException("The number of polygon coordinates must be a multiple of two");

                try
                {
                    for(int idx = 0; idx < coordinates.Length; idx += 2)
                        pts.Add(new Point(Convert.ToInt32(coordinates[idx], CultureInfo.InvariantCulture),
                            Convert.ToInt32(coordinates[idx + 1], CultureInfo.InvariantCulture)));
                }
                catch(FormatException ex)
                {
                    throw new ArgumentException("The polygon coordinates must be numeric", ex);
                }
            }
        }

        /// <summary>
        /// This is used to access the polygon point collection
        /// </summary>
        /// <seealso cref="Coordinates"/>
		[Category("Appearance"), Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Description("The polygon points for the image map area")]
        public PointCollection Points
        {
            get
            {
                if(this.ViewState["Points"] == null)
                    this.ViewState["Points"] = new PointCollection();

                return (PointCollection)this.ViewState["Points"];
            }
        }
        #endregion

        #region Private designer methods
        //=====================================================================

        // These are used because the default values for these properties don't work with the DefaultValue
        // attribute.

        /// <summary>
        /// The designer uses this to determine whether or not to serialize changes to the Points property
        /// </summary>
        /// <returns>True if it should serialize the property, false if not</returns>
        private bool ShouldSerializePoints()
        {
            return this.Points.Count > 0;
        }

        /// <summary>
        /// Reset the points collection
        /// </summary>
        private void ResetPoints()
        {
            this.Points.Clear();
        }
        #endregion

        #region Constructors
        //=====================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <overloads>There are three overloads for the constructor</overloads>
        public ImageAreaPolygon()
        {
        }

        /// <summary>
        /// Constructor.  This version takes the URL to which to navigate when clicked.
        /// </summary>
        /// <param name="url">The URL to which to navigate when clicked</param>
		public ImageAreaPolygon(string? url) : this(url, null)
		{
		}

        /// <summary>
        /// Constructor.  This version takes the URL to which to navigate when clicked and a tool tip.
        /// </summary>
        /// <param name="url">The URL to which to navigate when clicked</param>
        /// <param name="toolTip">The tool tip to show when the mouse hovers over the area</param>
		public ImageAreaPolygon(string? url, string? toolTip) : base(url, toolTip)
		{
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
            // Note that the polygon is closed
            UnsafeNativeMethods.DrawReversiblePolygon(g, this.Points, true, offset);
        }
        #endregion
    }
}
