using System.Windows;

namespace Testing.UI.Pages
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ResultPage
    {
        public ResultPage()
        {
            InitializeComponent();
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

        private void PageInitialized(object sender, System.EventArgs e)
        {
            AppController.SaveResult();
        }
    }
}
