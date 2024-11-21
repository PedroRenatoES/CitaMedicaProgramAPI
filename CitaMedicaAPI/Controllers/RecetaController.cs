using CitaMedicaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CitaMedicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetaController : ControllerBase
    {
        private readonly CitaMedica3Context _context;

        public RecetaController(CitaMedica3Context context)
        {
            _context = context;
        }

        [HttpGet("{idHistorial}")]
        public IActionResult GetRecetas(int idHistorial)
        {
            var recetas = _context.Recetas.Where(r => r.IdHistorial == idHistorial).ToList();
            return Ok(recetas);
        }
    }
}
