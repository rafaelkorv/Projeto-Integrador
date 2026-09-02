package com.backendpi.backend.service;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;
import static org.junit.jupiter.api.Assertions.assertSame;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.security.crypto.password.PasswordEncoder;

import com.backendpi.backend.model.Usuario;
import com.backendpi.backend.repository.UsuarioRepository;

@ExtendWith(MockitoExtension.class)
class UsuarioServiceTest {

    @Mock
    private UsuarioRepository usuarioRepository;

    @Mock
    private PasswordEncoder passwordEncoder;

    @InjectMocks
    private UsuarioService usuarioService;

    @Test
    void deveSalvarUsuarioComSenhaCriptografada() {
        Usuario usuario = new Usuario();
        usuario.setEmail("teste@email.com");
        usuario.setUsername("teste");
        usuario.setSenha("minhasenha123");

        when(usuarioRepository.existsByEmail("teste@email.com")).thenReturn(false);
        when(usuarioRepository.existsByUsername("teste")).thenReturn(false);
        when(passwordEncoder.encode("minhasenha123")).thenReturn("$2a$10$hashedPasswordValue1234567890123456789012345678901234567890");
        when(usuarioRepository.save(any(Usuario.class))).thenAnswer(invocation -> invocation.getArgument(0));

        Usuario salvo = usuarioService.salvar(usuario);

        assertNotNull(salvo);
        assertEquals("$2a$10$hashedPasswordValue1234567890123456789012345678901234567890", salvo.getSenha());
        verify(passwordEncoder).encode("minhasenha123");
        verify(usuarioRepository).save(usuario);
    }

    @Test
    void deveLogarPorEmailComSenhaCriptografada() {
        Usuario usuario = new Usuario();
        usuario.setEmail("ana@email.com");
        usuario.setSenha("$2a$10$hashedPasswordValue1234567890123456789012345678901234567890");

        when(usuarioRepository.findByEmail("ana@email.com")).thenReturn(usuario);
        when(passwordEncoder.matches("12345678", usuario.getSenha())).thenReturn(true);

        Usuario resultado = usuarioService.login("ana@email.com", null, "12345678");

        assertSame(usuario, resultado);
        verify(usuarioRepository).findByEmail("ana@email.com");
        verify(passwordEncoder).matches("12345678", usuario.getSenha());
    }

    @Test
    void deveLogarPorTelefoneComSenhaCriptografada() {
        Usuario usuario = new Usuario();
        usuario.setTelefone("11999990000");
        usuario.setSenha("$2a$10$hashedPasswordValue1234567890123456789012345678901234567890");

        when(usuarioRepository.findByTelefone("11999990000")).thenReturn(usuario);
        when(passwordEncoder.matches("12345678", usuario.getSenha())).thenReturn(true);

        Usuario resultado = usuarioService.login(null, "11999990000", "12345678");

        assertSame(usuario, resultado);
        verify(usuarioRepository).findByTelefone("11999990000");
        verify(passwordEncoder).matches("12345678", usuario.getSenha());
    }

    @Test
    void deveFazerMigracaoAutomaticaDeSenhaLegadaNoLogin() {
        Usuario usuario = new Usuario();
        usuario.setEmail("legado@email.com");
        usuario.setSenha("senhaTextoPuro");

        when(usuarioRepository.findByEmail("legado@email.com")).thenReturn(usuario);
        when(passwordEncoder.matches("senhaTextoPuro", "senhaTextoPuro")).thenReturn(false);
        when(passwordEncoder.encode("senhaTextoPuro")).thenReturn("$2a$10$novoHashGerado1234567890123456789012345678901234567890");
        when(usuarioRepository.save(usuario)).thenReturn(usuario);

        Usuario resultado = usuarioService.login("legado@email.com", null, "senhaTextoPuro");

        assertNotNull(resultado);
        assertEquals("$2a$10$novoHashGerado1234567890123456789012345678901234567890", resultado.getSenha());
        verify(passwordEncoder).encode("senhaTextoPuro");
        verify(usuarioRepository).save(usuario);
    }

    @Test
    void deveRetornarNullParaSenhaIncorreta() {
        Usuario usuario = new Usuario();
        usuario.setEmail("ana@email.com");
        usuario.setSenha("$2a$10$hashedPasswordValue1234567890123456789012345678901234567890");

        when(usuarioRepository.findByEmail("ana@email.com")).thenReturn(usuario);
        when(passwordEncoder.matches("senhaErrada", usuario.getSenha())).thenReturn(false);

        Usuario resultado = usuarioService.login("ana@email.com", null, "senhaErrada");

        assertNull(resultado);
    }
}
