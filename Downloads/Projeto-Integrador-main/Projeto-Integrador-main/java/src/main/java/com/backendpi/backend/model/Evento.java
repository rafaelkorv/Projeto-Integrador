package com.backendpi.backend.model;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;

import com.fasterxml.jackson.annotation.JsonFormat;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.persistence.Transient;

@Entity
@Table(name = "eventos")
public class Evento {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id_evento")
    private Long id;

    @Column(nullable = false, length = 150)
    private String titulo;

    @Column(columnDefinition = "TEXT")
    private String descricao;

    @Column(length = 40)
    private String categoria;

    @Column(name = "imagem_capa", length = 500)
    private String imagemCapa;

    @Column(name = "data_evento", nullable = false)
    @JsonFormat(pattern = "yyyy-MM-dd")
    private LocalDate dataEvento;

    @Column(name = "horario_inicio", nullable = false)
    @JsonFormat(pattern = "HH:mm:ss")
    private LocalTime horarioInicio;

    @Column(name = "horario_fim", nullable = false)
    @JsonFormat(pattern = "HH:mm:ss")
    private LocalTime horarioFim;

    @Column(name = "encerramento_inscricoes")
    @JsonFormat(pattern = "yyyy-MM-dd'T'HH:mm:ss")
    private LocalDateTime encerramentoInscricoes;

    @Column(name = "local_evento", nullable = false)
    private String localEvento;

    @Column(name = "comunidade_id")
    private Long comunidadeId;

    @Column(name = "criador_id", nullable = false)
    private Long criadorId;

    @Column(name = "limite_participantes")
    private Integer limiteParticipantes;

    @Column(nullable = false)
    private String status = "AGENDADO";

    @Column(name = "exige_checkin", nullable = false)
    private Boolean exigeCheckin;

    public Evento() {
        this.exigeCheckin = false;
    }

    public Long getId() {
        return id;
    }

    public String getTitulo() {
        return titulo;
    }

    public String getDescricao() {
        return descricao;
    }

    public String getCategoria() {
        return categoria;
    }

    public void setCategoria(String categoria) {
        this.categoria = categoria;
    }

    public String getImagemCapa() {
        return imagemCapa;
    }

    public void setImagemCapa(String imagemCapa) {
        this.imagemCapa = imagemCapa;
    }

    public LocalDate getDataEvento() {
        return dataEvento;
    }

    public LocalTime getHorarioInicio() {
        return horarioInicio;
    }

    public LocalTime getHorarioFim() {
        return horarioFim;
    }

    public String getLocalEvento() {
        return localEvento;
    }

    public Long getComunidadeId() {
        return comunidadeId;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public void setTitulo(String titulo) {
        this.titulo = titulo;
    }

    public void setDescricao(String descricao) {
        this.descricao = descricao;
    }

    public void setDataEvento(LocalDate dataEvento) {
        this.dataEvento = dataEvento;
    }

    public void setHorarioInicio(LocalTime horarioInicio) {
        this.horarioInicio = horarioInicio;
    }

    public void setHorarioFim(LocalTime horarioFim) {
        this.horarioFim = horarioFim;
    }

    public void setLocalEvento(String localEvento) {
        this.localEvento = localEvento;
    }

    public void setComunidadeId(Long comunidadeId) {
        this.comunidadeId = comunidadeId;
    }

    public Long getCriadorId() {
        return criadorId;
    }

    public void setCriadorId(Long criadorId) {
        this.criadorId = criadorId;
    }

    public Integer getLimiteParticipantes() {
        return limiteParticipantes;
    }

    public void setLimiteParticipantes(Integer limiteParticipantes) {
        this.limiteParticipantes = limiteParticipantes;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public Boolean getExigeCheckin() {
        return exigeCheckin;
    }

    public void setExigeCheckin(Boolean exigeCheckin) {
        this.exigeCheckin = exigeCheckin;
    }

    public LocalDateTime getEncerramentoInscricoes() {
        return encerramentoInscricoes;
    }

    public void setEncerramentoInscricoes(LocalDateTime encerramentoInscricoes) {
        this.encerramentoInscricoes = encerramentoInscricoes;
    }

    @Transient
    public String getSituacaoTemporal() {

        if ("CANCELADO".equals(status)) {
            return "CANCELADO";
        }

        if (dataEvento == null || horarioInicio == null || horarioFim == null) {
            return "INDEFINIDO";
        }

        LocalDateTime agora = LocalDateTime.now();

        LocalDateTime inicio = LocalDateTime.of(
                dataEvento,
                horarioInicio
        );

        LocalDateTime fim = LocalDateTime.of(
                dataEvento,
                horarioFim
        );

        if (agora.isBefore(inicio)) {
            return "AGENDADO";
        }

        if (!agora.isBefore(inicio) && agora.isBefore(fim)) {
            return "ACONTECENDO_AGORA";
        }

        return "ENCERRADO";
    }

    private BigDecimal precoIngresso;

    public BigDecimal getPrecoIngresso() {
    return precoIngresso;
}

public void setPrecoIngresso(BigDecimal precoIngresso) {
    this.precoIngresso = precoIngresso;
}

}
