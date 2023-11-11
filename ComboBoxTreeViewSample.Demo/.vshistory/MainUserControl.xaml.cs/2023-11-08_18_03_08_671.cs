using System.Windows.Controls;

namespace VortexWolf.Controls.Demo
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
