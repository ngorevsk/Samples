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
    public interface ITodoManager
    {
        ObservableCollection<ITodo> Parents { get; set; }
        
        void RemoveTodo(ITodo todo);

        void AddTodo(ITodo todo);

        void AddTodo(ITodo todo, ITodo parent);

        ITodo GetSelectedItem();
        
        int GetTotalParentCount();

        int GetTotalChildCount();

        int GetTotalTodoCount();
    }
}
