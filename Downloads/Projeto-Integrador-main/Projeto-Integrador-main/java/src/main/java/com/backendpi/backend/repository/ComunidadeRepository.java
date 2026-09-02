package com.backendpi.backend.repository;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import com.backendpi.backend.model.Comunidade;

public interface ComunidadeRepository extends JpaRepository<Comunidade, Long> {

    @Query("""
        SELECT c
        FROM Comunidade c
        JOIN c.membros m
        WHERE m.idUsuario = :idUsuario
        ORDER BY c.nome
        """)
    List<Comunidade> findByMembroId(
            @Param("idUsuario") Long idUsuario
    );
}