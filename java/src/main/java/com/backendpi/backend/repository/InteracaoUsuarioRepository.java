package com.backendpi.backend.repository;

import com.backendpi.backend.model.InteracaoUsuario;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface InteracaoUsuarioRepository extends JpaRepository<InteracaoUsuario, Long> {
    List<InteracaoUsuario> findByIdUsuario(Long idUsuario);
}