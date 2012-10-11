using System.Windows;
using System.Windows.Controls;

using Wanderer.Library.Wpf;

namespace Testing.Admin.UI.Pages
{
    /// <summary>
    /// Interaction logic for IssuePage.xaml
    /// </summary>
    public partial class IssuePage
    {
        public IssuePage()
        {
            InitializeComponent();
        }

        private void MaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            AppController.MaximizeRestore();

            var placementScrollViewer = UIHelper.FindChild<ScrollViewer>(Application.Current.MainWindow, AppController.PlacementScrollViewerName);

            if (placementScrollViewer == null) return;

            placementScrollViewer.VerticalScrollBarVisibility = Application.Current.MainWindow.WindowState ==
                                                                WindowState.Normal
                                                                    ? ScrollBarVisibility.Auto
                                                                    : ScrollBarVisibility.Disabled;
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            AppController.Minimize();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            AppController.Close();
        }
    }
}
