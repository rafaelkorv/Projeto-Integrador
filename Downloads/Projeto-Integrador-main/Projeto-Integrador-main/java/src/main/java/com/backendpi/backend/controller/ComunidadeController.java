package com.backendpi.backend.controller;

import java.util.List;
import java.util.Map;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.UUID;

import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.multipart.MultipartFile;

import com.backendpi.backend.model.Comunidade;
import com.backendpi.backend.service.ComunidadeService;

@RestController
@RequestMapping("/comunidades")
@CrossOrigin(origins = "*") // Muito importante para não dar erro de conexão com o JS!
public class ComunidadeController {

    private final ComunidadeService service;

    public ComunidadeController(ComunidadeService service) {
        this.service = service;
    }

    @GetMapping
    public List<Comunidade> listar() {
        return service.listar();
    }

    @GetMapping("/usuario/{idUsuario}")
    public List<Comunidade> listarPorUsuario(
            @PathVariable Long idUsuario) {

        return service.listarPorUsuario(idUsuario);
    }

    @PostMapping
    public Comunidade criar(@RequestBody Map<String, Object> dados) {
        return service.criar(dados);
    }

    @PostMapping("/{idComunidade}/imagem")
    public ResponseEntity<Comunidade> enviarImagem(
            @PathVariable Long idComunidade,
            @RequestParam("imagem") MultipartFile imagem) {
        try {
            if (imagem.isEmpty() || imagem.getContentType() == null
                    || !imagem.getContentType().startsWith("image/")) {
                return ResponseEntity.badRequest().build();
            }
            Comunidade comunidade = service.buscarPorId(idComunidade);
            Path pasta = Paths.get("uploads", "comunidades");
            Files.createDirectories(pasta);
            String nome = UUID.randomUUID() + "-" +
                    (imagem.getOriginalFilename() == null ? "imagem" : imagem.getOriginalFilename());
            Files.copy(imagem.getInputStream(), pasta.resolve(nome));
            comunidade.setImagemComunidade("uploads/comunidades/" + nome);
            return ResponseEntity.ok(service.salvar(comunidade));
        } catch (IOException | IllegalArgumentException erro) {
            return ResponseEntity.internalServerError().build();
        }
    }

    @PutMapping("/{idComunidade}/usuario/{idUsuario}")
    public Comunidade atualizar(
            @PathVariable Long idComunidade,
            @PathVariable Long idUsuario,
            @RequestBody Comunidade nova) {

        return service.atualizar(idComunidade, idUsuario, nova);
    }

    @DeleteMapping("/{idComunidade}/usuario/{idUsuario}")
    public void deletar(
            @PathVariable Long idComunidade,
            @PathVariable Long idUsuario) {

        service.deletar(idComunidade, idUsuario);
    }

    // entrada na comunidade
    @PostMapping("/{idComunidade}/participar/{idUsuario}")
    public void participarDaComunidade(@PathVariable Long idComunidade, @PathVariable Long idUsuario) {
        service.adicionarMembro(idComunidade, idUsuario);
    }

    @DeleteMapping("/{idComunidade}/membros/{idMembro}/usuario/{idSolicitante}")
    public void removerMembro(
            @PathVariable Long idComunidade,
            @PathVariable Long idMembro,
            @PathVariable Long idSolicitante) {

        service.removerMembro(
                idComunidade,
                idMembro,
                idSolicitante
        );
    }
}
