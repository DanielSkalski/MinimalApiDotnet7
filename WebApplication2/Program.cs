using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApplication2.Features.Menu;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzaMenuContext>(options =>
    options.UseInMemoryDatabase("PizzaRestaurant")
    //options.UseSqlServer(builder.Configuration.GetConnectionString("PizzaMenuContext") ?? throw new InvalidOperationException("Connection string 'PizzaMenuContext' not found."))
);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(
        "edit_menu",
        policy => policy.RequireRole("manager").RequireClaim("permission", "can_edit_menu")
    );

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SwaggerGeneratorOptions>(opts => { opts.InferSecuritySchemes = true; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPizzaInMenuEndpoints();

{
    using var scope = app.Services.CreateScope();
    using var db = scope.ServiceProvider.GetRequiredService<PizzaMenuContext>();
    db.Database.EnsureCreated();
}

app.Run();
