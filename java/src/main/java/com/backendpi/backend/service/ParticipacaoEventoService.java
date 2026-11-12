package com.backendpi.backend.service;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;

import com.backendpi.backend.model.Evento;
import com.backendpi.backend.model.EventoListaEspera;
import com.backendpi.backend.model.ParticipacaoEvento;
import com.backendpi.backend.repository.EventoRepository;
import com.backendpi.backend.repository.EventoListaEsperaRepository;
import com.backendpi.backend.repository.ParticipacaoEventoRepository;
import com.backendpi.backend.repository.UsuarioRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;

@Service
public class ParticipacaoEventoService {

    private final ParticipacaoEventoRepository participacaoRepository;
    private final EventoRepository eventoRepository;
    private final UsuarioRepository usuarioRepository;
    private final EventoListaEsperaRepository listaEsperaRepository;
    private final NotificacaoEventoService notificacaoService;

    public ParticipacaoEventoService(
            ParticipacaoEventoRepository participacaoRepository,
            EventoRepository eventoRepository,
            UsuarioRepository usuarioRepository,
            EventoListaEsperaRepository listaEsperaRepository,
            NotificacaoEventoService notificacaoService) {

        this.participacaoRepository = participacaoRepository;
        this.eventoRepository = eventoRepository;
        this.usuarioRepository = usuarioRepository;
        this.listaEsperaRepository = listaEsperaRepository;
        this.notificacaoService = notificacaoService;
    }

    @Transactional
    public ParticipacaoEvento participar(Long idEvento, Long idUsuario) {
        AuthenticatedUserContext.requireMatches(idUsuario);

        Evento evento = eventoRepository.findByIdForUpdate(idEvento)
                .orElseThrow(()
                        -> new RuntimeException("Evento não encontrado"));

        validarEventoAberto(evento);

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
        listaEsperaRepository.deleteByUsuarioIdAndEventoId(idUsuario, idEvento);

        String token = UUID.randomUUID().toString();
        participacao.setTokenIngresso(token);

        return participacaoRepository.save(participacao);
    }

    @Transactional
    public EventoListaEspera entrarNaListaEspera(Long idEvento, Long idUsuario) {
        AuthenticatedUserContext.requireMatches(idUsuario);
        Evento evento = eventoRepository.findByIdForUpdate(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));
        validarEventoAberto(evento);

        if (!usuarioRepository.existsById(idUsuario)) {
            throw new RuntimeException("Usuário não encontrado");
        }
        if (participacaoRepository.existsByUsuarioIdAndEventoId(idUsuario, idEvento)) {
            throw new RuntimeException("Você já está participando deste evento");
        }
        if (evento.getLimiteParticipantes() == null
                || participacaoRepository.countByEventoId(idEvento) < evento.getLimiteParticipantes()) {
            throw new RuntimeException("Ainda há vagas disponíveis neste evento");
        }
        if (listaEsperaRepository.existsByUsuarioIdAndEventoId(idUsuario, idEvento)) {
            throw new RuntimeException("Você já está na lista de espera");
        }

        EventoListaEspera entrada = new EventoListaEspera();
        entrada.setEventoId(idEvento);
        entrada.setUsuarioId(idUsuario);
        entrada.setAdicionadoEm(LocalDateTime.now());
        return listaEsperaRepository.save(entrada);
    }

    public boolean estaNaListaEspera(Long idEvento, Long idUsuario) {
        AuthenticatedUserContext.requireMatches(idUsuario);
        return listaEsperaRepository.existsByUsuarioIdAndEventoId(idUsuario, idEvento);
    }

    @Transactional
    public void sairDaListaEspera(Long idEvento, Long idUsuario) {
        AuthenticatedUserContext.requireMatches(idUsuario);
        listaEsperaRepository.deleteByUsuarioIdAndEventoId(idUsuario, idEvento);
    }

    @Transactional
    public void cancelarParticipacao(
            Long idEvento,
            Long idUsuario) {
        AuthenticatedUserContext.requireMatches(idUsuario);

        eventoRepository.findByIdForUpdate(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));
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
        promoverProximoDaFila(idEvento);
    }

    public List<ParticipacaoEvento> listarPorEvento(Long idEvento) {
        Evento evento = eventoRepository.findById(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));
        if (!evento.getCriadorId().equals(AuthenticatedUserContext.requireUserId())) {
            throw new ResponseStatusException(
                    HttpStatus.FORBIDDEN,
                    "Usuário sem permissão para consultar os participantes."
            );
        }

        return participacaoRepository.findByEventoId(idEvento);
    }

    public long contarParticipantes(Long idEvento) {
        return participacaoRepository.countByEventoId(idEvento);
    }

    public ResumoOrganizador obterResumoOrganizador(Long idEvento, Long idSolicitante) {
        AuthenticatedUserContext.requireMatches(idSolicitante);
        Evento evento = eventoRepository.findById(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));
        if (!evento.getCriadorId().equals(idSolicitante)) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão para consultar este evento");
        }

        List<ParticipacaoEvento> participantes = participacaoRepository.findByEventoId(idEvento);
        long presentes = participantes.stream()
                .filter(participacao -> "PRESENTE".equals(participacao.getStatus()))
                .count();
        long aguardando = listaEsperaRepository
                .findByEventoIdOrderByAdicionadoEmAscIdAsc(idEvento).size();
        return new ResumoOrganizador(
                participantes.size(),
                presentes,
                aguardando,
                evento.getLimiteParticipantes()
        );
    }

    public List<ParticipacaoEvento> exportarParticipantes(Long idEvento, Long idSolicitante) {
        obterResumoOrganizador(idEvento, idSolicitante);
        return participacaoRepository.findByEventoId(idEvento);
    }

    @Transactional
    public void promoverVagasDisponiveis(Long idEvento) {
        Evento evento = eventoRepository.findByIdForUpdate(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));
        if (evento.getLimiteParticipantes() == null
                || !podePromoverDaFila(evento)) {
            return;
        }
        while (participacaoRepository.countByEventoId(idEvento) < evento.getLimiteParticipantes()
                && !listaEsperaRepository
                        .findByEventoIdOrderByAdicionadoEmAscIdAsc(idEvento).isEmpty()) {
            promoverProximoDaFila(idEvento);
        }
    }

    public record ResumoOrganizador(
            long inscritos,
            long presentes,
            long listaEspera,
            Integer limiteVagas) {
    }

    @Transactional
    public void removerParticipante(
            Long idEvento,
            Long idParticipante,
            Long idSolicitante) {
        AuthenticatedUserContext.requireMatches(idSolicitante);

        Evento evento = eventoRepository.findByIdForUpdate(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));

        if (!evento.getCriadorId().equals(idSolicitante)) {
            throw new ResponseStatusException(
                    HttpStatus.FORBIDDEN,
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
        notificacaoService.notificarUsuario(
                idParticipante,
                idEvento,
                "INSCRICAO_REMOVIDA",
                "Sua inscrição foi removida pelo organizador."
        );
        promoverProximoDaFila(idEvento);
    }

    private void validarEventoAberto(Evento evento) {
        LocalDateTime agora = LocalDateTime.now();
        LocalDateTime fimEvento = LocalDateTime.of(
                evento.getDataEvento(),
                evento.getHorarioFim()
        );
        if ("CANCELADO".equals(evento.getStatus())) {
            throw new RuntimeException("Não é possível participar de um evento cancelado");
        }
        if (!agora.isBefore(fimEvento)) {
            throw new RuntimeException("Não é possível participar de um evento encerrado");
        }

        LocalDateTime limiteInscricao = evento.getEncerramentoInscricoes();
        if (limiteInscricao == null) {
            limiteInscricao = LocalDateTime.of(
                    evento.getDataEvento(),
                    evento.getHorarioInicio()
            );
        }
        if (agora.isAfter(limiteInscricao)) {
            throw new RuntimeException("As inscrições para este evento já foram encerradas");
        }
    }

    private void promoverProximoDaFila(Long idEvento) {
        Evento evento = eventoRepository.findById(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));
        if (evento.getLimiteParticipantes() == null
                || participacaoRepository.countByEventoId(idEvento) >= evento.getLimiteParticipantes()
                || !podePromoverDaFila(evento)) {
            return;
        }

        List<EventoListaEspera> fila =
                listaEsperaRepository.findByEventoIdOrderByAdicionadoEmAscIdAsc(idEvento);
        if (fila.isEmpty()) {
            return;
        }

        EventoListaEspera proximo = fila.get(0);
        if (participacaoRepository.existsByUsuarioIdAndEventoId(
                proximo.getUsuarioId(), idEvento)) {
            listaEsperaRepository.delete(proximo);
            promoverProximoDaFila(idEvento);
            return;
        }

        ParticipacaoEvento participacao = new ParticipacaoEvento();
        participacao.setUsuarioId(proximo.getUsuarioId());
        participacao.setEventoId(idEvento);
        participacao.setStatus("INSCRITO");
        participacao.setTokenIngresso(UUID.randomUUID().toString());
        participacaoRepository.save(participacao);
        listaEsperaRepository.delete(proximo);
        notificacaoService.notificarUsuario(
                proximo.getUsuarioId(),
                idEvento,
                "VAGA_DISPONIVEL",
                "Uma vaga foi liberada e você foi promovido da lista de espera."
        );
    }

    private boolean podePromoverDaFila(Evento evento) {
        if ("CANCELADO".equals(evento.getStatus())) {
            return false;
        }
        LocalDateTime agora = LocalDateTime.now();
        LocalDateTime inicio = LocalDateTime.of(
                evento.getDataEvento(),
                evento.getHorarioInicio()
        );
        LocalDateTime fim = LocalDateTime.of(
                evento.getDataEvento(),
                evento.getHorarioFim()
        );
        LocalDateTime limiteInscricao = evento.getEncerramentoInscricoes() == null
                ? inicio
                : evento.getEncerramentoInscricoes();
        return agora.isBefore(fim) && !agora.isAfter(limiteInscricao);
    }

    public ParticipacaoEvento validarIngresso(
            Long idEvento,
            Long idSolicitante,
            String tokenIngresso) {
        AuthenticatedUserContext.requireMatches(idSolicitante);

        Evento evento = eventoRepository.findById(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));

        if (!evento.getCriadorId().equals(idSolicitante)) {
            throw new ResponseStatusException(
                    HttpStatus.FORBIDDEN,
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
