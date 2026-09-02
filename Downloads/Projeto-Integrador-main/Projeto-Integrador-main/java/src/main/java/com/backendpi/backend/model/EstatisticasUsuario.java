package com.backendpi.backend.model;

import jakarta.persistence.*;
import java.sql.Timestamp;

@Entity
@Table(name = "estatisticas_usuario")
public class EstatisticasUsuario {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id_estatistica")
    private Long idEstatistica;

    @Column(name = "id_usuario")
    private Long idUsuario;

    @Column(name = "tempo_total_uso")
    private Integer tempoTotalUso;

    @Column(name = "posts_visualizados")
    private Integer postsVisualizados;

    @Column(name = "posts_criados")
    private Integer postsCriados;

    @Column(name = "comentarios_feitos")
    private Integer comentariosFeitos;

    @Column(name = "votos_realizados")
    private Integer votosRealizados;

    @Column(name = "ultimo_acesso", insertable = false, updatable = false)
    private Timestamp ultimoAcesso;

    public Long getIdEstatistica() {
        return idEstatistica;
    }

    public void setIdEstatistica(Long idEstatistica) {
        this.idEstatistica = idEstatistica;
    }

    public Long getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(Long idUsuario) {
        this.idUsuario = idUsuario;
    }

    public Integer getTempoTotalUso() {
        return tempoTotalUso;
    }

    public void setTempoTotalUso(Integer tempoTotalUso) {
        this.tempoTotalUso = tempoTotalUso;
    }

    public Integer getPostsVisualizados() {
        return postsVisualizados;
    }

    public void setPostsVisualizados(Integer postsVisualizados) {
        this.postsVisualizados = postsVisualizados;
    }

    public Integer getPostsCriados() {
        return postsCriados;
    }

    public void setPostsCriados(Integer postsCriados) {
        this.postsCriados = postsCriados;
    }

    public Integer getComentariosFeitos() {
        return comentariosFeitos;
    }

    public void setComentariosFeitos(Integer comentariosFeitos) {
        this.comentariosFeitos = comentariosFeitos;
    }

    public Integer getVotosRealizados() {
        return votosRealizados;
    }

    public void setVotosRealizados(Integer votosRealizados) {
        this.votosRealizados = votosRealizados;
    }

    public Timestamp getUltimoAcesso() {
        return ultimoAcesso;
    }
}