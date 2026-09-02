package com.backendpi.backend.repository;

import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

import com.backendpi.backend.model.Post;

public interface PostRepository extends JpaRepository<Post, Long> {

    List<Post> findByIdComunidade(Long idComunidade);
    List<Post> findByIdUsuarioOrderByDataPostagemDesc(Long idUsuario);
    List<Post> findAllByOrderByIdPostDesc();
    
    Page<Post> findAllByOrderByIdPostDesc(Pageable pageable);
    Page<Post> findByIdComunidadeOrderByIdPostDesc(Long idComunidade, Pageable pageable);
    Page<Post> findByIdUsuarioOrderByIdPostDesc(Long idUsuario, Pageable pageable);
}
