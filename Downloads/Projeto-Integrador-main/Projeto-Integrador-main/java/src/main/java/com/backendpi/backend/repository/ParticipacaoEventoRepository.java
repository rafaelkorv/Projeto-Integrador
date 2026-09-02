package com.backendpi.backend.repository;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import com.backendpi.backend.model.ParticipacaoEvento;

public interface ParticipacaoEventoRepository
        extends JpaRepository<ParticipacaoEvento, Long> {

    boolean existsByUsuarioIdAndEventoId(
            Long usuarioId,
            Long eventoId
    );

    List<ParticipacaoEvento> findByEventoId(Long eventoId);

    List<ParticipacaoEvento> findByUsuarioId(Long usuarioId);

    void deleteByUsuarioIdAndEventoId(
            Long usuarioId,
            Long eventoId
    );

    long countByEventoId(Long eventoId);

    Optional<ParticipacaoEvento> findByTokenIngresso(String tokenIngresso);

    @Query("""
    SELECT p.eventoId, COUNT(p)
    FROM ParticipacaoEvento p
    WHERE p.eventoId IN :idsEventos
    GROUP BY p.eventoId
    """)
    List<Object[]> contarParticipantesPorEventos(
            @Param("idsEventos") List<Long> idsEventos
    );
}
