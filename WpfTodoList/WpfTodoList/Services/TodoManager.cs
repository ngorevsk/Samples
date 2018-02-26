using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTodoList.Models;

namespace WpfTodoList.Services
{
    public class TodoManager : BindableBase, ITodoManager
    {
        #region Constructor

        public TodoManager()
        {
            Parents = new ObservableCollection<ITodo>();
        }

        #endregion

        #region Properties

        private ObservableCollection<ITodo> _parents;
        public ObservableCollection<ITodo> Parents
        {
            get { return _parents; }
            set { SetProperty(ref _parents, value); }
        }

        #endregion

        #region ITodo Members
        
        public void RemoveTodo(ITodo todo)
        {
            if (todo == null)
                return; // abort, cannot remove null todo

            if (Parents.Contains(todo)) // remove parent
                Parents.Remove(todo);
            else if (todo.Parent != null && todo.Parent.Children.Contains(todo))  // if todo has parent, remove child
                todo.Parent.Children.Remove(todo);

        }

        public void AddTodo(ITodo todo)
        {
            AddTodo(todo, null);
        }

        public void AddTodo(ITodo todo, ITodo parent)
        {
            if (todo == null)
                return; // abort, cannot add null todo

            if (parent != null && parent.Parent != null)
                return; // abort, cannot add grandchild todo

            if (parent != null) // add child
            {
                parent.Children.Add(todo);
                todo.Parent = parent;   // apply child -> parent relationship
            }
            else                    // otherwise add parent
                Parents.Add(todo);

            UpdateToggledForAddedItem(todo);
            
            UpdateHighlightingForItemsPastDeadline();
        }

        public ITodo GetSelectedItem()
        {
            ITodo selectedTodo = null;

            // scan through all parent and children todos looking for the one that is selected
            foreach (var parent in Parents)
            {
                if (parent.IsSelected)
                {
                    selectedTodo = parent;
                    break;
                }
                
                foreach (var child in parent.Children)
                {
                    if (child.IsSelected)
                    {
                        selectedTodo = child;
                        break;
                    }
                }
            }

            return selectedTodo;
        }
               
        public int GetTotalParentCount()
        {
            return Parents.Count;
        }
        
        public int GetTotalChildCount()
        {
            int count = 0;
            foreach(var parent in Parents)
            {
                foreach(var child in parent.Children)
                {
                    count++;
                }
            }

            return count;
        }

        public int GetTotalTodoCount()
        {
            int count = 0;
            foreach (var parent in Parents)
            {
                count++;

                foreach (var child in parent.Children)
                {
                    count++;
                }
            }

            return count;
        }

        #endregion

        #region Helper Methods

        private void UpdateToggledForAddedItem(ITodo todoItem)
        {
            if (todoItem != null && todoItem.Parent != null) // if child added, untoggle parent
                todoItem.Parent.IsCompleted = false;
        }

        private void UpdateHighlightingForItemsPastDeadline()
        {
            DateTime deadline = DateTime.Now.Date;

            foreach (var parent in Parents)
            {
                parent.IsHighlighted = parent.Deadline < deadline;

                foreach (var child in parent.Children)
                {
                    child.IsHighlighted = child.Deadline < deadline;
                }
            }
        }

        #endregion
    }
}
