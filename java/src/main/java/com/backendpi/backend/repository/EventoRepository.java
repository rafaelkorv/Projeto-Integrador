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
        AND (
            :pago IS NULL
            OR (:pago = TRUE AND e.precoIngresso > 0)
            OR (:pago = FALSE AND (e.precoIngresso IS NULL OR e.precoIngresso <= 0))
        )
        AND (:dataInicio IS NULL OR e.dataEvento >= :dataInicio)
        AND (:dataFim IS NULL OR e.dataEvento <= :dataFim)
        AND (
               :lat IS NULL OR :lng IS NULL OR :raioKm IS NULL OR
               (6371 * acos(
                   cos(radians(:lat)) * cos(radians(e.latitude)) *
                   cos(radians(e.longitude) - radians(:lng)) +
                   sin(radians(:lat)) * sin(radians(e.latitude))
               )) <= :raioKm
          )
        ORDER BY
            CASE
                WHEN e.dataEvento > CURRENT_DATE
                    OR (e.dataEvento = CURRENT_DATE AND e.horarioInicio >= CURRENT_TIME)
                THEN 0
                ELSE 1
            END ASC,
            CASE
                WHEN e.dataEvento > CURRENT_DATE
                    OR (e.dataEvento = CURRENT_DATE AND e.horarioInicio >= CURRENT_TIME)
                THEN e.dataEvento
            END ASC,
            CASE
                WHEN e.dataEvento > CURRENT_DATE
                    OR (e.dataEvento = CURRENT_DATE AND e.horarioInicio >= CURRENT_TIME)
                THEN e.horarioInicio
            END ASC,
            CASE
                WHEN e.dataEvento < CURRENT_DATE
                    OR (e.dataEvento = CURRENT_DATE AND e.horarioInicio < CURRENT_TIME)
                THEN e.dataEvento
            END DESC,
            CASE
                WHEN e.dataEvento < CURRENT_DATE
                    OR (e.dataEvento = CURRENT_DATE AND e.horarioInicio < CURRENT_TIME)
                THEN e.horarioInicio
            END DESC
        """)
    Page<Evento> buscarComFiltros(
            @Param("texto") String texto,
            @Param("status") String status,
            @Param("categoria") String categoria,
            @Param("comunidadeId") Long comunidadeId,
            @Param("pago") Boolean pago,
            @Param("dataInicio") LocalDate dataInicio,
            @Param("dataFim") LocalDate dataFim,
            @Param("lat") Double lat,
        @Param("lng") Double lng,
        @Param("raioKm") Double raioKm,
            Pageable pageable
    );

    List<Evento> findByCriadorIdOrderByDataEventoDescHorarioInicioDesc(
            Long criadorId
    );
}