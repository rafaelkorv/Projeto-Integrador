package com.backendpi.backend.repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.backendpi.backend.model.Usuario;

public interface UsuarioRepository
        extends JpaRepository<Usuario, Long> {

    boolean existsByEmail(String email);

    Usuario findByEmail(String email);

    Usuario findByTelefone(String telefone);

    Usuario findByEmailAndSenha(
            String email,
            String senha
    );

    Usuario findByTelefoneAndSenha(
            String telefone,
            String senha
    );

    boolean existsByUsername(String username);
    boolean existsByTelefone(String telefone);
}