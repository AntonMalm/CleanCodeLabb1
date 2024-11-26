using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebShop.DataAccess;
using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Notifications;
using WebShop.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebShopDb"))
);

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Register Notifications for Observer pattern
builder.Services.AddTransient<INotificationObserver, EmailNotification>();

// Add Controllers and configure JSON serialization options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null; 
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });


// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();