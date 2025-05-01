using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Services
{
    public interface ITodoProvider
    {
        Task<IEnumerable<Todo>> GetAllAsync();
        Task<IEnumerable<Todo>> SearchAsync(string searchTerm);
        Task<Todo?> GetByIdAsync(int id);
        Task<Todo> CreateAsync(Todo todo);
        Task<Todo> UpdateAsync(Todo todo);
        Task<bool> DeleteAsync(int id);
    }
}