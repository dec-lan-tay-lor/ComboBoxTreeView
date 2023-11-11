using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ComboBoxTreeView.Demo
{
    public class SomeHierarchyViewModel : ITreeViewItemModel
    {
        public SomeHierarchyViewModel(string title, List<SomeHierarchyViewModel> children, SomeHierarchyViewModel parent = null)
        {
            this.Title = title;
            this.Parent = parent;
            this.Children = children;

            if (this.Children != null)
            {
                this.Children.ForEach(ch => ch.Parent = this);
            }
        }

        public string Title { get; set; }

        public SomeHierarchyViewModel Parent { get; set; }

        public List<SomeHierarchyViewModel> Children { get; set; }

        #region ITreeViewItemModel
        public string SelectedValuePath
        {
            get { return Title; }
        }

        public string DisplayValuePath
        {
            get { return Title; }
        }

        private bool isExpanded;

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public IEnumerable<ITreeViewItemModel> GetHierarchy()
        {
            return GetAscendingHierarchy().Reverse().Cast<ITreeViewItemModel>();
        }

        public IEnumerable<ITreeViewItemModel> GetChildren()
        {
            if (this.Children != null)
            {
                return this.Children.Cast<ITreeViewItemModel>();
            }

            return null;
        }

        #endregion

        private IEnumerable<SomeHierarchyViewModel> GetAscendingHierarchy()
        {
            var vm = this;

            yield return vm;
            while (vm.Parent != null)
            {
                yield return vm.Parent;
                vm = vm.Parent;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
