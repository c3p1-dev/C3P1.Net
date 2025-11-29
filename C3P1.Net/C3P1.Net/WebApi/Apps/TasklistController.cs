using C3P1.Net.Client.Data;
using C3P1.Net.Client.Data.Apps.Tasklist;
using C3P1.Net.Client.Services.Apps;
using C3P1.Net.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.WebApi.Apps
{
    [Authorize]
    [Route("api/apps/[controller]")]
    [ApiController]
    public class TasklistController(ITasklistService tasklistService, AppDbContext context, UserManager<AppUser> userManager) : ControllerBase
    {
        private readonly ITasklistService _tasklistService = tasklistService;
        private readonly AppDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;

        // GET : api/apps/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTasklistAsync()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen due to [Authorize] attribute
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // get all tasks from current user
            var result = await _tasklistService.GetTasklistAsync(currentUserId);

            if (result == null)
                return BadRequest();
            else
                return Ok(result);
        }
        // GET : api/apps/[controller]/todo
        [HttpGet]
        [Route("todo")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTasklistTodoAsync()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // get all tasks from current user
            var result = await _tasklistService.GetTasklistTodoAsync(currentUserId);

            if (result == null)
                return BadRequest();
            else
                return result;
        }
        // GET : api/apps/[controller]/done
        [HttpGet]
        [Route("done")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTasklistDoneAsync()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // get all tasks from current user
            var result = await _tasklistService.GetTasklistDoneAsync(currentUserId);

            if (result == null)
                return BadRequest();
            else
                return result;
        }

        // POST : api/apps/[controller]
        // data [FromBody]
        [HttpPost]
        public async Task<ActionResult<bool>> AddTaskAsync([FromBody] TodoItem task)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // add a task
            bool result = await _tasklistService.AddTaskAsync(currentUserId, task);

            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // DELETE : api/apps/[controller]/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<bool>> DeleteTaskAsync(Guid id)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // delete task from id
            var result = await _tasklistService.DeleteTaskAsync(currentUserId, id);

            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // PUT : api/apps/[controller]
        // data [FromBody]
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateTaskAsync([FromBody] TodoItem task)
        {
            // update task
            var result = await _tasklistService.UpdateTaskAsync(task);

            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // GET : api/apps/[controller]/done/delete
        [HttpGet("done/delete")]
        public async Task<ActionResult<List<TodoItem>>> DeleteTasklistDoneAsync()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // delete done tasks
            var result = await _tasklistService.DeleteTasklistDoneAsync(currentUserId);

            if (result == null)
                return BadRequest();
            else
                return Ok(result);
        }

        // GET : api/apps/[controller]/todo/markasdone
        [HttpGet("todo/markasdone")]
        public async Task<ActionResult<List<TodoItem>>> MarkTasklistAsDoneAsync()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await _userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // mark all todo as done
            var result = await _tasklistService.MarkTasklistAsDoneAsync(currentUserId);

            if (result == null)
                return BadRequest();
            else
                return Ok(result);
        }
    }
}
