using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCore.Core.Models
{
    public class TodoItem
    {
        public int TodoItemId { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
    }
}
