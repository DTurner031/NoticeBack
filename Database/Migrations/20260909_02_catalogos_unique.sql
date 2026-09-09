USE portal_noticias;

-- ============================================================
-- Migración 3.2
-- Restricciones UNIQUE para catálogos principales
-- ============================================================

ALTER TABLE NombreRol
    ADD CONSTRAINT uq_nombrerol_nombre
    UNIQUE (NombreRol);

ALTER TABLE Categorias
    ADD CONSTRAINT uq_categorias_nombre
    UNIQUE (CategoriaNombre);

ALTER TABLE Academias
    ADD CONSTRAINT uq_academias_nombre
    UNIQUE (Nombre);

ALTER TABLE GruposTipos
    ADD CONSTRAINT uq_grupostipos_nombre
    UNIQUE (GrupoTipo);