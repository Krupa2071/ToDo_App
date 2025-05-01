using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApp.Data;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// adding services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// adding Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoApp API", Version = "v1" });
});

// configuring database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// registering services
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(connectionString));

// registering providers with interface
builder.Services.AddScoped<EntityFrameworkTodoProvider>();
builder.Services.AddSingleton<InMemoryTodoProvider>();

builder.Services.AddScoped<ITodoProvider>(serviceProvider => 
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var providerType = configuration.GetValue<string>("TodoProviderType");
    
    if (Enum.TryParse<TodoProviderType>(providerType, out var type))
    {
        if (type == TodoProviderType.EntityFramework)
            return serviceProvider.GetRequiredService<EntityFrameworkTodoProvider>();
        else
            return serviceProvider.GetRequiredService<InMemoryTodoProvider>();
    }
    
    return serviceProvider.GetRequiredService<EntityFrameworkTodoProvider>();
});


// configuring CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// configuring HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApp API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<TodoDbContext>();
        dbContext.Database.EnsureCreated();
        Console.WriteLine("Database created successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred creating the database: {ex.Message}");
    }
}

app.Run();