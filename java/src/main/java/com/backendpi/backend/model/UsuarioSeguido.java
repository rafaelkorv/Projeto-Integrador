package com.backendpi.backend.model;

import java.time.LocalDateTime;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;

@Entity
@Table(name = "usuarios_seguindo", uniqueConstraints = {
        @UniqueConstraint(columnNames = {"seguidor_id", "seguido_id"})
})
public class UsuarioSeguido {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "seguidor_id", nullable = false)
    private Long seguidorId;

    @Column(name = "seguido_id", nullable = false)
    private Long seguidoId;

    @Column(name = "seguido_em", nullable = false)
    private LocalDateTime seguidoEm;

    public Long getId() {
        return id;
    }

    public Long getSeguidorId() {
        return seguidorId;
    }

    public void setSeguidorId(Long seguidorId) {
        this.seguidorId = seguidorId;
    }

    public Long getSeguidoId() {
        return seguidoId;
    }

    public void setSeguidoId(Long seguidoId) {
        this.seguidoId = seguidoId;
    }

    public LocalDateTime getSeguidoEm() {
        return seguidoEm;
    }

    public void setSeguidoEm(LocalDateTime seguidoEm) {
        this.seguidoEm = seguidoEm;
    }
}
