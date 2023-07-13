
using Microsoft.EntityFrameworkCore;

public class Context : DbContext
   {
       public Context(DbContextOptions<Context> Opcions) : base(Opcions) { } 
       public DbSet<Entradas> Entradas {get; set;}
       public DbSet<EntradasDetalle> EntradasDetalle {get; set;} 
       public DbSet<Productos> Productos {get; set;}
    }