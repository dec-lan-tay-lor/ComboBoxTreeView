using System.Collections.Generic;

namespace ComboBoxTreeView.Demo
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            var items13 = new List<ierarchyViewModel>{
                                new ierarchyViewModel("Item 1.3.1", null),
                                new ierarchyViewModel("Item 1.3.2", null)};

            var items1 = new List<ierarchyViewModel>{
                                new ierarchyViewModel("Item 1.1", null),
                                new ierarchyViewModel("Item 1.2", null),
                                new ierarchyViewModel("Item 1.3", items13)};
            var items2 = new List<ierarchyViewModel>{
                                new ierarchyViewModel("Item 2.1", null),
                                new ierarchyViewModel("Item 2.2", null)};

            var outerItems = new List<ierarchyViewModel>{
                                new ierarchyViewModel("Item 1", items1),
                                new ierarchyViewModel("Item 2", items2)};

            this.Items = outerItems;
            this.SelectedItem = this.Items[0].Children[1];
        }

        public List<ierarchyViewModel> Items { get; set; }

        public ierarchyViewModel SelectedItem { get; set; }
    }
}
