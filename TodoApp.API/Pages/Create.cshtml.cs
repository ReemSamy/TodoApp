using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ITodoService _todoService;

        [BindProperty]
        public TodoDto Todo { get; set; }

        public CreateModel(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public void OnGet()
        {
            // Initial load, no action here
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _todoService.Add(Todo);
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
