using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTodoList.Models
{
    public interface ITodo
    {
        string Task { get; set; }

        DateTime Deadline { get; set; }

        bool IsCompleted { get; set; }
                
        string MoreDetails { get; set; }
        
        bool IsExpanded { get; set; }

        bool IsSelected { get; set; }

        bool IsHighlighted { get; set; }

        ITodo Parent { get; set; }

        ObservableCollection<ITodo> Children { get; set; }
        
    }
}
