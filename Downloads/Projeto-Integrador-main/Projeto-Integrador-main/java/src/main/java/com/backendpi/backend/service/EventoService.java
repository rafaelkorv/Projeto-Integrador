package com.backendpi.backend.service;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.stream.Collectors;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import com.backendpi.backend.dto.EventoResumoDTO;
import com.backendpi.backend.model.Evento;
import com.backendpi.backend.repository.EventoRepository;
import com.backendpi.backend.repository.ParticipacaoEventoRepository;

@Service
public class EventoService {

    private final EventoRepository eventoRepository;

    private final ParticipacaoEventoRepository participacaoEventoRepository;

    public EventoService(
            EventoRepository eventoRepository,
            ParticipacaoEventoRepository participacaoEventoRepository) {

        this.eventoRepository = eventoRepository;
        this.participacaoEventoRepository = participacaoEventoRepository;
    }

    public List<Evento> listarTodos() {
        return eventoRepository.findAll();
    }

    public List<Evento> listarPorCriador(Long idUsuario) {
        return eventoRepository
                .findByCriadorIdOrderByDataEventoDescHorarioInicioDesc(
                        idUsuario
                );
    }

    public List<Evento> listarPorParticipante(Long idUsuario) {
        return participacaoEventoRepository.findByUsuarioId(idUsuario)
                .stream()
                .map(participacao -> eventoRepository.findById(participacao.getEventoId()))
                .flatMap(Optional::stream)
                .sorted((primeiro, segundo) -> {
                    int porData = primeiro.getDataEvento().compareTo(segundo.getDataEvento());
                    return porData != 0
                            ? porData
                            : primeiro.getHorarioInicio().compareTo(segundo.getHorarioInicio());
                })
                .collect(Collectors.toList());
    }

    public Optional<Evento> buscarPorId(Long id) {
        return eventoRepository.findById(id);
    }

    public Evento salvar(Evento evento) {

        if (evento.getCriadorId() == null) {
            throw new RuntimeException("O evento precisa ter um criador");
        }

        if (evento.getPrecoIngresso() != null
        && evento.getPrecoIngresso().signum() < 0) {
    throw new RuntimeException(
            "O preço do ingresso não pode ser negativo"
    );
}

        if (evento.getLimiteParticipantes() != null
                && evento.getLimiteParticipantes() <= 0) {
            throw new RuntimeException(
                    "O limite de participantes deve ser maior que zero"
            );
        }

        evento.setStatus("AGENDADO");

        if (evento.getExigeCheckin() == null) {
            evento.setExigeCheckin(false);
        }

        validarDadosEvento(evento, true);

        return eventoRepository.save(evento);
    }

    public void deletar(Long idEvento, Long idUsuario) {

        Evento evento = eventoRepository.findById(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));

        if (!evento.getCriadorId().equals(idUsuario)) {
            throw new RuntimeException(
                    "Usuário sem permissão para excluir este evento"
            );
        }

        eventoRepository.delete(evento);
    }

    public Evento atualizar(
            Long idEvento,
            Long idUsuario,
            Evento novo) {

        Evento evento = eventoRepository.findById(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));

        if (!evento.getCriadorId().equals(idUsuario)) {
            throw new RuntimeException(
                    "Usuário sem permissão para editar este evento"
            );
        }

        if (evento.getPrecoIngresso() != null
        && evento.getPrecoIngresso().signum() < 0) {
    throw new RuntimeException(
            "O preço do ingresso não pode ser negativo"
    );
}

        if (novo.getLimiteParticipantes() != null
                && novo.getLimiteParticipantes() <= 0) {
            throw new RuntimeException(
                    "O limite de participantes deve ser maior que zero"
            );
        }

        evento.setTitulo(novo.getTitulo());
        evento.setDescricao(novo.getDescricao());
        evento.setCategoria(novo.getCategoria());
        evento.setDataEvento(novo.getDataEvento());
        evento.setHorarioInicio(novo.getHorarioInicio());
        evento.setHorarioFim(novo.getHorarioFim());
        evento.setEncerramentoInscricoes(novo.getEncerramentoInscricoes());
        evento.setLocalEvento(novo.getLocalEvento());
        evento.setComunidadeId(novo.getComunidadeId());
        evento.setLimiteParticipantes(novo.getLimiteParticipantes());
        evento.setExigeCheckin(novo.getExigeCheckin());
        validarDadosEvento(evento, false);
        evento.setPrecoIngresso(novo.getPrecoIngresso());

        return eventoRepository.save(evento);
    }

    public Evento cancelar(Long idEvento, Long idUsuario) {

        Evento evento = eventoRepository.findById(idEvento)
                .orElseThrow(() -> new RuntimeException("Evento não encontrado"));

        if (!evento.getCriadorId().equals(idUsuario)) {
            throw new RuntimeException(
                    "Usuário sem permissão para cancelar este evento"
            );
        }

        evento.setStatus("CANCELADO");

        return eventoRepository.save(evento);
    }

    public Page<EventoResumoDTO> buscarComFiltros(
            String texto,
            String status,
            String categoria,
            Long comunidadeId,
            LocalDate dataInicio,
            LocalDate dataFim,
            int pagina,
            int tamanho) {

        if (texto != null && texto.trim().isEmpty()) {
            texto = null;
        }

        if (status != null && status.trim().isEmpty()) {
            status = null;
        }

        if (categoria != null && categoria.trim().isEmpty()) {
            categoria = null;
        }

        Pageable pageable = PageRequest.of(
                pagina,
                tamanho,
                Sort.by("dataEvento")
                        .ascending()
                        .and(Sort.by("horarioInicio").ascending())
        );

        Page<Evento> paginaEventos
                = eventoRepository.buscarComFiltros(
                        texto,
                        status,
                        categoria,
                        comunidadeId,
                        dataInicio,
                        dataFim,
                        pageable
                );

        List<Evento> eventosFiltrados = paginaEventos.getContent();


        List<Long> idsEventos
                = eventosFiltrados.stream()
                        .map(Evento::getId)
                        .collect(Collectors.toList());

        // 3. Mapa: idEvento -> quantidade de participantes
        Map<Long, Long> quantidadePorEvento
                = new HashMap<>();

        // 4. Só consulta participantes se realmente houver eventos
        if (!idsEventos.isEmpty()) {

            List<Object[]> contagens
                    = participacaoEventoRepository
                            .contarParticipantesPorEventos(idsEventos);

            for (Object[] linha : contagens) {

                Long idEvento
                        = ((Number) linha[0]).longValue();

                Long quantidade
                        = ((Number) linha[1]).longValue();

                quantidadePorEvento.put(
                        idEvento,
                        quantidade
                );
            }
        }

        // 5. Transforma cada Evento em EventoResumoDTO
        List<EventoResumoDTO> eventosDTO
                = eventosFiltrados.stream()
                        .map(evento
                                -> new EventoResumoDTO(
                                evento.getId(),
                                evento.getTitulo(),
                                evento.getDescricao(),
                                evento.getCategoria(),
                                evento.getImagemCapa(),
                                evento.getDataEvento(),
                                evento.getHorarioInicio(),
                                evento.getHorarioFim(),
                                evento.getLocalEvento(),
                                evento.getComunidadeId(),
                                evento.getCriadorId(),
                                evento.getLimiteParticipantes(),
                                evento.getPrecoIngresso(),
                                evento.getExigeCheckin(),
                                evento.getStatus(),
                                evento.getEncerramentoInscricoes(),
                                quantidadePorEvento.getOrDefault(
                                        evento.getId(),
                                        0L
                                )
                        )
                        )
                        .collect(Collectors.toList());

        return new PageImpl<>(
                eventosDTO,
                pageable,
                eventosFiltrados.size()
        );
    }

    private void validarDadosEvento(Evento evento, boolean validarDataPassada) {

        if (evento.getDataEvento() == null
                || evento.getHorarioInicio() == null
                || evento.getHorarioFim() == null) {

            throw new RuntimeException(
                    "Data, horário de início e horário de fim são obrigatórios"
            );
        }

        LocalDateTime inicioEvento = LocalDateTime.of(
                evento.getDataEvento(),
                evento.getHorarioInicio()
        );

        LocalDateTime fimEvento = LocalDateTime.of(
                evento.getDataEvento(),
                evento.getHorarioFim()
        );

        if (!fimEvento.isAfter(inicioEvento)) {
            throw new RuntimeException(
                    "O horário de fim deve ser depois do horário de início"
            );
        }

        if (validarDataPassada
                && !inicioEvento.isAfter(LocalDateTime.now())) {

            throw new RuntimeException(
                    "Não é possível criar um evento no passado"
            );
        }

        if (evento.getEncerramentoInscricoes() != null
                && evento.getEncerramentoInscricoes().isAfter(inicioEvento)) {

            throw new RuntimeException(
                    "O encerramento das inscrições não pode acontecer depois do início do evento"
            );
        }
    }
}
