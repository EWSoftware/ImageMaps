//===============================================================================================================
// System  : Image Map Control Library
// File    : AssemblyInfo.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/03/2023
// Note    : Copyright 2004-2023, Eric Woodruff, All rights reserved
//
// Image map control library attributes
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

using System.Reflection;
using System.Security;
using System.Web.UI;

// General assembly information
[assembly: AssemblyTitle("EWSoftware Web Server Image Map Control Library")]

// Allow partially trusted callers when running with less than full trust
[assembly: AllowPartiallyTrustedCallers()]

// This prevents the website compiler from complaining about the security inheritance level of the designer
[assembly: SecurityRules(SecurityRuleSet.Level1)]

// ASP.NET tag prefix for the custom controls
[assembly: TagPrefix("EWSoftware.ImageMaps.Web.Controls", "ewsi")]

#if NET8_0_OR_GREATER
[assembly: System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
