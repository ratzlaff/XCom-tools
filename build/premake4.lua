solution "Progs"
	framework "3.5"
	location ".."

	configurations { "Debug", "Release" }
		flags { "Unsafe" }
		
	configuration { "Debug" }
		flags { "Symbols" }
		objdir "../obj/Debug"
		targetdir "../bin/Debug"
		
	configuration { "Release" }
		flags { "Optimize" }
		objdir "../obj/Release"
		targetdir "../bin/Release"
 
	-- MapView
	project "MapView"
		location "../MapView"
		kind "WindowedApp"
		language "C#"
		files { "../MapView/**.cs", "../MapView/**.resx", "../MapView/Properties/Settings.settings" }
		embedded { "../MapView/_Embedded/**.*" }
		
		links { "UtilLib", "MapLib", "ViewLib", "../libs/WeifenLuo.WinFormsUI.Docking.dll", "XCom", "System", "System.Core", "System.Data", "System.Design", "System.Windows.Forms", "System.Drawing", "System.Management" }
		prebuildcommands { "SubWCRev $(ProjectDir) $(ProjectDir)Properties\\AssemblyInfo.template $(ProjectDir)Properties\\AssemblyInfo.cs" }
		
	-- PckView
	project "PckView"
		location "../PckView"
		kind "WindowedApp"
		language "C#"
		files { "../PckView/**.cs", "../PckView/**.resx", "../PckView/Properties/Settings.settings" }
		embedded { "../PckView/_Embedded/**.*" }
		
		links { "UtilLib", "MapLib", "ViewLib", "System", "XCom", "System.Core", "System.Data", "System.Design", "System.Drawing", "System.Windows.Forms", "../libs/WeifenLuo.WinFormsUI.Docking.dll" }
		prebuildcommands { "SubWCRev $(ProjectDir) $(ProjectDir)Properties\\AssemblyInfo.template $(ProjectDir)Properties\\AssemblyInfo.cs" }
		
	-- XCom
	project "XCom"
		location "../XCom"
		kind "SharedLib"
		language "C#"
		files { "../XCom/**.cs", "../XCom/**.resx" }
		
		links { "UtilLib", "MapLib", "System", "System.Core", "System.Data", "System.Drawing", "System.Design", "System.Windows.Forms", "../libs/WeifenLuo.WinFormsUI.Docking.dll" }
		prebuildcommands { "SubWCRev $(ProjectDir) $(ProjectDir)Properties\\AssemblyInfo.template $(ProjectDir)Properties\\AssemblyInfo.cs" }
		
	-- Utility library
	project "UtilLib"
		location "../DSShared"
		kind "SharedLib"
		language "C#"
		files { "../DSShared/**.cs", "../DSShared/**.resx", "../DSShared/Properties/Settings.settings" }
		embedded { "../DSShared/_Embedded/**.*" }
		
		links { "System", "System.Core", "System.Data", "System.Design", "System.Drawing", "System.Windows.Forms" }
		prebuildcommands { "SubWCRev $(ProjectDir) $(ProjectDir)Properties\\AssemblyInfo.template $(ProjectDir)Properties\\AssemblyInfo.cs" }
		
	-- ViewLib
	project "ViewLib"
		location "../ViewLib"
		kind "SharedLib"
		language "C#"
		files { "../ViewLib/**.cs", "../ViewLib/**.resx", "../ViewLib/Properties/Settings.settings" }
		embedded { "../ViewLib/_Embedded/**.*" }

		links { "UtilLib", "MapLib", "../libs/WeifenLuo.WinFormsUI.Docking.dll", "System", "System.Core", "System.Data", "System.Design", "System.Drawing", "System.Management", "System.Windows.Forms" }
		prebuildcommands { "SubWCRev $(ProjectDir) $(ProjectDir)Properties\\AssemblyInfo.template $(ProjectDir)Properties\\AssemblyInfo.cs" }
		
	-- MapLib
	project "MapLib"
		location "../MapLib"
		kind "SharedLib"
		language "C#"
		files { "../MapLib/**.cs", "../MapLib/**.resx", "../MapLib/Properties/Settings.settings" }
		embedded { "../MapLib/_Embedded/**.*" }
		
		links { "UtilLib", "System", "System.Core", "System.Data", "System.Design", "System.Drawing", "System.Management", "System.Windows.Forms" }
		prebuildcommands { "SubWCRev $(ProjectDir) $(ProjectDir)Properties\\AssemblyInfo.template $(ProjectDir)Properties\\AssemblyInfo.cs" }
		
	-- project
	project "Build"
		kind "SharedLib"
		language "C#"
		files { "premake4.lua" }
		location "../build"
		targetdir "../obj"
