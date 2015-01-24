<%@ Page Language="c#" Inherits="ImageMapWebCS.DefaultPage" CodeFile="Default.aspx.cs" %>
<%@ Register TagPrefix="ewsi" Namespace="EWSoftware.ImageMaps.Web.Controls" Assembly="EWSoftware.ImageMaps.Web.Controls" %>

<!DOCTYPE html>
<html>
<head>
	<title>Image Map Web Server Control Test</title>
	<style>
		body {
			font-family: 'Segoe UI' , 'Lucida Grande' , Verdana, Arial, Helvetica, sans-serif;
		}
		table {
			width: 100%;
		}
	</style>
</head>

<body>
	<form id="frmDefaultPage" method="post" runat="server">
	<h1>Image Map Web Server Control Demo</h1>
	<p>For full details, source code, and a help file, see
<a href="https://github.com/EWSoftware/ImageMaps">https://github.com/EWSoftware/ImageMaps</a></p>

	<table>
		<tr>
			<td colspan="3">
				<p>The colored shapes on the left go to different web sites except the bottom right one which only
displays a tool tip.  The two text areas perform the actions they describe.</p>

				<p>The colored areas in the image map on the right post back to the server. The areas in the right side
image map will also fire validation events as demonstrated by the required text box below.</p>

				<p>The right-hand image map also demonstrates adding other HTML attributes to the image map such as
<strong>onmouseover</strong> and <strong>onmouseout</strong> events to show and hide a message for the image map
control as a whole and to change the image when the mouse enters the clickable image map areas.</p>

				<p><strong>Required Text</strong>
				<asp:TextBox ID="txtRequiredText" runat="server" TabIndex="1" />
				<asp:RequiredFieldValidator ID="rfVal" runat="server" Font-Bold="True"
					ErrorMessage="Some text is required in this field" ControlToValidate="txtRequiredText" />
				</p>
			</td>
		</tr>
		<tr>
			<td style="width: 70%;">
				<ewsi:ImageMap ID="imMap" runat="server" ToolTip="A test image" ImageUrl="Images/Shapes.jpg"
					BorderStyle="Solid" BorderWidth="1px" OnClick="imMap_Click">
					<ewsi:ImageAreaPolygon ToolTip="Load google.com in search pane"
						NavigateUrl="http://www.google.com" Target="Search"
						Coordinates="150,156,218,256,250,222,244,200,268,188,294,188,258,140,220,144,178,124"
						TabIndex="4" AccessKey="P" TagString="1" />
					<ewsi:ImageAreaRectangle ToolTip="Go to www.EWoodruff.us" Rectangle="15, 16, 149, 93"
						NavigateUrl="http://www.ewoodruff.us" TabIndex="2" AccessKey="R" TagString="2" />
					<ewsi:ImageAreaRectangle ToolTip="Only displays this tool tip" Rectangle="316, 175, 208, 122"
						Action="None" />
					<ewsi:ImageAreaCircle ToolTip="Load Microsoft.com in a new window" CenterPoint="380, 88" Radius="60"
						NavigateUrl="http://www.microsoft.com" Target="Blank" TabIndex="3" AccessKey="C" TagString="3" />
					<ewsi:ImageAreaRectangle ToolTip="Execute client-side script" Rectangle="12, 202, 134, 46"
						NavigateUrl="javascript: alert('Execute any client-side script');" TabIndex="5" TagString="4" />
					<ewsi:ImageAreaRectangle ToolTip="Click to enable/disable the other image map"
						Rectangle="6, 262, 194, 40" TabIndex="6" Action="PostBack" AccessKey="D" TagString="5" />
				</ewsi:ImageMap>
			</td>
			<td style="width: 5%;">
				&nbsp;
			</td>
			<td style="width: 25%; text-align: center;">
				<asp:Label ID="lblEnabledMsg" runat="server" /><br>
				<br>
				<div id="divMouseOver" style="visibility: hidden; margin-bottom: 10px;">
					<b>Mouse in image map</b></div>
				<ewsi:ImageMap ID="imClickMap" runat="server" ToolTip="Click an area to post back"
					ImageUrl="Images/PostBack.jpg" CausesValidation="True"
					OnClick="imClickMap_Click" onmouseover="javascript: IM_ShowHideText(true);"
						onmouseout="javascript: IM_ShowHideText(false);">
					<ewsi:ImageAreaRectangle Rectangle="0,0,20,20" Action="PostBack" ToolTip="Area 1" TabIndex="7"
						AccessKey="1" onmouseover="javascript: IM_Highlight(1);" onmouseout="javascript: IM_Highlight(0);" />
					<ewsi:ImageAreaRectangle Rectangle="80,0,20,20" Action="PostBack" ToolTip="Area 2" TabIndex="8"
						AccessKey="2" onmouseover="javascript: IM_Highlight(2);" onmouseout="javascript: IM_Highlight(0);" />
					<ewsi:ImageAreaRectangle Rectangle="0,80,20,20" Action="PostBack" ToolTip="Area 3" TabIndex="9"
						AccessKey="3" onmouseover="javascript: IM_Highlight(3);" onmouseout="javascript: IM_Highlight(0);" />
					<ewsi:ImageAreaRectangle Rectangle="80,80,20,20" Action="PostBack" ToolTip="Area 4" TabIndex="10"
						AccessKey="4" onmouseover="javascript: IM_Highlight(4);" onmouseout="javascript: IM_Highlight(0);" />
				</ewsi:ImageMap>
				<br>
				<asp:Label ID="lblClickMsg" runat="server" />
			</td>
		</tr>
	</table>
	</form>
	<script type="text/javascript">
<!--
		// Preload the images
		var image;

		image = new Image();

		image.src = "Images/Postback1.jpg";
		image.src = "Images/Postback2.jpg";
		image.src = "Images/Postback3.jpg";
		image.src = "Images/Postback4.jpg";
		image.src = "Images/Postback5.jpg";

		// Demo function for the main image map control.  Show the div control when the mouse is over the image map.
		function IM_ShowHideText(bShow)
		{
			var divCtl;

			divCtl = document.getElementById("divMouseOver");

			if(bShow)
				divCtl.style.visibility = "";
			else
				divCtl.style.visibility = "hidden";
		}

		// Demo function for the area controls.  Highlight the area that the mouse is over and unhighlight it when
		// the mouse is not over it.
		function IM_Highlight(imageNumber)
		{
			var img;

			img = document.getElementById("imClickMap");

			if(imageNumber == 0)
				img.src = "Images/Postback.jpg";
			else
				img.src = "Images/Postback" + imageNumber.toString() + ".jpg";
		}

//-->
	</script>
</body>
</html>
