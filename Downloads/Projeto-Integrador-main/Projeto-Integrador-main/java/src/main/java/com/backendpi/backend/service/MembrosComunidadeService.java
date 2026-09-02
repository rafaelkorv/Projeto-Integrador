package com.backendpi.backend.service;

import com.backendpi.backend.model.MembrosComunidade;
import com.backendpi.backend.repository.MembrosComunidadeRepository;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class MembrosComunidadeService {

    private final MembrosComunidadeRepository repository;

    public MembrosComunidadeService(MembrosComunidadeRepository repository) {
        this.repository = repository;
    }

    public List<MembrosComunidade> listar() { return repository.findAll(); }
    public MembrosComunidade salvar(MembrosComunidade membro) { return repository.save(membro); }
    public void deletar(Long id) { repository.deleteById(id); }
}