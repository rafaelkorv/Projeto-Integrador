package com.backendpi.backend.controller;

import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.backendpi.backend.model.NotificacaoEvento;
import com.backendpi.backend.service.NotificacaoEventoService;

@RestController
@RequestMapping("/api/notificacoes")
public class NotificacaoEventoController {

    private final NotificacaoEventoService notificacaoService;

    public NotificacaoEventoController(NotificacaoEventoService notificacaoService) {
        this.notificacaoService = notificacaoService;
    }

    @GetMapping("/usuario/{idUsuario}")
    public List<NotificacaoEvento> listar(@PathVariable Long idUsuario) {
        return notificacaoService.listar(idUsuario);
    }

    @PutMapping("/{idNotificacao}/lida/usuario/{idUsuario}")
    public ResponseEntity<Void> marcarComoLida(
            @PathVariable Long idNotificacao,
            @PathVariable Long idUsuario) {
        return notificacaoService.marcarComoLida(idUsuario, idNotificacao)
                ? ResponseEntity.noContent().build()
                : ResponseEntity.notFound().build();
    }
}
