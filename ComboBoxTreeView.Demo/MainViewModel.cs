using System.Collections.Generic;

namespace ComboBoxTreeView.Demo
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            var items13 = new List<TreeViewModel>{
                                new TreeViewModel("Item 1.3.1", null),
                                new TreeViewModel("Item 1.3.2", null)};

            var items1 = new List<TreeViewModel>{
                                new TreeViewModel("Item 1.1", null),
                                new TreeViewModel("Item 1.2", null),
                                new TreeViewModel("Item 1.3", items13)};
            var items2 = new List<TreeViewModel>{
                                new TreeViewModel("Item 2.1", null),
                                new TreeViewModel("Item 2.2", null)};

            var outerItems = new List<TreeViewModel>{
                                new TreeViewModel("Item 1", items1),
                                new TreeViewModel("Item 2", items2)};

            this.Items = outerItems;
            this.SelectedItem = this.Items[0].Children[1];
        }

        public List<TreeViewModel> Items { get; set; }

        public TreeViewModel SelectedItem { get; set; }
    }
}
