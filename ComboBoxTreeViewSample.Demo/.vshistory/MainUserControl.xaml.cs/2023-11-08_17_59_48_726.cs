using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ComboBoxTreeView.Demo
{
    public partial class MainUserControl : UserControl
    {
        public MainUserControl()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedModel = (SomeHierarchyViewModel)e.AddedItems[0];

            textBlock.Text = "SelectedItem: " + selectedModel.Title;
        }
    }
}
