package com.backendpi.backend.repository;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

import com.backendpi.backend.model.EventoListaEspera;

public interface EventoListaEsperaRepository extends JpaRepository<EventoListaEspera, Long> {

    List<EventoListaEspera> findByEventoIdOrderByAdicionadoEmAscIdAsc(Long eventoId);

    Optional<EventoListaEspera> findByUsuarioIdAndEventoId(Long usuarioId, Long eventoId);

    boolean existsByUsuarioIdAndEventoId(Long usuarioId, Long eventoId);

    void deleteByUsuarioIdAndEventoId(Long usuarioId, Long eventoId);

    void deleteByEventoId(Long eventoId);
}
