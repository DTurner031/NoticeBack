using System;
using System.Collections.Generic;

namespace PortalNoticiasAPI.Models
{
    public class Noticia
    {
        public int IdNoticia { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
        public int IdCategoria { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }

        // Relaciones
        public Usuario? Usuario { get; set; }
        public Categoria? Categoria { get; set; }
        public ICollection<NoticiaDocumento>? Documentos { get; set; }
        public ICollection<NoticiaComentario>? NoticiasComentarios { get; set; }
    }
}
