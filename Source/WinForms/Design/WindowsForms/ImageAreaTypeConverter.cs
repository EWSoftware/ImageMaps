//===============================================================================================================
// System  : Image Map Control Library
// File    : ImageAreaTypeConverter.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 12/31/2024
// Note    : Copyright 2004-2024, Eric Woodruff, All rights reserved
//
// This file contains the image area type converter
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
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace EWSoftware.ImageMaps.Design.Windows.Forms
{
#pragma warning disable CA1812

    /// <summary>
    /// This type converter is used on the Windows Forms image area objects so that they can be serialized to
    /// code.
    /// </summary>
    internal sealed class ImageAreaTypeConverter : TypeConverter
    {
        /// <summary>
        /// Gets a value indicating whether this converter can convert an object to the given destination type
        /// using the context.
        /// </summary>
        /// <param name="context">The format context</param>
        /// <param name="destinationType">The type to which to convert</param>
        /// <returns>Returns true if it can perform the conversion or false if it cannot</returns>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            if(destinationType == typeof(InstanceDescriptor))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Convert the specified object to the specified type
        /// </summary>
        /// <param name="context">A formatter context</param>
        /// <param name="culture">Culture-specific information</param>
        /// <param name="value">The object to convert</param>
        /// <param name="destinationType">The type to which to convert</param>
        /// <returns>The converted object</returns>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value,
          Type destinationType)
        {
            if(destinationType == typeof(InstanceDescriptor))
            {
                if((value as ImageMaps.Windows.Forms.ImageAreaRectangle) != null)
                {
                    ConstructorInfo ctor =
                        typeof(ImageMaps.Windows.Forms.ImageAreaRectangle).GetConstructor(
                        Type.EmptyTypes)!;

                    return new InstanceDescriptor(ctor, null, false);
                }

                if((value as ImageMaps.Windows.Forms.ImageAreaCircle) != null)
                {
                    ConstructorInfo ctor = typeof(
                        ImageMaps.Windows.Forms.ImageAreaCircle).GetConstructor(
                        Type.EmptyTypes)!;

                    return new InstanceDescriptor(ctor, null, false);
                }

                if((value as ImageMaps.Windows.Forms.ImageAreaEllipse) != null)
                {
                    ConstructorInfo ctor = typeof(
                        ImageMaps.Windows.Forms.ImageAreaEllipse).GetConstructor(
                        Type.EmptyTypes)!;

                    return new InstanceDescriptor(ctor, null, false);
                }

                if((value as ImageMaps.Windows.Forms.ImageAreaPolygon) != null)
                {
                    ConstructorInfo ctor = typeof(
                        ImageMaps.Windows.Forms.ImageAreaPolygon).GetConstructor(
                        Type.EmptyTypes)!;

                    return new InstanceDescriptor(ctor, null, false);
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
