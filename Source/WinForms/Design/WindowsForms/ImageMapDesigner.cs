//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageMapDesigner.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/08/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the image map designer
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/01/2004  EFW  Created the code
// 12/22/2004  EFW  Added support for new ellipse image area
// 06/28/2006  EFW  Reworked code for use with .NET 2.0
//===============================================================================================================

namespace EWSoftware.ImageMaps.Design.Windows.Forms
{
    /// <summary>
    /// This provides design time support for the Windows Forms
    /// <see cref="EWSoftware.ImageMaps.Windows.Forms.ImageMap"/> control.
    /// </summary>
    internal sealed class ImageMapDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        /// <summary>
        /// Gets the collection of components associated with the component managed by the designer
        /// </summary>
        public override System.Collections.ICollection AssociatedComponents
        {
            get
            {
                EWSoftware.ImageMaps.Windows.Forms.ImageMap im =
                    (this.Control as EWSoftware.ImageMaps.Windows.Forms.ImageMap);

                if(im != null)
                    return im.Areas;

                return base.AssociatedComponents;
            }
        }
    }
}
