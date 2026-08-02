using System;

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

    public class DocumentoUploadDto
    {
        public int IdNoticia { get; set; }
        public IFormFile Archivo { get; set; } = null!;
    }
}
