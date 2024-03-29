'================================================================================================================
' System  : Image Map Control Library
' File    : AssemblyInfo.vb
' Author  : Eric Woodruff  (Eric@EWoodruff.us)
' Updated : 01/03/2023
' Note    : Copyright 2004-2023, Eric Woodruff, All rights reserved
'
' Image Map Windows Forms control demo
'
' This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
' distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
' This notice, the author's name, and all copyright notices must remain intact in all applications,
' documentation, and source files.
'
'    Date     Who  Comments
' ===============================================================================================================
' 07/01/2004  EFW  Created the code
'================================================================================================================

Imports System.Reflection
Imports System.Resources
Imports System.Runtime.InteropServices

' General assembly information
<Assembly: AssemblyProduct("EWSoftware Windows Forms Image Map Controls")>
<Assembly: AssemblyTitle("Image Map Windows Forms Control Demo in C#")>
<Assembly: AssemblyDescription("Demonstrates the use of the Windows Forms Image Map controls.")>
<Assembly: AssemblyCompany("Eric Woodruff")>
<Assembly: AssemblyCopyright("Copyright \xA9 2004-2023, Eric Woodruff, All Rights Reserved")>
<Assembly: AssemblyCulture("")>
#If DEBUG
<Assembly: AssemblyConfiguration("Debug")>
#Else
<Assembly: AssemblyConfiguration("Release")>
#End If

' The assembly is CLS compliant
<Assembly: CLSCompliant(True)>

' Not visible to COM
<Assembly: ComVisible(False)>

' Resources contained within the assembly are English
<Assembly: NeutralResourcesLanguage("en")>

#If NET6_0_OR_GREATER
<Assembly: Runtime.Versioning.SupportedOSPlatform("windows")>
#End If

' Version numbers.  All version numbers for an assembly consists of the following four values:
'
'   Year of release
'   Month of release
'   Day of release
'   Revision (typically zero unless multiple releases are made on the same day)
'
<Assembly: AssemblyVersion("2023.1.3.0")>
<Assembly: AssemblyFileVersion("23.1.3.0")>
