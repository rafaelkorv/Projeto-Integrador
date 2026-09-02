package com.backendpi.backend.controller;

import com.backendpi.backend.model.EstatisticasUsuario;
import com.backendpi.backend.service.EstatisticasUsuarioService;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/estatisticas")
public class EstatisticasUsuarioController {
    private final EstatisticasUsuarioService service;

    public EstatisticasUsuarioController(EstatisticasUsuarioService service) {
        this.service = service;
    }

    @GetMapping
    public List<EstatisticasUsuario> listar() { return service.listar(); }

    @PostMapping
    public EstatisticasUsuario criar(@RequestBody EstatisticasUsuario estatistica) {
        return service.salvar(estatistica);
    }

    @DeleteMapping("/{id}")
    public void deletar(@PathVariable Long id) { service.deletar(id); }
}