//===============================================================================================================
// System  : Image Map Control Library
// File    : AssemblyInfo.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/03/2023
// Note    : Copyright 2004-2023, Eric Woodruff, All rights reserved
//
// Image Map Windows Forms control demo
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
// This notice, the author's name, and all copyright notices must remain intact in all applications,
// documentation, and source files.
//
//    Date     Who  Comments
// ==============================================================================================================
// 07/01/2004  EFW  Created the code
//===============================================================================================================

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General assembly information
[assembly: AssemblyProduct("EWSoftware Windows Forms Image Map Controls")]
[assembly: AssemblyTitle("Image Map Windows Forms Control Demo in C#")]
[assembly: AssemblyDescription("Demonstrates the use of the Windows Forms Image Map controls.")]
[assembly: AssemblyCompany("Eric Woodruff")]
[assembly: AssemblyCopyright("Copyright \xA9 2004-2023, Eric Woodruff, All Rights Reserved")]
[assembly: AssemblyCulture("")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

// The assembly is CLS compliant
[assembly: CLSCompliant(true)]

// Not visible to COM
[assembly: ComVisible(false)]

// Resources contained within the assembly are English
[assembly: NeutralResourcesLanguage("en")]

#if NET6_0_OR_GREATER
[assembly: System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif

// Version numbers.  All version numbers for an assembly consists of the following four values:
//
//      Year of release
//      Month of release
//      Day of release
//      Revision (typically zero unless multiple releases are made on the same day)
//
[assembly: AssemblyVersion("2023.1.3.0")]
[assembly: AssemblyFileVersion("23.1.3.0")]
