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
    public class DocumentosController : ControllerBase
    {
        private readonly PortalNoticiasContext _context;
        private readonly IWebHostEnvironment _env;

        public DocumentosController(PortalNoticiasContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("porNoticia/{idNoticia}")]
        public async Task<ActionResult<IEnumerable<DocumentoDto>>> GetDocumentosPorNoticia(int idNoticia)
        {
            var documentos = await _context.NoticiasDocumentos
                .Where(d => d.IdNoticia == idNoticia && d.Activo)
                .Select(d => new DocumentoDto
                {
                    IdNoticiaDocumento = d.IdNoticiaDocumento,
                    IdNoticia = d.IdNoticia,
                    DocumentoNombre = d.DocumentoNombre,
                    RutaArchivo = d.RutaArchivo,
                    TipoDocumento = d.TipoDocumento,
                    Activo = d.Activo
                })
                .ToListAsync();
            return Ok(documentos);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Docente")]
        public async Task<ActionResult<DocumentoDto>> SubirDocumento([FromForm] DocumentoUploadDto dto)
        {
            var noticiaExiste = await _context.Noticias.AnyAsync(n => n.IdNoticia == dto.IdNoticia && n.Activo);
            if (!noticiaExiste)
                return BadRequest(new { mensaje = "La noticia especificada no existe o está inactiva" });

            if (dto.Archivo == null || dto.Archivo.Length == 0)
                return BadRequest(new { mensaje = "No se recibió ningún archivo" });

            var carpetaUploads = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, "uploads");
            Directory.CreateDirectory(carpetaUploads);

            var nombreUnico = $"{Guid.NewGuid()}_{dto.Archivo.FileName}";
            var rutaFisica = Path.Combine(carpetaUploads, nombreUnico);

            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await dto.Archivo.CopyToAsync(stream);
            }

            var documento = new NoticiaDocumento
            {
                IdNoticia = dto.IdNoticia,
                DocumentoNombre = dto.Archivo.FileName,
                RutaArchivo = $"/uploads/{nombreUnico}",
                TipoDocumento = Path.GetExtension(dto.Archivo.FileName).TrimStart('.').ToUpper(),
                FechaAlta = DateTime.Now,
                Activo = true
            };

            _context.NoticiasDocumentos.Add(documento);
            await _context.SaveChangesAsync();

            return Ok(new DocumentoDto
            {
                IdNoticiaDocumento = documento.IdNoticiaDocumento,
                IdNoticia = documento.IdNoticia,
                DocumentoNombre = documento.DocumentoNombre,
                RutaArchivo = documento.RutaArchivo,
                TipoDocumento = documento.TipoDocumento,
                Activo = documento.Activo
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarDocumento(int id)
        {
            var documento = await _context.NoticiasDocumentos.FindAsync(id);
            if (documento == null || !documento.Activo)
                return NotFound(new { mensaje = $"No se encontró el documento con id {id}" });

            documento.Activo = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
