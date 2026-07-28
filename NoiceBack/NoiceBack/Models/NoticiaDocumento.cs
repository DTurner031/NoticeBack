using System;

namespace PortalNoticiasAPI.Models
{
    public class NoticiaDocumento
    {
        public int IdNoticiaDocumento { get; set; }
        public int IdNoticia { get; set; }
        public string DocumentoNombre { get; set; } = string.Empty;
        public string RutaArchivo { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }

        public Noticia? Noticia { get; set; }
    }
}
