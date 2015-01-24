//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageMapClickEvent.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/09/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the delegate definitions and event arguments classes for the image map click events
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/07/2004  EFW  Created the code
// 12/27/2004  EFW  Renamed file and added owner draw event stuff
// 06/28/2006  EFW  Reworked code for use with .NET 2.0
//===============================================================================================================

using System;

namespace EWSoftware.ImageMaps
{
    /// <summary>
    /// This is a custom event arguments class for the <see cref="IImageMap.Click"/> event
    /// </summary>
    /// <remarks>The event contains the index of the area that was clicked along with the X and Y coordinates of
    /// the click.  The coordinates are only valid in Windows Forms image maps when they are clicked on with the
    /// mouse and for web server control image maps when clicked on with the mouse and rendered in browsers that
    /// support the <c>event.offsetX</c> and <c>event.offsetY</c> properties such as Internet Explorer.  For
    /// browsers that do not support those properties, the coordinates will always be negative one (-1).  If an
    /// area's click event is fired by selecting it with the Enter key, the coordinates may not be within the
    /// bounds of the area in the web server control.  For image areas selected via the keyboard in the Windows
    /// Forms control, the coordinates will always be negative one (-1).</remarks>
    public class ImageMapClickEventArgs : EventArgs
    {
        #region Properties
        //=====================================================================

	    /// <summary>
        /// This read-only property is used to get the zero-based index of the area that was clicked
	    /// </summary>
        /// <remarks>Use this as an index into the <see cref="IImageMap.Areas"/> collection on the image map
        /// control.</remarks>
    	public int AreaIndex { get; private set; }

	    /// <summary>
        /// This read-only property is used to get the X coordinate of the click
	    /// </summary>
        /// <value>The X coordinate is only valid in Windows Forms image maps when they are clicked on with the
        /// mouse and for web server control image maps when clicked on with the mouse and rendered in browsers
        /// that support the <c>event.offsetX</c> property such as Internet Explorer.  For browsers that do not
        /// support the property, the coordinate will always be negative one (-1).  If an area's click event is
        /// fired by selecting it with the Enter key, the coordinate may not be within the bounds of the area in
        /// the web server control.  For image areas selected via the keyboard in the Windows Forms control, the
        /// coordinate will always be negative one (-1).</value>
    	public int XCoordinate { get; private set; }

	    /// <summary>
        /// This read-only property is used to get the Y coordinate of the click
	    /// </summary>
        /// <value>The Y coordinate is only valid in Windows Forms image maps when they are clicked on with the
        /// mouse and for web server control image maps when clicked on with the mouse and rendered in browsers
        /// that support the <c>event.offsetY</c> property such as Internet Explorer.  For browsers that do not
        /// support the property, the coordinate will always be negative one (-1).  If an area's click event is
        /// fired by selecting it with the Enter key, the coordinate may not be within the bounds of the area in
        /// the web server control.  For image areas selected via the keyboard in the Windows Forms control, the
        /// coordinate will always be negative one (-1).</value>
    	public int YCoordinate { get; private set; }

        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="areaIndex">The index of the clicked area</param>
        /// <param name="xCoordinate">The X coordinate of the click</param>
        /// <param name="yCoordinate">The Y coordinate of the click</param>
        public ImageMapClickEventArgs(int areaIndex, int xCoordinate, int yCoordinate)
        {
            this.AreaIndex = areaIndex;
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
        }
        #endregion
    }
}
