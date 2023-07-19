using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


    [Route("api/[controller]")]
    [ApiController]

    public class ProductosController : ControllerBase
    {
        private readonly Context _context;

        public ProductosController(Context context)
        {
            _context = context;
        }

        public bool Existe(int ProductoId)
        {
            return (_context.Productos?.Any(p => p.ProductoId == ProductoId)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Productos>>> Obtener()
        {
            if(_context.Productos == null)
            {
                return NotFound();
            }
            else
            {
                return await _context.Productos.ToListAsync();
            }
        }

        [HttpGet("{ProductoId}")]
        public async Task<ActionResult<Productos>> ObtenerProductos(int ProductoId)
        {
            if(_context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(ProductoId);

            if(producto == null)
            {
                return NotFound();
            }
            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Productos>> PostProductos(Productos productos)
        {
            if(!Existe(productos.ProductoId))
            {
                _context.Productos.Add(productos);
            }
            else
            {
                _context.Productos.Update(productos);
            }

            await _context.SaveChangesAsync();
            return Ok(productos);
        }

        [HttpDelete("{ProductoId}")]
        public async Task<IActionResult> Eliminar(int ProductoId)
        {
            if(_context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(ProductoId);

            if(producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
