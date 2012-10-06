using System;
using System.Threading;
using System.Windows.Threading;

using Testing.Admin.UI.Properties;

namespace Testing.Admin.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            // Hide splash screen
            ThreadPool.QueueUserWorkItem(o =>
                                             {
                                                 Thread.Sleep(Settings.Default.SplashScreenTimeout);
                                                 Dispatcher.Invoke(DispatcherPriority.Normal,
                                                                   new Action(NavigateToSelectAndUserInfoSettingsPage));
                                             }
                );
        }

        private void NavigateToSelectAndUserInfoSettingsPage()
        {
            NavigationService.Navigate(AppController.GetPage(ApplicationPages.SettingsPage));
        }
    }
}