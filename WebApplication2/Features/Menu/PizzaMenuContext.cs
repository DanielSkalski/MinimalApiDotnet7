using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Features.Menu;

public class PizzaMenuContext : DbContext
{
    public PizzaMenuContext(DbContextOptions<PizzaMenuContext> options)
        : base(options)
    {
    }

    public DbSet<PizzaInMenu> PizzaInMenu { get; set; } = default!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PizzaInMenu>().HasData(
            new PizzaInMenu { Id = 1, Name = "Margerita", Desc = "Nice and cheap pizza", Price = 21 },
            new PizzaInMenu { Id = 2, Name = "Capriciosa", Desc = "Delicious and traditional pizza", Price = 25 }
        );
    }
}
