using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Entradas
{
    [Key]
    public int EntradaId { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Today;

    [Required (ErrorMessage = "El concepto es obligatorio")]
    public string? Concepto { get; set; }

    [Required (ErrorMessage = "El Peso total es obligatorio")]
    public int PesoTotal { get; set; }

    [Required (ErrorMessage = "El ProductoId es obligatorio")]
    public int ProductoId { get; set; }

    [Required (ErrorMessage = "La cantidad producida es obligatoria")] 
    public int CantidadProducida { get; set; }

    [ForeignKey("EntradaId")]
    public List<EntradasDetalle> EntradasDetalle { get; set; } = new List<EntradasDetalle>();

 
}

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





