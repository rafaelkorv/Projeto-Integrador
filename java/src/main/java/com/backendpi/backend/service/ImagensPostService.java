package com.backendpi.backend.service;

import java.util.List;

import com.backendpi.backend.model.ImagensPost;
import com.backendpi.backend.repository.PostRepository;
import com.backendpi.backend.repository.ImagensPostRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;

@Service
public class ImagensPostService {
    private static final Logger logger = LoggerFactory.getLogger(ImagensPostService.class);
    private final ImagensPostRepository repository;
    private final PostRepository postRepository;
    private final CloudinaryImageStorageService imageStorage;

    public ImagensPostService(
            ImagensPostRepository repository,
            PostRepository postRepository,
            CloudinaryImageStorageService imageStorage) {
        this.repository = repository;
        this.postRepository = postRepository;
        this.imageStorage = imageStorage;
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
        imageStorage.delete(imagem.getPublicId());
        repository.delete(imagem);
    }

    public void deletarPorPost(Long idPost) {
        List<ImagensPost> imagens = listarPorPost(idPost);
        for (ImagensPost imagem : imagens) {
            imageStorage.delete(imagem.getPublicId());
            repository.delete(imagem);
        }
    }

    public ImagensPost salvarUpload(Long idPost, MultipartFile imagem) {
        validarPermissaoNoPost(idPost);
        if (repository.countByIdPost(idPost) >= 4) {
            throw new IllegalArgumentException("Cada publicação pode ter no máximo quatro imagens.");
        }
        CloudinaryImageStorageService.UploadedImage enviada =
                imageStorage.upload(imagem, "projeto-integrador/posts");
        try {
            ImagensPost registro = new ImagensPost();
            registro.setIdPost(idPost);
            registro.setUrl(enviada.secureUrl());
            registro.setPublicId(enviada.publicId());
            return repository.save(registro);
        } catch (RuntimeException erro) {
            try {
                imageStorage.delete(enviada.publicId());
            } catch (RuntimeException erroLimpeza) {
                logger.error("Não foi possível limpar imagem do Cloudinary após falha ao salvar no banco.", erroLimpeza);
            }
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