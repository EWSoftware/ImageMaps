//===============================================================================================================
// System  : Image Map Control Library
// File    : Default.aspx.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/10/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft C#
//
// This page is used to demonstrate the Image Map Web Server controls.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
//===============================================================================================================
// 07/01/2004  EFW  Created the code
//===============================================================================================================

using System;
using System.Drawing;
using System.Globalization;
using System.Web.UI;

using EWSoftware.ImageMaps;
using EWSoftware.ImageMaps.Web.Controls;

namespace ImageMapWebCS
{
	/// <summary>
	/// A demonstration of the EWSoftware ImageMap web server control.
	/// </summary>
	public partial class DefaultPage : System.Web.UI.Page
	{
        /// <summary>
        /// This is used to add a dynamic image area on the initial page load
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
		protected void Page_Load(object sender, EventArgs e)
		{
            if(!Page.IsPostBack)
            {
                lblEnabledMsg.Text = "Image map enabled";

                // Add a dynamic image area.  This is only created once when the page is first loaded.  On post
                // backs, the area is recreated from view state.
                ImageAreaRectangle r = new ImageAreaRectangle(new Rectangle(40, 57, 20, 20));

                r.Action = AreaClickAction.PostBack;
                r.ToolTip = "Area 5 (Dynamically added)";
                r.AccessKey = "5";
                r.Attributes.Add("onmouseover", "javascript: IM_Highlight(5);");
                r.Attributes.Add("onmouseout", "javascript: IM_Highlight(0);");

                // Tell the item to mark all properties as dirty so that it is restored entirely from view state
                // on post backs.  If not done, you must recreate the area on post backs.
                r.MarkAsDirty();

                imClickMap.Areas.Add(r);
            }

            // The Visual Studio designer loses the custom attributes on the image area controls in the HTML when
            // the collection editor is used.  This happens to ListItem objects in controls such as the ListBox
            // and DropDown controls too.  The only workaround is to set such attributes in code as shown below
            // when the page loads.
            ((ImageAreaBase)imClickMap.Areas[0]).Attributes.Add("onmouseover", "javascript: IM_Highlight(1);");
            ((ImageAreaBase)imClickMap.Areas[0]).Attributes.Add("onmouseout", "javascript: IM_Highlight(0);");
            ((ImageAreaBase)imClickMap.Areas[1]).Attributes.Add("onmouseover", "javascript: IM_Highlight(2);");
            ((ImageAreaBase)imClickMap.Areas[1]).Attributes.Add("onmouseout", "javascript: IM_Highlight(0);");
            ((ImageAreaBase)imClickMap.Areas[2]).Attributes.Add("onmouseover", "javascript: IM_Highlight(3);");
            ((ImageAreaBase)imClickMap.Areas[2]).Attributes.Add("onmouseout", "javascript: IM_Highlight(0);");
            ((ImageAreaBase)imClickMap.Areas[3]).Attributes.Add("onmouseover", "javascript: IM_Highlight(4);");
            ((ImageAreaBase)imClickMap.Areas[3]).Attributes.Add("onmouseout", "javascript: IM_Highlight(0);");
		}

        // These are used in the help file examples
        #region Click event examples

        /// <summary>
        /// This event is fired by the image area at the bottom of the left-side
        /// image map.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        /// <remarks>The parameters are not used and it does not cause validation
        /// events to fire.  It is used to enable or disable the right-side image
        /// map.</remarks>
        protected void imMap_Click(object sender, ImageMapClickEventArgs e)
        {
            imClickMap.Enabled = !imClickMap.Enabled;
            lblClickMsg.Text = String.Empty;
            lblEnabledMsg.Text = String.Format(CultureInfo.CurrentCulture, "Image map {0}",
                imClickMap.Enabled ? "enabled" : "disabled");

            // Apply a filter to "gray out" the image map and change the tool tip
            if(!imClickMap.Enabled)
            {
                imClickMap.Style.Add("opacity", ".25");
                imClickMap.Style.Add("filter", "Alpha(Opacity=25)");  // For older browsers
                imClickMap.ToolTip = "Disabled";
            }
            else
            {
                imClickMap.Style.Remove("opacity");
                imClickMap.Style.Remove("filter");
                imClickMap.ToolTip = "Click an area to post back";
            }
        }

        /// <summary>
        /// This event is fired when an area on the right-side image map is clicked
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        /// <remarks>It receives the zero based index of the clicked area plus the
        /// X,Y coordinates of the clicked point within the area.  This image map
        /// also causes validation events to fire.</remarks>
        protected void imClickMap_Click(object sender, ImageMapClickEventArgs e)
        {
            int clickCount = 0;

            if(Page.IsValid)
            {
                lblClickMsg.Text = String.Format(CultureInfo.CurrentCulture, "Clicked Area #{0}", e.AreaIndex + 1);

                // X and Y are only sent back by browsers that support the
                // event.offsetX and event.offsetY properties.
                if(e.XCoordinate != -1)
                    lblClickMsg.Text += String.Format(CultureInfo.CurrentCulture, "<br>At X,Y {0},{1}",
                        e.XCoordinate, e.YCoordinate);

                // Track the click count in the Tag property to test view state
                if(imClickMap.Areas[e.AreaIndex].Tag != null)
                    clickCount = (int)imClickMap.Areas[e.AreaIndex].Tag;

                clickCount++;

                lblClickMsg.Text += String.Format(CultureInfo.CurrentCulture, "<br>It has been clicked {0} times",
                    clickCount);

                imClickMap.Areas[e.AreaIndex].Tag = clickCount;
            }
        }
        #endregion
	}
}
