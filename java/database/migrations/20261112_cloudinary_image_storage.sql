ALTER TABLE usuarios
    ADD COLUMN foto_perfil_public_id VARCHAR(255) NULL;

ALTER TABLE imagens_post
    ADD COLUMN public_id VARCHAR(255) NULL;
