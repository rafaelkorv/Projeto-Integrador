package com.backendpi.backend.service;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;
import static org.junit.jupiter.api.Assertions.assertSame;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.util.Optional;

import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.web.server.ResponseStatusException;

import com.backendpi.backend.model.Usuario;
import com.backendpi.backend.repository.UsuarioRepository;
import com.backendpi.backend.security.AuthenticatedUserContext;

@ExtendWith(MockitoExtension.class)
class UsuarioServiceTest {

    @Mock
    private UsuarioRepository usuarioRepository;

    @Mock
    private PasswordEncoder passwordEncoder;

    @InjectMocks
    private UsuarioService usuarioService;

    @AfterEach
    void limparContextoAutenticado() {
        AuthenticatedUserContext.clear();
    }

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
    void deveIgnorarFotoEnviadaDuranteCadastro() {
        Usuario usuario = new Usuario();
        usuario.setEmail("foto@email.com");
        usuario.setUsername("foto");
        usuario.setSenha("senha-segura");
        usuario.setFotoPerfil("https://example.com/avatar");

        when(usuarioRepository.existsByEmail("foto@email.com")).thenReturn(false);
        when(usuarioRepository.existsByUsername("foto")).thenReturn(false);
        when(passwordEncoder.encode("senha-segura")).thenReturn("hash");
        when(usuarioRepository.save(any(Usuario.class))).thenAnswer(invocation -> invocation.getArgument(0));

        Usuario salvo = usuarioService.salvar(usuario);

        assertNull(salvo.getFotoPerfil());
    }

    @Test
    void atualizarNaoDeveAceitarUrlDeFotoArbitraria() {
        AuthenticatedUserContext.setUserId(7L);
        Usuario existente = new Usuario();
        existente.setFotoPerfil("uploads/perfis/avatar.jpg");
        Usuario dados = new Usuario();
        dados.setFotoPerfil("https://example.com/avatar");

        when(usuarioRepository.findById(7L)).thenReturn(Optional.of(existente));
        when(usuarioRepository.save(existente)).thenReturn(existente);

        Usuario atualizado = usuarioService.atualizar(7L, dados);

        assertEquals("uploads/perfis/avatar.jpg", atualizado.getFotoPerfil());
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

    @Test
    void deveAlterarSenhaComSenhaAtualValida() {
        AuthenticatedUserContext.setUserId(7L);
        Usuario usuario = new Usuario();
        usuario.setIdUsuario(7L);
        usuario.setSenha("hash-antigo");

        when(usuarioRepository.findById(7L)).thenReturn(Optional.of(usuario));
        when(passwordEncoder.matches("senha atual", "hash-antigo")).thenReturn(true);
        when(passwordEncoder.matches("senha nova segura", "hash-antigo")).thenReturn(false);
        when(passwordEncoder.encode("senha nova segura")).thenReturn("hash-novo");
        when(usuarioRepository.save(usuario)).thenReturn(usuario);

        Usuario atualizado = usuarioService.alterarSenha(7L, "senha atual", "senha nova segura");

        assertSame(usuario, atualizado);
        assertEquals("hash-novo", atualizado.getSenha());
        verify(passwordEncoder).encode("senha nova segura");
        verify(usuarioRepository).save(usuario);
    }

    @Test
    void deveRejeitarSenhaAtualIncorretaSemSalvar() {
        AuthenticatedUserContext.setUserId(7L);
        Usuario usuario = new Usuario();
        usuario.setSenha("hash-antigo");

        when(usuarioRepository.findById(7L)).thenReturn(Optional.of(usuario));
        when(passwordEncoder.matches("senha errada", "hash-antigo")).thenReturn(false);

        assertThrows(
                ResponseStatusException.class,
                () -> usuarioService.alterarSenha(7L, "senha errada", "senha nova segura"));
    }
}
