//===============================================================================================================
// System  : Image Map Control Library
// File    : WebImageAreaCollection.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/09/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived image area collection class specific to the web server image map control
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//   Date      Who  Comments
// ==============================================================================================================
// 07/01/2004  EFW  Created the code
// 11/23/2005  EFW  Fixed problems with view state on dynamic areas
// 07/09/2006  EFW  Reworked code for use with .NET 2.0
//===============================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;

namespace EWSoftware.ImageMaps.Web.Controls
{
    /// <summary>
    /// This is an <see cref="ImageAreaCollection"/>-derived class that implements
    /// <see cref="System.Web.UI.IStateManager"/> so that web-based image areas can track their view state.
    /// </summary>
    public class WebImageAreaCollection : ImageAreaCollection, IStateManager
    {
        #region Private data members
        //=====================================================================

        private bool trackViewState, saveAll;

        #endregion

        #region Constructors
        //=====================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <overloads>There are three overloads for the constructor.</overloads>
        public WebImageAreaCollection()
        {
        }

        /// <summary>
        /// Construct an empty collection associated with the specified <see cref="IImageMap"/> control
        /// </summary>
        /// <param name="map">The image map control that owns the collection</param>
        public WebImageAreaCollection(IImageMap map) : base(map)
        {
        }

        /// <summary>
        /// Construct collection from an enumerable list of <see cref="IImageArea"/> objects
        /// </summary>
        /// <param name="map">The image map control that owns the collection</param>
        /// <param name="areas">An enumerable list of <see cref="IImageArea"/> objects</param>
        public WebImageAreaCollection(IImageMap map, IEnumerable<IImageArea> areas) : base(map, areas)
        {
        }
        #endregion

        #region Method overrides
        //=====================================================================

        /// <summary>
        /// This is overridden to track view state for the inserted item
        /// </summary>
        /// <param name="index">The point at which to insert the item</param>
        /// <param name="item">The item to insert</param>
        protected override void InsertItem(int index, IImageArea item)
        {
            // If not added at the end, set the flag to indicate that all items should be serialized to view
            // state.
            if(index == this.Items.Count)
            {
                EWSoftware.ImageMaps.Web.Controls.ImageAreaBase a =
                  (EWSoftware.ImageMaps.Web.Controls.ImageAreaBase)item;

                if(trackViewState && !a.IsTrackingViewState)
                    a.TrackViewState();
            }
            else
                saveAll = true;

            base.InsertItem(index, item);
        }

        /// <summary>
        /// This is overridden to set the flag to indicate that all items should be serialized to view state if
        /// this is used.
        /// </summary>
        /// <param name="index">The index of the item to remove</param>
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            saveAll = true;
        }
        #endregion

        #region IStateManager interface implementation
        //=====================================================================

        /// <summary>
        /// Start tracking view state
        /// </summary>
        public void TrackViewState()
        {
            trackViewState = true;

            foreach(EWSoftware.ImageMaps.Web.Controls.ImageAreaBase a in this)
                a.TrackViewState();
        }

        /// <summary>
        /// Indicates whether or not view state is being tracked
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTrackingViewState
        {
            get { return trackViewState; }
        }

        /// <summary>
        /// Save view state
        /// </summary>
        /// <returns>The view state for the object</returns>
        public object SaveViewState()
        {
            EWSoftware.ImageMaps.Web.Controls.ImageAreaBase area;
            int idx, count = this.Count;

            if(count == 0)
                return null;

            // If something was inserted or removed, save the whole collection regardless of whether the objects
            // have changed view state or not.
            if(saveAll)
            {
                ImageAreaShape[] shapes = new ImageAreaShape[count];
                object[] states = new object[count];

                for(idx = 0; idx < count; idx++)
                {
                    area = (EWSoftware.ImageMaps.Web.Controls.ImageAreaBase)this[idx];
                    shapes[idx] = area.Shape;

                    // All properties must be saved so mark them all dirty
                    area.MarkAsDirty();
                    states[idx] = area.SaveViewState();
                }

                return new Triplet(count, shapes, states);
            }

            // If not saving all, just save the changed items
            ArrayList changedIndices = new ArrayList(count);
            ArrayList changedShapes = new ArrayList(count);
            ArrayList changedStates = new ArrayList(count);
            object state;

            for(idx = 0; idx < count; idx++)
            {
                state = ((EWSoftware.ImageMaps.Web.Controls.ImageAreaBase)this[idx]).SaveViewState();

                if(state != null)
                {
                    changedIndices.Add(idx);
                    changedShapes.Add(this[idx].Shape);
                    changedStates.Add(state);
                }
            }

            if(changedIndices.Count > 0)
                return new Triplet(changedIndices, changedShapes, changedStates);

            return null;
        }

        /// <summary>
        /// Load view state
        /// </summary>
        /// <param name="state">The saved view state</param>
        /// <exception cref="ArgumentException">This is thrown if an unexpected shape type is found in the view
        /// state.</exception>
        public void LoadViewState(object state)
        {
            EWSoftware.ImageMaps.Web.Controls.ImageAreaBase a;
            Triplet t;
            int idx, count, itemIdx;

            if(state == null)
                return;

            t = (Triplet)state;

            // Restore all items?
            if(!(t.First is ArrayList))
            {
                saveAll = true;
                this.Clear();

                count = (int)t.First;
                ImageAreaShape[] shapes = (ImageAreaShape[])t.Second;
                object[] states = (object[])t.Third;

                for(idx = 0; idx < count; idx++)
                {
                    switch(shapes[idx])
                    {
                        case ImageAreaShape.Rectangle:
                            a = new EWSoftware.ImageMaps.Web.Controls.ImageAreaRectangle();
                            break;

                        case ImageAreaShape.Circle:
                            a = new EWSoftware.ImageMaps.Web.Controls.ImageAreaCircle();
                            break;

                        case ImageAreaShape.Polygon:
                            a = new EWSoftware.ImageMaps.Web.Controls.ImageAreaPolygon();
                            break;

                        default:
                            throw new ArgumentException("Unexpected shape type in view state", "state");
                    }

                    a.TrackViewState();
                    a.LoadViewState(states[idx]);
                    this.Add(a);
                }

                return;
            }

            ArrayList changedIndices = (ArrayList)t.First, changedShapes = (ArrayList)t.Second,
                changedStates = (ArrayList)t.Third;

            for(idx = 0; idx < changedIndices.Count; idx++)
            {
                itemIdx = (int)changedIndices[idx];

                if(itemIdx < this.Count)
                {
                    ((EWSoftware.ImageMaps.Web.Controls.ImageAreaBase)this[itemIdx]).LoadViewState(changedStates[idx]);
                }
                else
                {
                    switch((ImageAreaShape)changedShapes[idx])
                    {
                        case ImageAreaShape.Rectangle:
                            a = new EWSoftware.ImageMaps.Web.Controls.ImageAreaRectangle();
                            break;

                        case ImageAreaShape.Circle:
                            a = new EWSoftware.ImageMaps.Web.Controls.ImageAreaCircle();
                            break;

                        case ImageAreaShape.Polygon:
                            a = new EWSoftware.ImageMaps.Web.Controls.ImageAreaPolygon();
                            break;

                        default:
                            throw new ArgumentException("Unexpected shape type in view state", "state");
                    }

                    a.TrackViewState();
                    a.LoadViewState(changedStates[idx]);
                    this.Add(a);
                }
            }
        }
        #endregion
    }
}
