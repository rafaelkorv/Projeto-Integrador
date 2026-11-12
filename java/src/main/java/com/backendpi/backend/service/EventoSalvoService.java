package com.backendpi.backend.service;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.backendpi.backend.model.Evento;
import com.backendpi.backend.model.EventoSalvo;
import com.backendpi.backend.repository.EventoRepository;
import com.backendpi.backend.repository.EventoSalvoRepository;
import com.backendpi.backend.repository.UsuarioRepository;

@Service
public class EventoSalvoService {

    private final EventoSalvoRepository eventoSalvoRepository;
    private final EventoRepository eventoRepository;
    private final UsuarioRepository usuarioRepository;

    public EventoSalvoService(
            EventoSalvoRepository eventoSalvoRepository,
            EventoRepository eventoRepository,
            UsuarioRepository usuarioRepository) {
        this.eventoSalvoRepository = eventoSalvoRepository;
        this.eventoRepository = eventoRepository;
        this.usuarioRepository = usuarioRepository;
    }

    public List<Evento> listarAgenda(Long usuarioId) {
        return eventoSalvoRepository.findByUsuarioIdOrderBySalvoEmDesc(usuarioId)
                .stream()
                .map(item -> eventoRepository.findById(item.getEventoId()).orElse(null))
                .filter(evento -> evento != null)
                .toList();
    }

    @Transactional
    public void salvar(Long usuarioId, Long eventoId) {
        if (!usuarioRepository.existsById(usuarioId)) {
            throw new IllegalArgumentException("Usuário não encontrado");
        }
        eventoRepository.findById(eventoId)
                .orElseThrow(() -> new IllegalArgumentException("Evento não encontrado"));
        if (eventoSalvoRepository.findByUsuarioIdAndEventoId(usuarioId, eventoId).isPresent()) {
            return;
        }

        EventoSalvo salvo = new EventoSalvo();
        salvo.setUsuarioId(usuarioId);
        salvo.setEventoId(eventoId);
        salvo.setSalvoEm(LocalDateTime.now());
        eventoSalvoRepository.save(salvo);
    }

    @Transactional
    public void remover(Long usuarioId, Long eventoId) {
        eventoSalvoRepository.findByUsuarioIdAndEventoId(usuarioId, eventoId)
                .ifPresent(eventoSalvoRepository::delete);
    }

    public boolean estaSalvo(Long usuarioId, Long eventoId) {
        return eventoSalvoRepository.findByUsuarioIdAndEventoId(usuarioId, eventoId).isPresent();
    }
}
