package com.backendpi.backend.service;

import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.util.List;

import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.web.server.ResponseStatusException;

import com.backendpi.backend.repository.MembrosComunidadeRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;

@ExtendWith(MockitoExtension.class)
class MembrosComunidadeServiceTest {

    @Mock
    private MembrosComunidadeRepository repository;

    @InjectMocks
    private MembrosComunidadeService service;

    @AfterEach
    void limparContextoAutenticado() {
        AuthenticatedUserContext.clear();
    }

    @Test
    void listaApenasAssociacoesDoUsuarioAutenticado() {
        AuthenticatedUserContext.setUserId(17L);
        when(repository.findByIdUsuario(17L)).thenReturn(List.of());

        service.listar();

        verify(repository).findByIdUsuario(17L);
    }

    @Test
    void exigeAutenticacaoParaListarAssociacoes() {
        assertThrows(ResponseStatusException.class, () -> service.listar());
    }
}
