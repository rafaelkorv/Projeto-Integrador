package com.backendpi.backend.service;

import com.backendpi.backend.model.Comentario;
import com.backendpi.backend.repository.ComentarioRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class ComentarioService {

    private final ComentarioRepository repository;

    public ComentarioService(ComentarioRepository repository) {
        this.repository = repository;
    }

    public List<Comentario> listar() {
        return repository.findAll();
    }

    public List<Comentario> listarPorPost(Long idPost) {
        return repository.findByIdPost(idPost);
    }

    public Comentario criar(Comentario comentario) {
        AuthenticatedUserContext.requireMatches(comentario.getIdUsuario());
        return repository.save(comentario);
    }

    public Comentario atualizar(Long id, Comentario novo) {
        Long idUsuario = AuthenticatedUserContext.requireUserId();
        return repository.findById(id).map(c -> {
            if (!c.getIdUsuario().equals(idUsuario)) {
                throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão para editar este comentário.");
            }
            c.setConteudo(novo.getConteudo());
            return repository.save(c);

        }).orElseThrow(() -> new RuntimeException("Comentário não encontrado"));
    }

    public void deletar(Long id) {
        Long idUsuario = AuthenticatedUserContext.requireUserId();
        Comentario comentario = repository.findById(id)
                .orElseThrow(() -> new RuntimeException("Comentário não encontrado"));
        if (!comentario.getIdUsuario().equals(idUsuario)) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão para excluir este comentário.");
        }
        repository.delete(comentario);
    }
}