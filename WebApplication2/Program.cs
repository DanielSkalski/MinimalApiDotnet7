using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication2.Features.Menu;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzaMenuContext>(options =>
    options.UseInMemoryDatabase("PizzaRestaurant")
    //options.UseSqlServer(builder.Configuration.GetConnectionString("PizzaMenuContext") ?? throw new InvalidOperationException("Connection string 'PizzaMenuContext' not found."))
);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.MapPizzaInMenuEndpoints();

using (var scope = app.Services.CreateScope())
using (var db = scope.ServiceProvider.GetRequiredService<PizzaMenuContext>())
{
    db.Database.EnsureCreated();
}

app.Run();
