package com.backendpi.backend.model;

import jakarta.persistence.*;
import java.sql.Timestamp;

@Entity
@Table(name = "interacoes_usuario")
public class InteracaoUsuario {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id_interacao")
    private Long idInteracao;

    @Column(name = "id_usuario")
    private Long idUsuario;

    @Column(name = "id_post")
    private Long idPost;

    @Column(name = "tipo_interacao")
    private String tipoInteracao;

    @Column(name = "data_interacao", insertable = false, updatable = false)
    private Timestamp dataInteracao;

    public Long getIdInteracao() {
        return idInteracao;
    }

    public void setIdInteracao(Long idInteracao) {
        this.idInteracao = idInteracao;
    }

    public Long getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(Long idUsuario) {
        this.idUsuario = idUsuario;
    }

    public Long getIdPost() {
        return idPost;
    }

    public void setIdPost(Long idPost) {
        this.idPost = idPost;
    }

    public String getTipoInteracao() {
        return tipoInteracao;
    }

    public void setTipoInteracao(String tipoInteracao) {
        this.tipoInteracao = tipoInteracao;
    }
}