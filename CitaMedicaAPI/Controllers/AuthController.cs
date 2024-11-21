using CitaMedicaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitaMedicaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CitaMedica3Context _context;

        public AuthController(CitaMedica3Context context)
        {
            _context = context;
        }

        // POST: api/Auth/Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario usuario, [FromQuery] string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return BadRequest(new { error = "El campo de contraseña es requerido." });
            }

            if (ModelState.IsValid)
            {
                // Asignar el rol de "Paciente"
                usuario.Rol = "paciente";
                usuario.ContraseñaHash = password; // Aquí se almacena la contraseña directamente sin hash.

                // Crear el nuevo usuario
                _context.Add(usuario);
                await _context.SaveChangesAsync();

                // Crear el paciente asociado
                var paciente = new Paciente
                {
                    IdUsuario = usuario.IdUsuario
                };
                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Usuario registrado exitosamente." });
            }

            return BadRequest(ModelState);
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "Email y contraseña son requeridos." });
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (usuario == null || usuario.ContraseñaHash != request.Password) // Comparación de contraseñas
            {
                return Unauthorized(new { Message = "Email o contraseña incorrectos." });
            }

            // Redirigir según el rol del usuario
            if (usuario.Rol == "Medico")
            {
                return Ok(new { Message = "Inicio de sesión exitoso", RedirectUrl = "/Medico/PantallaMedico" });
            }
            else if (usuario.Rol == "paciente")
            {
                var pacienteID = _context.Pacientes.FirstOrDefault(p => p.IdUsuario == usuario.IdUsuario);
                return Ok(new { Message = "Inicio de sesión exitoso", PacienteId = pacienteID.IdPaciente });
            }
            else if (usuario.Rol == "admin")
            {
                return Ok(new { Message = "Inicio de sesión exitoso", RedirectUrl = "/Administrador/PaginaPrincipal" });
            }

            return NotFound(new { Message = "Rol de usuario no encontrado." });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
