package com.backendpi.backend.repository;

import com.backendpi.backend.model.MembrosComunidade;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface MembrosComunidadeRepository extends JpaRepository<MembrosComunidade, Long> {
    List<MembrosComunidade> findByIdUsuario(Long idUsuario);
}