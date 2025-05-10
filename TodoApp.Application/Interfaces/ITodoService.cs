using TodoApp.Application.DTOs;

namespace TodoApp.Application.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDto>> GetAll();
        Task<IEnumerable<TodoDto>> GetAll(string status);
        Task<TodoDto> Get(Guid id);
        Task<int> Add(TodoDto todoDto);
        Task<int> Update(Guid id, TodoDto todoDto);
        Task<int> Delete(Guid id);
    }
}
