package com.backendpi.backend.model;

import jakarta.persistence.*;
import java.sql.Timestamp;

@Entity
@Table(name = "membros_comunidade")
public class MembrosComunidade {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "id_usuario")
    private Long idUsuario;

    @Column(name = "id_comunidade")
    private Long idComunidade;

    @Column(name = "data_entrada", insertable = false, updatable = false)
    private Timestamp dataEntrada;

    public Long getId() {
        return id;
    }

    public Long getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(Long idUsuario) {
        this.idUsuario = idUsuario;
    }

    public Long getIdComunidade() {
        return idComunidade;
    }

    public void setIdComunidade(Long idComunidade) {
        this.idComunidade = idComunidade;
    }
}