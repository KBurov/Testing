using System;
using System.Threading;
using System.Windows.Threading;

using Testing.UI.Properties;

namespace Testing.UI
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
                                                                   new Action(NavigateToUserFormPage));
                                             }
                );
        }

        private void NavigateToUserFormPage()
        {
            NavigationService.Navigate(AppController.GetPage(ApplicationPages.UserFormPage));
        }
    }
}
