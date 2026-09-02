package com.backendpi.backend.service;

import java.util.List;
import java.util.Optional;

import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import com.backendpi.backend.model.Usuario;
import com.backendpi.backend.repository.UsuarioRepository;

@Service
public class UsuarioService {

    private final UsuarioRepository repository;
    private final PasswordEncoder passwordEncoder;

    public UsuarioService(UsuarioRepository repository, PasswordEncoder passwordEncoder) {
        this.repository = repository;
        this.passwordEncoder = passwordEncoder;
    }

    public List<Usuario> listar() {
        return repository.findAll();
    }

    public Usuario salvar(Usuario usuario) {
        if ((usuario.getEmail() == null
                || usuario.getEmail().trim().isEmpty())
                && (usuario.getTelefone() == null
                || usuario.getTelefone().trim().isEmpty())) {

            throw new RuntimeException(
                    "Informe pelo menos um e-mail ou telefone"
            );
        }

        if (usuario.getEmail() != null
                && !usuario.getEmail().trim().isEmpty()
                && repository.existsByEmail(usuario.getEmail())) {

            throw new RuntimeException("Email já cadastrado");
        }

        if (repository.existsByUsername(usuario.getUsername())) {
            throw new RuntimeException(
                    "Nome de usuário já cadastrado"
            );
        }

        if (usuario.getTelefone() != null
                && !usuario.getTelefone().trim().isEmpty()
                && repository.existsByTelefone(usuario.getTelefone())) {

            throw new RuntimeException(
                    "Telefone já cadastrado"
            );
        }

        if (usuario.getSenha() != null && !usuario.getSenha().trim().isEmpty()) {
            if (!isBcryptHash(usuario.getSenha())) {
                usuario.setSenha(passwordEncoder.encode(usuario.getSenha()));
            }
        }

        return repository.save(usuario);
    }

    public Usuario atualizar(Long id, Usuario dados) {
        return repository.findById(id)
                .map(existente -> {
                    if (dados.getNome() != null) existente.setNome(dados.getNome());
                    if (dados.getNomeCompleto() != null) existente.setNomeCompleto(dados.getNomeCompleto());
                    if (dados.getUsername() != null) existente.setUsername(dados.getUsername());
                    if (dados.getEmail() != null) existente.setEmail(dados.getEmail());
                    if (dados.getTelefone() != null) existente.setTelefone(dados.getTelefone());
                    if (dados.getBio() != null) existente.setBio(dados.getBio());
                    if (dados.getFotoPerfil() != null) existente.setFotoPerfil(dados.getFotoPerfil());
                    if (dados.getDataNascimento() != null) existente.setDataNascimento(dados.getDataNascimento());
                    if (dados.getInteresses() != null) existente.setInteresses(dados.getInteresses());

                    if (dados.getSenha() != null && !dados.getSenha().trim().isEmpty()) {
                        if (!isBcryptHash(dados.getSenha())) {
                            existente.setSenha(passwordEncoder.encode(dados.getSenha()));
                        } else {
                            existente.setSenha(dados.getSenha());
                        }
                    }

                    return repository.save(existente);
                })
                .orElseThrow(() -> new RuntimeException("Usuário não encontrado com id: " + id));
    }

    public void deletar(Long id) {
        repository.deleteById(id);
    }

    public Usuario login(String email, String telefone, String senha) {
        if (senha == null || senha.isEmpty()) {
            return null;
        }

        Usuario usuario = null;
        if (email != null && !email.trim().isEmpty()) {
            usuario = repository.findByEmail(email.trim());
        } else if (telefone != null && !telefone.trim().isEmpty()) {
            usuario = repository.findByTelefone(telefone.trim());
        }

        if (usuario == null || usuario.getSenha() == null) {
            return null;
        }

        // 1. Verifica hash BCrypt
        if (passwordEncoder.matches(senha, usuario.getSenha())) {
            return usuario;
        }

        // 2. Compatibilidade legada: se a senha no banco era texto puro, valida e faz upgrade para BCrypt
        if (usuario.getSenha().equals(senha)) {
            usuario.setSenha(passwordEncoder.encode(senha));
            return repository.save(usuario);
        }

        return null;
    }

    public Optional<Usuario> buscarPorId(Long id) {
        return repository.findById(id);
    }

    private boolean isBcryptHash(String senha) {
        return senha != null && (senha.startsWith("$2a$") || senha.startsWith("$2b$") || senha.startsWith("$2y$")) && senha.length() == 60;
    }
}
