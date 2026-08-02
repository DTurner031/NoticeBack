using System;
using System.Collections.Generic;

namespace PortalNoticiasAPI.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public int NoIdentificacion { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }

        // Relaciones
        public NombreRol? Rol { get; set; }
        public ICollection<Noticia>? Noticias { get; set; }
        public ICollection<Comentario>? Comentarios { get; set; }
        public ICollection<Evento>? Eventos { get; set; }
    }
}
