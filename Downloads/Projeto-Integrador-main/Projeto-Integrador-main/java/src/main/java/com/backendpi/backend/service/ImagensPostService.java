package com.backendpi.backend.service;

import com.backendpi.backend.model.ImagensPost;
import com.backendpi.backend.repository.ImagensPostRepository;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class ImagensPostService {
    private final ImagensPostRepository repository;

    public ImagensPostService(ImagensPostRepository repository) {
        this.repository = repository;
    }

    public List<ImagensPost> listar() { return repository.findAll(); }
    public ImagensPost salvar(ImagensPost imagem) { return repository.save(imagem); }
    public void deletar(Long id) { repository.deleteById(id); }
}