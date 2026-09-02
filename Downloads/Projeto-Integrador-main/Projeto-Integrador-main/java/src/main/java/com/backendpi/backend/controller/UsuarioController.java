package com.backendpi.backend.controller;

import java.util.List;
import java.util.Map;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
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

import com.backendpi.backend.dto.UsuarioPerfilDTO;
import com.backendpi.backend.model.Usuario;
import com.backendpi.backend.repository.UsuarioRepository;
import com.backendpi.backend.service.GoogleDriveService;
import com.backendpi.backend.service.UsuarioService;

@RestController
@RequestMapping("/usuarios")
@CrossOrigin("*")
public class UsuarioController {

    private final UsuarioRepository usuarioRepository;
    private final UsuarioService usuarioService;
    private final GoogleDriveService googleDriveService;

    public UsuarioController(
            UsuarioRepository usuarioRepository,
            UsuarioService usuarioService,
            GoogleDriveService googleDriveService) {

        this.usuarioRepository = usuarioRepository;
        this.usuarioService = usuarioService;
        this.googleDriveService = googleDriveService;
    }

    @GetMapping
    public List<Usuario> listar() {
        return usuarioRepository.findAll();
    }

    @PostMapping
    public Usuario criar(@RequestBody Usuario usuario) {
        return usuarioService.salvar(usuario);
    }

    @PutMapping("/{id}")
    public Usuario atualizar(
            @PathVariable Long id,
            @RequestBody Usuario usuario
    ) {
        return usuarioService.atualizar(id, usuario);
    }

    @DeleteMapping("/{id}")
    public void deletar(@PathVariable Long id) {
        usuarioRepository.deleteById(id);
    }

    @GetMapping("/{id}")
    public ResponseEntity<UsuarioPerfilDTO> buscarPorId(
            @PathVariable Long id) {

        return usuarioRepository.findById(id)
                .map(usuario -> {

                    UsuarioPerfilDTO perfil
                            = new UsuarioPerfilDTO(
                                    usuario.getIdUsuario(),
                                    usuario.getNome(),
                                    usuario.getUsername(),
                                    usuario.getBio(),
                                    usuario.getFotoPerfil(),
                                    usuario.getInteresses()
                            );

                    return ResponseEntity.ok(perfil);
                })
                .orElse(ResponseEntity.notFound().build());
    }

    @PutMapping("/{id}/perfil")
    public ResponseEntity<?> atualizarPerfil(
            @PathVariable Long id,
            @RequestBody Usuario dadosPerfil) {

        return usuarioRepository.findById(id)
                .map(usuario -> {

                    String novoUsername = dadosPerfil.getUsername();

                    // Username não pode ficar vazio
                    if (novoUsername == null || novoUsername.trim().isEmpty()) {
                        return ResponseEntity
                                .badRequest()
                                .body("Username é obrigatório.");
                    }

                    novoUsername = novoUsername.trim();

                    // Só verifica duplicidade se o username realmente mudou
                    if (!novoUsername.equals(usuario.getUsername())
                            && usuarioRepository.existsByUsername(novoUsername)) {

                        return ResponseEntity
                                .badRequest()
                                .body("Username já está em uso.");
                    }

                    usuario.setNome(dadosPerfil.getNome());
                    usuario.setUsername(novoUsername);
                    usuario.setBio(dadosPerfil.getBio());

                    Usuario atualizado
                            = usuarioRepository.save(usuario);

                    UsuarioPerfilDTO perfil
                            = new UsuarioPerfilDTO(
                                    atualizado.getIdUsuario(),
                                    atualizado.getNome(),
                                    atualizado.getUsername(),
                                    atualizado.getBio(),
                                    atualizado.getFotoPerfil(),
                                    atualizado.getInteresses()
                            );

                    return ResponseEntity.ok(perfil);
                })
                .orElse(ResponseEntity.notFound().build());
    }

    @PostMapping("/login")
    public ResponseEntity<UsuarioPerfilDTO> login(
            @RequestBody Map<String, String> dados) {

        String email = dados.get("email");
        String telefone = dados.get("telefone");
        String senha = dados.get("senha");

        Usuario usuario
                = usuarioService.login(email, telefone, senha);

        if (usuario == null) {
            return ResponseEntity.status(401).build();
        }

        UsuarioPerfilDTO perfil
                = new UsuarioPerfilDTO(
                        usuario.getIdUsuario(),
                        usuario.getNome(),
                        usuario.getUsername(),
                        usuario.getBio(),
                        usuario.getFotoPerfil(),
                        usuario.getInteresses()
                );

        return ResponseEntity.ok(perfil);
    }

    @PostMapping("/{id}/foto")
    public ResponseEntity<?> atualizarFotoPerfil(
            @PathVariable Long id,
            @RequestParam("foto") MultipartFile foto) {

        Usuario usuario = usuarioRepository.findById(id)
                .orElse(null);

        if (usuario == null) {
            return ResponseEntity.notFound().build();
        }

        System.out.println("=== TROCA DE FOTO DE PERFIL (GOOGLE DRIVE) ===");
        System.out.println("Usuario: " + id);
        System.out.println("Arquivo: " + foto.getOriginalFilename());
        System.out.println("Tamanho: " + foto.getSize() + " bytes");

        if (foto.isEmpty()) {
            return ResponseEntity
                    .badRequest()
                    .body(Map.of("success", false, "error", "Selecione uma imagem."));
        }

        String tipo = foto.getContentType();
        System.out.println("TIPO RECEBIDO: " + tipo);

        if (tipo == null || !tipo.startsWith("image/")) {
            return ResponseEntity
                    .badRequest()
                    .body(Map.of("success", false, "error", "O arquivo precisa ser uma imagem."));
        }

        // Limite de 5 MB
        if (foto.getSize() > 5 * 1024 * 1024) {
            return ResponseEntity
                    .badRequest()
                    .body(Map.of("success", false, "error", "A foto deve ter no máximo 5 MB."));
        }

        try {
            // Salva a foto no Google Drive usando a classe GoogleDriveService pronta
            String usernameLimpo = (usuario.getUsername() != null && !usuario.getUsername().isBlank())
                    ? usuario.getUsername().trim()
                    : "usuario";

            String fileId = googleDriveService.salvarFotoPerfil(
                    foto,
                    usuario.getIdUsuario(),
                    usernameLimpo
            );

            usuario.setFotoPerfil(fileId);
            Usuario salvo = usuarioRepository.save(usuario);

            UsuarioPerfilDTO perfil = new UsuarioPerfilDTO(
                    salvo.getIdUsuario(),
                    salvo.getNome(),
                    salvo.getUsername(),
                    salvo.getBio(),
                    salvo.getFotoPerfil(),
                    salvo.getInteresses()
            );

            return ResponseEntity.ok(perfil);

        } catch (Exception erro) {
            erro.printStackTrace();
            return ResponseEntity
                    .internalServerError()
                    .body(Map.of("success", false, "error", "Erro ao salvar foto no Google Drive: " + erro.getMessage()));
        }
    }
}
