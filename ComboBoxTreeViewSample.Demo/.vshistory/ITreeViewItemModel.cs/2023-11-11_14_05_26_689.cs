﻿using System.Collections.Generic;
using System.ComponentModel;

namespace Utility.Controls
{
    public interface ITreeViewItemModel : INotifyPropertyChanged
    {
        string SelectedValuePath { get; }

        string DisplayValuePath { get; }

        bool IsExpanded { get; set; }

        bool IsSelected { get; set; }

        IEnumerable<ITreeViewItemModel> GetHierarchy();

        IEnumerable<ITreeViewItemModel> GetChildren();
    }
}