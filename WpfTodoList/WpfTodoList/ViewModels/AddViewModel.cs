using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTodoList.Models;
using WpfTodoList.Services;

namespace WpfTodoList.ViewModels
{
    class AddViewModel : BindableBase, INavigationAware
    {
        #region Variables

        private IRegionManager _regionManager;
        private ITodoManager _todoManager;

        #endregion

        #region Constructor

        public AddViewModel(IRegionManager regionManager, ITodoManager todoManager)
        {
            _regionManager = regionManager;
            _todoManager = todoManager;

            // init commands
            SaveCommand = new DelegateCommand(Save).ObservesCanExecute(() => CanSave);
            CancelCommand = new DelegateCommand(Cancel);
            InputUpdatedCommand = new DelegateCommand(ProcessInputUpdated);

            // init can executes
            CanSave = false;

            // init other vars
            Title = "Add Todo";
            DefaultDeadline = DateTime.Now.Date;
        }

        #endregion

        #region Commands
                
        private bool _canSave;
        public bool CanSave
        {
            get { return _canSave; }
            set { SetProperty(ref _canSave, value); }
        }
        public DelegateCommand SaveCommand { get; set; }
        
        public DelegateCommand CancelCommand { get; set; }

        public DelegateCommand InputUpdatedCommand { get; set; }

        #endregion

        #region Properties
        
        private ITodo _todoToAdd;
        public ITodo TodoToAdd
        {
            get { return _todoToAdd; }
            set { SetProperty(ref _todoToAdd, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private DateTime _defaultDeadline;
        public DateTime DefaultDeadline
        {
            get { return _defaultDeadline; }
            set { SetProperty(ref _defaultDeadline, value); }
        }
        
        #endregion

        #region Helper Methods

        // Triggered when text changed in UI control
        private void ProcessInputUpdated()
        {
            // update save button status
            CanSave = !string.IsNullOrWhiteSpace(TodoToAdd.Task);
        }

        // Triggered by save button
        private void Save()
        {
            // navigate to main view with saved item
            var parameters = new NavigationParameters();
            parameters.Add("todoToAdd", TodoToAdd);
            _regionManager.RequestNavigate("ContentRegion", "MainView", parameters);
        }

        // Triggered by cancel button
        private void Cancel()
        {
            // navigate to main view without saving
            _regionManager.RequestNavigate("ContentRegion", "MainView");
        }

        #endregion

        #region INavigationAware Members

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // init variables on navigation to this screen:
            TodoToAdd = new Todo(); // create blank todo item
            TodoToAdd.Deadline = DateTime.Now.Date; // apply default
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // do nothing
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // do nothing
        }

        #endregion

    }
}
