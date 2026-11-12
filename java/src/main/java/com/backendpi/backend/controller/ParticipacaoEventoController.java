package com.backendpi.backend.controller;

import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ContentDisposition;
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
    public ResponseEntity<Void> cancelarParticipacao(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario) {

        service.cancelarParticipacao(
                idEvento,
                idUsuario
        );

        return ResponseEntity.noContent().build();
    }

    @GetMapping("/{idEvento}/lista-espera/usuario/{idUsuario}")
    public boolean estaNaListaEspera(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario) {
        return service.estaNaListaEspera(idEvento, idUsuario);
    }

    @PostMapping("/{idEvento}/lista-espera/{idUsuario}")
    public ResponseEntity<Void> entrarNaListaEspera(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario) {
        service.entrarNaListaEspera(idEvento, idUsuario);
        return ResponseEntity.noContent().build();
    }

    @DeleteMapping("/{idEvento}/lista-espera/{idUsuario}")
    public ResponseEntity<Void> sairDaListaEspera(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario) {
        service.sairDaListaEspera(idEvento, idUsuario);
        return ResponseEntity.noContent().build();
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

    @GetMapping("/{idEvento}/organizador/{idSolicitante}/resumo")
    public ParticipacaoEventoService.ResumoOrganizador resumoOrganizador(
            @PathVariable Long idEvento,
            @PathVariable Long idSolicitante) {
        return service.obterResumoOrganizador(idEvento, idSolicitante);
    }

    @GetMapping("/{idEvento}/organizador/{idSolicitante}/participantes.csv")
    public ResponseEntity<byte[]> exportarParticipantes(
            @PathVariable Long idEvento,
            @PathVariable Long idSolicitante) {
        StringBuilder csv = new StringBuilder("usuario_id,status,data_checkin\n");
        service.exportarParticipantes(idEvento, idSolicitante).forEach(participacao ->
                csv.append(participacao.getUsuarioId()).append(',')
                        .append(participacao.getStatus()).append(',')
                        .append(participacao.getDataCheckin() == null
                                ? "" : participacao.getDataCheckin())
                        .append('\n')
        );
        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION,
                        ContentDisposition.attachment()
                                .filename("participantes-evento-" + idEvento + ".csv")
                                .build().toString())
                .contentType(new MediaType("text", "csv"))
                .body(csv.toString().getBytes(java.nio.charset.StandardCharsets.UTF_8));
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
