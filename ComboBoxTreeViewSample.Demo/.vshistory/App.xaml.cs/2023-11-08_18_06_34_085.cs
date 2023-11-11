﻿using ComboBoxTreeView.Demo;
using System.Windows;

namespace VortexWolf.ControlsSample
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new Window { Content = new MainUserControl() }.Show();
            base.OnStartup(e);
        }

    }
}
