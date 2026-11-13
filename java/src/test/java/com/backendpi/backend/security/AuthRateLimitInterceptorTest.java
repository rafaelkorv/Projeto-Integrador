package com.backendpi.backend.security;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.time.Duration;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;

@ExtendWith(MockitoExtension.class)
class AuthRateLimitInterceptorTest {

    @Mock
    private AuthRateLimitService rateLimitService;

    @InjectMocks
    private AuthRateLimitInterceptor interceptor;

    @Test
    void responde429QuandoLimiteDeLoginFoiAtingido() throws Exception {
        MockHttpServletRequest request = new MockHttpServletRequest();
        request.setMethod("POST");
        request.setServletPath("/auth/login");
        request.setRemoteAddr("192.0.2.10");
        MockHttpServletResponse response = new MockHttpServletResponse();
        when(rateLimitService.tryAcquire(
                "login:192.0.2.10",
                10,
                Duration.ofMinutes(15))).thenReturn(45L);

        boolean permitido = interceptor.preHandle(request, response, new Object());

        assertFalse(permitido);
        assertEquals(429, response.getStatus());
        assertEquals("45", response.getHeader("Retry-After"));
        verify(rateLimitService).tryAcquire(
                "login:192.0.2.10",
                10,
                Duration.ofMinutes(15));
    }
}
