using WebShop.Entities;
using WebShop.Notifications;
using WebShop.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext to DI container with the connection string from appsettings.json
builder.Services.AddDbContext<WebShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebShopDb"))
);

// Register Unit of Work and other services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<INotificationObserver, EmailNotification>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger(); 
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();