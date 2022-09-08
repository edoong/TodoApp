using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models.Entities;

namespace TodoApp.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TodoItemCommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodoItemCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TodoItemComments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemComment>>> GetTodoItemComment()
        {
          if (_context.TodoItemComment == null)
          {
              return NotFound();
          }
            return await _context.TodoItemComment.ToListAsync();
        }

        // GET: api/TodoItemComments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemComment>> GetTodoItemComment(int id)
        {
          if (_context.TodoItemComment == null)
          {
              return NotFound();
          }
            var todoItemComment = await _context.TodoItemComment.FindAsync(id);

            if (todoItemComment == null)
            {
                return NotFound();
            }

            return todoItemComment;
        }

        // PUT: api/TodoItemComments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItemComment(int id, TodoItemComment todoItemComment)
        {
            if (id != todoItemComment.Id)
            {
                return BadRequest();
            }
            var getPreviousDataTodoItemComment = await _context.TodoItemComment.Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if(getPreviousDataTodoItemComment != null)
            {
                todoItemComment.CreatedDate = getPreviousDataTodoItemComment.CreatedDate;
                todoItemComment.LastModifiedDate = DateTime.UtcNow;
            }
            _context.Entry(todoItemComment).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemCommentExists(id))
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

        // POST: api/TodoItemComments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItemComment>> PostTodoItemComment(TodoItemComment todoItemComment)
        {
          if (_context.TodoItemComment == null)
          {
              return Problem("Entity set 'ApplicationDbContext.TodoItemComment'  is null.");
          }
            todoItemComment.CreatedDate = DateTime.UtcNow;
            todoItemComment.LastModifiedDate = DateTime.UtcNow;

            _context.TodoItemComment.Add(todoItemComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItemComment", new { id = todoItemComment.Id }, todoItemComment);
        }

        // DELETE: api/TodoItemComments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItemComment(int id)
        {
            if (_context.TodoItemComment == null)
            {
                return NotFound();
            }
            var todoItemComment = await _context.TodoItemComment.FindAsync(id);
            if (todoItemComment == null)
            {
                return NotFound();
            }

            _context.TodoItemComment.Remove(todoItemComment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemCommentExists(int id)
        {
            return (_context.TodoItemComment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
