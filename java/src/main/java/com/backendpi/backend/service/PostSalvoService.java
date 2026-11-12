package com.backendpi.backend.service;

import org.springframework.stereotype.Service;
import java.util.List;

import com.backendpi.backend.model.PostSalvo;
import com.backendpi.backend.repository.PostSalvoRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;

@Service
public class PostSalvoService {

    private final PostSalvoRepository repository;

    public PostSalvoService(PostSalvoRepository repository) {
        this.repository = repository;
    }

    public List<PostSalvo> listar() {
        return repository.findByIdUsuario(AuthenticatedUserContext.requireUserId());
    }

    public PostSalvo salvar(PostSalvo postSalvo) {
        AuthenticatedUserContext.requireMatches(postSalvo.getIdUsuario());
        return repository.save(postSalvo);
    }

    public void deletar(Long id) {
        Long idUsuario = AuthenticatedUserContext.requireUserId();
        PostSalvo salvo = repository.findById(id)
                .orElseThrow(() -> new RuntimeException("Publicação salva não encontrada."));
        if (!idUsuario.equals(salvo.getIdUsuario())) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão para remover este favorito.");
        }
        repository.delete(salvo);
    }
}