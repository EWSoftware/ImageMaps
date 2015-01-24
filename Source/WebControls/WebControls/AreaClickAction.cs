//===============================================================================================================
// System  : Image Map Control Library
// File    : AreaClickAction.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/09/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the area click action enumerated type
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
    /// This enumerated type defines the action to take when an area is clicked in the web server
    /// <see cref="ImageMap"/> control.
    /// </summary>
    [Serializable]
    public enum AreaClickAction
    {
        /// <summary>Navigate to the URL specified in the <see cref="ImageAreaBase.NavigateUrl"/> property
        /// (default).  This action can also be used to execute client-side script by specifying the script to
        /// run in the <c>NavigateUrl</c> property.
        /// </summary>
        Navigate,
        /// <summary>Post back and fire the server-side image map <see cref="ImageMap.Click"/> event.</summary>
        PostBack,
        /// <summary>Displays a <see cref="ImageAreaBase.ToolTip"/> if one is specified but performs no action.</summary>
        None
    }
}
