using System.Windows.Controls;
using System.Windows.Input;

namespace Testing.Admin.UI.Controls
{
    /// <summary>
    /// Interaction logic for OrderIssue.xaml
    /// </summary>
    public partial class OrderIssue
    {
        public OrderIssue()
        {
            InitializeComponent();
        }

        private void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            var item = sender as ListViewItem;

            if (item != null)
                item.IsSelected = true;
        }
    }
}
