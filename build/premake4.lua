solution "XCSuite"
	framework "3.5"
	location ".."
	objdir "../obj"
	targetdir "../bin"
	configurations { "Debug", "Release" }
		flags { "Unsafe" }
		
	configuration { "Debug" }
		flags { "Symbols" }
		
	configuration { "Release" }
		flags { "Optimize" }
 
	-- MapView
	project "MapView"
		location "../MapView"
		kind "WindowedApp"
		language "C#"
		files { "../MapView/**.cs", "../MapView/**.resx" }
		embedded { "../MapView/_Embedded/**.*" }
		
		links { "DSShared", "XCom", "System", "System.Core", "System.Data", "System.Design", "System.Windows.Forms", "System.Drawing", "System.Management" }
		
	-- PckView
	project "PckView"
		location "../PckView"
		kind "WindowedApp"
		language "C#"
		files { "../PckView/**.cs", "../PckView/**.resx" }
		embedded { "../PckView/_Embedded/**.*" }
		
		links { "System", "XCom", "DSShared", "System.Core", "System.Data", "System.Design", "System.Drawing", "System.Windows.Forms" }
		
	-- XCom
	project "XCom"
		location "../XCom"
		kind "SharedLib"
		language "C#"
		files { "../XCom/**.cs", "../XCom/**.resx" }
		embedded { "../XCom/_Embedded/**.*" }
		excludes { "../XCom/Ignored/**.*" }
		
		links { "DSShared", "System", "System.Core", "System.Data", "System.Drawing", "System.Design", "System.Windows.Forms", "../DockPanel_Src/WeifenLuo.WinFormsUI.Docking.dll" }
		
	-- Utility library
	project "DSShared"
		location "../DSShared"
		kind "SharedLib"
		language "C#"
		files { "../DSShared/**.cs", "../DSShared/**.resx" }
		embedded { "../DSShared/_Embedded/**.*" }
		
		links { "System", "System.Core", "System.Data", "System.Design", "System.Drawing", "System.Windows.Forms" }
		
	-- Launcher/Updator
	project "XCSuite"
		location "../XCSuite"
		kind "SharedLib"
		language "C#"
		files { "../XCSuite/**.cs", "../XCSuite/**.resx" }
		embedded { "../XCSuite/_Embedded/**.*" }
		
		links { "DSShared", "XCom", "System", "System.Core", "System.Data", "System.Design", "System.Drawing", "System.Windows.Forms" }
		
	-- project
	project "Build"
		kind "SharedLib"
		language "C#"
		files { "premake4.lua" }
		location "../build"
