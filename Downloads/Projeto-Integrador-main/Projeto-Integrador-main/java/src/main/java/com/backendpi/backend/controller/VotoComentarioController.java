package com.backendpi.backend.controller;

import com.backendpi.backend.model.VotoComentario;
import com.backendpi.backend.service.VotoComentarioService;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/votos-comentario")
public class VotoComentarioController {

    private final VotoComentarioService service;

    public VotoComentarioController(VotoComentarioService service) {
        this.service = service;
    }

    @GetMapping
    public List<VotoComentario> listar() {
        return service.listar();
    }

    @PostMapping
    public VotoComentario criar(@RequestBody VotoComentario voto) {
        return service.criar(voto);
    }

    @DeleteMapping("/{id}")
    public void deletar(@PathVariable Long id) {
        service.deletar(id);
    }
}