using System.Windows;

namespace Testing.UI.Pages
{
    /// <summary>
    /// Interaction logic for UserFormPage.xaml
    /// </summary>
    public partial class UserFormPage
    {
        public UserFormPage()
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
