using C3P1.Net.Data;
using C3P1.Net.Data.Models;
using C3P1.Net.Identity.Data;
using C3P1.Net.Services.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace C3P1.Net.Services.API.Tools
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/tools/[controller]")]
    [ApiController]
    public class TasklistController : ControllerBase
    {
        private readonly ITasklistService _tasklistService;
        private readonly UserManager<C3P1User> _userManager;
        private readonly C3P1Context _context;
        // constructor
        public TasklistController(ITasklistService tasklistService, UserManager<C3P1User> userManager, C3P1Context context)
        {
            _tasklistService = tasklistService;
            _userManager = userManager;
            _context = context;
        }
        // GET: api/tools/<TasklistController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> Get()
        {
            var name = User.Identity?.Name;
            var user = _context.Users.Where(x => x.UserName == name).FirstOrDefault();
            if (user is null)
            {
                // should not happen
                return BadRequest();
            }

            var currentUserId = Guid.Parse(user.Id);
            var result = await _tasklistService.GetTasklistAsync(currentUserId);

            if (result is null)
            {
                return NotFound();
            }
            else
            {
                return result;
            }
        }
        // GET: api/tools/<TasklistController>/todo
        [HttpGet("todo")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodo()
        {
            var name = User.Identity?.Name;
            var user = _context.Users.Where(x => x.UserName == name).FirstOrDefault();
            if (user is null)
            {
                // should not happen
                return BadRequest();
            }

            var currentUserId = Guid.Parse(user.Id);
            var result = await _tasklistService.GetTodoTasklistAsync(currentUserId);

            if (result is not null)
            {
                return result;
            }
            else
            {
                return NotFound();
            }
        }
        // GET: api/tools/<TasklistController>/done
        [HttpGet("done")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetDone()
        {
            var name = User.Identity?.Name;
            var user = _context.Users.Where(x => x.UserName == name).FirstOrDefault();
            if (user is null)
            {
                // should not happen
                return BadRequest();
            }

            var currentUserId = Guid.Parse(user.Id);
            var result = await _tasklistService.GetDoneTasklistAsync(currentUserId);

            if (result is not null)
            {
                return result;
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/<TasklistController>/{Guid}
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> Get(Guid id)
        {
            var name = User.Identity?.Name;
            var user = _context.Users.Where(x => x.UserName == name).FirstOrDefault();
            if (user is null)
            {
                // should not happen
                return BadRequest();
            }

            var currentUserId = Guid.Parse(user.Id);
            var result = await _tasklistService.GetTodoItemAsync(currentUserId, id);

            if (result is not null)
            {
                return result;
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<TasklistController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TodoItem item)
        {
            var name = User.Identity?.Name;
            var user = _context.Users.Where(x => x.UserName == name).FirstOrDefault();
            if (user is null)
            {
                // should not happen
                return BadRequest();
            }

            var currentUserId = Guid.Parse(user.Id);

            var todoItem = new TodoItem
            {
                Title = item.Title,
                CreationTime = item.CreationTime,
                DueTime = item.DueTime
            };

            await _tasklistService.AddTodoItemAsync(currentUserId, todoItem);

            /*return CreatedAtAction(
                nameof(TodoItem),
                new { id = todoItem.Id },
                ItemToDTO(todoItem));*/

            return Ok(todoItem);
        }

        // PUT api/<TasklistController>/5
        [HttpPut/*("{id}")*/]
        public async Task<IActionResult> Put([FromBody] TodoItem item)
        {
            var name = User.Identity?.Name;
            var user = _context.Users.Where(x => x.UserName == name).FirstOrDefault();
            if (user is null)
            {
                // auth failed, should not happen
                return BadRequest();
            }

            var currentUserId = Guid.Parse(user.Id);

            try
            {
                var todoItem = await _tasklistService.UpdateTodoItemAsync(item);
                return Ok(todoItem);
            }
            catch
            {
                // item does not exist
                return BadRequest();
            }
        }

        // DELETE api/<TasklistController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var name = User.Identity?.Name;
            var user = _context.Users.Where(x => x.UserName == name).FirstOrDefault();
            if (user is null)
            {
                // auth failed, should not happen
                return BadRequest();
            }

            var currentUserId = Guid.Parse(user.Id);

            try
            {
                var todoItem = await _tasklistService.DeleteTodoItemAsync(currentUserId, id);
                return Ok(todoItem);
            }
            catch
            {
                // item does not exist
                return BadRequest();
            }
        }

        // DTO protecting from overposting
        /*private static TodoItem ItemToDTO(TodoItem todoItem) => new TodoItem
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Completed = todoItem.Completed,
            CreationTime = todoItem.CreationTime,
            DueTime = todoItem.DueTime
        };*/
    }
}
