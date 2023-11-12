using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Controls
{
    public class ComboBoxTreeView : ComboBox
    {
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(ComboBoxTreeView), new PropertyMetadata(null));
        public static readonly DependencyProperty ParentPathProperty = DependencyProperty.Register("ParentPath", typeof(string), typeof(ComboBoxTreeView), new PropertyMetadata());
        public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register("SelectedNode", typeof(object), typeof(ComboBoxTreeView), new FrameworkPropertyMetadata(default));
        public static readonly DependencyProperty IsCheckedPathProperty = DependencyProperty.Register("IsCheckedPath", typeof(string), typeof(ComboBoxTreeView), new PropertyMetadata());
        public static readonly DependencyProperty IsExpandedPathProperty = DependencyProperty.Register("IsExpandedPath", typeof(string), typeof(ComboBoxTreeView), new PropertyMetadata("IsExpanded"));
        public static readonly DependencyProperty IsSelectedPathProperty = DependencyProperty.Register("IsSelectedPath", typeof(string), typeof(ComboBoxTreeView), new PropertyMetadata("IsSelected"));

        private ExtendedTreeView _treeView;
        private ObservableCollection<object> list = new();

        static ComboBoxTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxTreeView), new FrameworkPropertyMetadata(typeof(ComboBoxTreeView)));
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            //don't call the method of the base class
        }

        public override void OnApplyTemplate()
        {
            _treeView = (ExtendedTreeView)this.GetTemplateChild("treeView");
            _treeView.OnHierarchyMouseUp += new MouseEventHandler(OnTreeViewHierarchyMouseUp);
            _treeView.OnChecked += _treeView_OnChecked;
            this.UpdateSelectedItem();
            base.OnApplyTemplate();
        }  

        private void _treeView_OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckableTreeViewItem { IsChecked: bool isChecked } item)
            {
                if(isChecked)
                {
                    list.Add(item.DataContext);
                }
                else if(list.Contains(item)) 
                {
                    list.Remove(item.DataContext);
                }
                SelectedItems = list;
            }
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            this.UpdateSelectedItem();
        }

        protected override void OnDropDownOpened(EventArgs e)
        {
            base.OnDropDownOpened(e);
            this.UpdateSelectedItem();
        }

        /// <summary>
        /// Handles clicks on any item in the tree view
        /// </summary>
        private void OnTreeViewHierarchyMouseUp(object sender, MouseEventArgs e)
        {
            var hierarchy = SelectItems();
            this.SelectedItem = hierarchy.First();
            this.SelectedItems = hierarchy;
            UpdateSelectedItem();
            this.IsDropDownOpen = false;
            this.SelectedNode = _treeView.SelectedItem;
        }

        #region properties

        public string IsExpandedPath
        {
            get { return (string)GetValue(IsExpandedPathProperty); }
            set { SetValue(IsExpandedPathProperty, value); }
        }

        public string IsSelectedPath
        {
            get { return (string)GetValue(IsSelectedPathProperty); }
            set { SetValue(IsSelectedPathProperty, value); }
        }

        public string IsCheckedPath
        {
            get { return (string)GetValue(IsCheckedPathProperty); }
            set { SetValue(IsCheckedPathProperty, value); }
        }

        public string ParentPath
        {
            get { return (string)GetValue(ParentPathProperty); }
            set { SetValue(ParentPathProperty, value); }
        }

        public object SelectedNode
        {
            get { return (object)GetValue(SelectedNodeProperty); }
            set { SetValue(SelectedNodeProperty, value); }
        }

        public IEnumerable SelectedItems
        {
            get { return (IEnumerable)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }


        #endregion properties

        private void UpdateSelectedItem()
        {
            if (_treeView.SelectedItem != null)
            {
                var hierarchy = SelectItems();
                SelectedItems = hierarchy;
                SelectedNode = hierarchy.Last();
            }
        }

        private object[] SelectItems()
        {
            var type = _treeView.SelectedItem.GetType();
            var propInfo = type.GetProperty(ParentPath);
            return TreeHelper.GetAncestors(_treeView.SelectedItem, a => propInfo.GetValue(a)).Reverse().ToArray();
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
