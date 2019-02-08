using BlazorCore.Core.Models;
using BlazorCore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCore.App.Services
{
    public class TodoListController 
    {
        private readonly AppDbContext _appDb;

        public TodoListController(AppDbContext appDb)
        {
            _appDb = appDb;
        }


        // POST: api/ToDo
        public void PostToDo(TodoItem todo)
        {
            _appDb.TodoItems.Add(todo);
             _appDb.SaveChanges();
        }

    }
}