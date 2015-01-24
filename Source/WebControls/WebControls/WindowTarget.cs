//===============================================================================================================
// System  : Image Map Control Library
// File    : WindowTarget.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/09/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the window target enumerated type
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

using System;

namespace EWSoftware.ImageMaps.Web.Controls
{
    /// <summary>
    /// This enumerated type defines the locations in which a browser window can be opened by the areas in a web
    /// server <see cref="ImageMap"/> control when clicked and their <see cref="ImageAreaBase.Action"/>
    /// property is set to <c>Navigate</c>.
    /// </summary>
    [Serializable]
    public enum WindowTarget
    {
        /// <summary>The URL is loaded into a new, unnamed window.</summary>
        Blank,
        /// <summary>The URL is loaded into the HTML content area of the Media Bar.  Available in Internet
        /// Explorer 6 or later.</summary>
        Media,
        /// <summary>The URL is loaded into the current frame's parent.  If the frame has no parent, this value
        /// acts like the value <c>Self</c>.</summary>
        Parent,
        /// <summary>Available in Internet Explorer 5 and later. The URL is opened in the browser's search
        /// pane.</summary>
        Search,
        /// <summary>The current document is replaced with the specified URL.</summary>
        Self,
        /// <summary>The URL replaces any frame sets that may be loaded.  If there are no frame sets defined,
        /// this value acts like the value <c>Self</c>.</summary>
        Top,
        /// <summary>The URL is loaded into another frame or window named by the
        /// <see cref="ImageAreaBase.TargetName" /> property.  If a window or frame already exists with the given
        /// name, the URL is loaded into the existing window or frame.</summary>
        Other
    }
}
