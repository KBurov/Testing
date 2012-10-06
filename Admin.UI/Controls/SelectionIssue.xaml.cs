using System.Windows.Controls;
using System.Windows.Input;

namespace Testing.Admin.UI.Controls
{
    /// <summary>
    /// Interaction logic for SelectionIssue.xaml
    /// </summary>
    public partial class SelectionIssue
    {
        public SelectionIssue()
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
