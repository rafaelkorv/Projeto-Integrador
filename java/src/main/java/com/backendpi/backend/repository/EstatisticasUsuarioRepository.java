package com.backendpi.backend.repository;

import com.backendpi.backend.model.EstatisticasUsuario;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface EstatisticasUsuarioRepository extends JpaRepository<EstatisticasUsuario, Long> {
    List<EstatisticasUsuario> findByIdUsuario(Long idUsuario);
}