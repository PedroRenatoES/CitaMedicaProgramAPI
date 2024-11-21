using CitaMedicaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitaMedicaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly CitaMedica3Context _context;

        public CitasController(CitaMedica3Context context)
        {
            _context = context;
        }

        // GET: api/citas/ver/{idPaciente}
        [HttpGet("ver/{idPaciente}")]
        public async Task<IActionResult> VerCitas(int idPaciente)
        {
            var paciente = await _context.Pacientes.FindAsync(idPaciente);

            if (paciente == null)
            {
                return NotFound(new { message = "Paciente no encontrado." });
            }

            var citas = await _context.Citas
                .Include(c => c.IdMedicoNavigation)
                    .ThenInclude(m => m.IdUsuarioNavigation)
                .Where(c => c.IdPaciente == paciente.IdPaciente)
                .ToListAsync();

            return Ok(citas);
        }

        // POST: api/citas/agendar
        [HttpPost("agendar")]
        public async Task<IActionResult> AgendarNuevaCita([FromBody] Cita cita)
        {
            if (ModelState.IsValid)
            {
                // Buscar el paciente asociado a la cita
                var paciente = await _context.Pacientes.FindAsync(cita.IdPaciente);
                if (paciente != null)
                {
                    _context.Citas.Add(cita);
                    await _context.SaveChangesAsync();

                    // Retornar solo el mensaje deseado
                    return Ok("Cita registrada exitosamente");
                }
                else
                {
                    return NotFound(new { message = "No se encontró un paciente con el ID proporcionado." });
                }
            }

            return BadRequest(ModelState);
        }


    }
}
