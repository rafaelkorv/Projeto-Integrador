package com.backendpi.backend.controller;

import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.backendpi.backend.model.ParticipacaoEvento;
import com.backendpi.backend.service.ParticipacaoEventoService;

@RestController
@RequestMapping("/api/eventos")
@CrossOrigin(origins = "*")
public class ParticipacaoEventoController {

    private final ParticipacaoEventoService service;

    public ParticipacaoEventoController(
            ParticipacaoEventoService service) {

        this.service = service;
    }

    @PostMapping("/{idEvento}/participar/{idUsuario}")
    public ResponseEntity<Void> participar(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario) {

        service.participar(
                idEvento,
                idUsuario
        );

        return ResponseEntity.ok().build();
    }

    @DeleteMapping("/{idEvento}/participar/{idUsuario}")
    public void cancelarParticipacao(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario) {

        service.cancelarParticipacao(
                idEvento,
                idUsuario
        );
    }

    @GetMapping("/{idEvento}/participantes")
    public List<ParticipacaoEvento> listarParticipantes(
            @PathVariable Long idEvento) {

        return service.listarPorEvento(idEvento);
    }

    @GetMapping("/{idEvento}/participantes/quantidade")
    public long contarParticipantes(
            @PathVariable Long idEvento) {

        return service.contarParticipantes(idEvento);
    }

    @DeleteMapping("/{idEvento}/participantes/{idParticipante}/usuario/{idSolicitante}")
    public void removerParticipante(
            @PathVariable Long idEvento,
            @PathVariable Long idParticipante,
            @PathVariable Long idSolicitante) {

        service.removerParticipante(
                idEvento,
                idParticipante,
                idSolicitante
        );
    }

    @PutMapping("/{idEvento}/validar-ingresso/usuario/{idSolicitante}")
    public ParticipacaoEvento validarIngresso(
            @PathVariable Long idEvento,
            @PathVariable Long idSolicitante,
            @RequestBody String tokenIngresso) {

        return service.validarIngresso(
                idEvento,
                idSolicitante,
                tokenIngresso
        );
    }
}
