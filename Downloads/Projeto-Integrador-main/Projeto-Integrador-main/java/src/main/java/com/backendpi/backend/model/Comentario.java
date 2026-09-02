package com.backendpi.backend.model;

import jakarta.persistence.*;
import java.sql.Timestamp;

@Entity
@Table(name = "comentarios")
public class Comentario {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id_comentario")
    private Long idComentario;

    private String conteudo;

    @Column(name = "id_usuario")
    private Long idUsuario;

    @Column(name = "id_post")
    private Long idPost;

    @Column(name = "data_comentario", insertable = false, updatable = false)
    private Timestamp dataComentario;

    public Long getIdComentario() {
        return idComentario;
    }

    public void setIdComentario(Long idComentario) {
        this.idComentario = idComentario;
    }

    public String getConteudo() {
        return conteudo;
    }

    public void setConteudo(String conteudo) {
        this.conteudo = conteudo;
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

    public Timestamp getDataComentario() {
        return dataComentario;
    }

    public void setDataComentario(Timestamp dataComentario) {
        this.dataComentario = dataComentario;
    }
}