'================================================================================================================
' System  : Image Map Control Library
' File    : ImageMapPropertyForm.vb
' Author  : Eric Woodruff  (Eric@EWoodruff.us)
' Updated : 01/03/2023
' Note    : Copyright 2004-2023, Eric Woodruff, All rights reserved
'
' This form is used to demonstrate the Image Map Windows Forms controls.  This form allows the user to modify
' the image map and image area properties interactively and see the effects.
'
' This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
' distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
' This notice, the author's name, and all copyright notices must remain intact in all applications,
' documentation, and source files.
'
'    Date     Who  Comments
' ===============================================================================================================
' 07/01/2004  EFW  Created the code
' 07/08/2006  EFW  Updated for use with .NET 2.0
'================================================================================================================

Imports System.Globalization
Imports System.IO

Imports EWSoftware.ImageMaps
Imports EWSoftware.ImageMaps.Windows.Forms

Public Class ImageMapPropertyForm
    Inherits System.Windows.Forms.Form

    #Region "Constructor"
    '==========================================================================

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        pgImageMapProps.SelectedObject = imMap
		pgImageMapProps.Refresh()
    End Sub

    #End Region

    #Region "Event handlers"
    '==========================================================================

    ''' <summary>
    ''' Refresh the display when properties are changed
    ''' </summary>
    ''' <param name="s">The sender of the event</param>
    ''' <param name="e">The event arguments</param>
    Private Sub pgImageMapProps_PropertyValueChanged(ByVal s As Object, _
      ByVal e As PropertyValueChangedEventArgs) Handles pgImageMapProps.PropertyValueChanged
        imMap.Invalidate()
        imMap.Update()
    End Sub

    ''' <summary>
    ''' A simple image area click handler
    ''' </summary>
    ''' <param name="sender">The sender of the event</param>
    ''' <param name="e">The event arguments</param>
    Private Sub imMap_Click(ByVal sender As Object, ByVal e As ImageMapClickEventArgs) Handles imMap.Click
        MessageBox.Show(String.Format(CultureInfo.CurrentCulture, "You clicked area #{0} ({1}) at point " &
            "{2}, {3}", e.AreaIndex + 1, imMap.Areas(e.AreaIndex).ToolTip, e.XCoordinate, e.YCoordinate),
            "Image Map Clicked", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>
    ''' Show the Image Map control help file if it can be found
    ''' </summary>
    ''' <param name="sender">The sender of the event</param>
    ''' <param name="e">The event arguments</param>
    Private Sub btnHelp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHelp.Click
        Try
            Process.Start(New ProcessStartInfo With {
                .FileName = "https://ewsoftware.github.io/ImageMaps",
                .UseShellExecute = true
            })
        Catch ex As Exception
            MessageBox.Show("Unable to start web browser for URL", "URL Error", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation)

            ' Log the exception to the debugger for the developer
            Debug.Write(ex.ToString())
        End Try
    End Sub

    ''' <summary>
    ''' Close this form
    ''' </summary>
    ''' <param name="sender">The sender of the event</param>
    ''' <param name="e">The event arguments</param>
    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    #End Region

End Class
