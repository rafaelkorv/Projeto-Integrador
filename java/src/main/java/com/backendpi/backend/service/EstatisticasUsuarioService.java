package com.backendpi.backend.service;

import com.backendpi.backend.model.EstatisticasUsuario;
import com.backendpi.backend.repository.EstatisticasUsuarioRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class EstatisticasUsuarioService {
    private final EstatisticasUsuarioRepository repository;

    public EstatisticasUsuarioService(EstatisticasUsuarioRepository repository) {
        this.repository = repository;
    }

    public List<EstatisticasUsuario> listar() {
        return repository.findByIdUsuario(AuthenticatedUserContext.requireUserId());
    }

    public EstatisticasUsuario salvar(EstatisticasUsuario estatistica) {
        AuthenticatedUserContext.requireMatches(estatistica.getIdUsuario());
        return repository.save(estatistica);
    }

    public void deletar(Long id) {
        Long idUsuario = AuthenticatedUserContext.requireUserId();
        EstatisticasUsuario estatistica = repository.findById(id)
                .orElseThrow(() -> new RuntimeException("Estatística não encontrada."));
        if (!idUsuario.equals(estatistica.getIdUsuario())) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão para remover estas estatísticas.");
        }
        repository.delete(estatistica);
    }
}