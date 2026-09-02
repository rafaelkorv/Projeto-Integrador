package com.backendpi.backend.service;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.backendpi.backend.model.Evento;
import com.backendpi.backend.model.ParticipacaoEvento;
import com.backendpi.backend.repository.EventoRepository;
import com.backendpi.backend.repository.ParticipacaoEventoRepository;
import com.backendpi.backend.repository.UsuarioRepository;

@Service
public class ParticipacaoEventoService {

    private final ParticipacaoEventoRepository participacaoRepository;
    private final EventoRepository eventoRepository;
    private final UsuarioRepository usuarioRepository;

    public ParticipacaoEventoService(
            ParticipacaoEventoRepository participacaoRepository,
            EventoRepository eventoRepository,
            UsuarioRepository usuarioRepository) {

        this.participacaoRepository = participacaoRepository;
        this.eventoRepository = eventoRepository;
        this.usuarioRepository = usuarioRepository;
    }

    public ParticipacaoEvento participar(Long idEvento, Long idUsuario) {

        Evento evento = eventoRepository.findById(idEvento)
                .orElseThrow(()
                        -> new RuntimeException("Evento não encontrado"));

        LocalDateTime agora = LocalDateTime.now();

        LocalDateTime fimEvento = LocalDateTime.of(
                evento.getDataEvento(),
                evento.getHorarioFim()
        );

        if (!agora.isBefore(fimEvento)) {
            throw new RuntimeException(
                    "Não é possível participar de um evento encerrado"
            );
        }

        LocalDateTime limiteInscricao
                = evento.getEncerramentoInscricoes();

        if (limiteInscricao == null) {
            limiteInscricao = LocalDateTime.of(
                    evento.getDataEvento(),
                    evento.getHorarioInicio()
            );
        }

        if (agora.isAfter(limiteInscricao)) {
            throw new RuntimeException(
                    "As inscrições para este evento já foram encerradas"
            );
        }

        if (!usuarioRepository.existsById(idUsuario)) {
            throw new RuntimeException("Usuário não encontrado");
        }

        if ("CANCELADO".equals(evento.getStatus())) {
            throw new RuntimeException(
                    "Não é possível participar de um evento cancelado"
            );
        }

        if (participacaoRepository.existsByUsuarioIdAndEventoId(
                idUsuario,
                idEvento)) {

            throw new RuntimeException(
                    "Usuário já está participando deste evento"
            );
        }

        long quantidadeParticipantes
                = participacaoRepository.countByEventoId(idEvento);

        if (evento.getLimiteParticipantes() != null
                && quantidadeParticipantes >= evento.getLimiteParticipantes()) {

            throw new RuntimeException(
                    "O evento atingiu o limite de participantes"
            );
        }

        ParticipacaoEvento participacao
                = new ParticipacaoEvento();

        participacao.setUsuarioId(idUsuario);
        participacao.setEventoId(idEvento);
        participacao.setStatus("INSCRITO");

        String token = UUID.randomUUID().toString();
        participacao.setTokenIngresso(token);

        return participacaoRepository.save(participacao);
    }

    @Transactional
    public void cancelarParticipacao(
            Long idEvento,
            Long idUsuario) {

        if (!participacaoRepository.existsByUsuarioIdAndEventoId(
                idUsuario,
                idEvento)) {

            throw new RuntimeException(
                    "Usuário não está participando deste evento"
            );
        }

        participacaoRepository.deleteByUsuarioIdAndEventoId(
                idUsuario,
                idEvento
        );
    }

    public List<ParticipacaoEvento> listarPorEvento(Long idEvento) {

        if (!eventoRepository.existsById(idEvento)) {
            throw new RuntimeException("Evento não encontrado");
        }

        return participacaoRepository.findByEventoId(idEvento);
    }

    public long contarParticipantes(Long idEvento) {
        return participacaoRepository.countByEventoId(idEvento);
    }

    @Transactional
    public void removerParticipante(
            Long idEvento,
            Long idParticipante,
            Long idSolicitante) {

        Evento evento = eventoRepository.findById(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));

        if (!evento.getCriadorId().equals(idSolicitante)) {
            throw new RuntimeException(
                    "Usuário sem permissão para remover participantes"
            );
        }

        if (!participacaoRepository.existsByUsuarioIdAndEventoId(
                idParticipante,
                idEvento)) {

            throw new RuntimeException(
                    "Usuário não participa deste evento"
            );
        }

        participacaoRepository.deleteByUsuarioIdAndEventoId(
                idParticipante,
                idEvento
        );
    }

    public ParticipacaoEvento validarIngresso(
            Long idEvento,
            Long idSolicitante,
            String tokenIngresso) {

        Evento evento = eventoRepository.findById(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));

        if (!evento.getCriadorId().equals(idSolicitante)) {
            throw new RuntimeException(
                    "Usuário sem permissão para validar ingressos deste evento"
            );
        }

        ParticipacaoEvento participacao
                = participacaoRepository.findByTokenIngresso(tokenIngresso)
                        .orElseThrow(()
                                -> new RuntimeException("Ingresso inválido"));

        if (!participacao.getEventoId().equals(idEvento)) {
            throw new RuntimeException(
                    "Este ingresso pertence a outro evento"
            );
        }

        if ("PRESENTE".equals(participacao.getStatus())) {
            throw new RuntimeException(
                    "Este ingresso já foi utilizado"
            );
        }

        if (!"INSCRITO".equals(participacao.getStatus())) {
            throw new RuntimeException(
                    "Este ingresso não pode ser utilizado"
            );
        }

        participacao.setStatus("PRESENTE");
        participacao.setDataCheckin(
                java.time.LocalDateTime.now()
        );

        return participacaoRepository.save(participacao);
    }
}
