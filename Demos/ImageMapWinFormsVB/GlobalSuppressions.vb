'
'
' This file is used by Code Analysis to maintain SuppressMessage 
' attributes that are applied to this project.
' Project-level suppressions either have no target or are given 
' a specific target and scoped to a namespace, type, member, etc.
'
' To add a suppression to this file, right-click the message in the 
' Code Analysis results, point to "Suppress Message", and click 
' "In Suppression File".
' You do not need to add suppressions to this file manually.

Imports System.Diagnostics.CodeAnalysis

<Assembly: SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope:="member", Target:="ImageMapWinForms.ImageMapEventsForm.#VisitWebSite_DrawImage(System.Object,EWSoftware.ImageMaps.Windows.Forms.DrawImageEventArgs)")>
<Assembly: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope:="member", Target:="ImageMapWinForms.ImageMapEventsForm.#VisitWebSite_DoubleClick(System.Object,EWSoftware.ImageMaps.ImageMapClickEventArgs)")>
<Assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification:="<Pending>", Scope:="member", Target:="~M:ImageMapWinForms.ImageMapPropertyForm.btnHelp_Click(System.Object,System.EventArgs)")>
