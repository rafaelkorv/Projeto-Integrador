package com.backendpi.backend.service;

import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

import com.backendpi.backend.model.Comunidade;
import com.backendpi.backend.model.Post;
import com.backendpi.backend.repository.ComentarioRepository;
import com.backendpi.backend.repository.ComunidadeRepository;
import com.backendpi.backend.repository.PostRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;

@Service
public class PostService {

    private final PostRepository repository;
    private final ComentarioRepository comentarioRepository;
    private final ComunidadeRepository comunidadeRepository;
    private final ImagensPostService imagensPostService;

    public PostService(
            PostRepository repository,
            ComentarioRepository comentarioRepository,
            ComunidadeRepository comunidadeRepository,
            ImagensPostService imagensPostService) {

        this.repository = repository;
        this.comentarioRepository = comentarioRepository;
        this.comunidadeRepository = comunidadeRepository;
        this.imagensPostService = imagensPostService;
    }

    public List<Post> listar() {
        return repository.findAllByOrderByIdPostDesc();
    }

    public Page<Post> listarPaginado(Pageable pageable) {
        return repository.findAllByOrderByIdPostDesc(pageable);
    }

    public Page<Post> listarPorComunidade(Long idComunidade, Pageable pageable) {
        return repository.findByIdComunidadeOrderByIdPostDesc(idComunidade, pageable);
    }

    public Page<Post> listarPorUsuarioPaginado(Long idUsuario, Pageable pageable) {
        return repository.findByIdUsuarioOrderByIdPostDesc(idUsuario, pageable);
    }

    public Post salvar(Post post) {
        AuthenticatedUserContext.requireMatches(post.getIdUsuario());

        // Post global: não precisa pertencer a comunidade
        if (post.getIdComunidade() == null) {
            return repository.save(post);
        }

        Comunidade comunidade = comunidadeRepository.findById(post.getIdComunidade())
                .orElseThrow(() -> new RuntimeException("Comunidade não encontrada"));

        boolean usuarioEhMembro = comunidade.getMembros()
                .stream()
                .anyMatch(membro
                        -> membro.getIdUsuario().equals(post.getIdUsuario())
                );

        if (!usuarioEhMembro) {
            throw new RuntimeException(
                    "Usuário precisa participar da comunidade para publicar"
            );
        }

        return repository.save(post);
    }

    public Post buscarPorId(Long id) {
        return repository.findById(id).orElse(null);
    }

    public void deletar(Long idPost, Long idUsuario) {
        AuthenticatedUserContext.requireMatches(idUsuario);

        Post post = repository.findById(idPost)
                .orElseThrow(() -> new RuntimeException("Post não encontrado"));

        boolean eAutor = post.getIdUsuario().equals(idUsuario);

        boolean eAdministrador = false;

        if (post.getIdComunidade() != null) {
            Comunidade comunidade = comunidadeRepository.findById(post.getIdComunidade())
                    .orElseThrow(() -> new RuntimeException("Comunidade não encontrada"));

            if (comunidade.getCriador() != null) {
                eAdministrador = comunidade.getCriador()
                        .getIdUsuario()
                        .equals(idUsuario);
            }
        }

        if (!eAutor && !eAdministrador) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão para excluir este post");
        }

        imagensPostService.deletarPorPost(idPost);
        comentarioRepository.deleteAll(
                comentarioRepository.findByIdPost(idPost)
        );

        repository.deleteById(idPost);
    }

    public Post atualizar(Long id, Post novo) {
        Long idUsuario = AuthenticatedUserContext.requireUserId();

        return repository.findById(id).map(post -> {
            if (!post.getIdUsuario().equals(idUsuario)) {
                throw new ResponseStatusException(
                        HttpStatus.FORBIDDEN,
                        "Usuário sem permissão para editar esta publicação"
                );
            }

            post.setTitulo(novo.getTitulo());
            post.setConteudo(novo.getConteudo());

            return repository.save(post);

        }).orElseThrow();
    }

    public List<Post> listarPorUsuario(Long idUsuario) {
        return repository.findByIdUsuarioOrderByDataPostagemDesc(idUsuario);
    }
}
