using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Models;
using TodoApp.Domain.Repository;
using TodoApp.Infrastructure.Context;

namespace TodoApp.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion

        #region CTOR
        public TodoRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<Todo> Get(Guid id) => await _context.Todos.FindAsync(id);

        public async Task<List<Todo>> GetAll() => await _context.Todos.ToListAsync();
        public async Task Add(Todo todo) => await _context.Todos.AddAsync(todo);
        public Task Update(Todo todo)
        {
            _context.Todos.Update(todo);
            return Task.CompletedTask;
        }
        public async Task Delete(Guid id) => await _context.Todos.Where(t => t.Id == id).ExecuteDeleteAsync();

        public async Task<int> SaveChanges() => await _context.SaveChangesAsync();
        #endregion
    }
}
