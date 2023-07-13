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

        private bool Existe(int EntradaId)
        {
            return (_context.Entradas?.Any(c => c.EntradaId == EntradaId)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entradas>>> Obtener()
        {
            if(_context.Entradas == null)
            {
                return NotFound();
            }
            return await _context.Entradas.ToListAsync();
        }

        [HttpGet("{EntradaId}")]
        public async Task<ActionResult<Entradas>> Obtener(int EntradaId)
        {
            if(_context.Entradas == null)
            {
                return NotFound();
            }

            var Entradas = await _context.Entradas.FindAsync(EntradaId);

            if(Entradas == null)
            {
                return NotFound();
            }

            return Entradas;
        }
         [HttpPost]
        public async Task<ActionResult<Entradas>> Crear(Entradas Entradas)
        {
            if(!Existe(Entradas.EntradaId))
            {
                _context.Entradas.Add(Entradas);
            }
            else
            {
                _context.Entradas.Update(Entradas);
            }
            await _context.SaveChangesAsync();
            return Ok(Entradas);
        }

        [HttpDelete("{EntradaId}")]
        public async Task<IActionResult> Eliminar(int EntradaId)
        {
            if(_context.Entradas == null)
            {
                return NotFound();
            }
            var Entradas = await _context.Entradas.FindAsync(EntradaId);
            if(Entradas == null)
            {
                return NotFound();
            }

            _context.Entradas.Remove(Entradas);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
    
        


        
    