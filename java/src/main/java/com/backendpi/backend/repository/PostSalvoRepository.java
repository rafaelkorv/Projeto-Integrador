package com.backendpi.backend.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import com.backendpi.backend.model.PostSalvo;
import java.util.List;

public interface PostSalvoRepository extends JpaRepository<PostSalvo, Long> {
    List<PostSalvo> findByIdUsuario(Long idUsuario);
}