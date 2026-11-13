package com.backendpi.backend.security;

import java.time.Duration;

import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Component;
import org.springframework.web.servlet.HandlerInterceptor;

import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

@Component
public class AuthRateLimitInterceptor implements HandlerInterceptor {

    private final AuthRateLimitService rateLimitService;

    public AuthRateLimitInterceptor(AuthRateLimitService rateLimitService) {
        this.rateLimitService = rateLimitService;
    }

    @Override
    public boolean preHandle(
            HttpServletRequest request,
            HttpServletResponse response,
            Object handler) throws Exception {
        if (!"POST".equals(request.getMethod())) {
            return true;
        }

        String clientAddress = request.getRemoteAddr();
        String path = request.getServletPath();
        String rateLimitKey;
        int maxAttempts;
        Duration window;

        if ("/auth/login".equals(path) || "/usuarios/login".equals(path)) {
            rateLimitKey = "login:" + clientAddress;
            maxAttempts = 10;
            window = Duration.ofMinutes(15);
        } else if ("/usuarios".equals(path)) {
            rateLimitKey = "registration:" + clientAddress;
            maxAttempts = 5;
            window = Duration.ofHours(1);
        } else {
            return true;
        }

        long retryAfterSeconds = rateLimitService.tryAcquire(
                rateLimitKey,
                maxAttempts,
                window);
        if (retryAfterSeconds == 0) {
            return true;
        }

        response.setStatus(HttpStatus.TOO_MANY_REQUESTS.value());
        response.setHeader("Retry-After", Long.toString(retryAfterSeconds));
        response.setContentType("application/json");
        response.setCharacterEncoding("UTF-8");
        response.getWriter().write(
                "{\"error\":\"Muitas tentativas. Aguarde antes de tentar novamente.\"}");
        return false;
    }
}
