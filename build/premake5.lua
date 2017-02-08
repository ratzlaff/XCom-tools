solution "Progs"
	framework "3.5"
	location ".."

	configurations { "Debug", "Release" }
		clr "Unsafe"
		
	configuration { "Debug" }
		symbols "On"
		objdir "../obj/Debug"
		targetdir "../bin/Debug"
		
	configuration { "Release" }
		flags { "Optimize" }
		objdir "../obj/Release"
		targetdir "../bin/Release"
 
	-- MapView
	project "MapView"
		location "."
		kind "WindowedApp"
		icon "../icons/MapEdit.ico"
		language "C#"
		files { "../MapView/**.cs", "../MapView/**.resx", "../MapView/Properties/Settings.settings", "../MapView/_Embedded/**.*" }
		links { "UtilLib", "MapLib", "ViewLib", "../libs/WeifenLuo.WinFormsUI.Docking.dll", "XCom", "System", "System.Core", "System.Data", "System.Design", "System.Windows.Forms", "System.Drawing", "System.Management" }
		filter "files:../MapView/_Embedded/**.*"
			buildaction "Embed"
		
	-- PckView
	project "PckView"
		location "."
		kind "WindowedApp"
		language "C#"
		links { "UtilLib", "MapLib", "ViewLib", "System", "XCom", "System.Core", "System.Data", "System.Design", "System.Drawing", "System.Windows.Forms", "../libs/WeifenLuo.WinFormsUI.Docking.dll" }
		files { "../PckView/**.cs", "../PckView/**.resx", "../PckView/Properties/Settings.settings", "../PckView/_Embedded/**.*" }
		filter "files:../PckView/_Embedded/**.*"
			buildaction "Embed"
		
	-- XCom
	project "XCom"
		location "."
		kind "SharedLib"
		language "C#"
		links { "UtilLib", "MapLib", "System", "System.Core", "System.Data", "System.Drawing", "System.Design", "System.Windows.Forms", "../libs/WeifenLuo.WinFormsUI.Docking.dll" }
		files { "../XCom/**.cs", "../XCom/**.resx", "../XCom/_Embedded/**.*" }
		filter "files:../XCom/_Embedded/**.*"
			buildaction "Embed"
		
	-- Utility library
	project "UtilLib"
		location "."
		kind "SharedLib"
		language "C#"
		links { "System", "System.Core", "System.Data", "System.Design", "System.Drawing", "System.Windows.Forms" }
		files { "../DSShared/**.cs", "../DSShared/**.resx", "../DSShared/Properties/Settings.settings", "../DSShared/_Embedded/**.*" }
		filter "files:../DSShared/_Embedded/**.*"
			buildaction "Embed"
		
	-- ViewLib
	project "ViewLib"
		location "."
		kind "SharedLib"
		language "C#"
		links { "UtilLib", "MapLib", "../libs/WeifenLuo.WinFormsUI.Docking.dll", "System", "System.Core", "System.Data", "System.Design", "System.Drawing", "System.Management", "System.Windows.Forms" }
		files { "../ViewLib/**.cs", "../ViewLib/**.resx", "../ViewLib/Properties/Settings.settings", "../ViewLib/_Embedded/**.*" }
		filter "files:../ViewLib/_Embedded/**.*"
			buildaction "Embed"
		
	-- MapLib
	project "MapLib"
		location "."
		kind "SharedLib"
		language "C#"
		links { "UtilLib", "System", "System.Core", "System.Data", "System.Design", "System.Drawing", "System.Management", "System.Windows.Forms" }
		files { "../MapLib/**.cs", "../MapLib/**.resx", "../MapLib/Properties/Settings.settings", "../MapLib/_Embedded/**.*" }
		filter "files:../MapLib/_Embedded/**.*"
			buildaction "Embed"

