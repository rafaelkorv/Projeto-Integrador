package com.backendpi.backend.security;

import java.security.SecureRandom;
import java.time.Instant;
import java.util.Base64;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.ConcurrentHashMap;

import org.springframework.stereotype.Service;

@Service
public class AuthSessionService {

    private static final long SESSION_LIFETIME_SECONDS = 12 * 60 * 60;

    private final SecureRandom secureRandom = new SecureRandom();
    private final Map<String, Session> sessions = new ConcurrentHashMap<>();

    public String criarSessao(Long userId) {
        byte[] bytes = new byte[32];
        secureRandom.nextBytes(bytes);
        String token = Base64.getUrlEncoder().withoutPadding().encodeToString(bytes);
        sessions.put(token, new Session(userId, Instant.now().plusSeconds(SESSION_LIFETIME_SECONDS)));
        return token;
    }

    public Optional<Long> buscarUsuarioId(String token) {
        if (token == null || token.isBlank()) {
            return Optional.empty();
        }
        Session session = sessions.get(token);
        if (session == null) {
            return Optional.empty();
        }
        if (!session.expiresAt().isAfter(Instant.now())) {
            sessions.remove(token, session);
            return Optional.empty();
        }
        return Optional.of(session.userId());
    }

    public void encerrarSessao(String token) {
        if (token != null) {
            sessions.remove(token);
        }
    }

    public void encerrarSessoesDoUsuario(Long userId) {
        sessions.entrySet().removeIf(entry -> entry.getValue().userId().equals(userId));
    }

    private record Session(Long userId, Instant expiresAt) {
    }
}
