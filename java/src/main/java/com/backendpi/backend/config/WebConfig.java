package com.backendpi.backend.config;

import java.nio.file.Paths;
import java.util.Arrays;

import org.springframework.context.annotation.Configuration;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.web.servlet.config.annotation.InterceptorRegistry;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.ResourceHandlerRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

import com.backendpi.backend.security.AuthRateLimitInterceptor;

@Configuration
public class WebConfig implements WebMvcConfigurer {

    private final String[] allowedOrigins;
    private final AuthRateLimitInterceptor authRateLimitInterceptor;

    public WebConfig(
            @Value("${APP_CORS_ALLOWED_ORIGINS:http://localhost:5500,http://127.0.0.1:5500}")
            String configuredOrigins,
            AuthRateLimitInterceptor authRateLimitInterceptor) {
        this.allowedOrigins = Arrays.stream(configuredOrigins.split(","))
                .map(String::trim)
                .filter(origin -> !origin.isEmpty())
                .toArray(String[]::new);
        this.authRateLimitInterceptor = authRateLimitInterceptor;
    }

    public String[] getAllowedOrigins() {
        return allowedOrigins.clone();
    }

    @Override
    public void addInterceptors(InterceptorRegistry registry) {
        registry.addInterceptor(authRateLimitInterceptor);
    }

    @Override
    public void addResourceHandlers(
            ResourceHandlerRegistry registry) {

        String caminhoUploads
                = Paths.get("uploads")
                        .toAbsolutePath()
                        .toUri()
                        .toString();

        registry
                .addResourceHandler("/uploads/**")
                .addResourceLocations(caminhoUploads);
    }

    @Override
    public void addCorsMappings(
            CorsRegistry registry
    ) {

        registry
                .addMapping("/**")
                .allowedOrigins(allowedOrigins)
                .allowedMethods(
                        "GET",
                        "POST",
                        "PUT",
                        "DELETE",
                        "PATCH",
                        "OPTIONS"
                )
                .allowedHeaders("*");
    }
}
