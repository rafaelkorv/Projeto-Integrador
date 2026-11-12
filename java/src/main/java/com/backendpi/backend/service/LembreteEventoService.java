package com.backendpi.backend.service;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.backendpi.backend.model.Evento;
import com.backendpi.backend.model.ParticipacaoEvento;
import com.backendpi.backend.repository.EventoRepository;
import com.backendpi.backend.repository.ParticipacaoEventoRepository;

@Service
public class LembreteEventoService {

    private final ParticipacaoEventoRepository participacaoRepository;
    private final EventoRepository eventoRepository;
    private final NotificacaoEventoService notificacaoService;

    public LembreteEventoService(
            ParticipacaoEventoRepository participacaoRepository,
            EventoRepository eventoRepository,
            NotificacaoEventoService notificacaoService) {
        this.participacaoRepository = participacaoRepository;
        this.eventoRepository = eventoRepository;
        this.notificacaoService = notificacaoService;
    }

    @Scheduled(fixedDelay = 900000, initialDelay = 900000)
    @Transactional
    public void enviarLembretesDeEventosProximos() {
        LocalDateTime agora = LocalDateTime.now();
        LocalDateTime limite = agora.plusHours(24);
        List<ParticipacaoEvento> pendentes =
                participacaoRepository.findByLembreteEnviadoFalse();

        for (ParticipacaoEvento participacao : pendentes) {
            Evento evento = eventoRepository.findById(participacao.getEventoId())
                    .orElse(null);
            if (evento == null) {
                participacao.setLembreteEnviado(true);
                continue;
            }

            if ("CANCELADO".equals(evento.getStatus())) {
                participacao.setLembreteEnviado(true);
                continue;
            }

            LocalDateTime inicio = LocalDateTime.of(
                    evento.getDataEvento(),
                    evento.getHorarioInicio()
            );
            if (!inicio.isAfter(agora)) {
                participacao.setLembreteEnviado(true);
            } else if (!inicio.isAfter(limite)) {
                notificacaoService.notificarUsuario(
                        participacao.getUsuarioId(),
                        evento.getId(),
                        "LEMBRETE_EVENTO",
                        "Lembrete: \"" + evento.getTitulo()
                                + "\" começa nas próximas 24 horas."
                );
                participacao.setLembreteEnviado(true);
            }
        }
    }
}
