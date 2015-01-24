//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaEvent.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/09/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the image area event enumerated type
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
    /// This enumerated type defines the events that can be raised by an image area
    /// </summary>
    [Serializable]
    public enum ImageAreaEvent
    {
        /// <summary>Fire the <see cref="ImageAreaBase.Click"/> event</summary>
        Click,
        /// <summary>Fire the <see cref="ImageAreaBase.DoubleClick"/> event</summary>
        DoubleClick,
        /// <summary>Fire the <see cref="ImageAreaBase.MouseDown"/> event</summary>
        MouseDown,
        /// <summary>Fire the <see cref="ImageAreaBase.MouseUp"/> event</summary>
        MouseUp,
        /// <summary>Fire the <see cref="ImageAreaBase.MouseEnter"/> event</summary>
        MouseEnter,
        /// <summary>Fire the <see cref="ImageAreaBase.MouseLeave"/> event</summary>
        MouseLeave,
        /// <summary>Fire the <see cref="ImageAreaBase.MouseHover"/> event</summary>
        MouseHover,
        /// <summary>Fire the <see cref="ImageAreaBase.MouseMove"/> event</summary>
        MouseMove,
        /// <summary>Fire the <see cref="ImageAreaBase.Enter"/> event</summary>
        Enter,
        /// <summary>Fire the <see cref="ImageAreaBase.Leave"/> event</summary>
        Leave,
        /// <summary>Fire the <see cref="ImageAreaBase.DrawImage"/> event</summary>
        DrawImage
    }
}
