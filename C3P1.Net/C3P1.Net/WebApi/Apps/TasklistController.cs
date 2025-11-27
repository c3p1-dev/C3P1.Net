using C3P1.Net.Client.Components.Apps.Tasklist;
using C3P1.Net.Client.Data;
using C3P1.Net.Client.Data.Apps.Tasklist;
using C3P1.Net.Client.Services.Apps;
using C3P1.Net.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace C3P1.WebApi.Apps
{
    [Authorize]
    [Route("api/apps/[controller]")]
    [ApiController]
    public class TasklistController : ControllerBase
    {
        private readonly ITasklistService _tasklistService;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TasklistController(ITasklistService tasklistService, AppDbContext context, UserManager<AppUser> userManager)
        {
            _tasklistService = tasklistService;
            _context = context;
            _userManager = userManager;
        }

        // GET : api/apps/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTasklist()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _context.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return BadRequest("Auth issue");
            }

            var currentUserId = Guid.Parse(user.Id);

            // get all tasks from current user
            var result = await _tasklistService.GetTasklistAsync(currentUserId);

            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return result;
            }
        }
        // GET : api/apps/[controller]/todo
        [HttpGet]
        [Route("todo")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTasklistTodo()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _context.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return BadRequest("Auth issue");
            }

            var currentUserId = Guid.Parse(user.Id);

            // get all tasks from current user
            var result = await _tasklistService.GetTasklistTodoAsync(currentUserId);

            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return result;
            }
        }
        // GET : api/apps/[controller]/done
        [HttpGet]
        [Route("done")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTasklistDone()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _context.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return BadRequest("Auth issue");
            }

            var currentUserId = Guid.Parse(user.Id);

            // get all tasks from current user
            var result = await _tasklistService.GetTasklistDoneAsync(currentUserId);

            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return result;
            }
        }

        // POST : api/apps/[controller]
        // data [FromBody]
        [HttpPost]
        public async Task<ActionResult<bool>> AddTask([FromBody] TodoItem task)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _context.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return BadRequest("Auth issue");
            }

            var currentUserId = Guid.Parse(user.Id);

            // add a task
            bool result = await _tasklistService.AddTaskAsync(currentUserId, task);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // DELETE : api/apps/[controller]/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<bool>> DeleteTask(Guid id)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _context.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return BadRequest("Auth issue");
            }

            var currentUserId = Guid.Parse(user.Id);

            // delete task from id
            var result = await _tasklistService.DeleteTaskAsync(currentUserId, id);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // PUT : api/apps/[controller]
        // data [FromBody]
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateTask([FromBody] TodoItem task)
        {
            // update task
            var result = await _tasklistService.UpdateTaskAsync(task);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // GET : api/apps/[controller]/done/delete
        [HttpGet("done/delete")]
        public async Task<ActionResult<List<TodoItem>>> DeleteTasklistDone()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _context.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return BadRequest("Auth issue");
            }

            var currentUserId = Guid.Parse(user.Id);

            // delete done tasks
            var result = await _tasklistService.DeleteTasklistDoneAsync(currentUserId);

            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(result);
            }
        }

        // GET : api/apps/[controller]/todo/markasdone
        [HttpGet("todo/markasdone")]
        public async Task<ActionResult<List<TodoItem>>> MarkTasklistAsDone()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _context.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return BadRequest("Auth issue");
            }

            var currentUserId = Guid.Parse(user.Id);

            // mark all todo as done
            var result = await _tasklistService.MarkTasklistAsDoneAsync(currentUserId);

            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
