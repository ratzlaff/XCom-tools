namespace XCom
{
	public class ConsoleSharedSpace
	{
		private const string X_CONSOLE = "xConsole";
		private readonly SharedSpace _sharedSpace;

		public ConsoleSharedSpace(SharedSpace sharedSpace)
		{
			_sharedSpace = sharedSpace;
		}

		public ConsoleForm GetConsole()
		{
			var console = _sharedSpace.GetObj(X_CONSOLE) as ConsoleForm;
			return console;
		}

		public ConsoleForm GetNewConsole()
		{
			var console = GetConsole();
			if (console != null) return console;
			console = (ConsoleForm) _sharedSpace.GetObj(X_CONSOLE, new ConsoleForm());
			return console;
		}
	}
}
