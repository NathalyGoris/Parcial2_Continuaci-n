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
        
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Productos>>> GetProductos()
    {
        if (_context.Productos == null)
        {
            return NotFound();
        }
        return await _context.Productos.ToListAsync();
    }

       
    [HttpGet("{id}")]
    public async Task<ActionResult<Productos>> GetProductos(int id)
    {
        if (_context.Productos == null)
        {
            return NotFound();
        }
        var productos = await _context.Productos.FindAsync(id);

        if (productos == null)
        {
            return NotFound();
        }

        return productos;
    }

        
    [HttpPost]
    public async Task<ActionResult<Productos>> PostProductos(Productos productos)
    {
        if (!ProductosExists(productos.ProductoId))
            _context.Productos.Add(productos);
        else
            _context.Productos.Update(productos);

        await _context.SaveChangesAsync();
        return Ok(productos);
    }

        
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductos(int id)
    {
        if (_context.Productos == null)
        {
            return NotFound();
        }
        var productos = await _context.Productos.FindAsync(id);
        if (productos == null)
        {
            return NotFound();
        }

        _context.Productos.Remove(productos);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductosExists(int id)
    {
        return (_context.Productos?.Any(p => p.ProductoId == id)).GetValueOrDefault();
    }
}
