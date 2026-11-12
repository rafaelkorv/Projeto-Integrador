package com.backendpi.backend.service;

import com.backendpi.backend.model.MembrosComunidade;
import com.backendpi.backend.repository.MembrosComunidadeRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class MembrosComunidadeService {

    private final MembrosComunidadeRepository repository;

    public MembrosComunidadeService(MembrosComunidadeRepository repository) {
        this.repository = repository;
    }

    public List<MembrosComunidade> listar() { return repository.findAll(); }
    public MembrosComunidade salvar(MembrosComunidade membro) {
        AuthenticatedUserContext.requireMatches(membro.getIdUsuario());
        return repository.save(membro);
    }

    public void deletar(Long id) {
        Long idUsuario = AuthenticatedUserContext.requireUserId();
        MembrosComunidade membro = repository.findById(id)
                .orElseThrow(() -> new RuntimeException("Participação na comunidade não encontrada."));
        if (!idUsuario.equals(membro.getIdUsuario())) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão para sair desta comunidade.");
        }
        repository.delete(membro);
    }
}