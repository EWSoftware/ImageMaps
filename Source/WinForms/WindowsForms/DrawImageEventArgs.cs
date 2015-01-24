//===============================================================================================================
// System  : Image Map Control Library
// File    : DrawImageEventArgs.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/09/2014
// Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the event arguments class for the image map owner draw event
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 06/28/2006  EFW  Created the code
//===============================================================================================================

using System;
using System.Drawing;

namespace EWSoftware.ImageMaps.Windows.Forms
{
    /// <summary>
    /// This is a custom event arguments class for the
    /// <see cref="EWSoftware.ImageMaps.Windows.Forms.ImageMap.DrawImage"/>
    /// event.
    /// </summary>
    /// <remarks>The event contains the graphics context, the drawing state of the item, an offset if the image
    /// is scrolled or centered, and a flag that can be use to indicate whether or not the base class should
    /// draw the default focus frame around the active image area.</remarks>
    /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex4']/*" />
    public class DrawImageEventArgs : EventArgs
    {
        #region Properties
        //=====================================================================

        /// <summary>
        /// This read-only property is used to get the graphics context for drawing
        /// </summary>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex4']/*" />
        public Graphics Graphics { get; private set; }

        /// <summary>
        /// This property is used to get or set the state of the item to draw
        /// </summary>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex4']/*" />
        public DrawState DrawState { get; set; }

        /// <summary>
        /// This read-only property is used to get the image offset
        /// </summary>
        /// <value>If the image map is scrolled or centered, this represent the offset to use in order to
        /// position drawn items correctly on the image map.
        /// </value>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex4']/*" />
        public Point ImageOffset { get; private set; }

        /// <summary>
        /// This property is used to get or set whether the image map should draw the default focus frame around
        /// the active image area.
        /// </summary>
        /// <value><para>If true, the image map will draw the default focus frame around the active image area.
        /// If false, it will assume that the owner draw event has done so and will not draw it.</para>
        /// 
        /// <para>For the image map <see cref="ImageMap.DrawImage"/> event, it defaults to true so that the image
        /// map draws the focus frame on the active area.  It can be set to false to cancel this behavior.</para>
        /// 
        /// <para>For the image area <see cref="ImageAreaBase.DrawImage"/> event, it defaults to false and
        /// assumes that the owner draw event will draw some sort of focus indicator for the image area.  If this
        /// is not the case, you can set it to true to have the image map draw the default focus frame around the
        /// active image area.</para></value>
        /// <include file="IMExamples.xml" path="Examples/ImageMap/HelpEx[@name='Ex4']/*" />
        public bool DrawFocus { get; set; }

        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>Constructor</summary>
        /// <param name="graphics">The graphics context</param>
        /// <param name="state">The drawing state of the item</param>
        /// <param name="offset">The image offset if scrolled</param>
        /// <param name="drawFocus">A flag indicating whether or not to draw the focus rectangle on return</param>
        public DrawImageEventArgs(Graphics graphics, DrawState state, Point offset, bool drawFocus)
        {
            this.Graphics = graphics;
            this.DrawState = state;
            this.ImageOffset = offset;
            this.DrawFocus = drawFocus;
        }
        #endregion
    }
}
