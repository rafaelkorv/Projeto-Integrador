package com.backendpi.backend.service;

import com.backendpi.backend.model.InteracaoUsuario;
import com.backendpi.backend.repository.InteracaoUsuarioRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class InteracaoUsuarioService {

    private final InteracaoUsuarioRepository repository;

    public InteracaoUsuarioService(InteracaoUsuarioRepository repository) {
        this.repository = repository;
    }

    public List<InteracaoUsuario> listar() {
        return repository.findByIdUsuario(AuthenticatedUserContext.requireUserId());
    }
    public InteracaoUsuario salvar(InteracaoUsuario interacao) {
        AuthenticatedUserContext.requireMatches(interacao.getIdUsuario());
        return repository.save(interacao);
    }

    public void deletar(Long id) {
        Long idUsuario = AuthenticatedUserContext.requireUserId();
        InteracaoUsuario interacao = repository.findById(id)
                .orElseThrow(() -> new RuntimeException("Interação não encontrada."));
        if (!idUsuario.equals(interacao.getIdUsuario())) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão para remover esta interação.");
        }
        repository.delete(interacao);
    }
}