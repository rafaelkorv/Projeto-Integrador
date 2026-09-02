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

import com.backendpi.backend.dto.MensagemDTO;
import com.backendpi.backend.service.MensagemService;

@RestController
@RequestMapping("/mensagens")
@CrossOrigin("*")
public class MensagemController {

    private final MensagemService mensagemService;

    public MensagemController(
            MensagemService mensagemService
    ) {
        this.mensagemService = mensagemService;
    }

    @PostMapping
    public ResponseEntity<?> enviar(
            @RequestBody Map<String, Object> dados
    ) {

        try {
            Long idConversa
                    = Long.valueOf(
                            dados.get("idConversa").toString()
                    );

            Long idRemetente
                    = Long.valueOf(
                            dados.get("idRemetente").toString()
                    );

            String conteudo
                    = dados.get("conteudo") != null
                    ? dados.get("conteudo").toString()
                    : null;

            MensagemDTO mensagem
                    = mensagemService.enviarMensagem(
                            idConversa,
                            idRemetente,
                            conteudo
                    );

            return ResponseEntity.ok(mensagem);

        } catch (RuntimeException erro) {

            return ResponseEntity
                    .badRequest()
                    .body(erro.getMessage());
        }
    }

    @GetMapping("/conversa/{idConversa}")
    public List<MensagemDTO> listar(
            @PathVariable Long idConversa
    ) {

        return mensagemService
                .listarMensagens(idConversa);
    }
}
