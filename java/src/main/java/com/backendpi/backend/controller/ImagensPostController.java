package com.backendpi.backend.controller;

import com.backendpi.backend.model.ImagensPost;
import com.backendpi.backend.service.ImagensPostService;
import com.backendpi.backend.service.PostService;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.web.multipart.MultipartFile;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/imagens-post")
public class ImagensPostController {
    private final ImagensPostService service;
    private final PostService postService;

    public ImagensPostController(ImagensPostService service, PostService postService) {
        this.service = service;
        this.postService = postService;
    }

    @GetMapping
    public List<ImagensPost> listar() { return service.listar(); }

    @GetMapping("/post/{idPost}")
    public List<ImagensPost> listarPorPost(@PathVariable Long idPost) {
        return service.listarPorPost(idPost);
    }

    @PostMapping
    public ImagensPost criar(@RequestBody ImagensPost imagem) { return service.salvar(imagem); }

    @PostMapping(value = "/upload", consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public ResponseEntity<?> upload(
            @RequestParam Long idPost,
            @RequestParam("imagem") MultipartFile imagem) {
        if (postService.buscarPorId(idPost) == null) {
            return ResponseEntity.notFound().build();
        }

        try {
            return ResponseEntity.ok(service.salvarUpload(idPost, imagem));
        } catch (ResponseStatusException erro) {
            throw erro;
        } catch (IllegalArgumentException erro) {
            return ResponseEntity.badRequest().body(erro.getMessage());
        } catch (Exception erro) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Não foi possível salvar a imagem.");
        }
    }

    @DeleteMapping("/{id}")
    public void deletar(@PathVariable Long id) { service.deletar(id); }
}