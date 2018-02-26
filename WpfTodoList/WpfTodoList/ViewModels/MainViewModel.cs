using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTodoList.Services;
using WpfTodoList.Models;

namespace WpfTodoList.ViewModels
{
    public class MainViewModel : BindableBase, INavigationAware
    {
        #region Variables

        private IRegionManager _regionManager;

        #endregion

        #region Constructor

        public MainViewModel(IRegionManager regionManager, ITodoManager todoManager)
        {
            _regionManager = regionManager;
            _todoManager = todoManager;

            // init commands
            SelectedItemChangedCommand = new DelegateCommand(ProcessSelectedItemChanged);
            AddTodoCommand = new DelegateCommand(AddTodo).ObservesCanExecute(() => CanAddTodo);
            DeleteTodoCommand = new DelegateCommand(DeleteTodo).ObservesCanExecute(() => CanDeleteTodo);
            MouseClickCommand = new DelegateCommand(TreeMouseClick);

            // init can executes
            CanAddTodo = true;
            CanDeleteTodo = false;

            // init other vars
            Title = "Todo List";
        }

        #endregion

        #region Commands

        public DelegateCommand SelectedItemChangedCommand { get; private set; }
        
        private bool _canAddTodo;
        public bool CanAddTodo
        {
            get { return _canAddTodo; }
            set { SetProperty(ref _canAddTodo, value); }
        }
        public DelegateCommand AddTodoCommand { get; set; }
        
        private bool _canDeleteTodo;
        public bool CanDeleteTodo
        {
            get { return _canDeleteTodo; }
            set { SetProperty(ref _canDeleteTodo, value); }
        }
        public DelegateCommand DeleteTodoCommand { get; set; }
                
        public DelegateCommand MouseClickCommand { get; set; }
                
        #endregion

        #region Properties
        
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ITodo _selectedItem;
        public ITodo SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        private ITodoManager _todoManager;
        public ITodoManager TodoManager
        {
            get { return _todoManager; }
            set { SetProperty(ref _todoManager, value); }
        }

        #endregion

        #region Helper Methods

        // Triggered when the mouse clicks anywhere on the TreeView control
        private void TreeMouseClick()
        {
            // workaround to unselect any selected items
            // (UI control doesn't provide an unselect option itself)
            if (SelectedItem != null)
                SelectedItem.IsSelected = false;
        }
        
        // Triggered by delete button
        private void DeleteTodo()
        {
            _todoManager.RemoveTodo(SelectedItem);
        }
        
        // Triggered by Add button
        private void AddTodo()
        {            
            // navigate to add view 
            _regionManager.RequestNavigate("ContentRegion", "AddView");
        }

        // Triggered when selection changes in TreeView (and unselect from TreeMouseClick)
        private void ProcessSelectedItemChanged()
        {
            // determine selected item
            SelectedItem = _todoManager.GetSelectedItem();
            
            // update can execute properties:

            // allow delete if todo is selected
            CanDeleteTodo = SelectedItem != null;

            // allow add if no selection or parent selected
            CanAddTodo = SelectedItem == null || (SelectedItem != null && SelectedItem.Parent == null); 
        }
        
        #endregion

        #region INavigationAware Members
        
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var todoToAdd = navigationContext.Parameters["todoToAdd"] as ITodo;

            if (todoToAdd != null)
            {
                // consider previous selection as parent
                var parentTodo = SelectedItem;

                _todoManager.AddTodo(todoToAdd, parentTodo);
            }
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
