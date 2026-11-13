package com.backendpi.backend.controller;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDate;
import java.util.List;
import java.util.UUID;

import org.springframework.data.domain.Page;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.multipart.MultipartFile;

import com.backendpi.backend.dto.EventoResumoDTO;
import com.backendpi.backend.model.Evento;
import com.backendpi.backend.security.AuthenticatedUserContext;
import com.backendpi.backend.service.EventoService;

@RestController
@RequestMapping("/api/eventos")
public class EventoController {

    private final EventoService eventoService;

    public EventoController(EventoService eventoService) {
        this.eventoService = eventoService;
    }

    @GetMapping
    public List<Evento> listarTodos() {
        return eventoService.listarTodos();
    }

    @GetMapping("/usuario/{idUsuario}")
    public List<Evento> listarPorCriador(
            @PathVariable Long idUsuario) {

        return eventoService.listarPorCriador(idUsuario);
    }

    @GetMapping("/participando/{idUsuario}")
    public List<Evento> listarPorParticipante(
            @PathVariable Long idUsuario) {

        return eventoService.listarPorParticipante(idUsuario);
    }

    @GetMapping("/{id}")
    public ResponseEntity<Evento> buscarPorId(@PathVariable Long id) {
        return eventoService.buscarPorId(id)
                .map(evento -> ResponseEntity.ok().body(evento))
                .orElse(ResponseEntity.notFound().build());
    }

    @PostMapping
    public Evento criar(@RequestBody Evento evento) {
        return eventoService.salvar(evento);
    }

    @PostMapping("/{idEvento}/capa")
    public ResponseEntity<Evento> enviarCapa(
            @PathVariable Long idEvento,
            @RequestParam("capa") MultipartFile capa) {
        try {
            if (capa.isEmpty() || capa.getContentType() == null) {
                return ResponseEntity.badRequest().build();
            }

            if (!List.of("image/jpeg", "image/png", "image/webp")
                    .contains(capa.getContentType())) {
                return ResponseEntity.badRequest().build();
            }

            if (capa.getSize() > 5 * 1024 * 1024) {
                return ResponseEntity.badRequest().build();
            }

            Evento evento = eventoService.buscarPorId(idEvento)
                    .orElseThrow(() -> new IllegalArgumentException("Evento não encontrado"));
            AuthenticatedUserContext.requireMatches(evento.getCriadorId());
            Path pasta = Paths.get("uploads", "eventos");
            Files.createDirectories(pasta);
            String extensao = switch (capa.getContentType()) {
                case "image/jpeg" -> ".jpg";
                case "image/png" -> ".png";
                case "image/webp" -> ".webp";
                default -> throw new IllegalArgumentException("Formato de banner inválido");
            };
            String nome = UUID.randomUUID() + extensao;
            Files.copy(capa.getInputStream(), pasta.resolve(nome));
            evento.setImagemCapa("uploads/eventos/" + nome);
            return ResponseEntity.ok(eventoService.salvar(evento));
        } catch (IOException | IllegalArgumentException erro) {
            return ResponseEntity.internalServerError().build();
        }
    }

    @DeleteMapping("/{idEvento}/usuario/{idUsuario}")
    public ResponseEntity<Void> deletar(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario) {

        eventoService.deletar(idEvento, idUsuario);

        return ResponseEntity.noContent().build();
    }

    @PutMapping("/{idEvento}/usuario/{idUsuario}")
    public Evento atualizar(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario,
            @RequestBody Evento novo) {

        return eventoService.atualizar(
                idEvento,
                idUsuario,
                novo
        );
    }

    @PutMapping("/{idEvento}/cancelar/usuario/{idUsuario}")
    public Evento cancelar(
            @PathVariable Long idEvento,
            @PathVariable Long idUsuario) {

        return eventoService.cancelar(idEvento, idUsuario);
    }

    @GetMapping("/buscar")
    public Page<EventoResumoDTO> buscarComFiltros(
            @RequestParam(required = false) String texto,
            @RequestParam(required = false) String categoria,
            @RequestParam(required = false) Long comunidadeId,
            @RequestParam(required = false) Boolean pago,
            @RequestParam(required = false)
            @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dataInicio,
            @RequestParam(required = false)
            @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dataFim,
            @RequestParam(required = false) String formato,
            @RequestParam(required = false) Double lat,
        @RequestParam(required = false) Double lng,
        @RequestParam(required = false, defaultValue = "50") Double raioKm, // Raio padrão: 50km
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "12") int size) {

        return eventoService.buscarComFiltros(
                texto,
                categoria,
                comunidadeId,
                pago,
                dataInicio,
                dataFim,
                formato,
                lat,
                lng,
                raioKm,
                page,
                size
        );
    }
}
