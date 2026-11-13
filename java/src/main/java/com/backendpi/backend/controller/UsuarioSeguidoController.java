package com.backendpi.backend.controller;

import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.backendpi.backend.service.UsuarioSeguidoService;

@RestController
@RequestMapping("/usuarios/me/seguindo")
public class UsuarioSeguidoController {

    private final UsuarioSeguidoService usuarioSeguidoService;

    public UsuarioSeguidoController(UsuarioSeguidoService usuarioSeguidoService) {
        this.usuarioSeguidoService = usuarioSeguidoService;
    }

    @GetMapping
    public List<Long> listarSeguindo() {
        return usuarioSeguidoService.listarSeguindo();
    }

    @PutMapping("/{idSeguido}")
    public ResponseEntity<Void> seguir(@PathVariable Long idSeguido) {
        usuarioSeguidoService.seguir(idSeguido);
        return ResponseEntity.noContent().build();
    }

    @DeleteMapping("/{idSeguido}")
    public ResponseEntity<Void> deixarDeSeguir(@PathVariable Long idSeguido) {
        usuarioSeguidoService.deixarDeSeguir(idSeguido);
        return ResponseEntity.noContent().build();
    }
}
