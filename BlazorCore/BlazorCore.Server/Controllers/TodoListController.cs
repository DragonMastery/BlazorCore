using BlazorCore.Core.Models;
using BlazorCore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETBlazorCRUDApp.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/todo")]
    public class TodoListController : Controller
    {
        private readonly AppDbContext _appDb;

        public TodoListController(AppDbContext appDb)
        {
            _appDb = appDb;
        }

        // GET: api/ToDo
        [HttpGet]
        public IEnumerable<TodoItem> GetToDo()
        {
            return _appDb.TodoItems;
        }

        // POST: api/ToDo
        [HttpPost]
        public async Task<IActionResult> PostToDo([FromBody] TodoItem todo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _appDb.TodoItems.Add(todo);
            await _appDb.SaveChangesAsync();
            return CreatedAtAction("GetToDo", new { id = todo.TodoItemId }, todo);
        }

        // PUT: api/ToDo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDo([FromRoute] int id, [FromBody] TodoItem todo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != todo.TodoItemId)
                return BadRequest();

            _appDb.Entry(todo).State = EntityState.Modified;
            try
            {
                await _appDb.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/ToDo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo([FromRoute] int id)
        {
            var ToDo = await _appDb.TodoItems.SingleOrDefaultAsync(m => m.TodoItemId == id);
            if (ToDo == null)
                return NotFound();

            _appDb.TodoItems.Remove(ToDo);
            await _appDb.SaveChangesAsync();
            return Ok(ToDo);
        }

        private bool ToDoExists(int id)
        {
            return _appDb.TodoItems.Any(e => e.TodoItemId == id);
        }
    }
}