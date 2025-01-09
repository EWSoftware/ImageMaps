# EWSoftware.ImageMaps Control Library
The image map controls allow you to define hot spots on an image and perform an action when a defined area is
clicked.  A Windows Forms and web server image map control are included.  Each contains a similar core set of
features:

* All of the standard image types are supported (BMP, JPEG, GIF, etc).
* Support for animation is included for those image types that use it such as GIF.
* The image's display height and width can be customized.
* Image areas can be defined using rectangles, circles, or polygons of any shape defined by a set of points.
* Tab order and validation events are supported.
* Image areas can be triggered by clicking on them with the mouse or by tabbing to them and hitting the Enter
key.
* Image areas support a **Tag** property so that you can associate user-defined information with each image
area.
* Both provide full design-time support including a graphical image area selection tool that saves you from
having to manually determine and type in the area coordinates.

The web server image map control renders a client-side image map that supports all of the expected features
including:

* Tooltip (alternate) text for each image area.
* An access key to select an image area by pressing **Alt** plus the defined access key.
* Image areas can be enabled or disabled.
* The action can be set to navigate to a URL (the default), post back to the server to fire a **Click**
event, or to do nothing and just display a tool tip if one is specified.
* When set to navigate, the target window can be defined to open the URL in a new or existing browser window.
* The navigate action can be used to execute client-side script.
* When set to post back, the event handler receives the index of the clicked area and the coordinates of the
click if supported.
* Custom attributes and view state are supported.

The Windows Forms image map control provides many features including:

* Scrolling is supported for displaying images larger than the bounds of the control.  Images smaller than the
control can be centered.
* Tooltip text for each image area.
* An access key to select an image area by pressing **Alt** plus the defined access key.
* Image areas can be enabled or disabled.
* The control uses double-buffering for a smooth, flicker-free display.
* The Windows Forms control provides an additional Ellipse image area to make it easy to define elliptical image
areas.
* The image map and image areas support owner draw mode to allow you to totally customize the display of the
image map or individual image areas.
* You can define event handlers on the image map and image areas to provide handling of mouse and focus events.

The demo web application included in the source code is available online so you can
[try it out](http://www.ewoodruff.us/ImageMapWebCS/Default.aspx).

NuGet packages are available:

* [EWSoftware.ImageMaps.Web.Controls](https://www.nuget.org/packages/EWSoftware.ImageMaps.Web.Controls)
* [EWSoftware.ImageMaps.Windows.Forms](https://www.nuget.org/packages/EWSoftware.ImageMaps.Windows.Forms)

See the [online help content](http://EWSoftware.github.io/ImageMaps/index.html) for usage and API information.

See the [Project Wiki](https://github.com/EWSoftware/ImageMaps/wiki) for information on requirements for
building the code and contributing to the project.
