package com.backendpi.backend.controller;

import org.springframework.web.bind.annotation.*;
import java.util.List;

import com.backendpi.backend.model.PostSalvo;
import com.backendpi.backend.service.PostSalvoService;

@RestController
@RequestMapping("/posts_salvos")
public class PostSalvoController {

    private final PostSalvoService service;

    public PostSalvoController(PostSalvoService service) {
        this.service = service;
    }

    @GetMapping
    public List<PostSalvo> listar() {
        return service.listar();
    }

    @PostMapping
    public PostSalvo salvar(@RequestBody PostSalvo postSalvo) {
        return service.salvar(postSalvo);
    }

    @DeleteMapping("/{id}")
    public void deletar(@PathVariable Long id) {
        service.deletar(id);
    }
}