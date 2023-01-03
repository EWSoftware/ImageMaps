//===============================================================================================================
// System  : Image Map Control Library
// File    : UnsafeNativeMethods.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/03/2023
// Note    : Copyright 2004-2023, Eric Woodruff, All rights reserved
//
// This internal class is used for access to some Win32 functions used to draw the focus frame on the image
// areas in the designer and Windows Forms image map control.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/14/2004  EFW  Created the code
// 12/23/2004  EFW  Added support for the ellipse image area
//===============================================================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace EWSoftware.ImageMaps
{
    /// <summary>
    /// This internal class is used for access to some Win32 functions used to draw the focus frame on the image
    /// areas in the designer and Windows Forms image map control.
    /// </summary>
    /// <remarks>To the best of my knowledge, all native Win32 GDI functions called are safe</remarks>
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        #region Constants
        //=====================================================================

        private const int PS_SOLID = 0;
        private const int R2_NOTXORPEN = 10;
        private const int NULL_BRUSH = 5;

        #endregion

        #region Common native Win32 functions
        //=====================================================================

        [DllImport("gdi32.dll")]
        private static extern int SetROP2(IntPtr hdc, int fnDrawMode);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreatePen(int fnPenStyle, int nWidth, int crColor);

        [DllImport("gdi32.dll")]
        private static extern IntPtr GetStockObject(int fnObject);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        private static extern int SetBkColor(IntPtr hdc, int crColor);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect,
            int nBottomRect);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Ellipse(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect,
            int nBottomRect);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool MoveToEx(IntPtr hdc, int X, int Y, IntPtr lpPoint);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);
        #endregion

        #region Common methods
        //=====================================================================

        /// <summary>
        /// This is called to draw a reversible rectangle
        /// </summary>
        /// <param name="g">The graphics object</param>
        /// <param name="p1">The upper left corner of the rectangle</param>
        /// <param name="p2">The lower right corner of the rectangle</param>
        /// <param name="offset">An amount to offset the coordinates when drawing in a scrolled view</param>
        internal static void DrawReversibleRectangle(Graphics g, Point p1, Point p2, Point offset)
        {
            IntPtr hDC = g.GetHdc();

            IntPtr pen = CreatePen(PS_SOLID, 1, ColorTranslator.ToWin32(Color.Black));

            int oldROP = SetROP2(hDC, R2_NOTXORPEN);
            IntPtr oldBrush = SelectObject(hDC, GetStockObject(NULL_BRUSH));
            IntPtr oldPen = SelectObject(hDC, pen);

            SetBkColor(hDC, ColorTranslator.ToWin32(Color.White));

            p1.Offset(offset.X, offset.Y);
            p2.Offset(offset.X, offset.Y);

            Rectangle(hDC, p1.X, p1.Y, p2.X, p2.Y);

            SelectObject(hDC, oldPen);
            SelectObject(hDC, oldBrush);
            SetROP2(hDC, oldROP);
            DeleteObject(pen);

            g.ReleaseHdc(hDC);
        }

        /// <summary>
        /// This is called to draw a reversible circle
        /// </summary>
        /// <param name="g">The graphics object</param>
        /// <param name="p">The center point of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="offset">An amount to offset the coordinates when drawing in a scrolled view</param>
        internal static void DrawReversibleCircle(Graphics g, Point p, int radius, Point offset)
        {
            IntPtr hDC = g.GetHdc();

            IntPtr pen = CreatePen(PS_SOLID, 1, ColorTranslator.ToWin32(Color.Black));

            int oldROP = SetROP2(hDC, R2_NOTXORPEN);
            IntPtr oldBrush = SelectObject(hDC, GetStockObject(NULL_BRUSH));
            IntPtr oldPen = SelectObject(hDC, pen);

            SetBkColor(hDC, ColorTranslator.ToWin32(Color.White));

            p.Offset(offset.X, offset.Y);

            Ellipse(hDC, p.X - radius, p.Y - radius, p.X + radius, p.Y + radius);

            SelectObject(hDC, oldPen);
            SelectObject(hDC, oldBrush);
            SetROP2(hDC, oldROP);
            DeleteObject(pen);

            g.ReleaseHdc(hDC);
        }

        /// <summary>
        /// This is called to draw a reversible ellipse
        /// </summary>
        /// <param name="g">The graphics object</param>
        /// <param name="p1">The upper left corner of the rectangle</param>
        /// <param name="p2">The lower right corner of the rectangle</param>
        /// <param name="offset">An amount to offset the coordinates when drawing in a scrolled view</param>
        internal static void DrawReversibleEllipse(Graphics g, Point p1, Point p2, Point offset)
        {
            IntPtr hDC = g.GetHdc();

            IntPtr pen = CreatePen(PS_SOLID, 1, ColorTranslator.ToWin32(Color.Black));

            int oldROP = SetROP2(hDC, R2_NOTXORPEN);
            IntPtr oldBrush = SelectObject(hDC, GetStockObject(NULL_BRUSH));
            IntPtr oldPen = SelectObject(hDC, pen);

            SetBkColor(hDC, ColorTranslator.ToWin32(Color.White));

            p1.Offset(offset.X, offset.Y);
            p2.Offset(offset.X, offset.Y);

            Ellipse(hDC, p1.X, p1.Y, p2.X, p2.Y);

            SelectObject(hDC, oldPen);
            SelectObject(hDC, oldBrush);
            SetROP2(hDC, oldROP);
            DeleteObject(pen);

            g.ReleaseHdc(hDC);
        }

        /// <summary>
        /// This is called to draw a reversible polygon
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="points">An enumerable list of points used to draw the polygon.</param>
        /// <param name="closePolygon">If true, a final line is automatically drawn from the last point to the
        /// starting point.</param>
        /// <param name="offset">An amount to offset the coordinates when drawing in a scrolled view.</param>
        internal static void DrawReversiblePolygon(Graphics g, IEnumerable<Point> points, bool closePolygon,
          Point offset)
        {
            Point pt, firstPoint = Point.Empty;
            bool isFirstPoint = true;

            IntPtr hDC = g.GetHdc();

            IntPtr pen = CreatePen(PS_SOLID, 1, ColorTranslator.ToWin32(Color.Black));

            int oldROP = SetROP2(hDC, R2_NOTXORPEN);
            IntPtr oldBrush = SelectObject(hDC, GetStockObject(NULL_BRUSH));
            IntPtr oldPen = SelectObject(hDC, pen);

            SetBkColor(hDC, ColorTranslator.ToWin32(Color.White));

            foreach(Point p in points)
            {
                pt = p;
                pt.Offset(offset.X, offset.Y);

                if(isFirstPoint)
                {
                    isFirstPoint = false;
                    firstPoint = pt;
                    MoveToEx(hDC, pt.X, pt.Y, IntPtr.Zero);
                }
                else
                    LineTo(hDC, pt.X, pt.Y);
            }

            if(closePolygon && !isFirstPoint)
                LineTo(hDC, firstPoint.X, firstPoint.Y);

            SelectObject(hDC, oldPen);
            SelectObject(hDC, oldBrush);
            SetROP2(hDC, oldROP);
            DeleteObject(pen);

            g.ReleaseHdc(hDC);
        }
        #endregion

        #region Windows Forms image map stuff
        //=====================================================================

#if !IMAGEMAPWEB
        private const uint TME_HOVER = 1;
        private const uint HOVER_DEFAULT = 0xFFFFFFFF;

        private struct TRACKMOUSEEVENT
        {
            public uint cbSize;
            public uint dwFlags;
            public IntPtr hwndTrack;
            public uint dwHoverTime;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

        /// <summary>
        /// Reset the mouse hover timer
        /// </summary>
        /// <param name="handle">The handle of the control in which to reset hover notification</param>
        internal static void ResetMouseHover(IntPtr handle)
        {
            TRACKMOUSEEVENT tme = new TRACKMOUSEEVENT
            {
                hwndTrack = handle,
                dwFlags = TME_HOVER,
                dwHoverTime = HOVER_DEFAULT,
            };

            tme.cbSize = (uint)Marshal.SizeOf(tme);

            TrackMouseEvent(ref tme);
        }

#endif
        #endregion
    }
}
