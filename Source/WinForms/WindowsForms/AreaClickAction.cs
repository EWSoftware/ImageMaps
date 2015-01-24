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
// 07/09/2004  EFW  Created the code
//===============================================================================================================

using System;

namespace EWSoftware.ImageMaps.Windows.Forms
{
    /// <summary>
    /// This enumerated type defines the action to take when an area is clicked in the Windows Forms
    /// <see cref="ImageMap"/> control.
    /// </summary>
    [Serializable]
    public enum AreaClickAction
    {
        /// <summary>Fire the image map <see cref="ImageMap.Click"/> event and the image area
        /// <see cref="ImageAreaBase.Click"/> event.</summary>
        FireEvent,
        /// <summary>Displays a <see cref="ImageAreaBase.ToolTip"/> if one is specified but performs no
        /// action.</summary>
        None
    }
}
