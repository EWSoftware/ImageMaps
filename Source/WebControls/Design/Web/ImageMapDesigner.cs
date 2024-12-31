//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageMapDesigner.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 12/31/2024
// Note    : Copyright 2004-2024, Eric Woodruff, All rights reserved
//
// This file contains the image map designer class
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
using System.Globalization;
using System.IO;
using System.Web.UI;

namespace EWSoftware.ImageMaps.Design.Web
{
	/// <summary>
	/// This provides design time support for the web server
    /// <see cref="ImageMaps.Web.Controls.ImageMap"/> control.
	/// </summary>
    internal sealed class ImageMapDesigner : System.Web.UI.Design.ControlDesigner
	{
		/// <summary>
		/// This returns the design time HTML for the <see cref="ImageMaps.Web.Controls.ImageMap"/>
        /// control.
		/// </summary>
		/// <returns>The design time HTML</returns>
        public override string GetDesignTimeHtml()
		{
            ImageMaps.Web.Controls.ImageMap im = (ImageMaps.Web.Controls.ImageMap)this.Component;
            bool isVisible = im.Visible;

			try
			{
                using StringWriter tw = new(CultureInfo.InvariantCulture);
                using HtmlTextWriter writer = new(tw);
                
                if(!isVisible)
                    im.Visible = true;

                im.RenderControl(writer);

                return tw.ToString();
            }
			catch(Exception ex)
			{
				return GetErrorDesignTimeHtml(ex);
			}
            finally
            {
                im.Visible = isVisible;
            }
		}

		/// <summary>
		/// Render a place holder describing an error that occurred while creating the design time HTML
		/// </summary>
		/// <param name="e">The exception that occurred</param>
		/// <returns>A string describing the error</returns>
        protected override string GetErrorDesignTimeHtml(Exception e)
		{
            return CreatePlaceHolderDesignTimeHtml(String.Format(CultureInfo.InvariantCulture,
                "There was an error and the control cannot be displayed.<br>Exception: {0}<br>{1}", e.Message,
                e.StackTrace));
		}
	}
}
