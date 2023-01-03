//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaBase.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/03/2023
// Note    : Copyright 2004-2023, Eric Woodruff, All rights reserved
//
// This file contains the abstract image area base class
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/01/2004  EFW  Created the code
// 06/28/2006  EFW  Reworked code for use with .NET 2.0
//===============================================================================================================

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web.UI;

using EWSoftware.ImageMaps.Design.Web;

namespace EWSoftware.ImageMaps.Web.Controls
{
    /// <summary>
    /// This is the abstract base class for the image area types of the web server <see cref="ImageMap"/> control
    /// </summary>
    /// <seealso cref="ImageAreaCircle"/>
    /// <seealso cref="ImageAreaPolygon"/>
    /// <seealso cref="ImageAreaRectangle"/>
    /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex1']/*" />
    public abstract class ImageAreaBase : IImageArea, IStateManager, IAttributeAccessor
    {
        #region Private data members
        //=====================================================================

        // The StateBag objects that allows you to save and restore view state and attribute information
        private readonly StateBag viewState;
        private StateBag attrState;

        // The attributes collection for custom attributes
        private System.Web.UI.AttributeCollection attributes;

        #endregion

        #region Properties
        //=====================================================================

        /// <summary>
        /// This must be overridden to specify the shape type
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public abstract ImageAreaShape Shape
        {
            get;
        }

        /// <summary>
        /// This must be overridden to specify the HTML shape name
        /// </summary>
        /// <seealso cref="Shape"/>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public abstract string ShapeText
        {
            get;
        }

        /// <summary>
        /// This must be overridden to get or set the coordinate values in string form
        /// </summary>
        /// <remarks>This is used when rendering the area to HTML</remarks>
        [Category("Appearance"), Bindable(true), MergableProperty(false), Description("The area's coordinates"),
          RefreshProperties(RefreshProperties.Repaint),
          Editor(typeof(ImageAreaCoordinateEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public abstract string Coordinates
        {
            get;
            set;
        }

        /// <summary>
        /// This is used to get or set the access key (a shortcut key or mnemonic) for the image area
        /// </summary>
        /// <value>The value should be a single alphanumeric character.  If the area is enabled and its
        /// <see cref="Action"/> is set to <c>Navigate</c> or <c>PostBack</c>, pressing the <strong>Alt</strong>
        /// key plus the access key will give the area the keyboard focus.</value>
        /// <exception cref="ArgumentOutOfRangeException">The specified access key is not a single character
        /// string.</exception>
        [Category("Behavior"), Bindable(true), DefaultValue(null), MergableProperty(false),
          Description("The access key for the area")]
        public string AccessKey
        {
            get => (string)viewState["AccessKey"];
            set
            {
                if(value != null)
                {
                    if(value.Length > 1)
                        throw new ArgumentOutOfRangeException(nameof(value));

                    if(value.Length == 0)
                        value = null;
                }

                viewState["AccessKey"] = value;
            }
        }

        /// <summary>
        /// This defines the action to take when the area is clicked
        /// </summary>
        /// <value>The default is to navigate to the URL specified in the <see cref="NavigateUrl" /> property</value>
        /// <seealso cref="NavigateUrl"/>
        /// <seealso cref="Target"/>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex2']/*" />
        [Category("Behavior"), DefaultValue(AreaClickAction.Navigate), Bindable(true),
          Description("The action to take when the area is clicked")]
        public AreaClickAction Action
        {
            get
            {
                object action = viewState["Action"];
                return action != null ? (AreaClickAction)action : AreaClickAction.Navigate;
            }
            set => viewState["Action"] = value;
        }

        /// <summary>
        /// This is used to enable or disable the image area
        /// </summary>
        /// <value>If set to false, the image area will not be rendered and effectively cannot be used</value>
        [Category("Behavior"), DefaultValue(true), Bindable(true), Description("Enable or disable the area")]
        public bool Enabled
        {
            get
            {
                object enabled = viewState["Enabled"];
                return (enabled == null) || (bool)enabled;
            }
            set => viewState["Enabled"] = value;
        }

        /// <summary>
        /// This is used to get or set the tab index of the area
        /// </summary>
        /// <value>Note that each area in the image map is considered to be one element on the page and should
        /// have a unique tab index value if they are being used.  The value of the tab index for the first image
        /// map area should be set to a value higher than the tab index of the control preceding the image map in
        /// the tab order.  The tab index of the control following the image map in the tab order will be set to
        /// a value higher than the tab index value of the last area in the image map.</value>
        [Category("Behavior"), DefaultValue(0), Bindable(true), Description("The tab index of the area")]
        public int TabIndex
        {
            get
            {
                object tabIndex = viewState["TabIndex"];
                return (tabIndex == null) ? 0 : (int)tabIndex;
            }
            set => viewState["TabIndex"] = value;
        }

        /// <summary>
        /// This is used to get or set the URL to which to navigate when the area is clicked and the
        /// <see cref="Action"/> property is set to <c>Navigate</c>.
        /// </summary>
        /// <value>This property is ignored if <see cref="Action"/> is not set to <c>Navigate</c>.  If null or
        /// blank and action is set to <c>Navigate</c> it will act as if the action is set to <c>None</c>.  This
        /// property can also be set to a client-side script expression that will be executed when clicked.</value>
        /// <seealso cref="Action"/>
        /// <seealso cref="Target"/>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex2']/*" />
        [Category("Navigation"), DefaultValue(null), Bindable(true),
          Description("The URL to which to navigate when clicked")]
        public string NavigateUrl
        {
            get => (string)viewState["NavigateUrl"];
            set
            {
                if(!String.IsNullOrWhiteSpace(value))
                    viewState["NavigateUrl"] = value;
                else
                    viewState["NavigateUrl"] = null;
            }
        }

        /// <summary>
        /// This is used to get or set the tool tip to display when the mouse hovers over the area
        /// </summary>
        [Category("Appearance"), DefaultValue(null), Bindable(true),
          Description("The tool tip to display when the mouse hovers over the area")]
        public string ToolTip
        {
            get => (string)viewState["ToolTip"];
            set
            {
                if(!String.IsNullOrWhiteSpace(value))
                    viewState["ToolTip"] = value;
                else
                    viewState["ToolTip"] = null;
            }
        }

        /// <summary>
        /// This is used to get or set an object that contains additional user-defined data for the image area
        /// </summary>
        /// <remarks>This property does not work in the designer as ASP.NET is unable to convert the tag to
        /// <see cref="Object"/> when it builds the page at runtime.  If assigning a tag in the design-time
        /// HTML, use the <see cref="TagString"/> property instead.</remarks>
        [Category("Data"), DefaultValue(null), Bindable(true), Browsable(false),
          TypeConverter(typeof(StringConverter)), Description("User-defined data associated with the control"),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Tag
        {
            get => viewState["Tag"];
            set => viewState["Tag"] = value;
        }

        /// <summary>
        /// This is used to get or set a string that contains additional user-defined data for the image area.
        /// This is for use in the HTML designer.
        /// </summary>
        /// <remarks>The <see cref="Tag"/> property does not work in the designer as ASP.NET is unable to convert
        /// the tag to <see cref="Object"/> when it builds the page at runtime.  If assigning a tag in the
        /// design-time HTML, use this property instead.</remarks>
        [Category("Data"), DefaultValue(null), Bindable(true),
          Description("User-defined data associated with the control")]
        public string TagString
        {
            get
            {
                object tag = viewState["Tag"];
                return tag?.ToString();
            }
            set => viewState["Tag"] = value;
        }

        /// <summary>
        /// This property is used to get or set the target location in which to open the window
        /// </summary>
        /// <value>If not set, it defaults to <c>Self</c> and the URL will replace the current document.  The
        /// <see cref="TargetName"/> property is cleared if not set to <c>Other</c>.</value>
        /// <seealso cref="Action"/>
        /// <seealso cref="NavigateUrl"/>
        /// <seealso cref="TargetName"/>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex2']/*" />
        [Category("Navigation"), DefaultValue(WindowTarget.Self), Bindable(true),
          RefreshProperties(RefreshProperties.Repaint),
          Description("The target location in which to open the window when clicked")]
        public WindowTarget Target
        {
            get
            {
                object target = viewState["Target"];
                return (target == null) ? WindowTarget.Self : (WindowTarget)target;
            }
            set
            {
                viewState["Target"] = value;

                if(value != WindowTarget.Other)
                    this.TargetName = null;
            }
        }

        /// <summary>
        /// This property is used to get or set the name of the target frame or window when the
        /// <see cref="Target" /> property is set to <c>Other</c>.
        /// </summary>
        /// <value>This property is ignored if the <see cref="Target"/> property is not set to <c>Other</c>.  If
        /// set to a non-null value, <see cref="Target"/> is set to <c>Other</c>.</value>
        /// <seealso cref="Target"/>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex2']/*" />
        [Category("Navigation"), DefaultValue(null), Bindable(true), RefreshProperties(RefreshProperties.Repaint),
          Description("The name of the target frame or window when Target is set to Other")]
        public string TargetName
        {
            get => (string)viewState["TargetName"];
            set
            {
                viewState["TargetName"] = value;

                if(value != null && value.Length > 0)
                    this.Target = WindowTarget.Other;
            }
        }

        /// <summary>
        /// Gets a collection of attribute name and value pairs for the area that are not directly supported by
        /// the class.
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
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Web.UI.AttributeCollection Attributes
        {
            get
            {
                if(attributes == null)
                {
                    if(attrState == null)
                    {
                        attrState = new StateBag(true);

                        if(this.IsTrackingViewState)
                            ((IStateManager)attrState).TrackViewState();
                    }

                    attributes = new System.Web.UI.AttributeCollection(attrState);
                }

                return attributes;
            }
        }

        /// <summary>
        /// This allows derived classes to store data in the control's view state
        /// </summary>
        protected StateBag ViewState => viewState;

        #endregion

        #region Events
        //=====================================================================

#pragma warning disable 0067

        /// <summary>
        /// This is part of the <see cref="IImageArea"/> interface, but it is not implemented by the web
        /// server control.
        /// </summary>
        /// <remarks>This is present because it is part of the <see cref="IImageArea"/> interface.  However, the
        /// web image map is HTML, there is no server-side visual representation, and changes will be reflected
        /// client-side whenever it is rendered.  As such, the properties of this class do not utilize the event.
        /// </remarks>
        /// <exclude/>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler ImageAreaChanged;

#pragma warning restore 0067

        #endregion

        #region Constructors
        //=====================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <overloads>There are three overloads for the constructor</overloads>
        protected ImageAreaBase()
        {
            viewState = new StateBag();
        }

        /// <summary>
        /// This version of the constructor takes a URL
        /// </summary>
        /// <param name="url">The URL to associate with the image area</param>
        protected ImageAreaBase(string url) : this()
        {
            this.NavigateUrl = url;
        }

        /// <summary>
        /// This version of the constructor takes a URL and a tool tip
        /// </summary>
        /// <param name="url">The URL to associate with the image area</param>
        /// <param name="toolTip">The tool tip to associate with the image area</param>
        protected ImageAreaBase(string url, string toolTip) : this(url)
        {
            this.ToolTip = toolTip;
        }
        #endregion

        #region Methods
        //=====================================================================

        /// <summary>
        /// This must be overridden to get the shape to draw a focus frame around itself
        /// </summary>
        /// <param name="g">The graphics object to use for drawing</param>
        /// <param name="offset">A point used to offset the image area coordinates (i.e. if the image is
        /// scrolled).</param>
        /// <remarks>The web image area control makes use of it in the coordinate editor</remarks>
        public abstract void DrawFocusFrame(Graphics g, Point offset);

        /// <summary>
        /// This is overridden to allow proper comparison of image area objects
        /// </summary>
        /// <param name="obj">The object to which this instance is compared.</param>
        /// <returns>Returns true if the object equals this instance, false if it does not</returns>
        public override bool Equals(object obj)
        {
            ImageAreaBase a = (obj as ImageAreaBase);

            if(a == null)
                return false;

            return (this.Shape == a.Shape && this.Coordinates == a.Coordinates &&
                this.AccessKey == a.AccessKey && this.Action == a.Action && this.Enabled == a.Enabled &&
                this.TabIndex == a.TabIndex && this.NavigateUrl == a.NavigateUrl && this.ToolTip == a.ToolTip &&
                this.Target == a.Target && this.TargetName == a.TargetName &&
                ((this.Tag == null && a.Tag == null) || (this.Tag != null && this.Tag.Equals(a.Tag))) &&
                this.Attributes.Equals(a.Attributes));
        }

        /// <summary>
        /// Get a hash code for the image area object
        /// </summary>
        /// <returns>Returns the hash code for the image area's string form</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Convert the image area instance to its string description
        /// </summary>
        /// <returns>Returns the description of the image area</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(80);

            if(this.ToolTip != null)
                sb.Append(this.ToolTip);

            if(this.NavigateUrl != null)
            {
                if(sb.Length > 0)
                    sb.Append(", ");

                sb.Append(this.NavigateUrl);
            }

            if(sb.Length > 0)
                sb.Append(", ");

            sb.AppendFormat("{0} at {1}", this.Shape.ToString(), this.Coordinates);

            return sb.ToString();
        }

        /// <summary>
        /// Mark all properties on the image area as dirty.
        /// </summary>
        /// <remarks>This property is useful for dynamically created image areas.  By calling this method, you
        /// can mark all properties as dirty to force them to get stored in view state.  By doing this, the area
        /// can be created once at the initial page load and then recreated on post backs entirely from view
        /// state.</remarks>
        public void MarkAsDirty()
        {
            if(!this.IsTrackingViewState)
                this.TrackViewState();

            foreach(string s in viewState.Keys)
                viewState.SetItemDirty(s, true);

            if(attrState != null)
                foreach(string s in attrState.Keys)
                    attrState.SetItemDirty(s, true);
        }
        #endregion

        #region IStateManager interface implementation
        //=====================================================================

        /// <summary>
        /// Start tracking view state
        /// </summary>
        public void TrackViewState()
        {
            ((IStateManager)viewState).TrackViewState();

            if(attrState != null)
                ((IStateManager)attrState).TrackViewState();
        }

        /// <summary>
        /// Indicates whether or not view state is being tracked
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTrackingViewState => ((IStateManager)viewState).IsTrackingViewState;

        /// <summary>
        /// Save view state
        /// </summary>
        /// <returns>The view state for the object</returns>
        public object SaveViewState()
        {
            object state, attrs;

            state = ((IStateManager)viewState).SaveViewState();

            if(attrState != null)
                attrs = ((IStateManager)attrState).SaveViewState();
            else
                attrs = null;

            if(state != null || attrs != null)
                return new Pair(state, attrs);

            return null;
        }

        /// <summary>
        /// Load view state
        /// </summary>
        /// <param name="state">The saved view state</param>
        public void LoadViewState(object state)
        {
            if(state != null)
            {
                Pair p = (state as Pair);

                if(p != null)
                {
                    ((IStateManager)viewState).LoadViewState(p.First);

                    if(p.Second != null)
                    {
                        if(attrState == null)
                        {
                            attrState = new StateBag(true);
                            ((IStateManager)attrState).TrackViewState();
                        }

                        ((IStateManager)attrState).LoadViewState(p.Second);
                    }
                }
            }
        }
        #endregion

        #region IAttributeAccessor interface implementation
        //=====================================================================

        /// <summary>
        /// Retrieve the specified attribute property from the control
        /// </summary>
        /// <param name="key">The key of the attribute to retrieve</param>
        /// <returns>The value of the attribute</returns>
        public string GetAttribute(string key)
        {
            return this.Attributes[key];
        }

        /// <summary>
        /// Assign an attribute and value to the control
        /// </summary>
        /// <param name="key">The key for the attribute</param>
        /// <param name="value">The value of the attribute</param>
        public void SetAttribute(string key, string value)
        {
            this.Attributes[key] = value;
        }
        #endregion
    }
}
