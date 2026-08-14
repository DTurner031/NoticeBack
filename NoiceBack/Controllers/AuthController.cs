using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalNoticiasAPI.Data;
using PortalNoticiasAPI.DTOs;
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

            // Mensaje genérico a propósito: no le decimos al atacante si falló el correo o la contraseña
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
    }
}
