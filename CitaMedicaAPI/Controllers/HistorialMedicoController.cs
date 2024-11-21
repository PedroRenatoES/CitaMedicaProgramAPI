using CitaMedicaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CitaMedicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialMedicoController : ControllerBase
    {
        private readonly CitaMedica3Context _context;

        public HistorialMedicoController(CitaMedica3Context context)
        {
            _context = context;
        }

        [HttpGet("{idPaciente}")]
        public IActionResult GetHistorial(int idPaciente)
        {
            var historial = _context.HistorialMedicos.Where(h => h.IdPaciente == idPaciente).ToList();
            return Ok(historial);
        }
    }
}
