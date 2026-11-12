package com.backendpi.backend.service;

import org.springframework.stereotype.Service;
import java.util.List;

import com.backendpi.backend.model.Voto;
import com.backendpi.backend.repository.VotoRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;

@Service
public class VotoService {

    private final VotoRepository repository;

    public VotoService(VotoRepository repository) {
        this.repository = repository;
    }

    public List<Voto> listar() {
        return repository.findAll();
    }

    public Voto criar(Voto voto) {
        AuthenticatedUserContext.requireMatches(voto.getIdUsuario());
        return repository.save(voto);
    }

    public void deletar(Long id) {
        Long idUsuario = AuthenticatedUserContext.requireUserId();
        Voto voto = repository.findById(id)
                .orElseThrow(() -> new RuntimeException("Curtida não encontrada."));
        if (!idUsuario.equals(voto.getIdUsuario())) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão para remover esta curtida.");
        }
        repository.delete(voto);
    }
}