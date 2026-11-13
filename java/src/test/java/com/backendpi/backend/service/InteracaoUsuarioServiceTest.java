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

import com.backendpi.backend.repository.InteracaoUsuarioRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;

@ExtendWith(MockitoExtension.class)
class InteracaoUsuarioServiceTest {

    @Mock
    private InteracaoUsuarioRepository repository;

    @InjectMocks
    private InteracaoUsuarioService service;

    @AfterEach
    void limparContextoAutenticado() {
        AuthenticatedUserContext.clear();
    }

    @Test
    void listaApenasInteracoesDoUsuarioAutenticado() {
        AuthenticatedUserContext.setUserId(23L);
        when(repository.findByIdUsuario(23L)).thenReturn(List.of());

        service.listar();

        verify(repository).findByIdUsuario(23L);
    }

    @Test
    void exigeAutenticacaoParaListarInteracoes() {
        assertThrows(ResponseStatusException.class, () -> service.listar());
    }
}
