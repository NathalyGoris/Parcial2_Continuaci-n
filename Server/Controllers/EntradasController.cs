using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

[Route("api/[controller]")]
[ApiController]
public class EntradasController : ControllerBase
{
    private readonly Context _context;
    public EntradasController(Context context)
    {
       _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Entradas>>> GetEntradas()
    {
        if (_context.Entradas == null)
        {
            return NotFound();
        }
        return await _context.Entradas.ToListAsync();
    }

    [HttpGet("{id}")]

        public async Task<ActionResult<Entradas>> GetEntradas(int id)
        {
            if (_context.Entradas == null)
            {
                return NotFound();
            }
            var entrada = await _context.Entradas.Include(e => e.EntradasDetalle).Where(e => e.EntradaId == id).FirstOrDefaultAsync();
            if (entrada == null)
            {
                return NotFound();
            }
            return entrada;
        }

    public bool EntradasExiste(int id)
    {
        return (_context.Entradas?.Any(e => e.EntradaId == id)).GetValueOrDefault();
    }

    [HttpPost]
    public async Task<ActionResult<Entradas>> PostEntradas(Entradas entradas)
    {
        if (!EntradasExiste(entradas.EntradaId))
        {
            foreach (var utilizado in entradas.EntradasDetalle)
            {
                var producto = _context.Productos.Find(utilizado.ProductoId);
                producto.Existencia -= (int)utilizado.CantidadUtilizada;
                _context.Entry(producto).State = EntityState.Modified;
            }

            _context.Entradas.Add(entradas);
        }
        else
        {
            var entradaAnterior = _context.Entradas
                .Include(e => e.EntradasDetalle)
                .AsNoTracking()
                .FirstOrDefault(e => e.EntradaId == entradas.EntradaId);

            foreach (var consumido in entradaAnterior.EntradasDetalle)
            {
                var producto = _context.Productos.Find(consumido.ProductoId);
                producto.Existencia += (int)consumido.CantidadUtilizada;
                _context.Entry(producto).State = EntityState.Modified;
            }

            var productoEntradaAnterior = _context.Productos.Find(entradaAnterior.ProductoId);
            productoEntradaAnterior.Existencia -= entradaAnterior.CantidadProducida;
            _context.Entry(productoEntradaAnterior).State = EntityState.Modified;
            _context.Database.ExecuteSqlRaw($"Delete from EntradasDetalles where EntradaId = {entradas.EntradaId}");

            foreach (var consumido in entradas.EntradasDetalle)
            {
                var producto = _context.Productos.Find(consumido.ProductoId);
                producto.Existencia -= (int)consumido.CantidadUtilizada;
                _context.Entry(producto).State = EntityState.Modified;
                _context.Entry(consumido).State = EntityState.Added;
            }

            var productoEntradaActual = _context.Productos.Find(entradas.ProductoId);
            productoEntradaActual.Existencia += entradas.CantidadProducida;
            _context.Entry(productoEntradaActual).State = EntityState.Modified;
            _context.Entradas.Update(entradas);
        }

        await _context.SaveChangesAsync();
        _context.Entry(entradas).State = EntityState.Detached;
        return Ok(entradas);
    }


    [HttpDelete("{idd}")]
    public async Task<IActionResult> EliminarEntrada(int EntradaId)
    {
        var entrada = await _context.Entradas.Include(e => e.EntradasDetalle).FirstOrDefaultAsync(e => e.EntradaId == EntradaId);

        if (entrada == null)
        {
            return NotFound();
        }

        foreach (var detalle in entrada.EntradasDetalle)
        {
            var producto = await _context.Productos.FindAsync(detalle.ProductoId);

            if (producto != null)
            {
                producto.Existencia += (int)detalle.CantidadUtilizada;
                _context.Productos.Update(producto);
            }
        }

        var productoPrincipal = await _context.Productos.FindAsync(entrada.ProductoId);

        if (productoPrincipal != null)
        {
            productoPrincipal.Existencia += entrada.CantidadProducida;
            _context.Productos.Update(productoPrincipal);
        }

        _context.Entradas.Remove(entrada);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
