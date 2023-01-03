'================================================================================================================
' System  : Image Map Control Library
' File    : ImageMapEventsForm.vb
' Author  : Eric Woodruff  (Eric@EWoodruff.us)
' Updated : 01/03/2023
' Note    : Copyright 2004-2023, Eric Woodruff, All rights reserved
'
' This form is used to demonstrate the handling of the various Image Map Windows Forms control events and the
' owner draw capabilities.
'
' This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
' distributed with the code and can be found at the project website: https://github.com/EWSoftware/ImageMaps.
' This notice, the author's name, and all copyright notices must remain intact in all applications,
' documentation, and source files.
'
'    Date     Who  Comments
' ===============================================================================================================
' 12/27/2004  EFW  Created the code
' 07/08/2006  EFW  Updated for use with .NET 2.0
'================================================================================================================

Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Globalization

Imports EWSoftware.ImageMaps
Imports EWSoftware.ImageMaps.Windows.Forms

Public Class ImageMapEventsForm
    Inherits System.Windows.Forms.Form

    #Region "Private data members"
    '==========================================================================

    ' Private data members for the demo
    Private ReadOnly buttonFont, hyperlinkFont As Font
    Private ReadOnly areaOwnerDrawOnOff, areaVisitWebSite As ImageAreaRectangle
    Private ReadOnly sfFormat As StringFormat

	Private ReadOnly imgFiller As Image

	Private ReadOnly iaNormal, iaDisabled As ImageAttributes
	Private ReadOnly cmNormal, cmDisabled As ColorMatrix

    #End Region

    #Region "Constructor"
    '==========================================================================

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        MyBase.New()

        InitializeComponent()

        ' This is used to format text for various image areas
        sfFormat = New StringFormat With {
            .Alignment = StringAlignment.Center,
            .LineAlignment = StringAlignment.Center,
            .HotkeyPrefix = HotkeyPrefix.Show
        }

            ' A couple of fonts for some of the image areas
        buttonFont = New Font("Segoe UI", 9!, FontStyle.Bold)
        hyperlinkFont = New Font("Segoe UI", 9!, FontStyle.Underline)

        ' Get the image for the "buttons" and define the attributes
        ' used to given them their various draw state effects.
		imgFiller = New Bitmap(My.Resources.Filler)

        ' Image attributes used to lighten normal buttons
        cmNormal = New ColorMatrix With {
            .Matrix33 = 0.5!
        }

        iaNormal = New ImageAttributes()
		iaNormal.SetColorMatrix(cmNormal, ColorMatrixFlag.Default, ColorAdjustType.Bitmap)

        ' Image attributes that lighten and desaturate disabled buttons
        cmDisabled = New ColorMatrix With {
            .Matrix00 = 0.33!,
            .Matrix01 = 0.33!,
            .Matrix02 = 0.33!,
            .Matrix10 = 0.33!,
            .Matrix11 = 0.33!,
            .Matrix12 = 0.33!,
            .Matrix20 = 0.33!,
            .Matrix21 = 0.33!,
            .Matrix22 = 0.33!,
            .Matrix33 = 0.5!
        }

        iaDisabled = New ImageAttributes()
		iaDisabled.SetColorMatrix(cmDisabled, ColorMatrixFlag.Default, ColorAdjustType.Bitmap)

        ' Hook up the event handlers.  Since they are not accessible in the designer, we must do it manually.
        ' Hang on to the references as we use them fairly often in here.
        areaOwnerDrawOnOff = DirectCast(imMap.Areas(0), ImageAreaRectangle)
        AddHandler areaOwnerDrawOnOff.Click, AddressOf areaOwnerDrawOnOff_Click
        AddHandler areaOwnerDrawOnOff.DrawImage, AddressOf OwnerDrawOnOff_DrawImage

        areaVisitWebSite = DirectCast(imMap.Areas(3), ImageAreaRectangle)
        AddHandler areaVisitWebSite.DrawImage, AddressOf VisitWebSite_DrawImage
        AddHandler areaVisitWebSite.DoubleClick, AddressOf VisitWebSite_DoubleClick

        ' The same draw event handler is used for the three "buttons".  The image map Click event handles the
        ' area click events.
        Dim a As ImageAreaBase = DirectCast(imMap.Areas(4), ImageAreaBase)
        AddHandler a.DrawImage, AddressOf Button_DrawImage

        a = DirectCast(imMap.Areas(5), ImageAreaBase)
        AddHandler a.DrawImage, AddressOf Button_DrawImage

        a = DirectCast(imMap.Areas(6), ImageAreaBase)
        AddHandler a.DrawImage, AddressOf Button_DrawImage

        ' These two just demonstrate mouse and focus event handling by the image areas
        a = DirectCast(imMap.Areas(1), ImageAreaBase)
        AddHandler a.DrawImage, AddressOf MouseArea_DrawImage
        AddHandler a.MouseEnter, AddressOf MouseArea_MouseEnter
        AddHandler a.MouseDown, AddressOf MouseArea_MouseDown
        AddHandler a.MouseUp, AddressOf MouseArea_MouseUp
        AddHandler a.MouseLeave, AddressOf MouseArea_MouseLeave

        ' These two tend to obscure the other mouse events.  You can uncomment them to see them in action.
'        AddHandler a.MouseMove, AddressOf MouseArea_MouseMove
'        AddHandler a.MouseHover, AddressOf MouseArea_MouseHover

        a = DirectCast(imMap.Areas(2), ImageAreaBase)
        AddHandler a.DrawImage, AddressOf FocusArea_DrawImage
        AddHandler a.Enter, AddressOf FocusArea_Enter
        AddHandler a.Leave, AddressOf FocusArea_Leave
    End Sub

    #End Region

    #Region "Event handlers"

    ''' <summary>
    ''' The image map can handle click events or the areas can handle their own events.  In this demo, the image
    ''' map handles the click event for areas 1, 2, 5, and 6.  Area 0 has it's own Click handler and area 3 only
    ''' responds to double-clicks with its own DoubleClick handler.
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image map)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub imMap_Click(sender As Object, e As ImageMapClickEventArgs) Handles imMap.Click
        ' We'll handle the click for image area 0 (toggle the image map's owner status)
        Select Case imMap.FocusedArea
            Case 5      ' Show the interactive property demo
                Using dlg As New ImageMapPropertyForm()
                    dlg.ShowDialog()
                End Using

            Case 6      ' Exit the application
                Me.Close()

            Case Else
                If imMap.FocusedArea <> 0 And imMap.FocusedArea <> 3 Then
                    MessageBox.Show(String.Format(CultureInfo.CurrentCulture, "You clicked area #{0} " &
                        "({1}) at point {2}, {3}", e.AreaIndex + 1, imMap.Areas(e.AreaIndex).ToolTip,
                        e.XCoordinate, e.YCoordinate), "Image Map Clicked", MessageBoxButtons.OK,
                        MessageBoxIcon.Information)
                End If

        End Select
    End Sub

    ' This is used in the help file
    #Region "Owner drawn image map help example"

    ''' <summary>
    ''' When the image map is owner drawn, you can draw just the background or
    ''' you can draw the image areas too.
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image map)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub imMap_DrawImage(sender As Object, e As DrawImageEventArgs) _
      Handles imMap.DrawImage
        ' For this demo, we'll just draw a gradient fill background.
        ' You could get the image using the ImageMap.Image property too.
        ' Offset the drawing rectangle by the image offset in the event
        ' arguments.  This indicates the true position of the image when
        ' centered or scrolled.
        Dim r As New Rectangle(e.ImageOffset.X, e.ImageOffset.Y, _
            imMap.ImageMapWidth, imMap.ImageMapHeight)

        Using lgBrush As New LinearGradientBrush(r, _
          Color.White, Color.SteelBlue, LinearGradientMode.ForwardDiagonal)
            e.Graphics.FillRectangle(lgBrush, r)
        End Using

        e.Graphics.DrawString("All items seen on this form are drawn by " & _
            "the image map or image area DrawImage event handlers.  " & _
            "Even with owner draw disabled for the image map, image " & _
            "areas can still be set to owner drawn.", imMap.Font, _
            Brushes.Black, New RectangleF(r.X + 250, r.Y + 10, 325, 100), _
            sfFormat)

        ' The image map can draw areas too.  In this case, we defer to
        ' the event handlers as they'll take over if owner drawing of the
        ' image map is turned off.
        Me.OwnerDrawOnOff_DrawImage(sender, e)
        Me.VisitWebSite_DrawImage(sender, e)
    End Sub

    #End Region

    ''' <summary>
    ''' Draw the "Owner Draw" toggle image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image map or image area in this demo)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub OwnerDrawOnOff_DrawImage(sender As Object, e As DrawImageEventArgs)
        Dim r As Rectangle = areaOwnerDrawOnOff.Rectangle
        r.Inflate(-2, -2)

        ' Offset the area rectangle by the draw event offset
        r.Offset(e.ImageOffset.X, e.ImageOffset.Y)

        If TypeOf sender Is ImageMap Then
            ControlPaint.DrawCheckBox(e.Graphics, r.X + 5, r.Y, r.Height, r.Height, ButtonState.Checked)

            r.X += 5 + r.Height
            r.Width -= 5 + r.Height

            ' Use the image map's font
            e.Graphics.DrawString("&Image map owner draw ON", imMap.Font, Brushes.Black,
                New RectangleF(CType(r.X, Single), CType(r.Y, Single), CType(r.Width, Single),
                CType(r.Height, Single)), sfFormat)
        Else
            ControlPaint.DrawCheckBox(e.Graphics, r.X + 5, r.Y, r.Height, r.Height, ButtonState.Normal)

            r.X += 5 + r.Height
            r.Width -= 5 + r.Height
            e.Graphics.DrawString("&Image map owner draw OFF", imMap.Font, _
                Brushes.Black, New RectangleF(CType(r.X, Single), _
                CType(r.Y, Single), CType(r.Width, Single), _
                CType(r.Height, Single)), sfFormat)

            ' Tell the image map to draw the focus rectangle if this is the focused area.  It's False by default
            ' if the area is drawing itself.
            e.DrawFocus = True
        End If
    End Sub

    ''' <summary>
    ''' The click event handler for the owner draw on/off image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub areaOwnerDrawOnOff_Click(sender As Object, e As ImageMapClickEventArgs)
        ' Turn owner draw on/off for the image map
        imMap.OwnerDraw = Not imMap.OwnerDraw

        ' Disable owner drawing of the two areas when the image map is owner drawn.  It draws them in that case.
        areaOwnerDrawOnOff.OwnerDraw = Not imMap.OwnerDraw
        areaVisitWebSite.OwnerDraw = Not imMap.OwnerDraw
    End Sub

    ''' <summary>
    ''' Draw the "Visit web site" image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image map or image area in this demo)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub VisitWebSite_DrawImage(sender As Object, e As DrawImageEventArgs)
        Dim f As Font = imMap.Font   ' Use image map font if not focused
        Dim m As ImageMap

        If TypeOf sender Is ImageMap Then
            m = DirectCast(sender, ImageMap)

            ' If this area is focused or contains the mouse, underline the hyperlink text
            If m.FocusedArea = 3 Or m.MouseArea = 3 Then
                f = hyperlinkFont

                ' Tell the image map not to draw the focus.  It's True by default in the image map's owner draw
                ' event.
                If m.FocusedArea = 3 Then
                    e.DrawFocus = False
                End If
            End If
        Else
            ' If the image area is drawing itself, use the event arguments to determine the draw state.  If hot
            ' lighting or focused, use the underlined font.
            If e.DrawState = DrawState.HotLight Or e.DrawState = DrawState.Focus Then
                f = hyperlinkFont
            End If
        End If

        Dim r As Rectangle = areaVisitWebSite.Rectangle

        ' Offset the area rectangle by the draw event offset
        r.Offset(e.ImageOffset.X, e.ImageOffset.Y)

        e.Graphics.DrawString("https://github.com/EWSoftware/ImageMaps", f, Brushes.Blue,
            New RectangleF(CType(r.X, Single), CType(r.Y, Single), CType(r.Width, Single),
            CType(r.Height, Single)), sfFormat)

        r.Width = CType(e.Graphics.MeasureString("Double-click to go to ", imMap.Font).Width + 4, Integer)
        r.X = r.X - r.Width + 10
        e.Graphics.DrawString("&Double-click to go to ", imMap.Font, Brushes.Black,
            New RectangleF(CType(r.X, Single), CType(r.Y, Single), CType(r.Width, Single),
            CType(r.Height, Single)), sfFormat)
    End Sub

    ''' <summary>
    ''' When the "Visit web site" area is double-clicked, open the web page
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub VisitWebSite_DoubleClick(sender As Object, e As ImageMapClickEventArgs)
        Try
            Process.Start(New ProcessStartInfo With {
                .FileName = "https://github.com/EWSoftware/ImageMaps",
                .UseShellExecute = true
            })
        Catch ex As Exception
            MessageBox.Show("Unable to start web browser for URL", "URL Error", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation)

            ' Log the exception to the debugger for the developer
            Debug.Write(ex.ToString())
        End Try
    End Sub

    ' This is used in the help file
    #Region "Owner drawn area help example"

    ''' <summary>
    ''' Draw the "button" image areas
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub Button_DrawImage(sender As Object, e As DrawImageEventArgs)
        Dim ia As ImageAttributes
        Dim g As Graphics = e.Graphics

        ' All are ellipse image areas
        Dim a As ImageAreaEllipse = DirectCast(sender, ImageAreaEllipse)
        Dim r As Rectangle = a.Ellipse

        ' Offset the area rectangle by the draw event offset
        r.Offset(e.ImageOffset.X, e.ImageOffset.Y)

        If Not a.Enabled Then
            ia = iaDisabled
        Else
            ia = iaNormal
        End If

        Using tb As New TextureBrush(imgFiller, _
          New Rectangle(0, 0, imgFiller.Width, imgFiller.Height), ia)
            tb.WrapMode = WrapMode.Tile

            ' Translate the brush coordinates to account for the offset
            Using m As New Matrix()
                m.Translate(CType(r.X, Single), CType(r.Y, Single))
                tb.Transform = m

                ' If the area is focused or hot lighted, give it a glow effect
                If e.DrawState = DrawState.Focus Or e.DrawState = DrawState.HotLight Then
                    Using pth As New GraphicsPath()
                        pth.AddEllipse(r)

                        Using pgb As New PathGradientBrush(pth)
                            pgb.CenterColor = Color.LightSteelBlue

                            If e.DrawState = DrawState.Focus Then
                                pgb.SurroundColors = New Color() { Color.Yellow }
                            Else
                                pgb.SurroundColors = New Color() { Color.Blue }
                            End If

                            pgb.FocusScales = New PointF(0.8!, 0.8!)

                            g.FillEllipse(pgb, r)
                        End Using
                    End Using
                End If

                ' Draw the filler
                g.FillEllipse(tb, r)
                g.DrawString(DirectCast(a.Tag, String), buttonFont, Brushes.Black, _
                    New RectangleF(CType(r.X, Single), CType(r.Y, Single), _
                    CType(r.Width, Single), CType(r.Height, Single)), sfFormat)
            End Using
        End Using
    End Sub

    #End Region

    ''' <summary>
    ''' Draw the mouse events image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub MouseArea_DrawImage(sender As Object, e As DrawImageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim a As ImageAreaCircle = DirectCast(sender, ImageAreaCircle)
        Dim nShineOffset As Integer = a.Radius / 2

        Dim r As New Rectangle(a.CenterPoint.X - a.Radius, a.CenterPoint.Y - a.Radius, a.Radius * 2, a.Radius * 2)
        r.Inflate(-2, -2)

        ' Offset the area rectangle by the draw event offset
        r.Offset(e.ImageOffset.X, e.ImageOffset.Y)

        Using pth As New GraphicsPath()
            pth.AddEllipse(r)

            Using pgb As New PathGradientBrush(pth)
                pgb.CenterColor = Color.White
                pgb.SurroundColors = New Color () { Color.BurlyWood }
                Dim shine As New Point(a.CenterPoint.X - nShineOffset, a.CenterPoint.Y - nShineOffset)
                shine.Offset(e.ImageOffset.X, e.ImageOffset.Y)
                pgb.CenterPoint = New PointF(shine.X, shine.Y)

                g.FillEllipse(pgb, r)
                g.DrawString(DirectCast(a.Tag, String), buttonFont, Brushes.Black,
                    New RectangleF(CType(r.X, Single), CType(r.Y, Single), CType(r.Width, Single),
                    CType(r.Height, Single)), sfFormat)
            End Using
        End Using

        ' We'll let the image map draw the focus frame when needed
        e.DrawFocus = True
    End Sub

    ''' <summary>
    ''' Handle the mouse enter event for the mouse image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub MouseArea_MouseEnter(sender As Object, e As EventArgs)
        Dim a As ImageAreaCircle = DirectCast(sender, ImageAreaCircle)
        a.Tag = "Mouse Entered Area"
    End Sub

    ''' <summary>
    ''' Handle the mouse move event for the mouse image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub MouseArea_MouseMove(sender As Object, e As MouseEventArgs)
        Dim a As ImageAreaCircle = DirectCast(sender, ImageAreaCircle)
        a.Tag = String.Format(CultureInfo.CurrentCulture, "Mouse Move ({0},{1})", e.X, e.Y)
        imMap.Invalidate()
        imMap.Update()
    End Sub

    ''' <summary>
    ''' Handle the mouse hover event for the mouse image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub MouseArea_MouseHover(sender As Object, e As EventArgs)
        Dim a As ImageAreaCircle = DirectCast(sender, ImageAreaCircle)
        a.Tag = "Mouse Hovering"
        imMap.Invalidate()
        imMap.Update()
    End Sub

    ''' <summary>
    ''' Handle the mouse down event for the mouse image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub MouseArea_MouseDown(sender As Object, e As MouseEventArgs)
        Dim a As ImageAreaCircle = DirectCast(sender, ImageAreaCircle)
        a.Tag = "Mouse Button Down"
        imMap.Invalidate()
        imMap.Update()
    End Sub

    ''' <summary>
    ''' Handle the mouse up event for the mouse image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub MouseArea_MouseUp(sender As Object, e As MouseEventArgs)
        Dim a As ImageAreaCircle = DirectCast(sender, ImageAreaCircle)
        a.Tag = "Mouse Button Up"
        imMap.Invalidate()
        imMap.Update()
    End Sub

    ''' <summary>
    ''' Handle the mouse leave event for the mouse image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub MouseArea_MouseLeave(sender As Object, e As EventArgs)
        Dim a As ImageAreaCircle = DirectCast(sender, ImageAreaCircle)
        a.Tag = "Mouse Left Area"
    End Sub

    ''' <summary>
    ''' Draw the focus enter/leave image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub FocusArea_DrawImage(sender As Object, e As DrawImageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim a As ImageAreaPolygon = DirectCast(sender, ImageAreaPolygon)
        Dim r As Rectangle = a.BoundingRectangle

        ' Offset the area rectangle by the draw event offset
        r.Offset(e.ImageOffset.X, e.ImageOffset.Y)

        Dim pts As Point() = New Point(a.Points.Count - 1) {}
        a.Points.CopyTo(pts, 0)

        Using pgb As New PathGradientBrush(pts)
            ' Translate the brush coordinates to account for the offset
            Using m As New Matrix()
                m.Translate(CType(e.ImageOffset.X, Single), CType(e.ImageOffset.Y, Single))
                pgb.Transform = m

                If e.DrawState = DrawState.Focus Then
                    pgb.SurroundColors = New Color() { Color.DarkOrange }
                Else
                    pgb.SurroundColors = New Color() { Color.Navy }
                End If

                pgb.CenterColor = Color.Gray
                g.FillRectangle(pgb, r)

                r.Y += 20
                g.DrawString(DirectCast(a.Tag, String), buttonFont, Brushes.Yellow,
                    New RectangleF(CType(r.X, Single), CType(r.Y, Single), CType(r.Width, Single),
                    CType(r.Height, Single)), sfFormat)
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Handle the enter event for the focus image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub FocusArea_Enter(sender As Object, e As EventArgs)
        Dim a As ImageAreaPolygon = DirectCast(sender, ImageAreaPolygon)
        a.Tag = "Area Got Focus"
    End Sub

    ''' <summary>
    ''' Handle the leave event for the focus image area
    ''' </summary>
    ''' <param name="sender">The sender of the event (the image area)</param>
    ''' <param name="e">The event arguments</param>
    Private Sub FocusArea_Leave(sender as Object, e As EventArgs)
        Dim a As ImageAreaPolygon = DirectCast(sender, ImageAreaPolygon)
        a.Tag = "Area Lost Focus"
    End Sub

    #End Region

End Class
