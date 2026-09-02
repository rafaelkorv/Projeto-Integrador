package com.backendpi.backend.service;

import org.springframework.stereotype.Service;
import java.util.List;

import com.backendpi.backend.model.PostSalvo;
import com.backendpi.backend.repository.PostSalvoRepository;

@Service
public class PostSalvoService {

    private final PostSalvoRepository repository;

    public PostSalvoService(PostSalvoRepository repository) {
        this.repository = repository;
    }

    public List<PostSalvo> listar() {
        return repository.findAll();
    }

    public PostSalvo salvar(PostSalvo postSalvo) {
        return repository.save(postSalvo);
    }

    public void deletar(Long id) {
        repository.deleteById(id);
    }
}