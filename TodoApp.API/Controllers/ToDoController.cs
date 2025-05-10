using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Enums;

namespace TodoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        #region Fields
        private readonly ITodoService _todoService;
        #endregion

        #region CTOR
        public ToDoController(ITodoService todoService)
        {
            _todoService = todoService;
        }
        #endregion

        #region Methods
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var todos = await _todoService.GetAll();
            return Ok(todos);
        }
        [HttpGet("GetAll/{status}")]
        public async Task<ActionResult> GetAll(TodoStatus status)
        {
            var todos = await _todoService.GetAll(status.ToString());
            return Ok(todos);
        }
        [HttpGet("Details/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var todo = await _todoService.Get(Guid.Parse(id));
            return Ok(todo);
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create(TodoDto todoDto)
        {
            var todo = await _todoService.Add(todoDto);
            if (todo == null)
                return BadRequest("Todo not created");
            return Ok();
        }
        [HttpPost("UpdateStatus/{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, TodoStatus status)
        {
            if (!Enum.IsDefined(typeof(TodoStatus), status))
                return BadRequest("Invalid status value.");

            var todo = await _todoService.Get(id);
            if (todo == null)
                return NotFound("Todo not found");
            todo.Status = status;
            var result = await _todoService.Update(id, todo);

            return Ok("Todo status updated successfully");
        }
        [HttpPost("Edit")]
        public async Task<ActionResult> Edit(TodoDto todoDto)
        {
            var todo =await _todoService.Update((Guid)todoDto.Id, todoDto);
            if (todo == null)
                return BadRequest("Todo not updated");
            return Ok();
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete(string id )
        {
            var todo = await _todoService.Delete(Guid.Parse(id));
            if (todo == null)
                return BadRequest("Todo not deleted");
            return Ok();
        }

    }
    #endregion
}
