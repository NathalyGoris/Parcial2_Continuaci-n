using System.ComponentModel.DataAnnotations;

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

 
}





