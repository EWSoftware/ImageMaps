//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageMapPropertyForm.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/03/2023
// Note    : Copyright 2004-2023, Eric Woodruff, All rights reserved
//
// This form is used to demonstrate the Image Map Windows Forms controls.  This form allows the user to modify
// the image map and image area properties interactively and see the effects.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/01/2004  EFW  Created the code
// 07/08/2006  EFW  Updated for use with .NET 2.0
//===============================================================================================================

using System;
using System.Globalization;
using System.Windows.Forms;

using EWSoftware.ImageMaps;

namespace ImageMapWinForms
{
	/// <summary>
    /// This form is used to demonstrate the Image Map Windows Forms controls.  This form allows the user to
    /// modify the image map and image area properties interactively and see the effects.
	/// </summary>
	public partial class ImageMapPropertyForm : Form
	{
        #region Constructor
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
		public ImageMapPropertyForm()
		{
			InitializeComponent();

			pgImageMapProps.SelectedObject = imMap;
			pgImageMapProps.Refresh();
		}
        #endregion

        #region Event handlers
        //=====================================================================

        /// <summary>
        /// Refresh the display when properties are changed
        /// </summary>
        /// <param name="s">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void pgImageMapProps_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            imMap.Invalidate();
            imMap.Update();
        }

        /// <summary>
        /// A simple image area click handler
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void imMap_Click(object sender, ImageMapClickEventArgs e)
        {
            MessageBox.Show(String.Format(CultureInfo.CurrentCulture, "You clicked area #{0} ({1}) at " +
                "point {2}, {3}", e.AreaIndex + 1, imMap.Areas[e.AreaIndex].ToolTip, e.XCoordinate, e.YCoordinate),
                "Image Map Clicked", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Show the Image Map control help file if it can be found
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://ewsoftware.github.io/ImageMaps",
                    UseShellExecute = true
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to start web browser for URL", "URL Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                // Log the exception to the debugger for the developer
                System.Diagnostics.Debug.Write(ex.ToString());
            }
        }

        /// <summary>
        /// Close this form
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
