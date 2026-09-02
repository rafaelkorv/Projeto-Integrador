package com.backendpi.backend.service;

import com.backendpi.backend.model.InteracaoUsuario;
import com.backendpi.backend.repository.InteracaoUsuarioRepository;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class InteracaoUsuarioService {

    private final InteracaoUsuarioRepository repository;

    public InteracaoUsuarioService(InteracaoUsuarioRepository repository) {
        this.repository = repository;
    }

    public List<InteracaoUsuario> listar() { return repository.findAll(); }
    public InteracaoUsuario salvar(InteracaoUsuario interacao) { return repository.save(interacao); }
    public void deletar(Long id) { repository.deleteById(id); }
}