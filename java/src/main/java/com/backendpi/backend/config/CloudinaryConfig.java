package com.backendpi.backend.config;

import java.util.HashMap;
import java.util.Map;

import com.cloudinary.Cloudinary;
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.env.Environment;

@Configuration(proxyBeanMethods = false)
public class CloudinaryConfig {

    @Bean
    @ConditionalOnProperty(name = "CLOUDINARY_CLOUD_NAME")
    public Cloudinary cloudinary(Environment environment) {
        Map<String, Object> configuration = new HashMap<>();
        configuration.put("cloud_name", required(environment, "CLOUDINARY_CLOUD_NAME"));
        configuration.put("api_key", required(environment, "CLOUDINARY_API_KEY"));
        configuration.put("api_secret", required(environment, "CLOUDINARY_API_SECRET"));
        configuration.put("secure", true);
        return new Cloudinary(configuration);
    }

    private String required(Environment environment, String name) {
        String value = environment.getProperty(name);
        if (value == null || value.isBlank()) {
            throw new IllegalStateException(
                    "Configure a variável de ambiente " + name + " para habilitar o Cloudinary.");
        }
        return value.trim();
    }
}
