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

        private bool Existe(int ProductoId)
        {
            return (_context.Productos?.Any(c => c.ProductoId == ProductoId)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Productos>>> Obtener()
        {
            if(_context.Productos == null)
            {
                return NotFound();
            }
            return await _context.Productos.ToListAsync();
        }

        [HttpGet("{ProductoId}")]
        public async Task<ActionResult<Productos>> Obtener(int ProductoId)
        {
            if(_context.Productos == null)
            {
                return NotFound();
            }

            var Productos = await _context.Productos.FindAsync(ProductoId);

            if(Productos == null)
            {
                return NotFound();
            }

            return Productos;
        }
         [HttpPost]
        public async Task<ActionResult<Productos>> Crear(Productos Productos)
        {
            if(!Existe(Productos.ProductoId))
            {
                _context.Productos.Add(Productos);
            }
            else
            {
                _context.Productos.Update(Productos);
            }
            await _context.SaveChangesAsync();
            return Ok(Productos);
        }

        
    }
    
        


        
    