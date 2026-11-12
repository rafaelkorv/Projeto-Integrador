package com.backendpi.backend.service;

import com.backendpi.backend.model.ImagensPost;
import com.backendpi.backend.repository.PostRepository;
import com.backendpi.backend.repository.ImagensPostRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;
import java.io.IOException;
import java.io.InputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;
import java.util.List;
import java.util.UUID;

@Service
public class ImagensPostService {
    private final ImagensPostRepository repository;
    private final PostRepository postRepository;

    public ImagensPostService(ImagensPostRepository repository, PostRepository postRepository) {
        this.repository = repository;
        this.postRepository = postRepository;
    }

    public List<ImagensPost> listar() { return repository.findAll(); }
    public List<ImagensPost> listarPorPost(Long idPost) {
        return repository.findByIdPostOrderByIdAsc(idPost);
    }

    public ImagensPost salvar(ImagensPost imagem) {
        validarPermissaoNoPost(imagem.getIdPost());
        return repository.save(imagem);
    }

    public void deletar(Long id) {
        ImagensPost imagem = repository.findById(id)
                .orElseThrow(() -> new RuntimeException("Imagem não encontrada."));
        validarPermissaoNoPost(imagem.getIdPost());
        repository.delete(imagem);
    }

    public ImagensPost salvarUpload(Long idPost, MultipartFile imagem) throws IOException {
        validarPermissaoNoPost(idPost);
        if (repository.countByIdPost(idPost) >= 4) {
            throw new IllegalArgumentException("Cada publicação pode ter no máximo quatro imagens.");
        }
        if (imagem == null || imagem.isEmpty()) {
            throw new IllegalArgumentException("Selecione uma imagem.");
        }
        if (imagem.getSize() > 5 * 1024 * 1024) {
            throw new IllegalArgumentException("Cada imagem deve ter no máximo 5 MB.");
        }

        String extensao = switch (imagem.getContentType() == null ? "" : imagem.getContentType()) {
            case "image/jpeg" -> ".jpg";
            case "image/png" -> ".png";
            case "image/webp" -> ".webp";
            default -> throw new IllegalArgumentException("Use uma imagem JPG, PNG ou WEBP.");
        };

        Path diretorio = Paths.get("uploads", "posts").toAbsolutePath().normalize();
        Files.createDirectories(diretorio);
        Path destino = diretorio.resolve(UUID.randomUUID() + extensao).normalize();
        if (!destino.startsWith(diretorio)) {
            throw new IllegalArgumentException("Nome de arquivo inválido.");
        }

        try (InputStream arquivo = imagem.getInputStream()) {
            Files.copy(arquivo, destino);
        }
        try {
            ImagensPost registro = new ImagensPost();
            registro.setIdPost(idPost);
            registro.setUrl("uploads/posts/" + destino.getFileName());
            return repository.save(registro);
        } catch (RuntimeException erro) {
            Files.deleteIfExists(destino);
            throw erro;
        }
    }

    private void validarPermissaoNoPost(Long idPost) {
        var post = postRepository.findById(idPost)
                .orElseThrow(() -> new IllegalArgumentException("Publicação não encontrada."));
        if (!post.getIdUsuario().equals(AuthenticatedUserContext.requireUserId())) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão para alterar imagens desta publicação.");
        }
    }
}