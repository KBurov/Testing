using System.Windows;

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
