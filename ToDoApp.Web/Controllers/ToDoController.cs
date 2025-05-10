
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Enums;

namespace TodoApp.Web.Controllers
{
    public class ToDoController : Controller
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
        public async Task<IActionResult> Index(TodoStatus? status)
        {
            if (status.HasValue)
            {
                var todos = await _todoService.GetAll(status.Value.ToString());
                return View(todos);
            }
            var lstTodos = await _todoService.GetAll();
            return View(lstTodos);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var todo = await _todoService.Get(Guid.Parse(id));
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoDto todo)
        {
            if (ModelState.IsValid)
            {
                await _todoService.Add(todo);
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var todo = await _todoService.Get(Guid.Parse(id));
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TodoDto todo)
        {
            if (ModelState.IsValid)
            {
              var updated =  await _todoService.Update((Guid)todo.Id, todo);
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditToDoStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var todo = await _todoService.Get(Guid.Parse(id));
            if (todo == null)
                return NotFound();

            todo.Status = TodoStatus.Completed;
            await _todoService.Update((Guid)todo.Id, todo);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost("ToDo/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var todo = await _todoService.Get(Guid.Parse(id));
            if (todo == null)
            {
                return NotFound();
            }

            await _todoService.Delete(Guid.Parse(id));

            return RedirectToAction(nameof(Index)); 
        }
    }
}
