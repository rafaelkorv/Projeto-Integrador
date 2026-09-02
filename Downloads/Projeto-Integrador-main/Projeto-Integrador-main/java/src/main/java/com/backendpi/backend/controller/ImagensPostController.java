package com.backendpi.backend.controller;

import com.backendpi.backend.model.ImagensPost;
import com.backendpi.backend.service.ImagensPostService;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/imagens-post")
public class ImagensPostController {
    private final ImagensPostService service;

    public ImagensPostController(ImagensPostService service) {
        this.service = service;
    }

    @GetMapping
    public List<ImagensPost> listar() { return service.listar(); }

    @PostMapping
    public ImagensPost criar(@RequestBody ImagensPost imagem) { return service.salvar(imagem); }

    @DeleteMapping("/{id}")
    public void deletar(@PathVariable Long id) { service.deletar(id); }
}