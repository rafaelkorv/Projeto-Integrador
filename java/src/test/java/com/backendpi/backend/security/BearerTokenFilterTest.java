package com.backendpi.backend.security;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.util.concurrent.atomic.AtomicBoolean;
import java.util.concurrent.atomic.AtomicReference;

import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.web.server.ResponseStatusException;

import jakarta.servlet.FilterChain;

class BearerTokenFilterTest {

    private final AuthSessionService sessionService = new AuthSessionService();
    private final TestableBearerTokenFilter filter = new TestableBearerTokenFilter(sessionService);

    @AfterEach
    void limparContexto() {
        AuthenticatedUserContext.clear();
    }

    @Test
    void tokenAutenticadoDefineIdentidadeApenasDuranteARequisicao() throws Exception {
        String token = sessionService.criarSessao(42L);
        MockHttpServletRequest request = request("POST", "/posts");
        request.addHeader("Authorization", "Bearer " + token);
        MockHttpServletResponse response = new MockHttpServletResponse();
        AtomicReference<Long> userIdNaRequisicao = new AtomicReference<>();

        filter.executar(request, response, (req, res) ->
                userIdNaRequisicao.set(AuthenticatedUserContext.requireUserId()));

        assertEquals(42L, userIdNaRequisicao.get());
        assertEquals(200, response.getStatus());
        assertThrows(ResponseStatusException.class, AuthenticatedUserContext::requireUserId);
    }

    @Test
    void escritaSemTokenEhRecusadaAntesDoController() throws Exception {
        MockHttpServletRequest request = request("DELETE", "/posts/8/usuario/42");
        MockHttpServletResponse response = new MockHttpServletResponse();
        AtomicBoolean controllerExecutado = new AtomicBoolean();

        filter.executar(request, response, (req, res) -> controllerExecutado.set(true));

        assertEquals(401, response.getStatus());
        assertFalse(controllerExecutado.get());
    }

    @Test
    void loginPermanecePublicoMesmoComTokenExpiradoNoCliente() throws Exception {
        MockHttpServletRequest request = request("POST", "/usuarios/login");
        request.addHeader("Authorization", "Bearer token-expirado");
        MockHttpServletResponse response = new MockHttpServletResponse();
        AtomicBoolean controllerExecutado = new AtomicBoolean();

        filter.executar(request, response, (req, res) -> controllerExecutado.set(true));

        assertEquals(200, response.getStatus());
        assertTrue(controllerExecutado.get());
    }

    @Test
    void sessaoEncerradaDeixaDeSerValida() {
        String token = sessionService.criarSessao(7L);

        sessionService.encerrarSessao(token);

        assertTrue(sessionService.buscarUsuarioId(token).isEmpty());
    }

    @Test
    void idDeUsuarioDiferenteDoAutenticadoEhRecusado() {
        AuthenticatedUserContext.setUserId(7L);

        ResponseStatusException erro = assertThrows(
                ResponseStatusException.class,
                () -> AuthenticatedUserContext.requireMatches(8L)
        );

        assertEquals(403, erro.getStatusCode().value());
    }

    private MockHttpServletRequest request(String method, String path) {
        MockHttpServletRequest request = new MockHttpServletRequest(method, path);
        request.setServletPath(path);
        return request;
    }

    private static class TestableBearerTokenFilter extends BearerTokenFilter {

        TestableBearerTokenFilter(AuthSessionService sessionService) {
            super(sessionService);
        }

        void executar(
                MockHttpServletRequest request,
                MockHttpServletResponse response,
                FilterChain chain) throws Exception {
            doFilterInternal(request, response, chain);
        }
    }
}
