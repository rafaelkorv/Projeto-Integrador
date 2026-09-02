package com.backendpi.backend.dto;

import java.time.LocalDateTime;

import com.backendpi.backend.model.Conversa;
import com.backendpi.backend.model.Usuario;

public class ConversaDTO {

    private Long idConversa;

    private Long idOutroUsuario;
    private String nomeOutroUsuario;
    private String usernameOutroUsuario;
    private String fotoPerfilOutroUsuario;

    private LocalDateTime dataCriacao;

    public ConversaDTO(Conversa conversa, Long idUsuarioAtual) {

        this.idConversa = conversa.getIdConversa();
        this.dataCriacao = conversa.getDataCriacao();

        Usuario outroUsuario;

        if (conversa.getUsuario1().getIdUsuario().equals(idUsuarioAtual)) {
            outroUsuario = conversa.getUsuario2();
        } else {
            outroUsuario = conversa.getUsuario1();
        }

        this.idOutroUsuario = outroUsuario.getIdUsuario();
        this.nomeOutroUsuario = outroUsuario.getNome();
        this.usernameOutroUsuario = outroUsuario.getUsername();
        this.fotoPerfilOutroUsuario = outroUsuario.getFotoPerfil();
    }

    public Long getIdConversa() {
        return idConversa;
    }

    public Long getIdOutroUsuario() {
        return idOutroUsuario;
    }

    public String getNomeOutroUsuario() {
        return nomeOutroUsuario;
    }

    public String getUsernameOutroUsuario() {
        return usernameOutroUsuario;
    }

    public String getFotoPerfilOutroUsuario() {
        return fotoPerfilOutroUsuario;
    }

    public LocalDateTime getDataCriacao() {
        return dataCriacao;
    }
}
