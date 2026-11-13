CREATE TABLE IF NOT EXISTS usuarios_seguindo (
    id BIGINT NOT NULL AUTO_INCREMENT,
    seguidor_id BIGINT NOT NULL,
    seguido_id BIGINT NOT NULL,
    seguido_em DATETIME NOT NULL,
    PRIMARY KEY (id),
    UNIQUE KEY uk_usuarios_seguindo_par (seguidor_id, seguido_id),
    KEY idx_usuarios_seguindo_seguidor_data (seguidor_id, seguido_em),
    CONSTRAINT fk_usuarios_seguindo_seguidor
        FOREIGN KEY (seguidor_id) REFERENCES usuarios (id_usuario) ON DELETE CASCADE,
    CONSTRAINT fk_usuarios_seguindo_seguido
        FOREIGN KEY (seguido_id) REFERENCES usuarios (id_usuario) ON DELETE CASCADE
);
