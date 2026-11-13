package com.backendpi.backend.config;

import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertThrows;

import org.junit.jupiter.api.Test;
import org.springframework.mock.env.MockEnvironment;

class CloudinaryConfigTest {

    private final CloudinaryConfig config = new CloudinaryConfig();

    @Test
    void criaClienteComAsVariaveisDeAmbiente() {
        MockEnvironment environment = new MockEnvironment()
                .withProperty("CLOUDINARY_CLOUD_NAME", "test-cloud")
                .withProperty("CLOUDINARY_API_KEY", "test-key")
                .withProperty("CLOUDINARY_API_SECRET", "test-secret");

        assertNotNull(config.cloudinary(environment));
    }

    @Test
    void rejeitaConfiguracaoParcial() {
        MockEnvironment environment = new MockEnvironment()
                .withProperty("CLOUDINARY_CLOUD_NAME", "test-cloud")
                .withProperty("CLOUDINARY_API_KEY", "test-key");

        assertThrows(IllegalStateException.class, () -> config.cloudinary(environment));
    }
}
