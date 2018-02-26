using Microsoft.Practices.Unity;
using Prism.Unity;
using WpfTodoList.Views;
using System.Windows;
using Prism.Mvvm;
using System.Reflection;
using System;
using Prism.Modularity;
using WpfTodoList.Modules;
using WpfTodoList.Services;

namespace WpfTodoList
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();            
        }

        protected override void ConfigureModuleCatalog()
        {
            var catalog = (ModuleCatalog)ModuleCatalog;
            catalog.AddModule(typeof(UIModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<ITodoManager, TodoManager>(new ContainerControlledLifetimeManager());
        }
    }
}
