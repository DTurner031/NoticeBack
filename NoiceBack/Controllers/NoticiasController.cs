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

        // GET: api/noticias
        // Devuelve solo las noticias activas, con el nombre del autor y la categoría
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

        // GET: api/noticias/5
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

        // POST: api/noticias
        [HttpPost]
        public async Task<ActionResult<NoticiaDto>> CrearNoticia(NoticiaCreateDto dto)
        {
            // Validar que el usuario y la categoría existan
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

        // PUT: api/noticias/5
        [HttpPut("{id}")]
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

        // DELETE: api/noticias/5
        // Borrado lógico: no elimina el registro, solo lo marca como inactivo
        [HttpDelete("{id}")]
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
