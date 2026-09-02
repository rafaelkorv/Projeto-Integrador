package com.backendpi.backend.dto;

import java.time.LocalDateTime;

import com.backendpi.backend.model.Mensagem;

public class MensagemDTO {

    private Long idMensagem;
    private Long idConversa;
    private Long idRemetente;
    private String nomeRemetente;
    private String conteudo;
    private LocalDateTime dataEnvio;
    private Boolean lida;

    public MensagemDTO(Mensagem mensagem) {
        this.idMensagem = mensagem.getIdMensagem();
        this.idConversa = mensagem.getConversa().getIdConversa();
        this.idRemetente = mensagem.getRemetente().getIdUsuario();
        this.nomeRemetente = mensagem.getRemetente().getNome();
        this.conteudo = mensagem.getConteudo();
        this.dataEnvio = mensagem.getDataEnvio();
        this.lida = mensagem.getLida();
    }

    public Long getIdMensagem() {
        return idMensagem;
    }

    public Long getIdConversa() {
        return idConversa;
    }

    public Long getIdRemetente() {
        return idRemetente;
    }

    public String getNomeRemetente() {
        return nomeRemetente;
    }

    public String getConteudo() {
        return conteudo;
    }

    public LocalDateTime getDataEnvio() {
        return dataEnvio;
    }

    public Boolean getLida() {
        return lida;
    }
}
