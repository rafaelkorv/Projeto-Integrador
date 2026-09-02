package com.backendpi.backend.service;

import com.backendpi.backend.model.VotoComentario;
import com.backendpi.backend.repository.VotoComentarioRepository;
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
        return repository.save(voto);
    }

    public void deletar(Long id) {
        repository.deleteById(id);
    }
}