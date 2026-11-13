package com.backendpi.backend.security;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.time.Duration;

import org.junit.jupiter.api.Test;

class AuthRateLimitServiceTest {

    @Test
    void deveBloquearDepoisDoLimiteDaJanela() {
        AuthRateLimitService service = new AuthRateLimitService();
        Duration janela = Duration.ofMinutes(15);

        assertEquals(0, service.tryAcquire("login:127.0.0.1", 2, janela));
        assertEquals(0, service.tryAcquire("login:127.0.0.1", 2, janela));
        assertTrue(service.tryAcquire("login:127.0.0.1", 2, janela) > 0);
    }

    @Test
    void deveManterLimitesSeparadosPorCliente() {
        AuthRateLimitService service = new AuthRateLimitService();
        Duration janela = Duration.ofMinutes(15);

        service.tryAcquire("login:127.0.0.1", 1, janela);

        assertEquals(0, service.tryAcquire("login:127.0.0.2", 1, janela));
    }
}
