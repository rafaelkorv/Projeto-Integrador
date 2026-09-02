package com.backendpi.backend.controller;

import org.springframework.web.bind.annotation.*;
import java.util.List;

import com.backendpi.backend.model.Voto;
import com.backendpi.backend.service.VotoService;

@RestController
@RequestMapping("/votos")
public class VotoController {

    private final VotoService service;

    public VotoController(VotoService service) {
        this.service = service;
    }

    @GetMapping
    public List<Voto> listar() {
        return service.listar();
    }

    @PostMapping
    public Voto criar(@RequestBody Voto voto) {
        return service.criar(voto);
    }

    @DeleteMapping("/{id}")
    public void deletar(@PathVariable Long id) {
        service.deletar(id);
    }
}