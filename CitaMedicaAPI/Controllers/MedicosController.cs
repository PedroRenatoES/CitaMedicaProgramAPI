using CitaMedicaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitaMedicaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicosController : ControllerBase
    {
        private readonly CitaMedica3Context _context;

        public MedicosController(CitaMedica3Context context)
        {
            _context = context;
        }

        // GET: api/medicos
        [HttpGet]
        public async Task<IActionResult> ObtenerMedicos(int? especialidadId = null)
        {
            // Si se proporciona un ID de especialidad, filtra; de lo contrario, obtiene todos los médicos
            var medicos = await _context.Medicos
                .Where(m => especialidadId == null || m.IdEspecialidad == especialidadId)
                .Select(m => new
                {
                    idMedico = m.IdMedico,
                    nombre = m.IdUsuarioNavigation.Nombre, // Asegúrate de que estas propiedades existan
                    apellido = m.IdUsuarioNavigation.Apellido // Asegúrate de que estas propiedades existan
                })
                .ToListAsync();

            return Ok(medicos);
        }
    }

}
