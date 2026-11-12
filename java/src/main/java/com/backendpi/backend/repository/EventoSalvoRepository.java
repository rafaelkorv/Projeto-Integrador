package com.backendpi.backend.repository;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

import com.backendpi.backend.model.EventoSalvo;

public interface EventoSalvoRepository extends JpaRepository<EventoSalvo, Long> {

    List<EventoSalvo> findByUsuarioIdOrderBySalvoEmDesc(Long usuarioId);

    Optional<EventoSalvo> findByUsuarioIdAndEventoId(Long usuarioId, Long eventoId);
}
