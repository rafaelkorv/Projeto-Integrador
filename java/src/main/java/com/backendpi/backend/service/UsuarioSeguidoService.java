package com.backendpi.backend.service;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import com.backendpi.backend.model.UsuarioSeguido;
import com.backendpi.backend.repository.UsuarioRepository;
import com.backendpi.backend.repository.UsuarioSeguidoRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;

@Service
public class UsuarioSeguidoService {

    private final UsuarioSeguidoRepository seguidoRepository;
    private final UsuarioRepository usuarioRepository;

    public UsuarioSeguidoService(
            UsuarioSeguidoRepository seguidoRepository,
            UsuarioRepository usuarioRepository) {
        this.seguidoRepository = seguidoRepository;
        this.usuarioRepository = usuarioRepository;
    }

    public List<Long> listarSeguindo() {
        Long seguidorId = AuthenticatedUserContext.requireUserId();
        return seguidoRepository.findBySeguidorIdOrderBySeguidoEmDesc(seguidorId)
                .stream()
                .map(UsuarioSeguido::getSeguidoId)
                .toList();
    }

    @Transactional
    public void seguir(Long seguidoId) {
        Long seguidorId = AuthenticatedUserContext.requireUserId();
        if (seguidorId.equals(seguidoId)) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Não é possível seguir a si mesmo.");
        }
        if (!usuarioRepository.existsById(seguidoId)) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Usuário não encontrado.");
        }
        if (seguidoRepository.findBySeguidorIdAndSeguidoId(seguidorId, seguidoId).isPresent()) {
            return;
        }

        UsuarioSeguido relacionamento = new UsuarioSeguido();
        relacionamento.setSeguidorId(seguidorId);
        relacionamento.setSeguidoId(seguidoId);
        relacionamento.setSeguidoEm(LocalDateTime.now());
        seguidoRepository.save(relacionamento);
    }

    @Transactional
    public void deixarDeSeguir(Long seguidoId) {
        Long seguidorId = AuthenticatedUserContext.requireUserId();
        seguidoRepository.deleteBySeguidorIdAndSeguidoId(seguidorId, seguidoId);
    }
}
