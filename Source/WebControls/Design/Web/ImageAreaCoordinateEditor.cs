//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaCoordinateEditor.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/09/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the image area coordinate editor class
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
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace EWSoftware.ImageMaps.Design.Web
{
    /// <summary>
    /// This provides design time support for classes derived from <see cref="IImageArea"/> to let the user set
    /// the area coordinates on the image interactively.
    /// </summary>
    [PermissionSet(SecurityAction.LinkDemand, Unrestricted = true),
      PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
    internal sealed class ImageAreaCoordinateEditor : UITypeEditor
    {
        #region Private data members
        //=====================================================================

        // This is used to store the image file path
        private static string lastFilePath;

        #endregion

        #region Method overrides
        //=====================================================================

        /// <summary>
        /// This is overridden to specify that the editor uses a modal dialog box for editing the collection
        /// </summary>
        /// <param name="context">The descriptor context</param>
        /// <returns>Always returns Modal</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

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

                    dlg.ImageFilename = LocateImageFile(((EWSoftware.ImageMaps.Web.Controls.ImageMap)im).ImageUrl);
                    dlg.ImageHeight = im.ImageMapHeight;
                    dlg.ImageWidth = im.ImageMapWidth;
                    dlg.Coordinates = (string)value;

                    if(srv.ShowDialog(dlg) == DialogResult.OK)
                        return dlg.Coordinates;
                }
            }

            return value;
        }
        #endregion

        #region Helper methods
        //=====================================================================

        /// <summary>
        /// This is used to locate the image file path
        /// </summary>
        /// <param name="imagePath">The image file path to locate</param>
        /// <remarks>If the path is relative, we won't be able to find it.  If so, get the user to locate it for
        /// us.  This probably could be done in a more automated fashion to avoid user involvement, but I wanted
        /// to keep it simple for the time being.  The path will be remembered and checked on subsequent uses of
        /// the editor.</remarks>
        private static string LocateImageFile(string imagePath)
        {
            string filename;

            // If it's relative to the current folder or fully qualified, we will find it
            if(File.Exists(imagePath))
                return imagePath;

            filename = Path.GetFileName(imagePath);

            // What about the current folder?  Only check it if the current folder ends with the image name's
            // folder.
            if(lastFilePath != null && lastFilePath.EndsWith(Path.GetDirectoryName(Environment.CurrentDirectory),
              StringComparison.OrdinalIgnoreCase) && File.Exists(filename))
                return filename;

            // What about the last selected folder?  Only check it if the last folder ends with the image name's
            // folder.
            if(lastFilePath != null && Path.GetDirectoryName(imagePath) != null &&
               lastFilePath.EndsWith(Path.GetDirectoryName(imagePath), StringComparison.OrdinalIgnoreCase))
            {
                filename = lastFilePath + @"\" + filename;

                if(File.Exists(filename))
                    return filename;
            }

            // No luck, ask the user
            using(OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Please locate the image file";
                dlg.InitialDirectory = Environment.CurrentDirectory;
                dlg.CheckFileExists = true;
                dlg.FileName = Path.GetFileName(filename);

                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    filename = dlg.FileName;

                    // Remember the path for later
                    lastFilePath = Path.GetDirectoryName(Path.GetFullPath(filename));
                }
                else
                    filename = null;
            }

            return filename;
        }
        #endregion
    }
}
