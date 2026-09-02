package com.backendpi.backend.repository;

import com.backendpi.backend.model.VotoComentario;
import org.springframework.data.jpa.repository.JpaRepository;

public interface VotoComentarioRepository extends JpaRepository<VotoComentario, Long> {
}