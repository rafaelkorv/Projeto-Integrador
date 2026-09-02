package com.backendpi.backend.controller;

import com.backendpi.backend.model.Comentario;
import com.backendpi.backend.service.ComentarioService;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/comentarios")
@CrossOrigin(origins = "*")
public class ComentarioController {

    private final ComentarioService service;

    public ComentarioController(ComentarioService service) {
        this.service = service;
    }

    @GetMapping
    public List<Comentario> listar() {
        return service.listar();
    }

    @GetMapping("/post/{idPost}")
    public List<Comentario> listarPorPost(@PathVariable Long idPost) {
        return service.listarPorPost(idPost);
    }

    @PostMapping
    public Comentario criar(@RequestBody Comentario comentario) {
        return service.criar(comentario);
    }

    @PutMapping("/{id}")
    public Comentario atualizar(@PathVariable Long id, @RequestBody Comentario novo) {
        return service.atualizar(id, novo);
    }

    @DeleteMapping("/{id}")
    public void deletar(@PathVariable Long id) {
        service.deletar(id);
    }
}