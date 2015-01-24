//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaShape.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/08/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the enumerated type that defines the image area shapes
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

namespace EWSoftware.ImageMaps
{
    /// <summary>
    /// This enumerated type defines the various image area shapes
    /// </summary>
    [Serializable]
    public enum ImageAreaShape
    {
        /// <summary>A rectangle (web and Windows Forms)</summary>
        Rectangle,
        /// <summary>A circle (web and Windows Forms)</summary>
        Circle,
        /// <summary>A polygon (web and Windows Forms)</summary>
        Polygon,
        /// <summary>An ellipse (Windows Forms only)</summary>
        Ellipse
    }
}
