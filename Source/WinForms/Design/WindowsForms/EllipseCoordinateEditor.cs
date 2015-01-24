//===============================================================================================================
// System  : Image Map Control Library
// File    : EllipseCoordinateEditor.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/08/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the ellipse coordinate editor
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 12/22/2004  EFW  Created the code
// 06/28/2006  EFW  Reworked code for use with .NET 2.0
//===============================================================================================================

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace EWSoftware.ImageMaps.Design.Windows.Forms
{
    /// <summary>
    /// This provides design time support for the Windows Forms control's
    /// <see cref="EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse"/> to let the user set the area
    /// coordinates on the image interactively.
    /// </summary>
    /// <remarks>The designer only gets a set of coordinates and can't tell the difference between a rectangle
    /// and an ellipse because they both use a rectangle to define the area.  This sets the shape so that the
    /// designer knows it's working with an ellipse.</remarks>
    internal sealed class EllipseCoordinateEditor : ImageAreaCoordinateEditor
    {
        /// <summary>
        /// This is overridden to display the editor dialog box and allows the user to edit the image map area
        /// coordinates in a more user-friendly fashion.
        /// </summary>
        /// <param name="context">The descriptor context</param>
        /// <param name="provider">The service provider</param>
        /// <param name="value">The image area coordinates</param>
        /// <returns>The edited image area coordinates</returns>
        /// <exception cref="ArgumentException">This is thrown if the area collection is not owned by an image
        /// map control.</exception>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService srv = null;

            // Get the forms editor service from the provider to display the form
            if(provider != null)
                srv = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if(srv != null)
            {
                using(ImageMapAreaEditorDlg dlg = new ImageMapAreaEditorDlg())
                {
                    IImageMap im = ImageAreaCollectionEditor.Areas.ImageMapControl;

                    if(im == null)
                        throw new ArgumentException("The area collection is not owned by an ImageMap");

                    dlg.Image = ((EWSoftware.ImageMaps.Windows.Forms.ImageMap)im).Image;
                    dlg.ImageHeight = im.ImageMapHeight;
                    dlg.ImageWidth = im.ImageMapWidth;
                    dlg.Coordinates = (string)value;

                    // The Coordinates property infers the shape from the number of coordinates.  As such, set
                    // the shape after setting the coordinates.
                    dlg.Shape = ImageAreaShape.Ellipse;

                    if(srv.ShowDialog(dlg) == DialogResult.OK)
                        return dlg.Coordinates;
                }
            }

            return value;
        }
    }
}
