using System;
using Microsoft.AspNetCore.Http;

namespace PortalNoticiasAPI.DTOs
{
    public class DocumentoDto
    {
        public int IdNoticiaDocumento { get; set; }
        public int IdNoticia { get; set; }
        public string DocumentoNombre { get; set; } = string.Empty;
        public string RutaArchivo { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    // Clase auxiliar requerida para que Swagger genere bien el formulario de subida de archivos
    public class DocumentoUploadDto
    {
        public int IdNoticia { get; set; }
        public IFormFile Archivo { get; set; } = null!;
    }
}
