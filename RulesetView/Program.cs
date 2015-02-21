using System;
using System.Threading;
using System.Windows.Forms;
using MapView.Forms.Error;
using RulesetView.Forms.MainRulesetViews;

namespace RulesetView
{
    static class Program
    {
        private static IErrorHandler _errorHandler;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _errorHandler = new ErrorWindowAdapter();
            Application.ThreadException += Application_ThreadException;
            Application.Run(new MainRulesetView());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            _errorHandler.HandleException(e.Exception);
        }
    }
}
