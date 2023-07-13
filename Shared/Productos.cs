using System.ComponentModel.DataAnnotations;

public class Productos
{
    [Key]
    public int ProductoId { get; set; }

    public string? Descripcion { get; set; } 

    public int Tipo { get; set; }

    public int Existencia { get; set; }

}
 