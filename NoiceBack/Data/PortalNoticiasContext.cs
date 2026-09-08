using Microsoft.EntityFrameworkCore;
using PortalNoticiasAPI.Models;

namespace PortalNoticiasAPI.Data
{
    public class PortalNoticiasContext : DbContext
    {
        public PortalNoticiasContext(DbContextOptions<PortalNoticiasContext> options)
            : base(options) { }

        public DbSet<NombreRol> NombreRoles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Noticia> Noticias { get; set; }
        public DbSet<NoticiaDocumento> NoticiasDocumentos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<NoticiaComentario> NoticiasComentarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Academia> Academias { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<GrupoTipo> GruposTipos { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<GrupoAlumno> GruposAlumnos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---------- NombreRol ----------
            modelBuilder.Entity<NombreRol>(e =>
            {
                e.ToTable("NombreRol");

                e.HasKey(x => x.IdRol);

                e.Property(x => x.IdRol)
                    .HasColumnName("idRol")
                    .ValueGeneratedOnAdd();

                e.Property(x => x.Nombre)
                    .HasColumnName("NombreRol")
                    .HasMaxLength(50)
                    .IsRequired();

                e.Property(x => x.FechaAlta)
                    .HasColumnName("FechaAlta")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(x => x.Activo)
                    .HasColumnName("Activo")
                    .HasColumnType("bit(1)")
                    .HasDefaultValue(true);
            });

            // ---------- Usuario ----------
            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("Usuarios");

                e.HasKey(x => x.IdUsuario);

                e.Property(x => x.IdUsuario)
                    .HasColumnName("idUsuario")
                    .ValueGeneratedOnAdd();

                e.Property(x => x.Nombre)
                    .HasColumnName("Nombre")
                    .HasMaxLength(150)
                    .IsRequired();

                e.Property(x => x.IdRol)
                    .HasColumnName("idRol")
                    .IsRequired();

                e.Property(x => x.NoIdentificacion)
                    .HasColumnName("NoIdentificacion")
                    .IsRequired();

                e.Property(x => x.Correo)
                    .HasColumnName("Correo")
                    .HasMaxLength(150)
                    .IsRequired();

                e.Property(x => x.Contrasena)
                    .HasColumnName("Contrasena")
                    .HasMaxLength(255)
                    .IsRequired();

                e.Property(x => x.FechaAlta)
                    .HasColumnName("FechaAlta")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(x => x.Activo)
                    .HasColumnName("Activo")
                    .HasColumnType("bit(1)")
                    .HasDefaultValue(true);

                e.HasIndex(x => x.Correo)
                    .IsUnique()
                    .HasDatabaseName("uq_usuarios_correo");

                e.HasIndex(x => x.NoIdentificacion)
                    .IsUnique()
                    .HasDatabaseName("uq_usuarios_noident");

                e.HasOne(x => x.Rol)
                    .WithMany(r => r.Usuarios)
                    .HasForeignKey(x => x.IdRol)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_usuarios_rol");
            });

            // ---------- Categoria ----------
            modelBuilder.Entity<Categoria>(e =>
            {
                e.ToTable("Categorias");

                e.HasKey(x => x.IdCategoria);

                e.Property(x => x.IdCategoria)
                    .HasColumnName("idCategoria")
                    .ValueGeneratedOnAdd();

                e.Property(x => x.CategoriaNombre)
                    .HasColumnName("CategoriaNombre")
                    .HasMaxLength(100)
                    .IsRequired();

                e.Property(x => x.Descripcion)
                    .HasColumnName("Descripcion")
                    .HasMaxLength(255);

                e.Property(x => x.FechaAlta)
                    .HasColumnName("FechaAlta")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(x => x.Activo)
                    .HasColumnName("Activo")
                    .HasColumnType("bit(1)")
                    .HasDefaultValue(true);
            });

            // ---------- Noticia ----------
            modelBuilder.Entity<Noticia>(e =>
            {
                e.ToTable("Noticias");

                e.HasKey(x => x.IdNoticia);

                e.Property(x => x.IdNoticia)
                    .HasColumnName("idNoticia")
                    .ValueGeneratedOnAdd();

                e.Property(x => x.Titulo)
                    .HasColumnName("Titulo")
                    .HasMaxLength(200)
                    .IsRequired();

                e.Property(x => x.Contenido)
                    .HasColumnName("Contenido")
                    .HasColumnType("text")
                    .IsRequired();

                e.Property(x => x.IdUsuario)
                    .HasColumnName("idUsuario")
                    .IsRequired();

                e.Property(x => x.IdCategoria)
                    .HasColumnName("idCategoria")
                    .IsRequired();

                e.Property(x => x.FechaPublicacion)
                    .HasColumnName("FechaPublicacion")
                    .HasColumnType("date")
                    .IsRequired();

                e.Property(x => x.FechaAlta)
                    .HasColumnName("FechaAlta")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(x => x.Activo)
                    .HasColumnName("Activo")
                    .HasColumnType("bit(1)")
                    .HasDefaultValue(true);

                e.HasOne(x => x.Usuario)
                    .WithMany(u => u.Noticias)
                    .HasForeignKey(x => x.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_noticias_usuario");

                e.HasOne(x => x.Categoria)
                    .WithMany(c => c.Noticias)
                    .HasForeignKey(x => x.IdCategoria)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_noticias_categoria");
            });

            // ---------- NoticiaDocumento ----------
            modelBuilder.Entity<NoticiaDocumento>(e =>
            {
                e.ToTable("NoticiasDocumentos");

                e.HasKey(x => x.IdNoticiaDocumento);

                e.Property(x => x.IdNoticiaDocumento)
                    .HasColumnName("idNoticiaDocumento")
                    .ValueGeneratedOnAdd();

                e.Property(x => x.IdNoticia)
                    .HasColumnName("idNoticia")
                    .IsRequired();

                e.Property(x => x.DocumentoNombre)
                    .HasColumnName("DocumentoNombre")
                    .HasMaxLength(200)
                    .IsRequired();

                e.Property(x => x.RutaArchivo)
                    .HasColumnName("RutaArchivo")
                    .HasMaxLength(500)
                    .IsRequired();

                e.Property(x => x.TipoDocumento)
                    .HasColumnName("TipoDocumento")
                    .HasMaxLength(50)
                    .IsRequired();

                e.Property(x => x.FechaAlta)
                    .HasColumnName("FechaAlta")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(x => x.Activo)
                    .HasColumnName("Activo")
                    .HasColumnType("bit(1)")
                    .HasDefaultValue(true);

                e.HasOne(x => x.Noticia)
                    .WithMany(n => n.Documentos)
                    .HasForeignKey(x => x.IdNoticia)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_noticiasdocumentos_noticia");
            });

            // ---------- Comentario ----------
            modelBuilder.Entity<Comentario>(e =>
            {
                e.ToTable("Comentarios");

                e.HasKey(x => x.IdComentario);

                e.Property(x => x.IdComentario)
                    .HasColumnName("idComentario")
                    .ValueGeneratedOnAdd();

                e.Property(x => x.ComentarioTexto)
                    .HasColumnName("Comentario")
                    .HasMaxLength(1000)
                    .IsRequired();

                e.Property(x => x.FechaPublicacion)
                    .HasColumnName("FechaPublicacion")
                    .HasColumnType("date")
                    .IsRequired();

                e.Property(x => x.IdUsuario)
                    .HasColumnName("idUsuario")
                    .IsRequired();

                e.Property(x => x.FechaAlta)
                    .HasColumnName("FechaAlta")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(x => x.Activo)
                    .HasColumnName("Activo")
                    .HasColumnType("bit(1)")
                    .HasDefaultValue(true);

                e.HasOne(x => x.Usuario)
                    .WithMany(u => u.Comentarios)
                    .HasForeignKey(x => x.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_comentarios_usuario");
            });

            // ---------- NoticiaComentario (puente) ----------
            modelBuilder.Entity<NoticiaComentario>(e =>
            {
                e.ToTable("NoticiasComentarios");

                e.HasKey(x => x.IdNoticiaComentario);

                e.Property(x => x.IdNoticiaComentario)
                    .HasColumnName("idNoticiaComentario")
                    .ValueGeneratedOnAdd();

                e.Property(x => x.IdNoticia)
                    .HasColumnName("idNoticia")
                    .IsRequired();

                e.Property(x => x.IdComentario)
                    .HasColumnName("idComentario")
                    .IsRequired();

                e.Property(x => x.FechaAlta)
                    .HasColumnName("FechaAlta")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(x => x.Activo)
                    .HasColumnName("Activo")
                    .HasColumnType("bit(1)")
                    .HasDefaultValue(true);

                e.HasOne(x => x.Noticia)
                    .WithMany(n => n.NoticiasComentarios)
                    .HasForeignKey(x => x.IdNoticia)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_noticom_noticia");

                e.HasOne(x => x.Comentario)
                    .WithMany(c => c.NoticiasComentarios)
                    .HasForeignKey(x => x.IdComentario)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_noticom_comentario");
            });

            // ---------- Evento ----------
            modelBuilder.Entity<Evento>(e =>
            {
                e.ToTable("Eventos");
                e.HasKey(x => x.IdEvento);
                e.Property(x => x.IdEvento).HasColumnName("idEvento");
                e.Property(x => x.Titulo).HasColumnName("Titulo").IsRequired();
                e.Property(x => x.Descripcion).HasColumnName("Descripcion");
                e.Property(x => x.Lugar).HasColumnName("Lugar");
                e.Property(x => x.FechaInicio).HasColumnName("FechaInicio");
                e.Property(x => x.FechaFin).HasColumnName("FechaFin");
                e.Property(x => x.HoraInicio).HasColumnName("HoraInicio");
                e.Property(x => x.HoraFin).HasColumnName("HoraFin");
                e.Property(x => x.IdUsuario).HasColumnName("idUsuario");
                e.Property(x => x.IdCategoria).HasColumnName("idCategoria");
                e.Property(x => x.FechaAlta).HasColumnName("FechaAlta");
                e.Property(x => x.Activo).HasColumnName("Activo");

                e.HasOne(x => x.Usuario)
                 .WithMany(u => u.Eventos)
                 .HasForeignKey(x => x.IdUsuario);

                e.HasOne(x => x.Categoria)
                 .WithMany(c => c.Eventos)
                 .HasForeignKey(x => x.IdCategoria)
                 .IsRequired(false);
            });

            // ---------- Academia ----------
            modelBuilder.Entity<Academia>(e =>
            {
                e.ToTable("Academias");
                e.HasKey(x => x.IdAcademia);
                e.Property(x => x.IdAcademia).HasColumnName("idAcademia");
                e.Property(x => x.Nombre).HasColumnName("Nombre").IsRequired();
                e.Property(x => x.Descripcion).HasColumnName("Descripcion");
                e.Property(x => x.FechaAlta).HasColumnName("FechaAlta");
                e.Property(x => x.Activo).HasColumnName("Activo");
            });

            // ---------- Materia ----------
            modelBuilder.Entity<Materia>(e =>
            {
                e.ToTable("Materias");
                e.HasKey(x => x.IdMateria);
                e.Property(x => x.IdMateria).HasColumnName("idMateria");
                e.Property(x => x.IdAcademia).HasColumnName("idAcademia");
                e.Property(x => x.MateriaNombre).HasColumnName("MateriaNombre").IsRequired();
                e.Property(x => x.Descripcion).HasColumnName("Descripcion");
                e.Property(x => x.FechaAlta).HasColumnName("FechaAlta");
                e.Property(x => x.Activo).HasColumnName("Activo");

                e.HasOne(x => x.Academia)
                 .WithMany(a => a.Materias)
                 .HasForeignKey(x => x.IdAcademia);
            });

            // ---------- GrupoTipo ----------
            modelBuilder.Entity<GrupoTipo>(e =>
            {
                e.ToTable("GruposTipos");
                e.HasKey(x => x.IdGrupoTipo);
                e.Property(x => x.IdGrupoTipo).HasColumnName("idGrupoTipo");
                e.Property(x => x.Tipo).HasColumnName("GrupoTipo").IsRequired();
                e.Property(x => x.FechaAlta).HasColumnName("FechaAlta");
                e.Property(x => x.Activo).HasColumnName("Activo");
            });

            // ---------- Grupo ----------
            modelBuilder.Entity<Grupo>(e =>
            {
                e.ToTable("Grupos");
                e.HasKey(x => x.IdGrupo);
                e.Property(x => x.IdGrupo).HasColumnName("idGrupo");
                e.Property(x => x.IdMateria).HasColumnName("idMateria");
                e.Property(x => x.IdUsuario).HasColumnName("idUsuario");
                e.Property(x => x.IdGrupoTipo).HasColumnName("idGrupoTipo");
                e.Property(x => x.FechaInicio).HasColumnName("FechaInicio");
                e.Property(x => x.FechaFin).HasColumnName("FechaFin");
                e.Property(x => x.HoraInicio).HasColumnName("HoraInicio");
                e.Property(x => x.HoraFin).HasColumnName("HoraFin");
                e.Property(x => x.Dias).HasColumnName("Dias").IsRequired();
                e.Property(x => x.FechaAlta).HasColumnName("FechaAlta");
                e.Property(x => x.Activo).HasColumnName("Activo");

                e.HasOne(x => x.Materia)
                 .WithMany(m => m.Grupos)
                 .HasForeignKey(x => x.IdMateria);

                e.HasOne(x => x.Profesor)
                 .WithMany()
                 .HasForeignKey(x => x.IdUsuario);

                e.HasOne(x => x.GrupoTipoNav)
                 .WithMany(gt => gt.Grupos)
                 .HasForeignKey(x => x.IdGrupoTipo);
            });

            // ---------- GrupoAlumno (puente) ----------
            modelBuilder.Entity<GrupoAlumno>(e =>
            {
                e.ToTable("GruposAlumnos");
                e.HasKey(x => x.IdGrupoAlumno);
                e.Property(x => x.IdGrupoAlumno).HasColumnName("idGrupoAlumno");
                e.Property(x => x.IdGrupo).HasColumnName("idGrupo");
                e.Property(x => x.IdUsuario).HasColumnName("idUsuario");
                e.Property(x => x.FechaAlta).HasColumnName("FechaAlta");
                e.Property(x => x.Activo).HasColumnName("Activo");

                e.HasOne(x => x.Grupo)
                 .WithMany(g => g.GruposAlumnos)
                 .HasForeignKey(x => x.IdGrupo);

                e.HasOne(x => x.Alumno)
                 .WithMany()
                 .HasForeignKey(x => x.IdUsuario);

                e.HasIndex(x => new { x.IdGrupo, x.IdUsuario }).IsUnique();
            });
        }
    }
}
