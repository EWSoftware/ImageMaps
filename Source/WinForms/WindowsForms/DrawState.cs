//===============================================================================================================
// System  : Image Map Control Library
// File    : DrawState.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/09/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the draw state enumerated type
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 06/28/2006  EFW  Created the code
//===============================================================================================================

using System;

namespace EWSoftware.ImageMaps.Windows.Forms
{
    /// <summary>
    /// This enumerated type defines the drawing states for the image map and the image areas when their
    /// <c>OwnerDraw</c> property is set to true.
    /// </summary>
    /// <seealso cref="ImageMap.OwnerDraw">ImageMap.OwnerDraw</seealso>
    /// <seealso cref="ImageAreaBase.OwnerDraw">ImageAreaBase.OwnerDraw</seealso>
    [Serializable]
    public enum DrawState
    {
        /// <summary>The item currently has no special state</summary>
        Normal,
        /// <summary>The item is disabled</summary>
        Disabled,
        /// <summary>The item has the focus</summary>
        Focus,
        /// <summary>The item is being hot-tracked (highlighted as the mouse pointer passes over it)</summary>
        HotLight
    }
}
