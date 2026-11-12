package com.backendpi.backend.security;

import java.util.Objects;

import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;

public final class AuthenticatedUserContext {

    private static final ThreadLocal<Long> USER_ID = new ThreadLocal<>();

    private AuthenticatedUserContext() {
    }

    public static void setUserId(Long userId) {
        USER_ID.set(userId);
    }

    public static void clear() {
        USER_ID.remove();
    }

    public static Long requireUserId() {
        Long userId = USER_ID.get();
        if (userId == null) {
            throw new ResponseStatusException(HttpStatus.UNAUTHORIZED, "Autenticação necessária.");
        }
        return userId;
    }

    public static void requireMatches(Long requestedUserId) {
        if (!Objects.equals(requireUserId(), requestedUserId)) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário sem permissão.");
        }
    }
}
