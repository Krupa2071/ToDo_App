using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoProvider _provider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public TodosController(
            ITodoProvider provider,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _provider = provider;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        // GET: api/todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAll()
        {
            var todos = await _provider.GetAllAsync();
            return Ok(todos);
        }

        // GET: api/todos/search?term={searchTerm}
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Todo>>> Search([FromQuery] string term)
        {
            var results = await _provider.SearchAsync(term);
            return Ok(results);
        }

        // GET: api/todos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo?>> GetById(int id)
        {
            var todo = await _provider.GetByIdAsync(id);
            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        // POST: api/todos
        [HttpPost]
        public async Task<ActionResult<Todo>> Create(Todo todo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _provider.CreateAsync(todo);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/todos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Todo todo)
        {
            if (id != todo.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _provider.UpdateAsync(todo);
            if (result == null)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/todos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _provider.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // POST: api/todos/provider/{type}
        [HttpPost("provider/{type}")]
        public IActionResult SetProvider(string type)
        {
            if (!Enum.TryParse<TodoProviderType>(type, out var providerType))
                return BadRequest(new { error = "Invalid provider type" });

            _configuration["TodoProviderType"] = type;
                
            return Ok(new { provider = type });
        }
    }
}