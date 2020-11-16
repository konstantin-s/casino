using prism_app.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using Prism.Unity;

namespace prism_app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<AppLog>();
        }
    }
}