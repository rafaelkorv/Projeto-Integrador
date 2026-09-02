package com.backendpi.backend.repository;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.backendpi.backend.model.Comentario;

public interface ComentarioRepository extends JpaRepository<Comentario, Long> {
    List<Comentario> findByIdPost(Long idPost);
}