package com.backendpi.backend.repository;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

import com.backendpi.backend.model.NotificacaoEvento;

public interface NotificacaoEventoRepository extends JpaRepository<NotificacaoEvento, Long> {

    List<NotificacaoEvento> findTop50ByUsuarioIdOrderByCriadoEmDesc(Long usuarioId);

    Optional<NotificacaoEvento> findByIdAndUsuarioId(Long id, Long usuarioId);
}
