package com.backendpi.backend.repository;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.backendpi.backend.model.Evento;

@Repository
public interface EventoRepository extends JpaRepository<Evento, Long> {

    @Query("""
        SELECT e
        FROM Evento e
        WHERE
            (:texto IS NULL
             OR LOWER(e.titulo) LIKE LOWER(CONCAT('%', :texto, '%'))
             OR LOWER(e.descricao) LIKE LOWER(CONCAT('%', :texto, '%')))
        AND (:status IS NULL OR e.status = :status)
       AND (
    :categoria IS NULL
    OR LOWER(TRIM(e.categoria)) = LOWER(TRIM(:categoria))
)
        AND (:comunidadeId IS NULL OR e.comunidadeId = :comunidadeId)
        AND (:dataInicio IS NULL OR e.dataEvento >= :dataInicio)
        AND (:dataFim IS NULL OR e.dataEvento <= :dataFim)
        """)
    Page<Evento> buscarComFiltros(
            @Param("texto") String texto,
            @Param("status") String status,
            @Param("categoria") String categoria,
            @Param("comunidadeId") Long comunidadeId,
            @Param("dataInicio") LocalDate dataInicio,
            @Param("dataFim") LocalDate dataFim,
            Pageable pageable
    );

    List<Evento> findByCriadorIdOrderByDataEventoDescHorarioInicioDesc(
            Long criadorId
    );
}
