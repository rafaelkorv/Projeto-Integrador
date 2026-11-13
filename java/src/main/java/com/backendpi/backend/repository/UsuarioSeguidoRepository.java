package com.backendpi.backend.repository;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

import com.backendpi.backend.model.UsuarioSeguido;

public interface UsuarioSeguidoRepository extends JpaRepository<UsuarioSeguido, Long> {

    List<UsuarioSeguido> findBySeguidorIdOrderBySeguidoEmDesc(Long seguidorId);

    Optional<UsuarioSeguido> findBySeguidorIdAndSeguidoId(Long seguidorId, Long seguidoId);

    void deleteBySeguidorIdAndSeguidoId(Long seguidorId, Long seguidoId);
}
