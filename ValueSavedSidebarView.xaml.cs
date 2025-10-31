using System.Windows;
using System.Windows.Controls;

namespace ValueSavedPlugin
{
    public partial class ValueSavedSidebarView : UserControl
    {
        private ValueSavedPlugin plugin;

        public ValueSavedSidebarView(ValueSavedPlugin plugin)
        {
            InitializeComponent();
            this.plugin = plugin;
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            double total = plugin.GetTotalSaved();
            TotalText.Text = "$" + total.ToString("0.00");
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateTotal();
        }
    }
}