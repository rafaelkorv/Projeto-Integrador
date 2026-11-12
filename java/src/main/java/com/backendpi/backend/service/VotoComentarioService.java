package com.backendpi.backend.service;

import com.backendpi.backend.model.VotoComentario;
import com.backendpi.backend.repository.VotoComentarioRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class VotoComentarioService {

    private final VotoComentarioRepository repository;

    public VotoComentarioService(VotoComentarioRepository repository) {
        this.repository = repository;
    }

    public List<VotoComentario> listar() {
        return repository.findAll();
    }

    public VotoComentario criar(VotoComentario voto) {
        AuthenticatedUserContext.requireMatches(voto.getIdUsuario());
        return repository.save(voto);
    }

    public void deletar(Long id) {
        Long idUsuario = AuthenticatedUserContext.requireUserId();
        VotoComentario voto = repository.findById(id)
                .orElseThrow(() -> new RuntimeException("Voto no comentário não encontrado."));
        if (!idUsuario.equals(voto.getIdUsuario())) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão para remover este voto.");
        }
        repository.delete(voto);
    }
}