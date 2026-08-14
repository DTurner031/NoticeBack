using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalNoticiasAPI.Data;
using PortalNoticiasAPI.DTOs;
using PortalNoticiasAPI.Models;

namespace PortalNoticiasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoticiasController : ControllerBase
    {
        private readonly PortalNoticiasContext _context;

        public NoticiasController(PortalNoticiasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoticiaDto>>> GetNoticias()
        {
            var noticias = await _context.Noticias
                .Include(n => n.Usuario)
                .Include(n => n.Categoria)
                .Where(n => n.Activo)
                .OrderByDescending(n => n.FechaPublicacion)
                .Select(n => new NoticiaDto
                {
                    IdNoticia = n.IdNoticia,
                    Titulo = n.Titulo,
                    Contenido = n.Contenido,
                    IdUsuario = n.IdUsuario,
                    AutorNombre = n.Usuario!.Nombre,
                    IdCategoria = n.IdCategoria,
                    CategoriaNombre = n.Categoria!.CategoriaNombre,
                    FechaPublicacion = n.FechaPublicacion,
                    FechaAlta = n.FechaAlta,
                    Activo = n.Activo
                })
                .ToListAsync();

            return Ok(noticias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoticiaDto>> GetNoticia(int id)
        {
            var noticia = await _context.Noticias
                .Include(n => n.Usuario)
                .Include(n => n.Categoria)
                .Where(n => n.IdNoticia == id && n.Activo)
                .Select(n => new NoticiaDto
                {
                    IdNoticia = n.IdNoticia,
                    Titulo = n.Titulo,
                    Contenido = n.Contenido,
                    IdUsuario = n.IdUsuario,
                    AutorNombre = n.Usuario!.Nombre,
                    IdCategoria = n.IdCategoria,
                    CategoriaNombre = n.Categoria!.CategoriaNombre,
                    FechaPublicacion = n.FechaPublicacion,
                    FechaAlta = n.FechaAlta,
                    Activo = n.Activo
                })
                .FirstOrDefaultAsync();

            if (noticia == null)
                return NotFound(new { mensaje = $"No se encontró la noticia con id {id}" });

            return Ok(noticia);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<NoticiaDto>> CrearNoticia(NoticiaCreateDto dto)
        {
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.IdUsuario == dto.IdUsuario && u.Activo);
            if (!usuarioExiste)
                return BadRequest(new { mensaje = "El usuario especificado no existe o está inactivo" });

            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.IdCategoria == dto.IdCategoria && c.Activo);
            if (!categoriaExiste)
                return BadRequest(new { mensaje = "La categoría especificada no existe o está inactiva" });

            var noticia = new Noticia
            {
                Titulo = dto.Titulo,
                Contenido = dto.Contenido,
                IdUsuario = dto.IdUsuario,
                IdCategoria = dto.IdCategoria,
                FechaPublicacion = dto.FechaPublicacion,
                FechaAlta = DateTime.Now,
                Activo = true
            };

            _context.Noticias.Add(noticia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNoticia), new { id = noticia.IdNoticia }, noticia);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarNoticia(int id, NoticiaUpdateDto dto)
        {
            var noticia = await _context.Noticias.FindAsync(id);
            if (noticia == null || !noticia.Activo)
                return NotFound(new { mensaje = $"No se encontró la noticia con id {id}" });

            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.IdCategoria == dto.IdCategoria && c.Activo);
            if (!categoriaExiste)
                return BadRequest(new { mensaje = "La categoría especificada no existe o está inactiva" });

            noticia.Titulo = dto.Titulo;
            noticia.Contenido = dto.Contenido;
            noticia.IdCategoria = dto.IdCategoria;
            noticia.FechaPublicacion = dto.FechaPublicacion;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarNoticia(int id)
        {
            var noticia = await _context.Noticias.FindAsync(id);
            if (noticia == null || !noticia.Activo)
                return NotFound(new { mensaje = $"No se encontró la noticia con id {id}" });

            noticia.Activo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
