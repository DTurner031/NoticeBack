using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalNoticiasAPI.Data;
using PortalNoticiasAPI.DTOs;
using PortalNoticiasAPI.Models;
using PortalNoticiasAPI.Services;

namespace PortalNoticiasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly PortalNoticiasContext _context;
        private readonly TokenService _tokenService;

        public AuthController(PortalNoticiasContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == dto.Correo && u.Activo);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos" });

            bool contrasenaValida = BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuario.Contrasena);
            if (!contrasenaValida)
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos" });

            var token = _tokenService.GenerarToken(usuario, usuario.Rol!.Nombre);

            return Ok(new LoginResponseDto
            {
                Token = token,
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                IdRol = usuario.IdRol,
                RolNombre = usuario.Rol.Nombre
            });
        }

        // POST: api/auth/register
        // Público (no requiere estar logueado). Siempre crea el usuario con rol "Alumno".
        [HttpPost("register")]
        public async Task<ActionResult<LoginResponseDto>> Register(RegisterDto dto)
        {
            var correoExiste = await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo);
            if (correoExiste)
                return BadRequest(new { mensaje = "Ya existe un usuario con ese correo" });

            var boletaExiste = await _context.Usuarios.AnyAsync(u => u.NoIdentificacion == dto.NoIdentificacion);
            if (boletaExiste)
                return BadRequest(new { mensaje = "Ya existe un usuario con ese número de identificación" });

            var rolAlumno = await _context.NombreRoles.FirstOrDefaultAsync(r => r.Nombre == "Alumno" && r.Activo);
            if (rolAlumno == null)
                return StatusCode(500, new { mensaje = "No se encontró el rol 'Alumno' configurado en el sistema" });

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                IdRol = rolAlumno.IdRol,
                NoIdentificacion = dto.NoIdentificacion,
                Correo = dto.Correo,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                FechaAlta = DateTime.Now,
                Activo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Auto-login: le devolvemos el token de una vez para que no tenga que iniciar sesión aparte
            var token = _tokenService.GenerarToken(usuario, rolAlumno.Nombre);

            return Ok(new LoginResponseDto
            {
                Token = token,
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                IdRol = usuario.IdRol,
                RolNombre = rolAlumno.Nombre
            });
        }
    }
}
