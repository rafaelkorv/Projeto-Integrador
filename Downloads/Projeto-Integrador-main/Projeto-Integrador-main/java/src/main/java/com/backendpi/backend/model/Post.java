package com.backendpi.backend.model;

import java.sql.Timestamp;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

@Entity
@Table(name = "posts")
public class Post {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id_post")
    private Long idPost;

    private String titulo;

    private String conteudo;

    @Column(name = "id_usuario")
    private Long idUsuario;

    @Column(name = "id_comunidade")
    private Long idComunidade;

    @Column(name = "data_postagem", insertable = false, updatable = false)
    private Timestamp dataPostagem;

    // --- GETTERS E SETTERS ---
    public Long getIdPost() { return idPost; }
    public void setIdPost(Long idPost) { this.idPost = idPost; }

    public String getTitulo() { return titulo; }
    public void setTitulo(String titulo) { this.titulo = titulo; }

    public String getConteudo() { return conteudo; }
    public void setConteudo(String conteudo) { this.conteudo = conteudo; }

    public Long getIdUsuario() { return idUsuario; }
    public void setIdUsuario(Long idUsuario) { this.idUsuario = idUsuario; }

    public Long getIdComunidade() { return idComunidade; }
    public void setIdComunidade(Long idComunidade) { this.idComunidade = idComunidade; }

    public Timestamp getDataPostagem() { return dataPostagem; }
}