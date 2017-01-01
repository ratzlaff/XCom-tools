using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using XCom;
using XCom.Interfaces;

namespace MapView.Forms.Error.WarningConsole
{
	public class ConsoleWarningHandler
		:
		IWarningHandler
	{
		private readonly ConsoleSharedSpace _consoleSharedSpace;

		public ConsoleWarningHandler(ConsoleSharedSpace consoleSharedSpace)
		{
			_consoleSharedSpace = consoleSharedSpace;
		}

		public void HandleWarning(string message)
		{
			var console = _consoleSharedSpace.GetConsole();
			if (console == null)
			{
				console = _consoleSharedSpace.GetNewConsole();
				console.Show();
			}
			xConsole.AddLine("WARNING: " + message);
		}
	}
}
