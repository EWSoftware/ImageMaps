//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaCollection.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/08/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a type-safe collection class and its enumerator that can contain the various
// IImageArea-derived objects.  It is used by both the Windows Forms and the web server image map controls.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//   Date      Who  Comments
// ==============================================================================================================
// 07/01/2004  EFW  Created the code
// 07/09/2006  EFW  Reworked code for use with .NET 2.0
//===============================================================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EWSoftware.ImageMaps
{
	/// <summary>
	/// A type-safe collection of <see cref="IImageArea"/> objects
	/// </summary>
    /// <remarks>The class has a type-safe enumerator.  It is used directly by the Windows Forms image map
    /// control.
    /// </remarks>
    [Serializable]
    public class ImageAreaCollection : Collection<IImageArea>
	{
        #region Private data members
        //=====================================================================

        // This is used to reference the parent image map.  It's used by the ImageAreaCoordinateEditor designer
        // class.
        private IImageMap im;

        #endregion

        #region Properties
        //=====================================================================

        /// <summary>
        /// This property is used to get the <see cref="IImageMap"/> control that uses this collection.  Its
        /// primary use is by the <c>ImageAreaCoordinateEditor</c> designer class so that it can get the image
        /// information it needs at design time.
        /// </summary>
        public IImageMap ImageMapControl
        {
            get { return im; }
        }
        #endregion

        #region Events
        //=====================================================================

        /// <summary>
        /// This event is raised when an image area property changes that affects its visual presentation in the
        /// image map control such as its position or enabled state.
        /// </summary>
        public event EventHandler ImageAreaChanged;

        /// <summary>
        /// This raises the <see cref="ImageAreaChanged "/> event on behalf of an image area in the collection
        /// </summary>
        /// <param name="sender">This will usually be the image area object that changed.  However, if the
        /// collection is cleared via the <see cref="Collection{T}.Clear"/> method, it will be the collection
        /// object itself.</param>
        /// <param name="e">The event arguments.</param>
        private void OnImageAreaChanged(object sender, EventArgs e)
        {
            var handler = ImageAreaChanged;

            if(handler != null)
                handler(sender, e);
        }
        #endregion

        #region Constructors
        //=====================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <overloads>There are three overloads for the constructor</overloads>
        public ImageAreaCollection()
        {
        }

        /// <summary>
        /// Construct an empty collection associated with the specified <see cref="IImageMap"/> control
        /// </summary>
        /// <param name="map">The image map control that owns the collection</param>
        public ImageAreaCollection(IImageMap map)
        {
            im = map;
        }

        /// <summary>
        /// Construct a collection from an enumerable list of <see cref="IImageArea"/> objects
        /// </summary>
        /// <param name="map">The image map control that owns the collection</param>
        /// <param name="areas">An enumerable list of <see cref="IImageArea"/> objects to add</param>
        public ImageAreaCollection(IImageMap map, IEnumerable<IImageArea> areas) : this(map)
        {
            this.AddRange(areas);
        }
        #endregion

        #region Helper methods
        //=====================================================================

        /// <summary>
        /// Add a range of <see cref="IImageArea"/> objects from an enumerable list
        /// </summary>
        /// <param name="areas">An enumerable list of <see cref="IImageArea"/> objects to add</param>
        public void AddRange(IEnumerable<IImageArea> areas)
        {
            if(areas != null)
                foreach(IImageArea a in areas)
                    this.Add(a);
        }
        #endregion

        #region Method overrides used to fire the image area changed event
        //=====================================================================

        /// <summary>
        /// This is overridden to notify the image map that all of the image areas have been removed
        /// </summary>
        protected override void ClearItems()
        {
            foreach(var area in this)
                area.ImageAreaChanged -= this.OnImageAreaChanged;

            base.ClearItems();
            this.OnImageAreaChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// This is overridden to notify the image map that a new image area has been added to the collection
        /// </summary>
        /// <param name="index">The position at which the object is inserted</param>
        /// <param name="item">The item inserted</param>
        protected override void InsertItem(int index, IImageArea item)
        {
            item.ImageAreaChanged += this.OnImageAreaChanged;

            base.InsertItem(index, item);
            this.OnImageAreaChanged(item, EventArgs.Empty);
        }

        /// <summary>
        /// This is overridden to notify the image map that an image area has been removed from the collection
        /// </summary>
        /// <param name="index">The position at which the object is removed</param>
        protected override void RemoveItem(int index)
        {
            IImageArea area = this[index];

            area.ImageAreaChanged -= this.OnImageAreaChanged;

            base.RemoveItem(index);
            this.OnImageAreaChanged(area, EventArgs.Empty);
        }

        /// <summary>
        /// This is overridden to notify the image map that a new image area has replaced another in the
        /// collection.
        /// </summary>
        /// <param name="index">The position at which the object is stored</param>
        /// <param name="item">The item that is stored</param>
        protected override void SetItem(int index, IImageArea item)
        {
            IImageArea area = this[index];

            area.ImageAreaChanged -= this.OnImageAreaChanged;
            item.ImageAreaChanged += this.OnImageAreaChanged;

            base.SetItem(index, item);
            this.OnImageAreaChanged(item, EventArgs.Empty);
        }
        #endregion
    }
}
