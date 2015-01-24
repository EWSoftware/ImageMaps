<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImageMapEventsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                If Not (buttonFont Is Nothing) Then buttonFont.Dispose()

                If Not (hyperlinkFont Is Nothing) Then hyperlinkFont.Dispose()

                If Not (sfFormat Is Nothing) Then sfFormat.Dispose()

                If Not (imgFiller Is Nothing) Then imgFiller.Dispose()

                If Not (iaNormal Is Nothing) Then iaNormal.Dispose()

                If Not (iaDisabled Is Nothing) Then iaDisabled.Dispose()

                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ImageAreaRectangle1 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle()
        Dim ImageAreaCircle1 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaCircle = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaCircle()
        Dim ImageAreaPolygon1 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaPolygon = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaPolygon()
        Dim ImageAreaRectangle2 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle()
        Dim ImageAreaEllipse1 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse()
        Dim ImageAreaEllipse2 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse()
        Dim ImageAreaEllipse3 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ImageMapEventsForm))
        Me.imMap = New EWSoftware.ImageMaps.Windows.Forms.ImageMap()
        Me.SuspendLayout
        '
        'imMap
        '
        ImageAreaRectangle1.AccessKey = "I"
        ImageAreaRectangle1.Rectangle = New System.Drawing.Rectangle(14, 47, 208, 21)
        ImageAreaRectangle1.TabIndex = 1
        ImageAreaRectangle1.ToolTip = "Turn image map owner draw on/off"
        ImageAreaCircle1.CenterPoint = New System.Drawing.Point(107, 167)
        ImageAreaCircle1.OwnerDraw = true
        ImageAreaCircle1.Radius = 71
        ImageAreaCircle1.TabIndex = 2
        ImageAreaCircle1.Tag = "Mouse event demo"
        ImageAreaCircle1.ToolTip = "Demonstrate mouse events"
        ImageAreaPolygon1.OwnerDraw = true
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(310, 114))
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(210, 214))
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(410, 214))
        ImageAreaPolygon1.TabIndex = 3
        ImageAreaPolygon1.Tag = "Focus demo"
        ImageAreaPolygon1.ToolTip = "Demonstrate focus enter/leave"
        ImageAreaRectangle2.AccessKey = "D"
        ImageAreaRectangle2.Rectangle = New System.Drawing.Rectangle(130, 272, 312, 21)
        ImageAreaRectangle2.TabIndex = 4
        ImageAreaRectangle2.ToolTip = "Double-click to visit the web site"
        ImageAreaEllipse1.Ellipse = New System.Drawing.Rectangle(453, 161, 131, 30)
        ImageAreaEllipse1.Enabled = false
        ImageAreaEllipse1.OwnerDraw = true
        ImageAreaEllipse1.TabIndex = 5
        ImageAreaEllipse1.Tag = "Disabled"
        ImageAreaEllipse1.ToolTip = "Disabled owner drawn area"
        ImageAreaEllipse2.AccessKey = "P"
        ImageAreaEllipse2.Ellipse = New System.Drawing.Rectangle(453, 208, 131, 30)
        ImageAreaEllipse2.OwnerDraw = true
        ImageAreaEllipse2.TabIndex = 6
        ImageAreaEllipse2.Tag = "&Properties"
        ImageAreaEllipse2.ToolTip = "Interactive property demo"
        ImageAreaEllipse3.AccessKey = "X"
        ImageAreaEllipse3.Ellipse = New System.Drawing.Rectangle(453, 252, 131, 30)
        ImageAreaEllipse3.OwnerDraw = true
        ImageAreaEllipse3.TabIndex = 7
        ImageAreaEllipse3.Tag = "E&xit"
        ImageAreaEllipse3.ToolTip = "Exit this demo"
        Me.imMap.Areas.Add(ImageAreaRectangle1)
        Me.imMap.Areas.Add(ImageAreaCircle1)
        Me.imMap.Areas.Add(ImageAreaPolygon1)
        Me.imMap.Areas.Add(ImageAreaRectangle2)
        Me.imMap.Areas.Add(ImageAreaEllipse1)
        Me.imMap.Areas.Add(ImageAreaEllipse2)
        Me.imMap.Areas.Add(ImageAreaEllipse3)
        Me.imMap.Dock = System.Windows.Forms.DockStyle.Fill
        Me.imMap.ImageMapHeight = 300
        Me.imMap.ImageMapWidth = 600
        Me.imMap.Location = New System.Drawing.Point(0, 0)
        Me.imMap.Name = "imMap"
        Me.imMap.OwnerDraw = true
        Me.imMap.Size = New System.Drawing.Size(600, 300)
        Me.imMap.TabIndex = 0
        Me.imMap.ToolTip = Nothing
        '
        'ImageMapEventsForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(600, 300)
        Me.Controls.Add(Me.imMap)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(200, 200)
        Me.Name = "ImageMapEventsForm"
        Me.Text = "EWSoftware Image Map Control Demo"
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents imMap As EWSoftware.ImageMaps.Windows.Forms.ImageMap

End Class
