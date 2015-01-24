<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImageMapPropertyForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
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
        Me.components = New System.ComponentModel.Container()
        Dim ImageAreaRectangle1 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle()
        Dim ImageAreaCircle1 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaCircle = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaCircle()
        Dim ImageAreaPolygon1 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaPolygon = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaPolygon()
        Dim ImageAreaRectangle2 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaRectangle()
        Dim ImageAreaEllipse1 As EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse = New EWSoftware.ImageMaps.Windows.Forms.ImageAreaEllipse()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ImageMapPropertyForm))
        Me.imMap = New EWSoftware.ImageMaps.Windows.Forms.ImageMap()
        Me.pgImageMapProps = New System.Windows.Forms.PropertyGrid()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.ttTips = New System.Windows.Forms.ToolTip(Me.components)
        Me.label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout
        '
        'imMap
        '
        Me.imMap.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        ImageAreaRectangle1.AccessKey = "R"
        ImageAreaRectangle1.Rectangle = New System.Drawing.Rectangle(16, 16, 150, 94)
        ImageAreaRectangle1.TabIndex = 3
        ImageAreaRectangle1.ToolTip = "A rectangle area"
        ImageAreaCircle1.AccessKey = "C"
        ImageAreaCircle1.CenterPoint = New System.Drawing.Point(444, 88)
        ImageAreaCircle1.Radius = 60
        ImageAreaCircle1.TabIndex = 1
        ImageAreaCircle1.ToolTip = "A circle area"
        ImageAreaPolygon1.AccessKey = "P"
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(42, 186))
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(110, 286))
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(144, 250))
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(138, 230))
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(160, 216))
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(190, 214))
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(152, 168))
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(112, 172))
        ImageAreaPolygon1.Points.Add(New System.Drawing.Point(70, 154))
        ImageAreaPolygon1.TabIndex = 2
        ImageAreaPolygon1.ToolTip = "A polygon area"
        ImageAreaRectangle2.Action = EWSoftware.ImageMaps.Windows.Forms.AreaClickAction.None
        ImageAreaRectangle2.Rectangle = New System.Drawing.Rectangle(316, 176, 206, 118)
        ImageAreaRectangle2.ToolTip = "A tool tip only area"
        ImageAreaEllipse1.AccessKey = "E"
        ImageAreaEllipse1.Ellipse = New System.Drawing.Rectangle(199, 88, 128, 58)
        ImageAreaEllipse1.TabIndex = 4
        ImageAreaEllipse1.ToolTip = "An ellipse area"
        Me.imMap.Areas.Add(ImageAreaRectangle1)
        Me.imMap.Areas.Add(ImageAreaCircle1)
        Me.imMap.Areas.Add(ImageAreaPolygon1)
        Me.imMap.Areas.Add(ImageAreaRectangle2)
        Me.imMap.Areas.Add(ImageAreaEllipse1)
        Me.imMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.imMap.Image = CType(resources.GetObject("imMap.Image"),System.Drawing.Image)
        Me.imMap.Location = New System.Drawing.Point(12, 12)
        Me.imMap.Name = "imMap"
        Me.imMap.Size = New System.Drawing.Size(574, 451)
        Me.imMap.TabIndex = 0
        Me.imMap.ToolTip = "A test image"
        '
        'pgImageMapProps
        '
        Me.pgImageMapProps.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.pgImageMapProps.LineColor = System.Drawing.SystemColors.ScrollBar
        Me.pgImageMapProps.Location = New System.Drawing.Point(592, 12)
        Me.pgImageMapProps.Name = "pgImageMapProps"
        Me.pgImageMapProps.Size = New System.Drawing.Size(288, 451)
        Me.pgImageMapProps.TabIndex = 1
        '
        'btnHelp
        '
        Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnHelp.Location = New System.Drawing.Point(12, 473)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(75, 32)
        Me.btnHelp.TabIndex = 2
        Me.btnHelp.Text = "&Help"
        Me.ttTips.SetToolTip(Me.btnHelp, "Open ImageMap help file")
        '
        'btnExit
        '
        Me.btnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnExit.Location = New System.Drawing.Point(805, 473)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 32)
        Me.btnExit.TabIndex = 3
        Me.btnExit.Text = "E&xit"
        Me.ttTips.SetToolTip(Me.btnExit, "Exit the demo")
        '
        'label1
        '
        Me.label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.label1.Location = New System.Drawing.Point(139, 466)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(614, 46)
        Me.label1.TabIndex = 4
        Me.label1.Text = "Use the property grid on the right to modify the image map properties and to acce"& _ 
    "ss the image area collection editor and coordinate editor."
        Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ImageMapPropertyForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(892, 517)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.pgImageMapProps)
        Me.Controls.Add(Me.imMap)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(500, 300)
        Me.Name = "ImageMapPropertyForm"
        Me.ShowInTaskbar = false
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Windows Forms Image Map Demo"
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents imMap As EWSoftware.ImageMaps.Windows.Forms.ImageMap
    Friend WithEvents pgImageMapProps As System.Windows.Forms.PropertyGrid
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ttTips As System.Windows.Forms.ToolTip
    Friend WithEvents label1 As System.Windows.Forms.Label
End Class
