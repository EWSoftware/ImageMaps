//===============================================================================================================
// System  : Image Map Control Library
// File    : IImageArea.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 12/31/2024
// Note    : Copyright 2004-2024, Eric Woodruff, All rights reserved
//
// This file contains an interface containing common elements for the Windows Forms and the web server control
// image map area classes.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/07/2004  EFW  Created the code
// 12/22/2004  EFW  Added new ellipse image area type
//===============================================================================================================

using System;
using System.Drawing;

namespace EWSoftware.ImageMaps
{
    /// <summary>
	/// This is the common interface for both the Windows Forms and the web server control image area types
	/// </summary>
    public interface IImageArea
	{
        #region Properties
        //=====================================================================

        /// <summary>
        /// This read-only property is used to get the shape type
        /// </summary>
        ImageAreaShape Shape { get; }

        /// <summary>
        /// This is used to get or set the coordinate values in string form
        /// </summary>
        /// <remarks>This is used when rendering the area to HTML.  It is also used in the designer to use the
        /// graphical coordinate selection dialog.</remarks>
        string Coordinates { get; set; }

        /// <summary>
        /// This is used to get or set the access key (a shortcut key or mnemonic) for the image area
        /// </summary>
        /// <value>The value should be a single alphanumeric character</value>
        string? AccessKey { get; set; }

        /// <summary>
        /// This is used to get or set the enabled state of the image area
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// This is used to get or set the tab index of the image area
        /// </summary>
        int TabIndex { get; set; }

        /// <summary>
        /// This is used to get or set the tool tip for the image area
        /// </summary>
        string? ToolTip { get; set; }

        /// <summary>
        /// This is used to get or set an object that contains additional user-defined data for the image area
        /// </summary>
        object? Tag { get; set; }

        #endregion

        #region Events
        //=====================================================================

        /// <summary>
        /// This event is raised when an image area property changes that affects its visual presentation in the
        /// image map control such as its position or enabled state.
        /// </summary>
        event EventHandler ImageAreaChanged;

        #endregion

        #region Methods
        //=====================================================================

        /// <summary>
        /// This is used to get the shape to draw a focus frame around itself
        /// </summary>
        /// <param name="g">The graphics object to use for drawing.</param>
        /// <param name="offset">A point used to offset the image area coordinates (i.e. if the image is
        /// scrolled).</param>
        /// <remarks>This is used mainly by the Windows Form image area controls.  However, the web image area
        /// control makes use of it in the coordinate editor.</remarks>
        void DrawFocusFrame(Graphics g, Point offset);

        #endregion
    }
}
