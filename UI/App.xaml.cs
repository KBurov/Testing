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
            // Apply skin
            AppController.ApplySkin();
            // Load localized strings
            AppController.ApplyInterfaceLanguage();
        }
    }
}
