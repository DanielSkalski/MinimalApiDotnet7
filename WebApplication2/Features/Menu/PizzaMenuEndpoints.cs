using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApplication2.Features.Menu;

public static class PizzaMenuEndpoints
{
    public static void MapPizzaInMenuEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/PizzaMenu")
            .AddEndpointFilterFactory(Filters.RequestAuditor)
            .WithTags(nameof(PizzaInMenu));

        group.MapGet("/", async (PizzaMenuContext db) =>
        {
            return await db.PizzaInMenu.Select(x => new PizzaListItemDto(x.Id, x.Name, $"{x.Price} zl")).ToListAsync();
        })
        .WithName("Get All Pizzas In Menu")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<PizzaDetailsDto>, NotFound>> (int id, PizzaMenuContext db) =>
        {
            return await db.PizzaInMenu.FindAsync(id)
                is PizzaInMenu model
                    ? TypedResults.Ok(new PizzaDetailsDto
                    (
                        model.Id,
                        model.Name,
                        model.Desc,
                        $"{model.Price} zl"
                    ))
                    : TypedResults.NotFound();
        })
        .WithName("Get Pizza In Menu By Id")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, PizzaUpdateDto pizzaUpdate, PizzaMenuContext db) =>
        {
            var foundModel = await db.PizzaInMenu.FindAsync(id);

            if (foundModel is null)
            {
                return TypedResults.NotFound();
            }

            foundModel.Name = pizzaUpdate.Name;
            foundModel.Desc = pizzaUpdate.Desc;
            foundModel.Price = pizzaUpdate.Price;

            db.Update(foundModel);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        })
        .WithName("Update Pizza In Menu")
        .WithOpenApi();

        group.MapPost("/", async (PizzaInMenu pizzaInMenu, PizzaMenuContext db) =>
        {
            db.PizzaInMenu.Add(pizzaInMenu);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/PizzaMenu/{pizzaInMenu.Id}", pizzaInMenu);
        })
        .WithName("Create Pizza In Menu")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok<PizzaInMenu>, NotFound>> (int id, PizzaMenuContext db) =>
        {
            if (await db.PizzaInMenu.FindAsync(id) is PizzaInMenu pizzaInMenu)
            {
                db.PizzaInMenu.Remove(pizzaInMenu);
                await db.SaveChangesAsync();
                return TypedResults.Ok(pizzaInMenu);
            }

            return TypedResults.NotFound();
        })
        .WithName("Delete Pizza From Menu")
        .WithOpenApi();
    }

    record PizzaListItemDto(int Id, string Name, string Price);
    record PizzaDetailsDto(int Id, string Name, string Desc, string Price);
    record PizzaUpdateDto(string Name, string Desc, decimal Price);
}
