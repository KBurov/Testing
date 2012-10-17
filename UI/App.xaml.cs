using System;
using System.Windows;

namespace Testing.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            // Apply skin
            AppController.ApplySkin();
            // Load localized strings
            AppController.ApplyInterfaceLanguage();
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = e.ExceptionObject as Exception;

                if (ex != null)
                    MessageBox.Show(
                        "Please contact the developers with the following information:\n\n" + ex.Message + ex.StackTrace,
                        "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
// ReSharper disable EmptyGeneralCatchClause
            catch {}
// ReSharper restore EmptyGeneralCatchClause
        }
    }
}