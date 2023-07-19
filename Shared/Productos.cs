using System.ComponentModel.DataAnnotations;

public class Productos
{
    [Key]
    public int ProductoId { get; set; }

    public string? Descripcion { get; set; }

    public int Tipo { get; set; }

    [Required(ErrorMessage = "El precio de compra es obligatorio")]
    public double PrecioCompra { get; set; }

    [Required(ErrorMessage = "El precio de venta es obligatorio")]
    public double PrecioVenta { get; set; }

    public int Existencia { get; set; }

    public void UpdateExistencia(int cantidadUtilizada)
    {
        throw new NotImplementedException();
    }
}
 