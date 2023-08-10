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

    [HttpGet("{Id}")]

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
                Productos? producto = new Productos();
                foreach (var Consumido in entradas.EntradasDetalle)
                {
                    producto = _context.Productos.Find(Consumido.ProductoId);

                    if (producto != null)
                    {
                        producto.Existencia -= Consumido.CantidadUtilizada;
                        _context.Productos.Update(producto);
                        await _context.SaveChangesAsync();
                        _context.Entry(producto).State = EntityState.Detached;
                    }
                }
                await _context.Entradas.AddAsync(entradas);
            }
            else
            {
                var entradaAnterior = _context.Entradas.Include(e => e.EntradasDetalle).AsNoTracking()
                .FirstOrDefault(e => e.EntradaId == entradas.EntradaId);

                Productos? producto = new Productos();

                if (entradaAnterior != null && entradaAnterior.EntradasDetalle != null)
                {
                    foreach (var productoConsumido in entradaAnterior.EntradasDetalle)
                    {
                        if (productoConsumido != null)
                        {
                            producto = _context.Productos.Find(productoConsumido.ProductoId);

                            if (producto != null)
                            {
                                producto.Existencia += productoConsumido.CantidadUtilizada;
                                _context.Productos.Update(producto);
                                await _context.SaveChangesAsync();
                                _context.Entry(producto).State = EntityState.Detached;
                            }
                        }
                    }
                }

                if (entradaAnterior != null)
                {
                    producto = _context.Productos.Find(entradaAnterior.ProductoId);

                    if (producto != null)
                    {
                        producto.Existencia -= entradaAnterior.CantidadProducida;
                        _context.Productos.Update(producto);
                        await _context.SaveChangesAsync();
                        _context.Entry(producto).State = EntityState.Detached;
                    }
                }

                _context.Database.ExecuteSqlRaw($"Delete from entradasDetalle where EntradaId = {entradas.EntradaId}");

                foreach (var productoConsumido in entradas.EntradasDetalle)
                {
                    producto = _context.Productos.Find(productoConsumido.ProductoId);

                    if (producto != null)
                    {
                        producto.Existencia -= productoConsumido.CantidadUtilizada;
                        _context.Productos.Update(producto);
                        await _context.SaveChangesAsync();
                        _context.Entry(producto).State = EntityState.Detached;
                        _context.Entry(productoConsumido).State = EntityState.Added;
                    }
                }

                producto = _context.Productos.Find(entradas.ProductoId);

                if (producto != null)
                {
                    producto.Existencia += entradas.CantidadProducida;
                    _context.Productos.Update(producto);
                    await _context.SaveChangesAsync();
                    _context.Entry(producto).State = EntityState.Detached;
                }
                _context.Entradas.Update(entradas);
            }

            await _context.SaveChangesAsync();
            _context.Entry(entradas).State = EntityState.Detached;
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
