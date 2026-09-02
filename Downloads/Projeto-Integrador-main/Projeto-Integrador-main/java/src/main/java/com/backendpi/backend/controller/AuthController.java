package com.backendpi.backend.controller;

import com.backendpi.backend.model.Usuario;
import com.backendpi.backend.service.UsuarioService;

import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/auth")
@CrossOrigin("*")
public class AuthController {

    private final UsuarioService usuarioService;

    public AuthController(UsuarioService usuarioService) {
        this.usuarioService = usuarioService;
    }

    @PostMapping("/login")
    public Usuario login(@RequestBody Usuario usuario) {
        if (usuario == null) {
            return null;
        }
        return usuarioService.login(usuario.getEmail(), usuario.getTelefone(), usuario.getSenha());
    }
}