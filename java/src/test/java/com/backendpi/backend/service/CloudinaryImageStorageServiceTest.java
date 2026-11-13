package com.backendpi.backend.service;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyMap;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.util.Map;

import com.cloudinary.Cloudinary;
import com.cloudinary.Uploader;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.ObjectProvider;
import org.springframework.http.HttpStatus;
import org.springframework.mock.web.MockMultipartFile;
import org.springframework.web.server.ResponseStatusException;

class CloudinaryImageStorageServiceTest {

    private ObjectProvider<Cloudinary> cloudinaryProvider;
    private Cloudinary cloudinary;
    private Uploader uploader;
    private CloudinaryImageStorageService service;

    @BeforeEach
    void setUp() {
        cloudinaryProvider = mock();
        cloudinary = mock();
        uploader = mock();
        when(cloudinaryProvider.getIfAvailable()).thenReturn(cloudinary);
        when(cloudinary.uploader()).thenReturn(uploader);
        service = new CloudinaryImageStorageService(cloudinaryProvider);
    }

    @Test
    void enviaImagemEDevolveUrlSeguraEIdentificador() throws Exception {
        MockMultipartFile image = new MockMultipartFile(
                "imagem", "foto.jpg", "image/jpeg", new byte[]{1, 2, 3});
        when(uploader.upload(any(Object.class), anyMap()))
                .thenReturn(Map.of("secure_url", "https://res.cloudinary.com/test/image/upload/foto.jpg",
                        "public_id", "projeto-integrador/posts/foto"));

        CloudinaryImageStorageService.UploadedImage result =
                service.upload(image, "projeto-integrador/posts");

        assertEquals("https://res.cloudinary.com/test/image/upload/foto.jpg", result.secureUrl());
        assertEquals("projeto-integrador/posts/foto", result.publicId());
    }

    @Test
    void informaServicoIndisponivelQuandoCloudinaryNaoEstaConfigurado() {
        when(cloudinaryProvider.getIfAvailable()).thenReturn(null);
        MockMultipartFile image = new MockMultipartFile(
                "imagem", "foto.jpg", "image/jpeg", new byte[]{1});

        ResponseStatusException error = assertThrows(
                ResponseStatusException.class,
                () -> service.upload(image, "projeto-integrador/posts"));

        assertEquals(HttpStatus.SERVICE_UNAVAILABLE, error.getStatusCode());
    }

    @Test
    void rejeitaFormatosNaoPermitidosAntesDeContatarCloudinary() {
        MockMultipartFile image = new MockMultipartFile(
                "imagem", "arquivo.txt", "text/plain", new byte[]{1});

        assertThrows(IllegalArgumentException.class,
                () -> service.upload(image, "projeto-integrador/posts"));
        org.mockito.Mockito.verifyNoInteractions(cloudinaryProvider);
    }

    @Test
    void removeImagemPeloIdentificador() throws Exception {
        when(uploader.destroy(any(String.class), anyMap())).thenReturn(Map.of("result", "ok"));

        service.delete("projeto-integrador/posts/foto");

        verify(uploader).destroy(any(String.class), anyMap());
    }
}
