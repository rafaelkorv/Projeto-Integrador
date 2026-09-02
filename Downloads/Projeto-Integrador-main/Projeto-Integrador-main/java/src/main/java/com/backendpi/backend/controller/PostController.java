package com.backendpi.backend.controller;

import java.util.List;

import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.backendpi.backend.model.Post;
import com.backendpi.backend.service.PostService;

@RestController
@RequestMapping("/posts")
@CrossOrigin(origins = "*")
public class PostController {

    private final PostService service;

    public PostController(PostService service) {
        this.service = service;
    }

    @GetMapping
    public Object listar(
            @RequestParam(required = false) Integer page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(required = false) Long idComunidade) {

        if (page != null) {
            PageRequest pageable = PageRequest.of(
                    Math.max(0, page),
                    Math.max(1, size),
                    Sort.by(Sort.Direction.DESC, "idPost")
            );

            if (idComunidade != null) {
                return service.listarPorComunidade(idComunidade, pageable);
            }
            return service.listarPaginado(pageable);
        }

        return service.listar();
    }

    @PostMapping
    public Post salvar(@RequestBody Post post) {
        return service.salvar(post);
    }

    @GetMapping("/{id}")
    public Post buscarPorId(@PathVariable Long id) {
        return service.buscarPorId(id);
    }

    @DeleteMapping("/{idPost}/usuario/{idUsuario}")
    public void deletar(
            @PathVariable Long idPost,
            @PathVariable Long idUsuario) {

        service.deletar(idPost, idUsuario);
    }

    @GetMapping("/usuario/{idUsuario}")
    public Object listarPorUsuario(
            @PathVariable Long idUsuario,
            @RequestParam(required = false) Integer page,
            @RequestParam(defaultValue = "10") int size) {

        if (page != null) {
            PageRequest pageable = PageRequest.of(
                    Math.max(0, page),
                    Math.max(1, size),
                    Sort.by(Sort.Direction.DESC, "idPost")
            );
            return service.listarPorUsuarioPaginado(idUsuario, pageable);
        }

        return service.listarPorUsuario(idUsuario);
    }
}
