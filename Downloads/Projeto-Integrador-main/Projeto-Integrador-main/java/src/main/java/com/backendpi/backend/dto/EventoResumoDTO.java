package com.backendpi.backend.dto;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;

public class EventoResumoDTO {

    private Long id;
    private String titulo;
    private String descricao;
    private String categoria;
    private String imagemCapa;
    private LocalDate dataEvento;
    private LocalTime horarioInicio;
    private LocalTime horarioFim;
    private String localEvento;
    private Long comunidadeId;
    private Long criadorId;
    private Integer limiteParticipantes;
    private Boolean exigeCheckin;
    private String status;
    private LocalDateTime encerramentoInscricoes;
    private Long quantidadeParticipantes;
    private BigDecimal precoIngresso;

    public EventoResumoDTO(
            Long id,
            String titulo,
            String descricao,
            String categoria,
            String imagemCapa,
            LocalDate dataEvento,
            LocalTime horarioInicio,
            LocalTime horarioFim,
            String localEvento,
            Long comunidadeId,
            Long criadorId,
            Integer limiteParticipantes,
            BigDecimal precoIngresso,
            Boolean exigeCheckin,
            String status,
            LocalDateTime encerramentoInscricoes,
            Long quantidadeParticipantes) {

        this.id = id;
        this.titulo = titulo;
        this.descricao = descricao;
        this.categoria = categoria;
        this.imagemCapa = imagemCapa;
        this.dataEvento = dataEvento;
        this.horarioInicio = horarioInicio;
        this.horarioFim = horarioFim;
        this.localEvento = localEvento;
        this.comunidadeId = comunidadeId;
        this.criadorId = criadorId;
        this.limiteParticipantes = limiteParticipantes;
        this.precoIngresso = precoIngresso;
        this.exigeCheckin = exigeCheckin;
        this.status = status;
        this.encerramentoInscricoes = encerramentoInscricoes;
        this.quantidadeParticipantes = quantidadeParticipantes;
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

    public String getImagemCapa() {
        return imagemCapa;
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

    public Long getCriadorId() {
        return criadorId;
    }

    public Integer getLimiteParticipantes() {
        return limiteParticipantes;
    }

    public Boolean getExigeCheckin() {
        return exigeCheckin;
    }

    public String getStatus() {
        return status;
    }

    public LocalDateTime getEncerramentoInscricoes() {
        return encerramentoInscricoes;
    }

    public Long getQuantidadeParticipantes() {
        return quantidadeParticipantes;
    }

    public String getSituacaoTemporal() {

        if ("CANCELADO".equals(status)) {
            return "CANCELADO";
        }

        if (dataEvento == null
                || horarioInicio == null
                || horarioFim == null) {
            return "INDEFINIDO";
        }

        LocalDateTime agora = LocalDateTime.now();

        LocalDateTime inicio
                = LocalDateTime.of(dataEvento, horarioInicio);

        LocalDateTime fim
                = LocalDateTime.of(dataEvento, horarioFim);

        if (agora.isBefore(inicio)) {
            return "AGENDADO";
        }

        if (agora.isBefore(fim)) {
            return "ACONTECENDO_AGORA";
        }

        return "ENCERRADO";
    }

    public BigDecimal getPrecoIngresso() {
    return precoIngresso;
}

public void setPrecoIngresso(BigDecimal precoIngresso) {
    this.precoIngresso = precoIngresso;
}
}
