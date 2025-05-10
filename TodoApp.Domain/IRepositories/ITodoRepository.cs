using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Models;

namespace TodoApp.Domain.Repository
{
    public interface ITodoRepository
    {
        Task<Todo> Get(Guid id);
        Task<List<Todo>> GetAll();
        Task Add(Todo todo);
        Task Update(Todo todo);
        Task Delete(Guid id);
        Task<int> SaveChanges();
    }
}
