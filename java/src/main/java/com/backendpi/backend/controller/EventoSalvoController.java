package com.backendpi.backend.controller;

import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.backendpi.backend.model.Evento;
import com.backendpi.backend.service.EventoSalvoService;

@RestController
@RequestMapping("/api/eventos")
public class EventoSalvoController {

    private final EventoSalvoService eventoSalvoService;

    public EventoSalvoController(EventoSalvoService eventoSalvoService) {
        this.eventoSalvoService = eventoSalvoService;
    }

    @GetMapping("/agenda/{idUsuario}")
    public List<Evento> listarAgenda(@PathVariable Long idUsuario) {
        return eventoSalvoService.listarAgenda(idUsuario);
    }

    @GetMapping("/{idEvento}/salvo/usuario/{idUsuario}")
    public boolean estaSalvo(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario) {
        return eventoSalvoService.estaSalvo(idUsuario, idEvento);
    }

    @PostMapping("/{idEvento}/salvo/usuario/{idUsuario}")
    public ResponseEntity<Void> salvar(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario) {
        eventoSalvoService.salvar(idUsuario, idEvento);
        return ResponseEntity.noContent().build();
    }

    @DeleteMapping("/{idEvento}/salvo/usuario/{idUsuario}")
    public ResponseEntity<Void> remover(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario) {
        eventoSalvoService.remover(idUsuario, idEvento);
        return ResponseEntity.noContent().build();
    }
}
