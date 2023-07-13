using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class EntradasDetalle
{
    [Key]
    public int DetalleId { get; set; }

     [ForeignKey("EntradaId")]
    public int EntradaId { get; set; }

     [ForeignKey("ProductoId")]
    public int ProductoId { get; set; }

    [Required (ErrorMessage = "La cantidad utilizada es obligatoria")]
    public int CantidadUtilizada { get; set; }

}