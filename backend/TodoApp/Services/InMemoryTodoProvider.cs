using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class InMemoryTodoProvider : ITodoProvider
    {
        private readonly List<Todo> _todos;
        private int _nextId = 1;

        public InMemoryTodoProvider()
        {
            // initializing with sample data
            _todos = new List<Todo>
            {
                new Todo
                {
                    Id = _nextId++,
                    Title = "In-Memory Task 1",
                    Description = "This is a task from the in-memory provider",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Todo
                {
                    Id = _nextId++,
                    Title = "In-Memory Task 2",
                    Description = "Another task from in-memory storage",
                    IsCompleted = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        public Task<IEnumerable<Todo>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Todo>>(_todos);
        }

        public Task<IEnumerable<Todo>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAllAsync();

            var result = _todos
                .Where(t => t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Task.FromResult<IEnumerable<Todo>>(result);
        }

        public Task<Todo?> GetByIdAsync(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(todo);
        }

        public Task<Todo> CreateAsync(Todo todo)
        {
            todo.Id = _nextId++;
            todo.CreatedAt = DateTime.UtcNow;
            _todos.Add(todo);
            return Task.FromResult(todo);
        }

        public async Task<Todo> UpdateAsync(Todo todo)
        {
            var existingTodo = await GetByIdAsync(todo.Id);
            if (existingTodo == null)
                return null;

            // trying to update existing todo
            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.IsCompleted = todo.IsCompleted;
            existingTodo.UpdatedAt = DateTime.UtcNow;

            return existingTodo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await GetByIdAsync(id);
            if (todo == null)
                return false;

            _todos.Remove(todo);
            return true;
        }
    }
}