using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITodoService _todoService;
        public List<TodoDto> Todos { get; set; }

        public IndexModel(ITodoService todoService)
        {
            _todoService = todoService;
        }
        public async Task OnGet(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                Todos = (await _todoService.GetAll()).ToList();
            }
            else
            {
                Todos = (await _todoService.GetAll(status)).ToList();
            }
        }
    }
}
