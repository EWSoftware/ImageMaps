//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageMap.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/09/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived WebControl class that can be used to render an image map on a web form (an image
// with hyperlink hot spots).
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
// Version     Date     Who  Comments
// ==============================================================================================================
// 1.0.0.0  07/01/2004  EFW  Created the code
// 2.0.0.0  06/28/2006  EFW  Reworked code for use with .NET 2.0
// 2.0.0.1  07/07/2007  EFW  Added ForceScriptRegistration property
//===============================================================================================================

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

using EWSoftware.ImageMaps.Design.Web;

namespace EWSoftware.ImageMaps.Web.Controls
{
    /// <summary>
    /// This is a derived <see cref="System.Web.UI.WebControls.WebControl" /> class that can be used to render an
    /// image map (an image with hyperlink hot spots).
    /// </summary>
    /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex1']/*" />
	[DefaultProperty("ImageUrl"), DefaultEvent("Click"), ParseChildren(true, "Areas"),
      ToolboxData("<{0}:ImageMap runat=server />"), Designer(typeof(ImageMapDesigner))]
	public class ImageMap : WebControl, IPostBackEventHandler, IImageMap
	{
        #region Private data members
        //=====================================================================

        private WebImageAreaCollection imageAreas;

        #endregion

        #region Properties
        //=====================================================================

        /// <summary>
        /// Gets or sets the alignment of the image map control in relation to other elements on the web page
        /// </summary>
        [Description("Specify the image alignment"), Bindable(true), Category("Layout"),
          DefaultValue(ImageAlign.NotSet)]
        public virtual ImageAlign ImageAlign
        {
            get
            {
                object oAlign = this.ViewState["ImageAlign"];
                return (oAlign == null) ? ImageAlign.NotSet : (ImageAlign)oAlign;
            }
            set { this.ViewState["ImageAlign"] = value; }
        }

        /// <summary>
        /// Gets or sets the location of an image to display in the image map control
        /// </summary>
        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor)), DefaultValue(""), Bindable(true),
          Category("Appearance"), Description("The URL of the image to display")]
        public virtual string ImageUrl
        {
            get
            {
                object oURL = this.ViewState["ImageUrl"];
                return (oURL == null) ? String.Empty : (string)oURL;
            }
            set { this.ViewState["ImageUrl"] = value; }
        }

        /// <summary>
        /// This property is used to determine whether or not validation occurs when an image map area is clicked
        /// </summary>
        /// <value>The default is false and it does not cause validation</value>
        [Category("Behavior"), DefaultValue(false), Bindable(false),
          Description("Indicates whether or not validation occurs when clicked")]
        public bool CausesValidation
        {
            get
            {
                object oValidate = this.ViewState["CausesValidation"];
                return (oValidate == null) ? false : (bool)oValidate;
            }
            set { this.ViewState["CausesValidation"] = value; }
        }

        /// <summary>
        /// This property is used to force registration of the postback script even if it may not be used
        /// </summary>
        /// <value>The default is false and the script is only registered if there are image areas with their
        /// <see cref="ImageAreaBase.Action"/> set to <c>PostBack</c>.  You can set this to true to force the
        /// postback script to be registered so that it appears in the page.  This is useful if you are using
        /// the image map in an Ajax panel and the control does not contain image areas when the page is
        /// initially loaded.</value>
        [Category("Behavior"), DefaultValue(false),
          Description("Force registration of the postback script.  This is useful when the control appears in " +
            "an Ajax panel and contains no image areas on the initial page load.")]
        public bool ForceScriptRegistration
        {
            get
            {
                object oForceScript = this.ViewState["ForceScript"];
                return (oForceScript == null) ? false : (bool)oForceScript;
            }
            set { this.ViewState["ForceScript"] = value; }
        }

        /// <summary>
        /// This property can be used to specify the form control ID for the page to override the default
        /// behavior of <see cref="OnPreRender"/>.
        /// </summary>
        /// <value>Leave it blank to let the control find the form ID or set it to the form control's ID to
        /// override the search behavior.  See <see cref="OnPreRender"/> for more information.</value>
        [Category("Behavior"), DefaultValue(null), Bindable(true),
          Description("The form ID to use as an override if necessary for post back image areas")]
        public string FormId
        {
            get { return (string)this.ViewState["FormId"]; }
            set { this.ViewState["FormId"] = value; }
        }
        #endregion

        #region Hidden properties
        //=====================================================================

        // These properties do not apply so they are hidden

        /// <summary>
        /// Image maps do not use this property so it is hidden.  It always returns the base class font.
        /// </summary>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override FontInfo Font
        {
            get { return base.Font; }
        }

        /// <summary>
        /// Image maps do not use this property so it is hidden.  It always returns the base class background
        /// color.
        /// </summary>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color BackColor
        {
            get { return base.BackColor; }
        }

        /// <summary>
        /// Image maps do not use this property so it is hidden.  It always returns the base class foreground
        /// color.
        /// </summary>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
        }

		/// <summary>
		/// This is used by the <see cref="ImageMapWidth"/> property that is implemented as part of the
        /// <see cref="IImageMap"/> interface.  As such, this property is hidden.
		/// </summary>
		/// <exception cref="ArgumentException">This is thrown if the width is not specified in pixels</exception>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Unit Width
		{
			get { return base.Width; }
			set
            {
				if(value != Unit.Empty && value.Type != UnitType.Pixel)
					throw new ArgumentException("Image map width must be specified in pixels", "value");

				base.Width = value;
			}
		}

		/// <summary>
		/// This is used by the <see cref="ImageMapHeight"/> property that is implemented as part of the
        /// <see cref="IImageMap"/> interface.  As such, this property is hidden.
		/// </summary>
		/// <exception cref="ArgumentException">This is thrown if the height is not specified in pixels</exception>
        /// <exclude/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
          Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Unit Height
		{
			get { return base.Height; }
			set
			{
				if(value != Unit.Empty && value.Type != UnitType.Pixel)
					throw new ArgumentException("Image map height must be specified in pixels", "value");

				base.Height = value;
			}
		}
        #endregion

        #region Private designer methods
        //=====================================================================

        // These are used because the default values for these properties don't work with the DefaultValue
        // attribute.

        /// <summary>
        /// The designer uses this to determine whether or not to serialize changes to the Areas property
        /// </summary>
        /// <returns>True if the property should be serialized, false if not</returns>
        private bool ShouldSerializeAreas()
        {
            return this.Areas.Count > 0;
        }

        /// <summary>
        /// Reset the areas collection
        /// </summary>
        private void ResetAreas()
        {
            this.Areas.Clear();
        }
        #endregion

        #region IPostBackEventHandler interface implementation
        //=====================================================================

        /// <summary>
        /// This fires the image map <see cref="Click"/> event if necessary
        /// </summary>
        /// <param name="eventArgument">The event arguments</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            string value;
			int xCoord, yCoord, pos, area = Int32.Parse(Page.Request.Params["EWSIM_AREA"],
                CultureInfo.InvariantCulture);

			if(area >= 0)
			{
                value = Page.Request.Params["EWSIM_XCOORD"];
                pos = value.IndexOf('.');

                // The coordinate value may be a decimal value.  We'll ignore the fractional part.
                if(pos != -1)
                    xCoord = Int32.Parse(value.Substring(0, pos), CultureInfo.InvariantCulture);
                else
                    xCoord = Int32.Parse(value, CultureInfo.InvariantCulture);

                value = Page.Request.Params["EWSIM_YCOORD"];
                pos = value.IndexOf('.');

                if(pos != -1)
                    yCoord = Int32.Parse(value.Substring(0, pos), CultureInfo.InvariantCulture);
                else
                    yCoord = Int32.Parse(value, CultureInfo.InvariantCulture);

                if(this.CausesValidation)
                    this.Page.Validate();

				this.OnClick(new ImageMapClickEventArgs(area, xCoord, yCoord));
			}
        }
        #endregion

        #region IImageMap interface implementation
        //=====================================================================

        /// <summary>
        /// Gets or sets the tool tip text (alternate text) displayed in the image map control when the image is
        /// unavailable. Browsers that support the tool tips feature display this text as a tool tip for all
        /// undefined areas of the image.
        /// </summary>
        [Bindable(true), Description("Tool tip/alternate display text for the image"),
          Category("Appearance"), DefaultValue("")]
        public override string ToolTip
        {
            get { return base.ToolTip; }
            set { base.ToolTip = value; }
        }

        /// <summary>
		/// Gets or sets the width of the image in the image map control in pixels
        /// </summary>
        /// <value>If set to zero (the default), the image map will use the image's width</value>
        /// <seealso cref="ImageMapHeight"/>
		[Bindable(true), Category("Layout"), DefaultValue(0), Description("Specify the image map width in pixels")]
        public int ImageMapWidth
        {
            get { return (int)this.Width.Value; }
            set
            {
                if(value == 0)
                    this.Width = Unit.Empty;
                else
                    this.Width = new Unit(value);
            }
        }

        /// <summary>
		/// Gets or sets the height of the image in the image map control in pixels
        /// </summary>
        /// <value>If set to zero (the default), the image map will use the image's height.</value>
        /// <seealso cref="ImageMapWidth"/>
		[Bindable(true), Category("Layout"), DefaultValue(0), Description("Specify the image map height in pixels")]
        public int ImageMapHeight
        {
            get { return (int)this.Height.Value; }
            set
            {
                if(value == 0)
                    this.Height = Unit.Empty;
                else
                    this.Height = new Unit(value);
            }
        }

        /// <summary>
        /// This returns the collection of areas defined for the image map
        /// </summary>
        /// <remarks>
        /// <note type="important">
        /// If you use the Visual Studio designer to edit the collection of image areas, custom attributes on the
        /// image area items will be lost (i.e. things like <c>onmouseover</c> or other such attributes that are
        /// not normal properties of the image area object).  This appears to be a bug in the designer as the
        /// same thing happens to <c>ListItem</c> objects in controls such as the <c>ListBox</c> and
        /// <c>DropDownList</c>.  Use code to add custom attributes to the image areas instead.
        /// </note>
        /// </remarks>
		[Bindable(true), Category("Layout"), MergableProperty(false),
		  DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
          PersistenceMode(PersistenceMode.InnerDefaultProperty),
          Description("The collection of areas that represent hot spots on the image map"),
    	  Editor(typeof(ImageAreaCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ImageAreaCollection Areas
        {
            get
            {
                // Create on first use
                if(imageAreas == null)
                    imageAreas = new WebImageAreaCollection(this);

                return imageAreas;
            }
        }

        /// <summary>
        /// This event is raised when a hot spot on the image map is clicked
        /// </summary>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex1']/*" />
		[Category("Action"), Description("Fires when a hot spot on the image map is clicked")]
        public event EventHandler<ImageMapClickEventArgs> Click;

        /// <summary>
        /// This raises the image map <see cref="Click"/> event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnClick(ImageMapClickEventArgs e)
        {
            var handler = Click;

            if(handler != null)
                handler(this, e);
        }
        #endregion

        #region Methods
        //=====================================================================

        /// <summary>
        /// This private method is used to format the script used for a post back area's <c>OnClick</c> event
        /// handler.
        /// </summary>
        /// <param name="area">The area index</param>
        /// <returns>The JavaScript needed by the area to do the post back</returns>
        /// <remarks>The script changes based on the value of the <see cref="CausesValidation"/> property</remarks>
        private string FormatOnClickScript(int area)
        {
            StringBuilder sb = new StringBuilder("javascript: ", 256);

            // The utility method normally used to get the validation prefix code is an internal member so we
            // can't call it.  As such, we have to hard code things here.
            if(this.CausesValidation && this.Page.Validators.Count > 0)
                sb.Append("if(typeof(Page_ClientValidate) != \'function\' || Page_ClientValidate()) { ");

            sb.AppendFormat("EWSIM_OnAreaClick({0}); ", area);
            sb.Append(this.Page.ClientScript.GetPostBackEventReference(this, String.Empty));

            if(this.CausesValidation && this.Page.Validators.Count > 0)
                sb.Append("; }");

            return sb.ToString();
        }

        /// <summary>
        /// This is overridden to load view state for the control
        /// </summary>
        /// <param name="savedState">The view state for the control</param>
        protected override void LoadViewState(object savedState)
        {
            Pair p = (savedState as Pair);

            if(p != null)
            {
                base.LoadViewState(p.First);
                ((WebImageAreaCollection)this.Areas).LoadViewState(p.Second);
            }
        }

        /// <summary>
        /// This is overridden to save view state for the control
        /// </summary>
        /// <returns>The view state for the control</returns>
        protected override object SaveViewState()
        {
            object controlState = base.SaveViewState();
            object areas = ((WebImageAreaCollection)this.Areas).SaveViewState();

            if(controlState != null || areas != null)
                return new Pair(controlState, areas);

            return null;
        }

        /// <summary>
        /// This is overridden to track view state for the control
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            ((WebImageAreaCollection)this.Areas).TrackViewState();
        }

        /// <summary>
        /// This is overridden to register the hidden fields used by the postback code if any of the image map
        /// areas have their <see cref="ImageAreaBase.Action" /> set to <c>PostBack</c>.  It also registers a
        /// script block containing a helper function used in that situation as well.
        /// </summary>
        /// <param name="e">The event arguments</param>
        /// <remarks>Normally, the control will search for the form control on the page when it needs to register
        /// the helper script for post back image areas.  If it cannot find the form control, it will default to
        /// using index zero (forms[0]) instead.  If you have other form controls on the page (i.e. normal HTML
        /// forms in addition to the ASP.NET Run at Server form), you may need to use the <see cref="FormId"/>
        /// property to specify the actual form ID to use for the post back script.  If specified, that form ID
        /// will override the search behavior and will always be used.  If null or blank, the above noted search
        /// and fallback behavior will be used instead.
        /// </remarks>
        protected override void OnPreRender(EventArgs e)
        {
            string formID = null, script;
            bool registerScript = this.ForceScriptRegistration;

            base.OnPreRender(e);

            if(this.Context != null && this.Page != null && this.Enabled)
            {
                // Skip it if the script and fields are already registered
                if(this.Page.ClientScript.IsClientScriptBlockRegistered("EWS_ImageMap"))
                    return;

                // If not forced, only register the script if there are postback areas that need it
                if(!registerScript)
                    foreach(ImageAreaBase a in this.Areas)
                        if(a.Action == AreaClickAction.PostBack)
                        {
                            registerScript = true;
                            break;
                        }

                if(registerScript)
                {
                    this.Page.ClientScript.RegisterHiddenField("EWSIM_AREA", "-1");
                    this.Page.ClientScript.RegisterHiddenField("EWSIM_XCOORD", "-1");
                    this.Page.ClientScript.RegisterHiddenField("EWSIM_YCOORD", "-1");
                    formID = this.FormId;

                    if(formID == null || formID.Length == 0)
                        foreach(Control ctl in this.Page.Controls)
                            if(ctl is System.Web.UI.HtmlControls.HtmlForm)
                            {
                                formID = ctl.UniqueID;
                                break;
                            }

                    // If not found, default to the first form on the page
                    if(formID == null || formID.Length == 0)
                        formID = "0";
                    else
                        formID = "'" + formID + "'";

                    script = String.Format(CultureInfo.InvariantCulture,
@"<script type='text/javascript'>
<!--
function EWSIM_OnAreaClick(nArea)
{{
    document.forms[{0}].EWSIM_AREA.value = nArea;

    if(typeof(event) != 'undefined' && typeof(event.offsetX) != 'undefined')
    {{
        document.forms[{0}].EWSIM_XCOORD.value = event.offsetX;
        document.forms[{0}].EWSIM_YCOORD.value = event.offsetY;
    }}
}}
//-->
</script>", formID);

                    this.Page.ClientScript.RegisterClientScriptBlock(typeof(ImageMap), "EWS_ImageMap", script);
                }
            }
        }

        /// <summary>
        /// This is overridden to add attributes to the image tag
        /// </summary>
        /// <param name="writer">The output stream to which the HTML is rendered</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if(this.Page != null)
                this.Page.VerifyRenderingInServerForm(this);

            base.AddAttributesToRender(writer);

            if(this.ImageUrl.Length > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Src, this.ImageUrl);

            if(this.ToolTip.Length > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, this.ToolTip);

            if(this.ImageAlign != ImageAlign.NotSet)
                writer.AddAttribute(HtmlTextWriterAttribute.Align, this.ImageAlign.ToString());

            if(this.BorderWidth.IsEmpty)
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");

            // The map is only rendered if enabled
            if(this.Enabled && this.Page != null)
                writer.AddAttribute("usemap", "#" + this.ClientID + "_Map", false);
        }

        /// <summary>
        /// This is overridden to render the image map HTML
        /// </summary>
        /// <param name="writer">The output stream to which the HTML is rendered</param>
        protected override void Render(HtmlTextWriter writer)
        {
            int idx = 0;

            // Render the image tag
            this.AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.WriteLine();

            // Don't render the map if disabled or in design mode
            if(!this.Enabled || this.Page == null || this.Context == null)
                return;

            // Render the opening map tag
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_Map");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + "_Map");
            writer.RenderBeginTag(HtmlTextWriterTag.Map);

            // Render the areas
            foreach(ImageAreaBase a in this.Areas)
            {
                // Don't render disabled areas.  That way the option works for any browser type.  The "disabled"
                // attribute only works with Opera from what I've read.
                if(!a.Enabled)
                {
                    idx++;
                    continue;
                }

                writer.WriteBeginTag("area");
                writer.WriteAttribute("shape", a.ShapeText);
                writer.WriteAttribute("coords", a.Coordinates);

                if(a.ToolTip != null)
                {
                    writer.WriteAttribute("alt", a.ToolTip);
                    writer.WriteAttribute("title", a.ToolTip);
                }

                if(a.AccessKey != null && a.Action != AreaClickAction.None)
                    writer.WriteAttribute("accesskey", a.AccessKey);

                if(a.TabIndex != 0)
                    writer.WriteAttribute("tabindex", a.TabIndex.ToString(CultureInfo.InvariantCulture));

                switch(a.Action)
                {
                    case AreaClickAction.Navigate:
                        // If null or blank act like action is None
                        if(a.NavigateUrl == null || a.NavigateUrl.Length == 0)
                            writer.WriteAttribute("nohref", null);
                        else
                        {
                            writer.WriteAttribute("href", a.NavigateUrl);

                            if(a.Target != WindowTarget.Self)
                                if(a.Target == WindowTarget.Other)
                                    writer.WriteAttribute("target", a.TargetName);
                                else
                                    writer.WriteAttribute("target", "_" + a.Target.ToString().ToLowerInvariant());
                        }
                        break;

                    case AreaClickAction.PostBack:
                        writer.WriteAttribute("href", "#");
                        writer.WriteAttribute("onclick", this.FormatOnClickScript(idx));
                        break;

                    default:
                        writer.WriteAttribute("nohref", null);
                        break;
                }

                // Render custom attributes
    			foreach(string strKey in a.Attributes.Keys)
				    writer.WriteAttribute(strKey, a.Attributes[strKey]);

                writer.WriteLine(HtmlTextWriter.SelfClosingTagEnd);
                idx++;
            }

            writer.RenderEndTag();      // Close the map tag
        }
        #endregion
    }
}
