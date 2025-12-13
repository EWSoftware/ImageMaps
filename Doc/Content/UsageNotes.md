---
uid: 7e07e290-7738-4d8b-bcf1-8eb4b33706b0
alt-uid: UsageNotes
title: Welcome
keywords: usage notes, Welcome
---

<autoOutline />

## Image Map Features
The image map controls allow you to define hot spots on an image and perform an action when a
defined area is clicked.  A Windows Forms and a web server image map control are included.  Each contains a
similar set of core features:

- All of the standard image types are supported (BMP, JPEG, GIF, etc.).
- Support for animation is included for those image types that use it such as GIF.
- The image's display height and width can be customized.
- Image areas can be defined using rectangles, circles, or polygons of any shape defined by a set of points.
- Tab order and validation events are supported.
- Image areas can be triggered by clicking on them with the mouse or by tabbing to them and hitting the Enter key.
- Image areas support a `Tag` property so that you can associate user-defined information with each image area.
- Both provide full design-time support including a graphical image area selection tool that saves you from
  having to manually determine and type in the area coordinates.

## Web Server Image Map Control
The web server image map control renders a client-side image map that supports all of the expected features
including:

- Tool tip (alternate) text can be defined for each image area.
- An access key can be defined to select an image area by pressing **Alt** plus the defined access key.
- Image areas can be enabled or disabled.
- The action can be set to navigate to a URL (the default), post back to the server to fire a `Click` event, or
  to do nothing and just display a tool tip if one is specified.
- When set to navigate, the target window can be defined to open the URL in a new or existing browser window.
- The navigate action can be used to execute client-side script.
- When set to post back, the event handler receives the index of the clicked area and the coordinates of the
  click if supported.
- Custom attributes are supported to define additional behavior such as `onmouseover` events, etc.
- The image map and its areas also support view state so changes made to the controls across post backs are
  retained.  This also allows you to define dynamic image areas once at page creation and have them available
  across post backs without having to recreate them every time.

The standard image map control supplied with ASP.NET lacks many of the features listed above and
also lacks the superior design-time support found in the `EWSoftware.ImageMaps` control.
The web server image map control has been tested in Internet Explorer, Firefox, and Chrome.  It also works well
in earlier versions of those browsers and should work well in any other browser that provides support for
client-side image maps.

## Windows Forms Image Map Control
The Windows Forms image map control provides a similar control for rich client applications.  It displays the
selected image within its bounds and provides the following features:

- Scrolling is supported for displaying images larger than the bounds of the control.
- For images smaller than the control, it can be set to center the image within itself or shrink its bounds to
  fit the image.
- Tool tip text can be defined for each image area.
- An access key can be defined to select an image area by pressing **Alt** plus the defined access key.
- Tab indexes can be defined for image areas so that the control participates in the tab order of its parent.
- Image areas can be enabled or disabled.
- The action can be set to fire a `Click` event or to do nothing and just display a tool tip if one is specified.
- When set to fire the `Click` event, the event handler receives the index of the clicked area and the
  coordinates of the click.
- The control uses double-buffering for a smooth, flicker-free display.
- The Windows Forms control provides an additional `ImageAreaEllipse` image area to make it easy to define
  elliptical image areas.
- The image map and image areas support owner draw mode to allow you to totally customize the display of the
  image map or individual image areas.  Image areas can be set to owner draw mode even if the image map itself
  is not.
- You can define event handlers on the image map to provide generic handling of mouse and focus events for all
  image areas.  In addition, the image map will forward all mouse and focus events to the image areas so that you
  can handle the events individually by connecting the appropriate event handlers to the image areas themselves.

## Installing the Assemblies in the Global Assembly Cache
The `EWSoftware.ImageMaps` assemblies have strong names.  As such, you can
install the assemblies in the Global Assembly Cache (GAC) if so desired.  To do so, use the
**GACUTIL** command line tool as follows replacing the path to the assemblies to match the
installed location on your system:

``` none
gacutil -i \GH\ImageMaps\Deployment\EWSoftware.ImageMaps.Web.Controls.dll
gacutil -i \GH\ImageMaps\Deployment\EWSoftware.ImageMaps.Windows.Forms.dll
```

To remove the assemblies from the Global Assembly Cache, use the **GACUTIL** command line tool as follows:

``` none
gacutil -u EWSoftware.ImageMaps.Web.Controls
gacutil -u EWSoftware.ImageMaps.Windows.Forms
```

## Installing the Controls in the Visual Studio Toolbox
Installing the controls in the Visual Studio toolbox will allow you to use the controls at design time by
dragging them from the toolbox and dropping them on to your form.  To install them for the first time or
to refresh the toolbox after the control assemblies have been updated, do the following:

- Open the toolbox window.  Right click on it and select the **Show All** option.
- If you do not currently have a tab for the control library, create one by right clicking on the toolbox window,
  selecting **Add Tab**, and then typing in a name for the tab such as **EWSoftware Image Map Controls**.
- Click on the tab for the control library so that it is selected.
- If the tab is not empty, remove the existing controls by doing the following:
  - Right click on the toolbox tab and select **Choose Items...**.
  - In the dialog box that opens, select the **.NET Framework Components** tab.  Click on the **Namespace**
    header to sort the list of controls by namespace.
  - Locate the `EWSoftware.ImageMaps` namespaces and uncheck all of the controls in those namespaces.
  - Click the **OK** button and the controls will be removed from the toolbox.  You are now ready to add the new
    version of the control library.
- To add the controls to the toolbox tab, do the following:
  - Right click on the toolbox tab and select **Choose Items...**.
  - In the dialog box that opens, select the **.NET Framework Components** tab.  Click on the **Namespace**
    header to sort the list of controls by namespace.
  - Click on the **Browse** button and navigate to where the *EWSoftware.ImageMaps.Web.Controls.dll* and
    *EWSoftware.ImageMaps.Windows.Forms.dll* assemblies are stored.  Select one and click the **Open** button.
    This will add the controls in the assembly to the list in the dialog box.  They will all be selected and
    checked by default.  Repeat this step for the other assembly.
  - Click the **OK** button to complete the process and the new controls will appear in the toolbox tab.

Note that both controls are called `ImageMap` so you may want to put each one in its own tab or right click on
them and use the **Rename** option to change their names to their fully qualified namespace to keep them
straight.  However, this is not a necessity as when developing a web project, the Windows Forms control will be
disabled in the toolbox and when developing a Windows Forms application, the web control will be disabled in the
toolbox.

## Using the Assemblies in Your Projects
> [!TIP]
> NuGet packages are available for both the web server and Windows Forms controls.  Search for the
> package names *EWSoftware.ImageMap.Web* and *EWSoftware.ImageMap.WinForms*.

The web server classes can be found in the assembly *EWSoftware.ImageMaps.Web.Controls.dll*.
The Windows Forms classes can be found in the *EWSoftware.ImageMaps.Windows.Forms.dll*
assembly.  In order to use the classes, add a reference to the appropriate assembly in your project.  To add the
reference, open your project in Visual Studio, open the **Solution Explorer** pane of the project, right
click on the **References** folder, select **Add Reference...**, click the **Browse** button and
navigate to the folder in which you placed the assemblies, and finally select the *EWSoftware.ImageMaps.Web.Controls.dll*
or *EWSoftware.ImageMaps.Windows.Forms.dll* assembly and click **OK** to close the
dialogs and add the reference.

In code modules that use classes from the `EWSoftware.ImageMaps` namespaces,
you will have to add a `using` statement (`Imports` in VB.NET) for
one or more of the following namespaces:

- `EWSoftware.ImageMaps` (Common classes, events, etc.)
- `EWSoftware.ImageMaps.Web.Controls` (Web control classes)
- `EWSoftware.ImageMaps.Windows.Forms` (Windows Forms classes)

## Demonstration Applications
Web and Windows Forms applications in C# and VB.NET are provided that demonstrate the basic use of
the image map controls.  The web applications show the various features of the web server image map control such
as navigation, calling JavaScript, posting back to fire the server-side `Click` event,
etc.  The Windows Forms applications show off the various capabilities of the Windows Forms image map control.
The main form uses an owner drawn image map and image areas to demonstrate the owner drawn capabilities of the
image areas (focused areas, disabled areas, hot lighting, etc.) as well as the event handling abilities of the
individual image areas (mouse and focus events).  A second form presents an image map control and a property grid
in which you can modify the image map's properties at runtime to try out its various features.  The demo is set
up with a default image and some image areas but you can change the image and redefine the areas to see what the
control can do.  This includes using the collection editor and visual coordinate selection tool.

To try out the demo web applications, open the projects in Visual Studio.  Once built, they are set
up to run using the development web server.  The startup page in each is *Default.aspx*.  If
you are using a remote server, you will need to set up the virtual directories and copy the demo application
files to the server locations.

## See Also
**Other Resources**  
[](@562d9967-e690-4042-b9d6-f6db567c1c29)  
[](@5f8d60e8-3767-47f9-92af-8570d42d5e37)  
[](@ad1fd96f-bab4-4925-b620-29b634cfc4b0)  
