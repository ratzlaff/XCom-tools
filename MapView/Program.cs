using System;
using System.Reflection;
using System.Windows.Forms;

namespace MapView
{
	static class Program
	{

		[STAThread]
		public static void Main()
		{
#if RELEASE
			ReleaseRun();
#else
			TestRun();
#endif
		} 

		private static void ReleaseRun()
		{
			// https://msdn.microsoft.com/en-us/library/system.appdomain.aspx
			// Construct and initialize settings for a second AppDomain.
			AppDomainSetup ads = new AppDomainSetup();
			ads.ApplicationBase = System.Environment.CurrentDirectory;
			ads.DisallowBindingRedirects = false;
			ads.DisallowCodeDownload = true;
			ads.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

			// Create the second AppDomain.
			AppDomain ad2 = AppDomain.CreateDomain("MV Domain", null, ads);

			// Create an instance of MarshalByRefType in the second AppDomain.
			// A proxy to the object is returned.
			Startup startup = (Startup)ad2.CreateInstanceAndUnwrap(
																Assembly.GetEntryAssembly().FullName,
																typeof(Startup).FullName);

			startup.RunProgram();

			//Console.WriteLine("Disposing of appdomain");
			AppDomain.Unload(ad2);
		}

		private static void TestRun()
		{
			var startup = new Startup();
			startup.RunProgram();
		}
	}
}
