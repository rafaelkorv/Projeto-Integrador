ALTER TABLE usuario_evento
    ADD COLUMN lembrete_enviado BOOLEAN NOT NULL DEFAULT FALSE;

CREATE TABLE IF NOT EXISTS eventos_lista_espera (
    id BIGINT NOT NULL AUTO_INCREMENT,
    usuario_id BIGINT NOT NULL,
    evento_id BIGINT NOT NULL,
    adicionado_em DATETIME NOT NULL,
    PRIMARY KEY (id),
    UNIQUE KEY uk_lista_espera_usuario_evento (usuario_id, evento_id),
    KEY idx_lista_espera_evento_ordem (evento_id, adicionado_em, id)
);

CREATE TABLE IF NOT EXISTS eventos_salvos (
    id BIGINT NOT NULL AUTO_INCREMENT,
    usuario_id BIGINT NOT NULL,
    evento_id BIGINT NOT NULL,
    salvo_em DATETIME NOT NULL,
    PRIMARY KEY (id),
    UNIQUE KEY uk_eventos_salvos_usuario_evento (usuario_id, evento_id),
    KEY idx_eventos_salvos_usuario_data (usuario_id, salvo_em)
);

CREATE TABLE IF NOT EXISTS notificacoes_evento (
    id BIGINT NOT NULL AUTO_INCREMENT,
    usuario_id BIGINT NOT NULL,
    evento_id BIGINT NULL,
    tipo VARCHAR(40) NOT NULL,
    mensagem VARCHAR(500) NOT NULL,
    criado_em DATETIME NOT NULL,
    lida BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY (id),
    KEY idx_notificacoes_usuario_data (usuario_id, criado_em)
);
