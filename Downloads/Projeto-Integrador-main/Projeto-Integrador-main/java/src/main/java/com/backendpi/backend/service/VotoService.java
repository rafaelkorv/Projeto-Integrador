package com.backendpi.backend.service;

import org.springframework.stereotype.Service;
import java.util.List;

import com.backendpi.backend.model.Voto;
import com.backendpi.backend.repository.VotoRepository;

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
        return repository.save(voto);
    }

    public void deletar(Long id) {
        repository.deleteById(id);
    }
}