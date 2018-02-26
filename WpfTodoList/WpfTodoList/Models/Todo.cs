using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfTodoList.Services;

namespace WpfTodoList.Models
{
    public class Todo: BindableBase, ITodo
    {

        #region Variables

        private static bool _isCompletedUpdateAllowed;

        #endregion

        #region Constructor

        public Todo()
        {            
            _isCompletedUpdateAllowed = true;
            Children = new ObservableCollection<ITodo>();
        }        

        #endregion

        #region ITodo Members

        private string _task;
        public string Task
        {
            get { return _task; }
            set { SetProperty(ref _task, value); }
        }
        
        private DateTime _deadline;
        public DateTime Deadline
        {
            get { return _deadline; }
            set { SetProperty(ref _deadline, value); }
        }
        
        public bool _isCompleted;
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                SetProperty(ref _isCompleted, value);

                if (_isCompletedUpdateAllowed)
                    UpdateFamilyCompletedStatus();
            }
        }
        
        private string _moreDetails;
        public string MoreDetails
        {
            get { return _moreDetails; }
            set { SetProperty(ref _moreDetails, value); }
        }
        
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        private bool _isHighlighted;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set { SetProperty(ref _isHighlighted, value); }
        }
                
        private ITodo _parent;
        public ITodo Parent 
        {
            get { return _parent; }
            set { SetProperty(ref _parent, value); }
        }
        
        private ObservableCollection<ITodo> _children;
        public ObservableCollection<ITodo> Children
        {
            get { return _children; }
            set { SetProperty(ref _children, value); }
        }

        #endregion

        #region Helper Methods
        
        private void UpdateFamilyCompletedStatus()
        {
            _isCompletedUpdateAllowed = false; // don't allow this method to be call recursively while updating family

            // if this is a parent todo, apply same completed status to all children
            if (Parent == null)
            {
                foreach (var child in Children)
                {
                    child.IsCompleted = IsCompleted;
                }
            }
            else // if this is child, mark parent as completed when all sibling children are done and vice versa
            {
                bool allSiblingsCompleted = Parent.Children.All(x => x.IsCompleted);
                Parent.IsCompleted = allSiblingsCompleted;
            }

            _isCompletedUpdateAllowed = true;
        }
        
        #endregion
    }
}
