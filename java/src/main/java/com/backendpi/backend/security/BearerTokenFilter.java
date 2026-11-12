package com.backendpi.backend.security;

import java.io.IOException;

import org.springframework.http.HttpMethod;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

@Component
public class BearerTokenFilter extends OncePerRequestFilter {

    private final AuthSessionService sessionService;

    public BearerTokenFilter(AuthSessionService sessionService) {
        this.sessionService = sessionService;
    }

    @Override
    protected void doFilterInternal(
            HttpServletRequest request,
            HttpServletResponse response,
            FilterChain filterChain) throws ServletException, IOException {

        String token = extrairToken(request.getHeader("Authorization"));
        Long userId = sessionService.buscarUsuarioId(token).orElse(null);
        boolean endpointPublicoDeAutenticacao = HttpMethod.POST.matches(request.getMethod())
                && ("/usuarios".equals(request.getServletPath())
                || "/usuarios/login".equals(request.getServletPath())
                || "/auth/login".equals(request.getServletPath()));
        if (token != null && userId == null && !endpointPublicoDeAutenticacao) {
            response.sendError(HttpServletResponse.SC_UNAUTHORIZED, "Sessão inválida ou expirada.");
            return;
        }

        boolean metodoDeEscrita = !HttpMethod.GET.matches(request.getMethod())
                && !HttpMethod.HEAD.matches(request.getMethod())
                && !HttpMethod.OPTIONS.matches(request.getMethod());

        if (metodoDeEscrita && !endpointPublicoDeAutenticacao && userId == null) {
            response.sendError(HttpServletResponse.SC_UNAUTHORIZED, "Autenticação necessária.");
            return;
        }

        if (userId != null) {
            AuthenticatedUserContext.setUserId(userId);
        }
        try {
            filterChain.doFilter(request, response);
        } finally {
            AuthenticatedUserContext.clear();
        }
    }

    private String extrairToken(String authorization) {
        if (authorization == null || !authorization.startsWith("Bearer ")) {
            return null;
        }
        return authorization.substring(7).trim();
    }
}
