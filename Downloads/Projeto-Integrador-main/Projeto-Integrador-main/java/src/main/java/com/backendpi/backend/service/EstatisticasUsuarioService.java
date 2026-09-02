package com.backendpi.backend.service;

import com.backendpi.backend.model.EstatisticasUsuario;
import com.backendpi.backend.repository.EstatisticasUsuarioRepository;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class EstatisticasUsuarioService {
    private final EstatisticasUsuarioRepository repository;

    public EstatisticasUsuarioService(EstatisticasUsuarioRepository repository) {
        this.repository = repository;
    }

    public List<EstatisticasUsuario> listar() { return repository.findAll(); }
    public EstatisticasUsuario salvar(EstatisticasUsuario estatistica) { return repository.save(estatistica); }
    public void deletar(Long id) { repository.deleteById(id); }
}