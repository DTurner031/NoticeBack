USE portal_noticias;

-- ============================================================
-- Migración 3.1
-- Usuarios.NoIdentificacion:
-- INT -> VARCHAR(30)
-- ============================================================

ALTER TABLE Usuarios
    MODIFY COLUMN NoIdentificacion VARCHAR(30) NOT NULL;