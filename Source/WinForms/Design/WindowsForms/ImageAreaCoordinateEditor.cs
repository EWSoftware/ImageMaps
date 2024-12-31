//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaCoordinateEditor.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 12/31/2024
// Note    : Copyright 2004-2024, Eric Woodruff, All rights reserved
//
// This file contains the image area coordinate editor
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

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace EWSoftware.ImageMaps.Design.Windows.Forms
{
    /// <summary>
    /// This provides design time support for classes derived from <see cref="IImageArea"/> to let the user set
    /// the area coordinates on the image interactively.
    /// </summary>
    internal class ImageAreaCoordinateEditor : UITypeEditor
    {
        /// <summary>
        /// This is overridden to specify that the editor uses a modal dialog box for editing the collection
        /// </summary>
        /// <param name="context">The descriptor context</param>
        /// <returns>Always returns Modal</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// This is overridden to display the editor dialog box and allows
        /// the user to edit the image map area coordinates in a more
        /// user-friendly fashion.
        /// </summary>
        /// <param name="context">The descriptor context</param>
        /// <param name="provider">The service provider</param>
        /// <param name="value">The image area coordinates</param>
        /// <returns>The edited image area coordinates</returns>
        /// <exception cref="ArgumentException">This is thrown if the area collection is not owned by an image
        /// map control.</exception>
        public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
        {
            // Get the forms editor service from the provider to display the form
            var srv = (IWindowsFormsEditorService?)provider.GetService(typeof(IWindowsFormsEditorService));

            if(srv != null)
            {
                IImageMap im = ImageAreaCollectionEditor.Areas?.ImageMapControl ??
                    throw new ArgumentException("The area collection is not owned by an ImageMap");

                using ImageMapAreaEditorDlg dlg = new()
                {
                    Image = ((ImageMaps.Windows.Forms.ImageMap)im).Image,
                    ImageHeight = im.ImageMapHeight,
                    ImageWidth = im.ImageMapWidth,
                    Coordinates = (string?)value ?? String.Empty
                };

                if(srv.ShowDialog(dlg) == DialogResult.OK)
                    return dlg.Coordinates;
            }

            return value;
        }
    }
}
