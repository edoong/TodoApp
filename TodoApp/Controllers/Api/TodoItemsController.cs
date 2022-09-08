using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models.Entities;

namespace TodoApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TodoItemsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItem()
        {
            if (_context.TodoItem == null)
            {
                return NotFound();
            }

            var getTodo = await _context.TodoItem
                  .Where(x => x.TaskOwner == _userManager.GetUserId(User))
                  .Include(x => x.ItemComments.OrderByDescending(x => x.CreatedDate))
                  .ToListAsync();

            foreach (var item in getTodo)
            {
                foreach (var itemComment in item.ItemComments)
                {
                    itemComment.UserName = _userManager.FindByIdAsync(itemComment.UserId).Result.UserName;
                }
            }

            return getTodo;
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            if (_context.TodoItem == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItem
                .Where(x => x.TaskOwner == _userManager.GetUserId(User))
                .Include(x => x.ItemComments.OrderByDescending(x => x.CreatedDate))
                .FirstOrDefaultAsync(x => x.Id == id);


            if (todoItem == null)
            {
                return NotFound();
            }

            foreach (var itemComment in todoItem.ItemComments)
            {
                itemComment.UserName = _userManager.FindByIdAsync(itemComment.UserId).Result.UserName;
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            var getPreviousTodoItem = await _context.TodoItem.Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (getPreviousTodoItem != null)
            {
                todoItem.CreatedDate = getPreviousTodoItem.CreatedDate;
                todoItem.LastModifiedDate = DateTime.UtcNow;
            }
            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            if (_context.TodoItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TodoItem'  is null.");
            }
            todoItem.CreatedDate = DateTime.UtcNow;
            todoItem.LastModifiedDate = DateTime.UtcNow;
            _context.TodoItem.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            if (_context.TodoItem == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItem.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(int id)
        {
            return (_context.TodoItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
