using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Controls
{

    public class ComboBoxTreeView : ComboBox
    {
        public static readonly DependencyProperty SelectedHierarchyProperty = DependencyProperty.Register("SelectedHierarchy", typeof(IEnumerable), typeof(ComboBoxTreeView), new PropertyMetadata(null));
        public static readonly DependencyProperty ParentPathProperty = DependencyProperty.Register("ParentPath", typeof(string), typeof(ComboBoxTreeView), new PropertyMetadata());
        public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register("SelectedNode", typeof(object), typeof(ComboBoxTreeView), new FrameworkPropertyMetadata(default));

        private ExtendedTreeView _treeView;

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
            this.UpdateSelectedItem();
            base.OnApplyTemplate();
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
            var hierarchy = selectedHierarchy();
            this.SelectedItem = hierarchy.First();
            this.SelectedHierarchy = hierarchy;
            UpdateSelectedItem();
            this.IsDropDownOpen = false;
            this.SelectedNode = _treeView.SelectedItem;
   
        }

        #region properties

        public object SelectedNode
        {
            get { return (object)GetValue(SelectedNodeProperty); }
            set { SetValue(SelectedNodeProperty, value); }
        }

        public IEnumerable SelectedHierarchy
        {
            get { return (IEnumerable)GetValue(SelectedHierarchyProperty); }
            set { SetValue(SelectedHierarchyProperty, value); }
        }

        public string ParentPath
        {
            get { return (string)GetValue(ParentPathProperty); }
            set { SetValue(ParentPathProperty, value); }
        }
        #endregion properties

        private void UpdateSelectedItem()
        {
            if (_treeView.SelectedItem != null)
            {
                var hierarchy = selectedHierarchy();
                SelectedHierarchy = hierarchy;
                SelectedNode = hierarchy.Last();
            }
        }

        private object[] selectedHierarchy()
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
