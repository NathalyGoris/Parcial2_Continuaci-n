using Microsoft.EntityFrameworkCore;

public class Context : DbContext
{
    public DbSet<Entradas> Entradas { get; set; }
    public DbSet<Productos> Productos { get; set; }
    
    public Context(DbContextOptions<Context> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Productos>().HasData
        (
            new Productos
            {
                ProductoId = 1,
                Descripcion = "Mani",
                PrecioCompra = 5,
                PrecioVenta = 10,
                Existencia = 100
            },

            new Productos
            {
                ProductoId = 2,
                Descripcion = "Pistachos",
                PrecioCompra = 10,
                PrecioVenta = 20,
                Existencia = 200
            },

            new Productos
            {
                ProductoId = 3,
                Descripcion = "Pasas",
                PrecioCompra = 5,
                PrecioVenta = 10,
                Existencia = 250
            },

            new Productos
            {
                ProductoId = 4,
                Descripcion = "Ciruelas",
                PrecioCompra = 25,
                PrecioVenta = 50,
                Existencia = 350
            },

            new Productos
            {
                ProductoId = 5,
                Descripcion = "Mixto MPP 0.5 lb",
                PrecioCompra = 0,
                PrecioVenta = 75,
                Existencia = 20
            }
            ,
            new Productos
            {
                ProductoId = 6,
                Descripcion = "Mixto MPC 0.5 lb",
                PrecioCompra = 0,
                PrecioVenta = 100,
                Existencia = 50
            },

            new Productos
            {
                ProductoId = 7,
                Descripcion = "Mixto MPP 0.2 lb",
                PrecioCompra = 0,
                PrecioVenta = 30,
                Existencia = 103
            }
        );
    }
}