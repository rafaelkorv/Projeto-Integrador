package com.backendpi.backend.repository;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.backendpi.backend.model.Mensagem;

public interface MensagemRepository extends JpaRepository<Mensagem, Long> {

    List<Mensagem> findByConversa_IdConversaOrderByDataEnvioAsc(
            Long idConversa
    );
}
