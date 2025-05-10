using AutoMapper;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Enums;
using TodoApp.Domain.Models;
using TodoApp.Domain.Repository;

namespace TodoApp.Application.Services
{
    public class TodoService : ITodoService
    {
        #region Fields
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;
        #endregion
        #region CTOR
        public TodoService(ITodoRepository todoRepository , IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;

        }
        #endregion
        #region Methods
        public async Task<TodoDto> Get(Guid id)
        {
           var todo = await _todoRepository.Get(id);
            if (todo == null)
                throw new Exception("Todo not found");

            return _mapper.Map<TodoDto>(todo);
        }

        public async Task<IEnumerable<TodoDto>> GetAll()
        {
            var todos = await _todoRepository.GetAll();
            if (todos == null)
                throw new Exception("Todos not found");

            return _mapper.Map<IEnumerable<TodoDto>>(todos);
        }
        public async Task<List<TodoDto>> GetAll(string status)
        {
            var todos = await _todoRepository.GetAll();

            if (Enum.IsDefined(typeof(TodoStatus), status))
            {
                TodoStatus statusEnum = (TodoStatus)Enum.Parse(typeof(TodoStatus), status, true);
                todos = todos.Where(t => t.Status == statusEnum).ToList();
            }

            return _mapper.Map<List<TodoDto>>(todos);
        }
        public async Task<int> Add(TodoDto todoDto)
        {
            var todo = _mapper.Map<Todo>(todoDto);
            todo.Id = Guid.NewGuid();
            if (todo == null)
                throw new Exception("Invalid Todo data");

            await _todoRepository.Add(todo);
            return await _todoRepository.SaveChanges();
        }

        public async Task<int> Update(Guid id, TodoDto todoDto)
        {
            var existingTodo = await _todoRepository.Get(id);
            if (existingTodo == null)
                throw new Exception("Todo not found");

            var updatedToDo = _mapper.Map(todoDto, existingTodo); 
            await _todoRepository.Update(updatedToDo); 
            return await _todoRepository.SaveChanges();
        }
        public Task<int> Delete(Guid id)
        {
            var existingTodo = _todoRepository.Get(id);
            if (existingTodo == null)
                throw new Exception("Todo not found");
            _todoRepository.Delete(id);
            return _todoRepository.SaveChanges();
        }
        #endregion
    }
}
