using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
    [ApiController]
    public class EntradasDetalleController : ControllerBase
    {   
        private readonly Context _context;

        public EntradasDetalleController(Context context)
        {
            _context = context;
        }

        private bool Existe(int DetalleId)
        {
            return (_context.EntradasDetalle?.Any(c => c.DetalleId == DetalleId)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntradasDetalle>>> Obtener()
        {
            if(_context.EntradasDetalle == null)
            {
                return NotFound();
            }
            return await _context.EntradasDetalle.ToListAsync();
        }

        [HttpGet("{DetalleId}")]
        public async Task<ActionResult<EntradasDetalle>> Obtener(int DetalleId)
        {
            if(_context.EntradasDetalle == null)
            {
                return NotFound();
            }

            var EntradasDetalle = await _context.EntradasDetalle.FindAsync(DetalleId);

            if(EntradasDetalle == null)
            {
                return NotFound();
            }

            return EntradasDetalle;
        }
         [HttpPost]
        public async Task<ActionResult<EntradasDetalle>> Crear(EntradasDetalle EntradasDetalle)
        {
            if(!Existe(EntradasDetalle.DetalleId))
            {
                _context.EntradasDetalle.Add(EntradasDetalle);
            }
            else
            {
                _context.EntradasDetalle.Update(EntradasDetalle);
            }
            await _context.SaveChangesAsync();
            return Ok(EntradasDetalle);
        }

        [HttpDelete("{DetalleId}")]
        public async Task<IActionResult> Eliminar(int DetalleId)
        {
            if(_context.EntradasDetalle == null)
            {
                return NotFound();
            }
            var EntradasDetalle = await _context.EntradasDetalle.FindAsync(DetalleId);
            if(EntradasDetalle == null)
            {
                return NotFound();
            }

            _context.EntradasDetalle.Remove(EntradasDetalle);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
    
        


        
    