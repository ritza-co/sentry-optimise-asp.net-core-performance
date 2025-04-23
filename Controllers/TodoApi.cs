using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace todo_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoController> _logger;

        public TodoController(TodoContext context, ILogger<TodoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            try
            {
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(35));

                // Simulate a long operation (DB call, I/O, etc.)
                await Task.Delay(TimeSpan.FromSeconds(30), timeoutCts.Token);

                // Would not reach here unless delay is under timeout
                var result = await _context.TodoItems.ToListAsync(timeoutCts.Token);
                return result;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetTodoItem(int id)
        {
            var todoItem = _context.TodoItems.Find(id);
            if (todoItem == null)
                return NotFound();
            return todoItem;
        }

        [HttpPost]
        public ActionResult<TodoItem> CreateTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();
            return item;
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodoItem(int id, TodoItem item)
        {
            var existing = _context.TodoItems.Find(id);
            if (existing == null)
                return NotFound();

            existing.Title = item.Title;
            existing.IsDone = item.IsDone;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodoItem(int id)
        {
            var todoItem = _context.TodoItems.Find(id);
            if (todoItem == null)
                return NotFound();

            _context.TodoItems.Remove(todoItem);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            for (int i = 1; i <= 1000; i++)
            {
                var todo = new TodoItem { Title = $"Todo #{i}", IsDone = false };
                _context.TodoItems.Add(todo);

                for (int j = 1; j <= 10; j++)
                {
                    _context.Comments.Add(
                        new Comment { Content = $"Comment {j} on Todo {i}", TodoItem = todo }
                    );
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Seeded with todos and comments");
        }

        [HttpGet("nplusone-comments")]
        public async Task<IActionResult> NPlusOneComments()
        {
            var todos = await _context
                .TodoItems.OrderBy(t => t.Id) // optional for stable ordering
                .Take(500) // make sure it's slow enough
                .ToListAsync(); // one query

            foreach (var todo in todos)
            {
                todo.Comments = await _context
                    .Comments.Where(c => c.TodoItemId == todo.Id)
                    .ToListAsync(); // N+1 queries
            }

            return Ok(todos);
        }

        [HttpGet("with-comments-fixed")]
        public async Task<IActionResult> WithCommentsFixed()
        {
            var todos = await _context.TodoItems.Take(500).Include(t => t.Comments).ToListAsync(); // 1 query with join

            return Ok(todos);
        }
    }
}
