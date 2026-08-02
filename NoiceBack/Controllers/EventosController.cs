using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalNoticiasAPI.Data;
using PortalNoticiasAPI.DTOs;
using PortalNoticiasAPI.Models;

namespace PortalNoticiasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly PortalNoticiasContext _context;
        public EventosController(PortalNoticiasContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventoDto>>> GetEventos()
        {
            var eventos = await _context.Eventos
                .Include(e => e.Usuario)
                .Include(e => e.Categoria)
                .Where(e => e.Activo)
                .OrderBy(e => e.FechaInicio)
                .Select(e => new EventoDto
                {
                    IdEvento = e.IdEvento,
                    Titulo = e.Titulo,
                    Descripcion = e.Descripcion,
                    Lugar = e.Lugar,
                    FechaInicio = e.FechaInicio,
                    FechaFin = e.FechaFin,
                    HoraInicio = e.HoraInicio,
                    HoraFin = e.HoraFin,
                    IdUsuario = e.IdUsuario,
                    OrganizadorNombre = e.Usuario!.Nombre,
                    IdCategoria = e.IdCategoria,
                    CategoriaNombre = e.Categoria != null ? e.Categoria.CategoriaNombre : null,
                    Activo = e.Activo
                })
                .ToListAsync();
            return Ok(eventos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventoDto>> GetEvento(int id)
        {
            var evento = await _context.Eventos
                .Include(e => e.Usuario)
                .Include(e => e.Categoria)
                .Where(e => e.IdEvento == id && e.Activo)
                .Select(e => new EventoDto
                {
                    IdEvento = e.IdEvento,
                    Titulo = e.Titulo,
                    Descripcion = e.Descripcion,
                    Lugar = e.Lugar,
                    FechaInicio = e.FechaInicio,
                    FechaFin = e.FechaFin,
                    HoraInicio = e.HoraInicio,
                    HoraFin = e.HoraFin,
                    IdUsuario = e.IdUsuario,
                    OrganizadorNombre = e.Usuario!.Nombre,
                    IdCategoria = e.IdCategoria,
                    CategoriaNombre = e.Categoria != null ? e.Categoria.CategoriaNombre : null,
                    Activo = e.Activo
                })
                .FirstOrDefaultAsync();

            if (evento == null)
                return NotFound(new { mensaje = $"No se encontró el evento con id {id}" });

            return Ok(evento);
        }

        [HttpPost]
        public async Task<ActionResult<EventoDto>> CrearEvento(EventoCreateUpdateDto dto)
        {
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.IdUsuario == dto.IdUsuario && u.Activo);
            if (!usuarioExiste)
                return BadRequest(new { mensaje = "El usuario especificado no existe o está inactivo" });

            if (dto.FechaFin < dto.FechaInicio)
                return BadRequest(new { mensaje = "La fecha de fin no puede ser anterior a la fecha de inicio" });

            var evento = new Evento
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Lugar = dto.Lugar,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                IdUsuario = dto.IdUsuario,
                IdCategoria = dto.IdCategoria,
                FechaAlta = DateTime.Now,
                Activo = true
            };

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvento), new { id = evento.IdEvento }, evento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEvento(int id, EventoCreateUpdateDto dto)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null || !evento.Activo)
                return NotFound(new { mensaje = $"No se encontró el evento con id {id}" });

            evento.Titulo = dto.Titulo;
            evento.Descripcion = dto.Descripcion;
            evento.Lugar = dto.Lugar;
            evento.FechaInicio = dto.FechaInicio;
            evento.FechaFin = dto.FechaFin;
            evento.HoraInicio = dto.HoraInicio;
            evento.HoraFin = dto.HoraFin;
            evento.IdCategoria = dto.IdCategoria;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null || !evento.Activo)
                return NotFound(new { mensaje = $"No se encontró el evento con id {id}" });

            evento.Activo = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
