package com.backendpi.backend.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import com.backendpi.backend.model.Voto;

public interface VotoRepository extends JpaRepository<Voto, Long> {
}