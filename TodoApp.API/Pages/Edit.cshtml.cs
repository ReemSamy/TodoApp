using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Pages
{
    public class EditModel : PageModel
    {
        private readonly ITodoService _todoService;

        [BindProperty]
        public TodoDto Todo { get; set; }

        public EditModel(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public async Task OnGetAsync(Guid id)
        {
            Todo = await _todoService.Get(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _todoService.Update((Guid)Todo.Id, Todo);
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
