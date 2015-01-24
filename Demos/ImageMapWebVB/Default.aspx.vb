'================================================================================================================
' System  : Image Map Control Library
' File    : Default.aspx.vb
' Author  : Eric Woodruff  (Eric@EWoodruff.us)
' Updated : 07/10/2014
' Note    : Copyright 2004-2014, Eric Woodruff, All rights reserved
' Compiler: Microsoft C#
'
' This page is used to demonstrate the Image Map Web Server controls.
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

Imports System.Drawing
Imports System.Globalization

Imports EWSoftware.ImageMaps
Imports EWSoftware.ImageMaps.Web.Controls

Namespace ImageMapWebVB

Partial Class DefaultPage
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' This is used to add a dynamic image area on the initial page load
    ''' </summary>
    ''' <param name="sender">The sender of the event</param>
    ''' <param name="e">The event arguments</param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            lblEnabledMsg.Text = "Image map enabled"

            ' Add a dynamic image area.  This is only created once when the page is first loaded.  On post backs,
            ' the area is recreated from view state.
            Dim r As New ImageAreaRectangle(New Rectangle(40, 57, 20, 20))

            r.Action = AreaClickAction.PostBack
            r.ToolTip = "Area 5 (Dynamically added)"
            r.AccessKey = "5"
            r.Attributes.Add("onmouseover", "javascript: IM_Highlight(5);")
            r.Attributes.Add("onmouseout", "javascript: IM_Highlight(0);")

            ' Tell the item to mark all properties as dirty so that it is restored entirely from view state on
            ' post backs.  If not done, you must recreate the area on post backs.
            r.MarkAsDirty()

            imClickMap.Areas.Add(r)
        End If

        ' The Visual Studio designer loses the custom attributes on the image area controls in the HTML when the
        ' collection editor is used.  This happens to ListItem objects in controls such as the ListBox and
        ' DropDown controls too.  The only workaround is to set such attributes in code as shown below when the
        ' page loads.
        DirectCast(imClickMap.Areas(0), ImageAreaBase).Attributes.Add("onmouseover", "javascript: IM_Highlight(1);")
        DirectCast(imClickMap.Areas(0), ImageAreaBase).Attributes.Add("onmouseout", "javascript: IM_Highlight(0);")
        DirectCast(imClickMap.Areas(1), ImageAreaBase).Attributes.Add("onmouseover", "javascript: IM_Highlight(2);")
        DirectCast(imClickMap.Areas(1), ImageAreaBase).Attributes.Add("onmouseout", "javascript: IM_Highlight(0);")
        DirectCast(imClickMap.Areas(2), ImageAreaBase).Attributes.Add("onmouseover", "javascript: IM_Highlight(3);")
        DirectCast(imClickMap.Areas(2), ImageAreaBase).Attributes.Add("onmouseout", "javascript: IM_Highlight(0);")
        DirectCast(imClickMap.Areas(3), ImageAreaBase).Attributes.Add("onmouseover", "javascript: IM_Highlight(4);")
        DirectCast(imClickMap.Areas(3), ImageAreaBase).Attributes.Add("onmouseout", "javascript: IM_Highlight(0);")

    End Sub

    ' These are used in the help file examples
    #Region "Click event examples"

    ' This event is fired by the image area at the bottom of the left-side image
    ' map.  The parameters are not used and it does not cause validation events
    ' to fire.  It's used to enable or disable the right-side image map.
    Private Sub imMap_Click(ByVal sender As Object,
        ByVal e As ImageMapClickEventArgs) Handles imMap.Click

        imClickMap.Enabled = Not imClickMap.Enabled
        lblClickMsg.Text = String.Empty
        lblEnabledMsg.Text = String.Format(CultureInfo.CurrentCulture, "Image map {0}",
            IIf(imClickMap.Enabled, "enabled", "disabled"))

        ' Apply a filter to "gray out" the image map and change the tool tip
        If Not imClickMap.Enabled Then
            imClickMap.Style.Add("opacity", ".25")
            imClickMap.Style.Add("filter", "Alpha(Opacity=25)")  ' For older browsers
            imClickMap.ToolTip = "Disabled"
        Else
            imClickMap.Style.Remove("opacity")
            imClickMap.Style.Remove("filter")
            imClickMap.ToolTip = "Click an area to post back"
        End If
    End Sub

    ''' <summary>
    ''' This event is fired by the image area at the bottom of the left-side
    ''' image map.
    ''' </summary>
    ''' <param name="sender">The sender of the event</param>
    ''' <param name="e">The event arguments</param>
    ''' <remarks>The parameters are not used and it does not cause validation
    ''' events to fire.  It is used to enable or disable the right-side image
    ''' map.</remarks>
    Private Sub imClickMap_Click(ByVal sender As Object,
        ByVal e As ImageMapClickEventArgs) Handles imClickMap.Click

        Dim clickCount As Integer = 0

        If Page.IsValid Then
            lblClickMsg.Text = String.Format(CultureInfo.CurrentCulture, "Clicked Area #{0}", e.AreaIndex + 1)

            ' X and Y are only sent back by browsers that support the event.offsetX
            ' and event.offsetY properties
            If e.XCoordinate <> -1 Then
                lblClickMsg.Text += String.Format(CultureInfo.CurrentCulture, "<br>At X,Y {0},{1}", e.XCoordinate,
                    e.YCoordinate)
            End If

            ' Track the click count in the Tag property to test view state
            If Not (imClickMap.Areas(e.AreaIndex).Tag Is Nothing) Then
                clickCount = DirectCast(imClickMap.Areas(e.AreaIndex).Tag, Integer)
            End If

            clickCount += 1
            lblClickMsg.Text += String.Format(CultureInfo.CurrentCulture, "<br>It has been clicked {0} times",
                clickCount)

            imClickMap.Areas(e.AreaIndex).Tag = clickCount
        End If
    End Sub

    #End Region
End Class

End Namespace
