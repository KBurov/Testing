using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Testing.UI.Pages
{
    /// <summary>
    /// Interaction logic for TimeOverPage.xaml
    /// </summary>
    public partial class TimeOverPage
    {
        private const int TimeOverPageTimeout = 3000;

        public TimeOverPage()
        {
            InitializeComponent();
            // Go to result page
            ThreadPool.QueueUserWorkItem(o =>
                                             {
                                                 Thread.Sleep(TimeOverPageTimeout);
                                                 Dispatcher.Invoke(DispatcherPriority.Normal,
                                                                   new Action(NavigateToResultPage));
                                             }
                );
        }

        private void MaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            AppController.MaximizeRestore();
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            AppController.Minimize();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            AppController.Close();
        }

        private void NavigateToResultPage()
        {
            if (NavigationService != null)
                NavigationService.Navigate(AppController.GetPage(ApplicationPages.ResultPage));
        }
    }
}
