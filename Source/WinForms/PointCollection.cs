//===============================================================================================================
// System  : Image Map Control Library
// File    : PointCollection.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/09/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a type-safe Point collection class and its enumerator.
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace EWSoftware.ImageMaps
{
	/// <summary>
	/// A type-safe collection of <see cref="Point"/> objects.
	/// </summary>
    /// <remarks>The class has a type-safe enumerator</remarks>
	[Editor(typeof(CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [Serializable]
    public class PointCollection : Collection<Point>
    {
        #region Constructors
        //=====================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <overloads>There are two overloads for the constructor</overloads>
        public PointCollection()
        {
        }

        /// <summary>
        /// Construct a collection from an enumerable list of <see cref="Point"/> objects
        /// </summary>
        /// <param name="points">An enumerable list of <see cref="Point"/> objects to add</param>
        public PointCollection(IEnumerable<Point> points)
        {
            this.AddRange(points);
        }
        #endregion

        #region Helper methods
        //=====================================================================

        /// <summary>
        /// Add a range of <see cref="Point"/> objects from an enumerable list
        /// </summary>
        /// <param name="points">An enumerable list of <see cref="Point"/> objects to add</param>
        public void AddRange(IEnumerable<Point> points)
        {
            if(points != null)
                foreach(Point p in points)
                    this.Add(p);
        }
        #endregion
    }
}
