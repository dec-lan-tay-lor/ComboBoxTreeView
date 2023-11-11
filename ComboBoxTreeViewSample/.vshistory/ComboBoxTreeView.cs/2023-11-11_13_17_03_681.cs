using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VortexWolf.Controls
{
    public class ComboBoxTreeView : ComboBox
    {
        public new static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ComboBoxTreeView), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedItemChanged)));
        public static readonly DependencyProperty SelectedHierarchyProperty = DependencyProperty.Register("SelectedHierarchy", typeof(IEnumerable<string>), typeof(ComboBoxTreeView), new PropertyMetadata(null, OnSelectedHierarchyChanged));
        public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(ComboBoxTreeView), new PropertyMetadata());
        public static readonly DependencyProperty ParentProperty = DependencyProperty.Register("Parent", typeof(string), typeof(ComboBoxTreeView), new PropertyMetadata());


        private ExtendedTreeView _treeView;
        private ContentPresenter _contentPresenter;

        public ComboBoxTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxTreeView), new FrameworkPropertyMetadata(typeof(ComboBoxTreeView)));
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            //don't call the method of the base class
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _treeView = (ExtendedTreeView)this.GetTemplateChild("treeView");
            _treeView.OnHierarchyMouseUp += new MouseEventHandler(OnTreeViewHierarchyMouseUp);
            _contentPresenter = (ContentPresenter)this.GetTemplateChild("ContentSite");

            this.SetSelectedItemToHeader();
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            this.SelectedItem = _treeView.SelectedItem;
            this.SetSelectedItemToHeader();
        }

        protected override void OnDropDownOpened(EventArgs e)
        {
            base.OnDropDownOpened(e);
            this.SetSelectedItemToHeader();
        }

        /// <summary>
        /// Handles clicks on any item in the tree view
        /// </summary>
        private void OnTreeViewHierarchyMouseUp(object sender, MouseEventArgs e)
        {
            //This line isn't obligatory because it is executed in the OnDropDownClosed method, but be it so
            this.SelectedItem = _treeView.SelectedItem;

            this.IsDropDownOpen = false;
        }

        /// <summary>
        /// Selected item of the TreeView
        /// </summary>
        public new object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }


        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ComboBoxTreeView)sender).UpdateSelectedItem();
        }

        /// <summary>
        /// Selected hierarchy of the treeview
        /// </summary>
        public IEnumerable<string> SelectedHierarchy
        {
            get { return (IEnumerable<string>)GetValue(SelectedHierarchyProperty); }
            set { SetValue(SelectedHierarchyProperty, value); }
        }



        private static void OnSelectedHierarchyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ComboBoxTreeView)sender).UpdateSelectedHierarchy();
        }



        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        public string Parent
        {
            get { return (string)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }


        //private void UpdateItemsSource()
        //{
        //    //var allItems = new List<ITreeViewItemModel>();

        //    //Action<IEnumerable<ITreeViewItemModel>> selectAllItemsRecursively = null;
        //    //selectAllItemsRecursively = items =>
        //    //{
        //    //    if (items == null)
        //    //    {
        //    //        return;
        //    //    }

        //    //    foreach (var item in items)
        //    //    {
        //    //        allItems.Add(item);
        //    //        selectAllItemsRecursively(item.GetChildren());
        //    //    }
        //    //};

        //    //selectAllItemsRecursively(this.ItemsSource);

        //    //base.ItemsSource = allItems.Count > 0 ? allItems : null;
        //}

        private void UpdateSelectedItem()
        {
            if (this.SelectedItem is TreeViewItem)
            {
                //I would rather use a correct object instead of TreeViewItem
                this.SelectedItem = ((TreeViewItem)this.SelectedItem).DataContext;
            }
            else if(SelectedItem!=null)
            {
                //Update the selected hierarchy and displays
                var hierarchy = selectedHierarchy();

                this.SetSelectedItemToHeader();

                base.SelectedItem = this.SelectedItem;
            }
        }

        private string[] selectedHierarchy()
        {
            var type = SelectedItem.GetType();
            var propInfo = type.GetProperty(Parent);
            var selectedInfo = type.GetProperty(SelectedValuePath);

            return TreeHelper.GetAncestors(SelectedItem, a => propInfo.GetValue(a)).Select(h => (string)selectedInfo.GetValue(h)).Reverse().ToArray();
        }

        private void UpdateSelectedHierarchy()
        {
            if (ItemsSource != null && this.SelectedHierarchy != null)
            {
                //Find corresponding items and expand or select them

                var item = SelectItem(ItemsSource, this.selectedHierarchy());
                this.SelectedItem = item;
            }
        }

        /// <summary>
        /// Searches the items of the hierarchy inside the items source and selects the last found item
        /// </summary>
        private static object SelectItem(IEnumerable items, IEnumerable<string> selectedHierarchy)
        {
            var enumerator = items.GetEnumerator();
            if (items == null || selectedHierarchy == null || !enumerator.MoveNext() || !selectedHierarchy.Any())
            {
                return null;
            }

            var hierarchy = selectedHierarchy.ToList();
            var currentItems = items;
            TreeViewItem selectedItem = null;

            for (int i = 0; i < hierarchy.Count; i++)
            {
                enumerator = items.GetEnumerator();
                TreeViewItem currentItem = null;
                while (enumerator.MoveNext())
                {
                    //currentItems.FirstOrDefault(ci => ci.SelectedValuePath == hierarchy[i]);
                    if (enumerator.Current.ToString() == hierarchy[i])
                        selectedItem = enumerator.Current as TreeViewItem;

                }
                // get next item in the hierarchy from the collection of child items
                //var currentItem = currentItems.FirstOrDefault(ci => ci.SelectedValuePath == hierarchy[i]);
                if (currentItem == null)
                {
                    break;
                }

                selectedItem = currentItem;

                // rewrite the current collection of child items
                currentItems = selectedItem.ItemsSource;
                if (currentItems == null)
                {
                    break;
                }

                // the intermediate items will be expanded
                if (i != hierarchy.Count - 1)
                {
                    selectedItem.IsExpanded = true;
                }
            }

            if (selectedItem != null)
            {
                selectedItem.IsSelected = true;
            }

            return selectedItem;
        }

        /// <summary>
        /// Gets the hierarchy of the selected tree item and displays it at the combobox header
        /// </summary>
        private void SetSelectedItemToHeader()
        {
            string content = null;

            //var item = this.SelectedItem as ITreeViewItemModel;
            if (this.SelectedItem != null)
            {
                //Get hierarchy and display it as the selected item
                var hierarchy = selectedHierarchy();
                if (hierarchy.Length > 0)
                {
                    content = string.Join(" - ", hierarchy);
                }
            }

            this.SetContentAsTextBlock(content);
        }

        /// <summary>
        /// Gets the combobox header and displays the specified content there
        /// </summary>
        private void SetContentAsTextBlock(string content)
        {
            if (_contentPresenter == null)
            {
                return;
            }

            var tb = _contentPresenter.Content as TextBlock;
            if (tb == null)
            {
                _contentPresenter.Content = tb = new TextBlock();
            }
            tb.Text = content ?? ' '.ToString();

            _contentPresenter.ContentTemplate = null;
        }
    }


    static class TreeHelper
    {

        public static IEnumerable<object> GetAncestors(object vm, Func<object, object> parent)
        {
            yield return vm;
            while (parent(vm) != null)
            {
                yield return parent(vm);
                vm = parent(vm);
            }
        }

    }
}
