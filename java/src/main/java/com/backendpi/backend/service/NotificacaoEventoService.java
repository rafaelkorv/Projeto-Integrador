package com.backendpi.backend.service;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.backendpi.backend.model.NotificacaoEvento;
import com.backendpi.backend.repository.NotificacaoEventoRepository;
import com.backendpi.backend.repository.ParticipacaoEventoRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;

@Service
public class NotificacaoEventoService {

    private final NotificacaoEventoRepository notificacaoRepository;
    private final ParticipacaoEventoRepository participacaoRepository;

    public NotificacaoEventoService(
            NotificacaoEventoRepository notificacaoRepository,
            ParticipacaoEventoRepository participacaoRepository) {
        this.notificacaoRepository = notificacaoRepository;
        this.participacaoRepository = participacaoRepository;
    }

    @Transactional
    public void notificarUsuario(Long usuarioId, Long eventoId, String tipo, String mensagem) {
        NotificacaoEvento notificacao = new NotificacaoEvento();
        notificacao.setUsuarioId(usuarioId);
        notificacao.setEventoId(eventoId);
        notificacao.setTipo(tipo);
        notificacao.setMensagem(mensagem);
        notificacao.setCriadoEm(LocalDateTime.now());
        notificacao.setLida(false);
        notificacaoRepository.save(notificacao);
    }

    @Transactional
    public void notificarParticipantes(Long eventoId, String tipo, String mensagem) {
        participacaoRepository.findByEventoId(eventoId).forEach(participacao ->
                notificarUsuario(participacao.getUsuarioId(), eventoId, tipo, mensagem)
        );
    }


    public List<NotificacaoEvento> listar(Long usuarioId) {
        AuthenticatedUserContext.requireMatches(usuarioId);
        return notificacaoRepository.findTop50ByUsuarioIdOrderByCriadoEmDesc(usuarioId);
    }

    @Transactional
    public boolean marcarComoLida(Long usuarioId, Long notificacaoId) {
        AuthenticatedUserContext.requireMatches(usuarioId);
        NotificacaoEvento notificacao = notificacaoRepository
                .findByIdAndUsuarioId(notificacaoId, usuarioId)
                .orElse(null);
        if (notificacao == null) {
            return false;
        }
        notificacao.setLida(true);
        return true;
    }
}
