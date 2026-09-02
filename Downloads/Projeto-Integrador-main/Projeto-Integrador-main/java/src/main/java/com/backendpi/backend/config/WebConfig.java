package com.backendpi.backend.config;

import java.nio.file.Paths;

import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.ResourceHandlerRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

@Configuration
public class WebConfig implements WebMvcConfigurer {

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
                .allowedOriginPatterns("*")
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
