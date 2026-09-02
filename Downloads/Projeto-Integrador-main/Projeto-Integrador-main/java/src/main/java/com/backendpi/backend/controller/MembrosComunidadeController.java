package com.backendpi.backend.controller;

import com.backendpi.backend.model.MembrosComunidade;
import com.backendpi.backend.service.MembrosComunidadeService;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/membros")
public class MembrosComunidadeController {

    private final MembrosComunidadeService service;

    public MembrosComunidadeController(MembrosComunidadeService service) {
        this.service = service;
    }

    @GetMapping
    public List<MembrosComunidade> listar() { return service.listar(); }

    @PostMapping
    public MembrosComunidade criar(@RequestBody MembrosComunidade membro) {
        return service.salvar(membro);
    }

    @DeleteMapping("/{id}")
    public void deletar(@PathVariable Long id) { service.deletar(id); }
}