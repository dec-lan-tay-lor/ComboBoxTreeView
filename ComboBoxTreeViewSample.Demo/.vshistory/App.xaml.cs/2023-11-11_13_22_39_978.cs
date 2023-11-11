using ComboBoxTreeView.Demo;
using System.Windows;

namespace ComboBoxTreeView.Demo
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //new Window { Content = new MainUserControl() }.Show();
            new Window { Content = new SecondaryUserControl(), Height=50, Width=200 }.Show();
            base.OnStartup(e);
        }

    }
}
