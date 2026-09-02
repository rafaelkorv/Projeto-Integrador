package com.backendpi.backend.repository;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

import com.backendpi.backend.model.Conversa;

public interface ConversaRepository extends JpaRepository<Conversa, Long> {

    Optional<Conversa> findByUsuario1_IdUsuarioAndUsuario2_IdUsuario(
            Long usuario1Id,
            Long usuario2Id
    );

    List<Conversa> findByUsuario1_IdUsuarioOrUsuario2_IdUsuario(
            Long usuario1Id,
            Long usuario2Id
    );
}
