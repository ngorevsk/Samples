using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;
using WpfTodoList.Views;

namespace WpfTodoList.Modules
{
    class UIModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public UIModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterTypeForNavigation<MainView>();
            _container.RegisterTypeForNavigation<AddView>();

            // display initial view
            _regionManager.RequestNavigate("ContentRegion", "MainView");
        }
    }
}
