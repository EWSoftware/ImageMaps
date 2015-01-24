//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaCollectionEditor.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/08/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the image area collection editor
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
using System.ComponentModel.Design;

namespace EWSoftware.ImageMaps.Design.Windows.Forms
{
    /// <summary>
    /// This provides basic design time support for the <see cref="ImageAreaCollection"/>.
    /// </summary>
    internal sealed class ImageAreaCollectionEditor : CollectionEditor
    {
        #region Properties
        //=====================================================================

        /// <summary>
        /// This static property is used to get a reference to the current image area collection being edited
        /// </summary>
        /// <value>Through the area collection, we can get a reference to the parent image map.  The coordinate
        /// editor uses this to get the image to display and it's size information.</value>
        public static ImageAreaCollection Areas { get; private set; }

        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The collection type to be edited</param>
        public ImageAreaCollectionEditor(Type type) : base(type)
        {
        }
        #endregion

        #region Method overrides
        //=====================================================================

        /// <summary>
        /// This returns an array of types that can be created.
        /// </summary>
        /// <returns>The types returned are based on the type of ImageMap control that owns the image area
        /// collection.</returns>
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] {
                typeof(EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle),
                typeof(EWSoftware.ImageMaps.Windows.Forms.ImageAreaCircle),
                typeof(EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse),
                typeof(EWSoftware.ImageMaps.Windows.Forms.ImageAreaPolygon) };
        }

        /// <summary>
        /// This creates an instance of the type.
        /// </summary>
        /// <returns>A new instance of an object</returns>
        /// <param name="itemType">The item type to be created</param>
        protected override object CreateInstance(Type itemType)
        {
            return base.CreateInstance(itemType);
        }

        /// <summary>
        /// This is overridden to store a reference to the image area collection for use by the static
        /// <see cref="Areas"/> property above.
        /// </summary>
        /// <param name="context">The descriptor context</param>
        /// <param name="provider">The provider</param>
        /// <param name="value">The collection as an object</param>
        /// <returns>The edited collection as an object</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Areas = (ImageAreaCollection)value;

            object editedValue = base.EditValue(context, provider, value);

            Areas = null;

            return editedValue;
        }
        #endregion
    }
}
