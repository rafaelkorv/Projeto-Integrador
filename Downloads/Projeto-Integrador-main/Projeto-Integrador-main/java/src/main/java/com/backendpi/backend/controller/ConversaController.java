package com.backendpi.backend.controller;

import java.util.List;
import java.util.Map;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.backendpi.backend.dto.ConversaDTO;
import com.backendpi.backend.service.ConversaService;

@RestController
@RequestMapping("/conversas")
@CrossOrigin("*")
public class ConversaController {

    private final ConversaService conversaService;

    public ConversaController(
            ConversaService conversaService
    ) {
        this.conversaService = conversaService;
    }

    @PostMapping
    public ResponseEntity<?> criarOuBuscar(
            @RequestBody Map<String, Long> dados
    ) {

        try {
            Long idUsuario = dados.get("idUsuario");
            Long idOutroUsuario
                    = dados.get("idOutroUsuario");

            ConversaDTO conversa
                    = conversaService.criarOuBuscarConversa(
                            idUsuario,
                            idOutroUsuario
                    );

            return ResponseEntity.ok(conversa);

        } catch (RuntimeException erro) {
            return ResponseEntity
                    .badRequest()
                    .body(erro.getMessage());
        }
    }

    @GetMapping("/usuario/{idUsuario}")
    public List<ConversaDTO> listarPorUsuario(
            @PathVariable Long idUsuario
    ) {

        return conversaService
                .listarConversasDoUsuario(idUsuario);
    }
}
