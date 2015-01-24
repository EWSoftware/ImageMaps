//===============================================================================================================
// System  : Image Map Control Library
// File    : IImageMap.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/08/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains an interface containing common elements for the Windows Forms and the web server control
// image map classes.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/07/2004  EFW  Created the code
// 07/08/2006  EFW  Updated for use with .NET 2.0
//===============================================================================================================

using System;

namespace EWSoftware.ImageMaps
{
    /// <summary>
    /// This interface defines events, properties, and methods common to both the Windows Forms and the web
    /// server control image map classes.
    /// </summary>
    public interface IImageMap
    {
        #region Properties
        //=====================================================================

        /// <summary>
        /// Gets or sets the tool tip text displayed for all undefined areas of the image
        /// </summary>
        string ToolTip { get; set; }

        /// <summary>
		/// Gets or sets the width of the image in the image map control in pixels
        /// </summary>
        int ImageMapWidth { get; set; }

        /// <summary>
		/// Gets or sets the height of the image in the image map control in pixels
        /// </summary>
        int ImageMapHeight { get; set; }

        /// <summary>
        /// This returns the collection of areas defined for the image map
        /// </summary>
        ImageAreaCollection Areas { get; }

        #endregion

        #region Events
        //=====================================================================

        /// <summary>
        /// This event is raised when a hot spot on the image map is clicked
        /// </summary>
        event EventHandler<ImageMapClickEventArgs> Click;

        #endregion
    }
}
