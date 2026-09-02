    package com.backendpi.backend.controller;

    import java.util.List;

    import org.springframework.web.bind.annotation.DeleteMapping;
    import org.springframework.web.bind.annotation.GetMapping;
    import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.backendpi.backend.model.InteracaoUsuario;
import com.backendpi.backend.service.InteracaoUsuarioService;

    @RestController
    @RequestMapping("/interacoes")
    public class InteracaoUsuarioController {

        private final InteracaoUsuarioService service;

        public InteracaoUsuarioController(InteracaoUsuarioService service) {
            this.service = service;
        }

        @GetMapping
        public List<InteracaoUsuario> listar() { return service.listar(); }

        @PostMapping
        public InteracaoUsuario criar(@RequestBody InteracaoUsuario interacao) {
            return service.salvar(interacao);
        }

        @DeleteMapping("/{id}")
        public void deletar(@PathVariable Long id) { service.deletar(id); }
    }