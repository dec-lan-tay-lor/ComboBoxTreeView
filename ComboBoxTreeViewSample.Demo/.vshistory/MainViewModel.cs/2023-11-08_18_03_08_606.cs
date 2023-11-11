using System.Collections.Generic;

namespace VortexWolf.Controls.Demo
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            var items13 = new List<SomeHierarchyViewModel>{
                                new SomeHierarchyViewModel("Item 1.3.1", null),
                                new SomeHierarchyViewModel("Item 1.3.2", null)};

            var items1 = new List<SomeHierarchyViewModel>{
                                new SomeHierarchyViewModel("Item 1.1", null),
                                new SomeHierarchyViewModel("Item 1.2", null),
                                new SomeHierarchyViewModel("Item 1.3", items13)};
            var items2 = new List<SomeHierarchyViewModel>{
                                new SomeHierarchyViewModel("Item 2.1", null),
                                new SomeHierarchyViewModel("Item 2.2", null)};

            var outerItems = new List<SomeHierarchyViewModel>{
                                new SomeHierarchyViewModel("Item 1", items1),
                                new SomeHierarchyViewModel("Item 2", items2)};

            this.Items = outerItems;
            this.SelectedItem = this.Items[0].Children[1];
        }

        public List<SomeHierarchyViewModel> Items { get; set; }

        public SomeHierarchyViewModel SelectedItem { get; set; }
    }
}
