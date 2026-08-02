using System;

namespace PortalNoticiasAPI.DTOs
{
    // Lo que la API devuelve al frontend
    public class NoticiaDto
    {
        public int IdNoticia { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
        public string? AutorNombre { get; set; }
        public int IdCategoria { get; set; }
        public string? CategoriaNombre { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }
    }

    // Lo que el frontend envía para crear una noticia
    public class NoticiaCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
        public int IdCategoria { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }

    // Lo que el frontend envía para actualizar una noticia
    public class NoticiaUpdateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public int IdCategoria { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
