-- ============================================================
-- Portal de Noticias Institucional - ESIME Zacatenco
-- Script de creación de base de datos (MySQL 8+)
-- ============================================================

CREATE DATABASE IF NOT EXISTS portal_noticias
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE portal_noticias;

-- ============================================================
-- 1. ROLES Y USUARIOS
-- ============================================================

CREATE TABLE NombreRol (
    idRol       INT AUTO_INCREMENT PRIMARY KEY,
    NombreRol   VARCHAR(50)  NOT NULL,
    FechaAlta   DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo      BIT          NOT NULL DEFAULT 1
) ENGINE=InnoDB;

CREATE TABLE Usuarios (
    idUsuario        INT AUTO_INCREMENT PRIMARY KEY,
    Nombre           VARCHAR(150) NOT NULL,
    idRol            INT          NOT NULL,
    NoIdentificacion VARCHAR(30)  NOT NULL,
    Correo           VARCHAR(150) NOT NULL,
    Contrasena       VARCHAR(255) NOT NULL,
    FechaAlta        DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo           BIT          NOT NULL DEFAULT 1,
    CONSTRAINT fk_usuarios_rol FOREIGN KEY (idRol) REFERENCES NombreRol(idRol),
    CONSTRAINT uq_usuarios_correo UNIQUE (Correo),
    CONSTRAINT uq_usuarios_noident UNIQUE (NoIdentificacion)
) ENGINE=InnoDB;

-- ============================================================
-- 2. CATEGORÍAS Y NOTICIAS (antes "Avisos")
-- ============================================================

CREATE TABLE Categorias (
    idCategoria      INT AUTO_INCREMENT PRIMARY KEY,
    CategoriaNombre  VARCHAR(100) NOT NULL,
    Descripcion      VARCHAR(255),
    FechaAlta        DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo           BIT          NOT NULL DEFAULT 1
) ENGINE=InnoDB;

CREATE TABLE Noticias (
    idNoticia        INT AUTO_INCREMENT PRIMARY KEY,
    Titulo           VARCHAR(200) NOT NULL,
    Contenido        TEXT         NOT NULL,
    idUsuario        INT          NOT NULL,
    idCategoria      INT          NOT NULL,
    FechaPublicacion DATE         NOT NULL,
    FechaAlta        DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo           BIT          NOT NULL DEFAULT 1,
    CONSTRAINT fk_noticias_usuario   FOREIGN KEY (idUsuario) REFERENCES Usuarios(idUsuario),
    CONSTRAINT fk_noticias_categoria FOREIGN KEY (idCategoria) REFERENCES Categorias(idCategoria)
) ENGINE=InnoDB;

CREATE TABLE NoticiasDocumentos (
    idNoticiaDocumento INT AUTO_INCREMENT PRIMARY KEY,
    idNoticia          INT          NOT NULL,
    DocumentoNombre    VARCHAR(200) NOT NULL,
    RutaArchivo        VARCHAR(500) NOT NULL,
    TipoDocumento      VARCHAR(50)  NOT NULL,
    FechaAlta          DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo             BIT          NOT NULL DEFAULT 1,
    CONSTRAINT fk_noticiasdocumentos_noticia FOREIGN KEY (idNoticia) REFERENCES Noticias(idNoticia)
) ENGINE=InnoDB;

CREATE TABLE Comentarios (
    idComentario     INT AUTO_INCREMENT PRIMARY KEY,
    Comentario       VARCHAR(1000) NOT NULL,
    FechaPublicacion DATE          NOT NULL,
    idUsuario        INT           NOT NULL,
    FechaAlta        DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo           BIT           NOT NULL DEFAULT 1,
    CONSTRAINT fk_comentarios_usuario FOREIGN KEY (idUsuario) REFERENCES Usuarios(idUsuario)
) ENGINE=InnoDB;

CREATE TABLE NoticiasComentarios (
    idNoticiaComentario INT AUTO_INCREMENT PRIMARY KEY,
    idNoticia           INT      NOT NULL,
    idComentario        INT      NOT NULL,
    FechaAlta           DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo               BIT      NOT NULL DEFAULT 1,
    CONSTRAINT fk_noticom_noticia    FOREIGN KEY (idNoticia) REFERENCES Noticias(idNoticia),
    CONSTRAINT fk_noticom_comentario FOREIGN KEY (idComentario) REFERENCES Comentarios(idComentario)
) ENGINE=InnoDB;

-- ============================================================
-- 3. EVENTOS (agregada - no estaba en el diagrama original)
-- ============================================================

CREATE TABLE Eventos (
    idEvento     INT AUTO_INCREMENT PRIMARY KEY,
    Titulo       VARCHAR(200) NOT NULL,
    Descripcion  TEXT,
    Lugar        VARCHAR(200),
    FechaInicio  DATE         NOT NULL,
    FechaFin     DATE         NOT NULL,
    HoraInicio   TIME,
    HoraFin      TIME,
    idUsuario    INT          NOT NULL,   -- quién lo publicó/organiza
    idCategoria  INT          NULL,       -- opcional: reutiliza Categorias
    FechaAlta    DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo       BIT          NOT NULL DEFAULT 1,
    CONSTRAINT fk_eventos_usuario   FOREIGN KEY (idUsuario) REFERENCES Usuarios(idUsuario),
    CONSTRAINT fk_eventos_categoria FOREIGN KEY (idCategoria) REFERENCES Categorias(idCategoria)
) ENGINE=InnoDB;

-- ============================================================
-- 4. ESTRUCTURA ACADÉMICA
-- ============================================================

CREATE TABLE Academias (
    idAcademia   INT AUTO_INCREMENT PRIMARY KEY,
    Nombre       VARCHAR(150) NOT NULL,
    Descripcion  VARCHAR(255),
    FechaAlta    DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo       BIT          NOT NULL DEFAULT 1
) ENGINE=InnoDB;

CREATE TABLE Materias (
    idMateria     INT AUTO_INCREMENT PRIMARY KEY,
    idAcademia    INT          NOT NULL,
    MateriaNombre VARCHAR(150) NOT NULL,
    Descripcion   VARCHAR(255),
    FechaAlta     DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo        BIT          NOT NULL DEFAULT 1,
    CONSTRAINT fk_materias_academia FOREIGN KEY (idAcademia) REFERENCES Academias(idAcademia)
) ENGINE=InnoDB;

CREATE TABLE GruposTipos (
    idGrupoTipo INT AUTO_INCREMENT PRIMARY KEY,
    GrupoTipo   VARCHAR(100) NOT NULL,
    FechaAlta   DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo      BIT          NOT NULL DEFAULT 1
) ENGINE=InnoDB;

CREATE TABLE Grupos (
    idGrupo       INT AUTO_INCREMENT PRIMARY KEY,
    idMateria     INT      NOT NULL,
    idUsuario     INT      NOT NULL,  -- profesor
    idGrupoTipo   INT      NOT NULL,
    FechaInicio   DATE     NOT NULL,
    FechaFin      DATE     NOT NULL,
    HoraInicio    TIME     NOT NULL,
    HoraFin       TIME     NOT NULL,
    Dias          VARCHAR(50) NOT NULL,
    FechaAlta     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo        BIT      NOT NULL DEFAULT 1,
    CONSTRAINT fk_grupos_materia   FOREIGN KEY (idMateria) REFERENCES Materias(idMateria),
    CONSTRAINT fk_grupos_profesor  FOREIGN KEY (idUsuario) REFERENCES Usuarios(idUsuario),
    CONSTRAINT fk_grupos_tipo      FOREIGN KEY (idGrupoTipo) REFERENCES GruposTipos(idGrupoTipo)
) ENGINE=InnoDB;

CREATE TABLE GruposAlumnos (
    idGrupoAlumno INT AUTO_INCREMENT PRIMARY KEY,
    idGrupo       INT      NOT NULL,
    idUsuario     INT      NOT NULL,  -- alumno
    FechaAlta     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Activo        BIT      NOT NULL DEFAULT 1,
    CONSTRAINT fk_gpoalumnos_grupo   FOREIGN KEY (idGrupo) REFERENCES Grupos(idGrupo),
    CONSTRAINT fk_gpoalumnos_alumno  FOREIGN KEY (idUsuario) REFERENCES Usuarios(idUsuario),
    CONSTRAINT uq_gpoalumnos UNIQUE (idGrupo, idUsuario)
) ENGINE=InnoDB;

-- ============================================================
-- 5. DATOS INICIALES BÁSICOS (catálogos)
-- ============================================================

INSERT INTO NombreRol (NombreRol) VALUES
    ('Administrador'),
    ('Docente'),
    ('Alumno');

INSERT INTO Categorias (CategoriaNombre, Descripcion) VALUES
    ('General', 'Noticias generales de la institución'),
    ('Académico', 'Noticias relacionadas con actividades académicas'),
    ('Eventos', 'Convocatorias y actividades especiales');

INSERT INTO GruposTipos (GrupoTipo) VALUES
    ('Teoría'),
    ('Laboratorio'),
    ('Mixto');

-- ============================================================
-- Fin del script
-- ============================================================
