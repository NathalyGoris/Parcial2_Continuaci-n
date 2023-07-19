using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


    [Route("api/[controller]")]
    [ApiController]

    public class EntradasController : ControllerBase
    {
        private readonly Context _context;

        public EntradasController(Context context)
        {
            _context = context;
        }

        public bool Existe(int EntradaId)
        {
            return (_context.Entradas?.Any(e => e.EntradaId == EntradaId)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entradas>>> Obtener()
        {
            if(_context.Entradas == null)
            {
                return NotFound();
            }
            else
            {
                return await _context.Entradas.ToListAsync();
            }
        }

        [HttpGet("{EntradaId}")]
        public async Task<ActionResult<Entradas>> ObtenerEntradas(int EntradaId)
        {
            if(_context.Entradas == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entradas.Include(e => e.EntradasDetalle).Where( e => e.EntradaId == EntradaId).FirstOrDefaultAsync();

            if(entrada == null)
            {
                return NotFound();
            }

            foreach(var item in entrada.EntradasDetalle)
            {
                Console.WriteLine($"{item.DetalleId}, {item.EntradaId}, {item.ProductoId}, {item.CantidadUtilizada}");
            }

            return entrada;
        }
        
        public async Task<ActionResult<Entradas>> PostEntradas(Entradas entradas)
{
    foreach (var productoConsumido in entradas.EntradasDetalle)
    {
        var producto = _context.Productos.Find(productoConsumido.ProductoId);

        if (producto != null)
        {
            producto.Existencia -= productoConsumido.CantidadUtilizada;
            _context.Productos.Update(producto);
        }
    }

    if (Existe(entradas.EntradaId))
    {
        var entradaAnterior = _context.Entradas
            .Include(e => e.EntradasDetalle)
            .AsNoTracking()
            .FirstOrDefault(e => e.EntradaId == entradas.EntradaId);

        if (entradaAnterior != null && entradaAnterior.EntradasDetalle != null)
        {
            foreach (var productoConsumido in entradaAnterior.EntradasDetalle)
            {
                var producto = _context.Productos.Find(productoConsumido.ProductoId);

                if (producto != null)
                {
                    producto.Existencia += productoConsumido.CantidadUtilizada;
                    _context.Productos.Update(producto);
                }
            }
        }

        if (entradaAnterior != null)
        {
            var producto = _context.Productos.Find(entradaAnterior.ProductoId);

            if (producto != null)
            {
                producto.Existencia -= entradaAnterior.CantidadProducida;
                _context.Productos.Update(producto);
            }
        }

        _context.Database.ExecuteSqlRaw($"Delete from entradasDetalle where EntradaId = {entradas.EntradaId}");
    }

    var productoActual = _context.Productos.Find(entradas.ProductoId);

    if (productoActual != null)
    {
        productoActual.Existencia += entradas.CantidadProducida;
        _context.Productos.Update(productoActual);
    }

    if (!Existe(entradas.EntradaId))
    {
        await _context.Entradas.AddAsync(entradas);
    }
    else
    {
        _context.Entradas.Update(entradas);
    }

    await _context.SaveChangesAsync();
    return Ok(entradas);
}


        [HttpDelete("{EntradaId}")]
        public async Task<IActionResult> EliminarEntrada(int EntradaId)
        {
            var entrada = await _context.Entradas.Include(e => e.EntradasDetalle).FirstOrDefaultAsync(e => e.EntradaId == EntradaId);

            if (entrada == null)
            {
                return NotFound();
            }

            foreach (var productoConsumido in entrada.EntradasDetalle)
            {
                var producto = await _context.Productos.FindAsync(productoConsumido.ProductoId);

                if (producto != null)
                {
                    producto.Existencia += productoConsumido.CantidadUtilizada;
                    _context.Productos.Update(producto);
                }
            }

            var productoInicial = await _context.Productos.FindAsync(entrada.ProductoId);

            if (productoInicial != null)
            {
                productoInicial.Existencia += entrada.CantidadProducida;
                _context.Productos.Update(productoInicial);
            }

            _context.Entradas.Remove(entrada);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
  }
  