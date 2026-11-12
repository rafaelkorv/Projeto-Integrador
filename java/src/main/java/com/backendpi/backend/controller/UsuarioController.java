package com.backendpi.backend.controller;

import java.io.InputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;
import java.util.Map;
import java.util.UUID;

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
import com.backendpi.backend.service.UsuarioService;
import com.backendpi.backend.security.AuthSessionService;
import com.backendpi.backend.security.AuthenticatedUserContext;

@RestController
@RequestMapping("/usuarios")
@CrossOrigin("*")
public class UsuarioController {

    private final UsuarioRepository usuarioRepository;
    private final UsuarioService usuarioService;
    private final AuthSessionService authSessionService;

    public UsuarioController(
            UsuarioRepository usuarioRepository,
            UsuarioService usuarioService,
            AuthSessionService authSessionService) {

        this.usuarioRepository = usuarioRepository;
        this.usuarioService = usuarioService;
        this.authSessionService = authSessionService;
    }

    @GetMapping
    public List<UsuarioPerfilDTO> listar() {
        return usuarioRepository.findAll().stream()
                .map(this::criarPerfil)
                .toList();
    }

    @PostMapping
    public UsuarioPerfilDTO criar(@RequestBody Usuario usuario) {
        Usuario salvo = usuarioService.salvar(usuario);
        return criarPerfilComSessao(salvo);
    }

    @PutMapping("/{id}")
    public UsuarioPerfilDTO atualizar(
            @PathVariable Long id,
            @RequestBody Usuario usuario
    ) {
        AuthenticatedUserContext.requireMatches(id);
        return criarPerfil(usuarioService.atualizar(id, usuario));
    }

    @DeleteMapping("/{id}")
    public void deletar(@PathVariable Long id) {
        AuthenticatedUserContext.requireMatches(id);
        usuarioService.deletar(id);
        authSessionService.encerrarSessoesDoUsuario(id);
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

        AuthenticatedUserContext.requireMatches(id);
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

        return ResponseEntity.ok(criarPerfilComSessao(usuario));
    }

    @PostMapping("/{id}/foto")
    public ResponseEntity<?> atualizarFotoPerfil(
            @PathVariable Long id,
            @RequestParam("foto") MultipartFile foto) {

        AuthenticatedUserContext.requireMatches(id);
        Usuario usuario = usuarioRepository.findById(id).orElse(null);
        if (usuario == null) {
            return ResponseEntity.notFound().build();
        }
        if (foto.isEmpty()) {
            return ResponseEntity.badRequest()
                    .body(Map.of("success", false, "error", "Selecione uma imagem."));
        }

        String extensao = switch (foto.getContentType() == null ? "" : foto.getContentType()) {
            case "image/jpeg" -> ".jpg";
            case "image/png" -> ".png";
            case "image/webp" -> ".webp";
            default -> null;
        };
        if (extensao == null) {
            return ResponseEntity.badRequest()
                    .body(Map.of("success", false, "error", "Use uma imagem JPG, PNG ou WEBP."));
        }
        if (foto.getSize() > 5 * 1024 * 1024) {
            return ResponseEntity.badRequest()
                    .body(Map.of("success", false, "error", "A foto deve ter no máximo 5 MB."));
        }

        try {
            Path diretorio = Paths.get("uploads", "perfis").toAbsolutePath().normalize();
            Files.createDirectories(diretorio);
            Path destino = diretorio.resolve(UUID.randomUUID() + extensao).normalize();
            if (!destino.startsWith(diretorio)) {
                return ResponseEntity.badRequest()
                        .body(Map.of("success", false, "error", "Nome de arquivo inválido."));
            }

            try (InputStream arquivo = foto.getInputStream()) {
                Files.copy(arquivo, destino);
            }

            String caminhoAnterior = usuario.getFotoPerfil();
            usuario.setFotoPerfil("uploads/perfis/" + destino.getFileName());
            Usuario salvo;
            try {
                salvo = usuarioRepository.save(usuario);
            } catch (RuntimeException erroBanco) {
                Files.deleteIfExists(destino);
                throw erroBanco;
            }

            if (caminhoAnterior != null && caminhoAnterior.startsWith("uploads/perfis/")) {
                try {
                    Path fotoAnterior = diretorio.resolve(Paths.get(caminhoAnterior).getFileName()).normalize();
                    if (fotoAnterior.startsWith(diretorio) && !fotoAnterior.equals(destino)) {
                        Files.deleteIfExists(fotoAnterior);
                    }
                } catch (Exception erroLimpeza) {
                    System.err.println("Não foi possível remover a foto de perfil anterior: "
                            + erroLimpeza.getMessage());
                }
            }

            return ResponseEntity.ok(criarPerfil(salvo));
        } catch (Exception erro) {
            erro.printStackTrace();
            return ResponseEntity.internalServerError()
                    .body(Map.of("success", false, "error", "Não foi possível salvar a foto de perfil."));
        }
    }

    private UsuarioPerfilDTO criarPerfil(Usuario usuario) {
        return new UsuarioPerfilDTO(
                usuario.getIdUsuario(),
                usuario.getNome(),
                usuario.getUsername(),
                usuario.getBio(),
                usuario.getFotoPerfil(),
                usuario.getInteresses()
        );
    }

    private UsuarioPerfilDTO criarPerfilComSessao(Usuario usuario) {
        UsuarioPerfilDTO perfil = criarPerfil(usuario);
        perfil.setAuthToken(authSessionService.criarSessao(usuario.getIdUsuario()));
        return perfil;
    }
}
