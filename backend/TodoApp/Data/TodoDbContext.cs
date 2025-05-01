using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // initial data for checking
            modelBuilder.Entity<Todo>().HasData(
                new Todo
                {
                    Id = 1,
                    Title = "Learn Factory Pattern",
                    Description = "Implement the factory pattern in a real-world application",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Todo
                {
                    Id = 2,
                    Title = "Implement Debounce",
                    Description = "Add debounce functionality to search operations",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Todo
                {
                    Id = 3,
                    Title = "Style the Application",
                    Description = "Add CSS styling to improve UI/UX",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}