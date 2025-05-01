using Microsoft.Extensions.DependencyInjection;
using System;

namespace TodoApp.Services
{
    public enum TodoProviderType
    {
        EntityFramework,
        InMemory
    }

    public class TodoProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public TodoProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITodoProvider CreateProvider(TodoProviderType type)
        {
            return type switch
            {
                TodoProviderType.EntityFramework => _serviceProvider.GetRequiredService<EntityFrameworkTodoProvider>(),
                TodoProviderType.InMemory => _serviceProvider.GetRequiredService<InMemoryTodoProvider>(),
                _ => throw new ArgumentException($"Unsupported provider type: {type}", nameof(type))
            };
        }
    }
}