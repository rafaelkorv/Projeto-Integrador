package com.backendpi.backend.repository;

import com.backendpi.backend.model.InteracaoUsuario;
import org.springframework.data.jpa.repository.JpaRepository;

public interface InteracaoUsuarioRepository extends JpaRepository<InteracaoUsuario, Long> {
}