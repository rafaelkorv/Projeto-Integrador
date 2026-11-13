package com.backendpi.backend.service;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.util.List;
import java.util.Optional;

import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.web.server.ResponseStatusException;

import com.backendpi.backend.model.UsuarioSeguido;
import com.backendpi.backend.repository.UsuarioRepository;
import com.backendpi.backend.repository.UsuarioSeguidoRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;

@ExtendWith(MockitoExtension.class)
class UsuarioSeguidoServiceTest {

    @Mock
    private UsuarioSeguidoRepository seguidoRepository;

    @Mock
    private UsuarioRepository usuarioRepository;

    @InjectMocks
    private UsuarioSeguidoService usuarioSeguidoService;

    @AfterEach
    void limparContextoAutenticado() {
        AuthenticatedUserContext.clear();
    }

    @Test
    void deveSalvarSeguidoParaUsuarioAutenticado() {
        AuthenticatedUserContext.setUserId(7L);
        when(usuarioRepository.existsById(12L)).thenReturn(true);
        when(seguidoRepository.findBySeguidorIdAndSeguidoId(7L, 12L)).thenReturn(Optional.empty());

        usuarioSeguidoService.seguir(12L);

        verify(seguidoRepository).save(any(UsuarioSeguido.class));
    }

    @Test
    void naoDevePermitirSeguirAPropriaConta() {
        AuthenticatedUserContext.setUserId(7L);

        assertThrows(ResponseStatusException.class, () -> usuarioSeguidoService.seguir(7L));

        verify(seguidoRepository, never()).save(any(UsuarioSeguido.class));
    }

    @Test
    void deveListarSomenteIdsSeguidosPeloUsuarioAutenticado() {
        AuthenticatedUserContext.setUserId(7L);
        UsuarioSeguido seguido = new UsuarioSeguido();
        seguido.setSeguidoId(12L);
        when(seguidoRepository.findBySeguidorIdOrderBySeguidoEmDesc(7L))
                .thenReturn(List.of(seguido));

        assertEquals(List.of(12L), usuarioSeguidoService.listarSeguindo());
    }
}
