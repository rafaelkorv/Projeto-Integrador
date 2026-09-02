package com.backendpi.backend.service;

import com.backendpi.backend.model.Comentario;
import com.backendpi.backend.repository.ComentarioRepository;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class ComentarioService {

    private final ComentarioRepository repository;

    public ComentarioService(ComentarioRepository repository) {
        this.repository = repository;
    }

    public List<Comentario> listar() {
        return repository.findAll();
    }

    public List<Comentario> listarPorPost(Long idPost) {
        return repository.findByIdPost(idPost);
    }

    public Comentario criar(Comentario comentario) {
        return repository.save(comentario);
    }

    public Comentario atualizar(Long id, Comentario novo) {
        return repository.findById(id).map(c -> {

            c.setConteudo(novo.getConteudo());
            c.setIdUsuario(novo.getIdUsuario());
            c.setIdPost(novo.getIdPost());

            return repository.save(c);

        }).orElseThrow(() -> new RuntimeException("Comentário não encontrado"));
    }

    public void deletar(Long id) {
        repository.deleteById(id);
    }
}