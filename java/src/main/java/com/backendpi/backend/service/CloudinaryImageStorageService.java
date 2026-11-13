package com.backendpi.backend.service;

import java.io.IOException;
import java.util.Map;

import com.cloudinary.Cloudinary;
import com.cloudinary.utils.ObjectUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.ObjectProvider;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;
import org.springframework.web.server.ResponseStatusException;

@Service
public class CloudinaryImageStorageService {

    private static final Logger logger = LoggerFactory.getLogger(CloudinaryImageStorageService.class);
    private static final long MAX_IMAGE_SIZE = 5 * 1024 * 1024;
    private final ObjectProvider<Cloudinary> cloudinaryProvider;

    public CloudinaryImageStorageService(ObjectProvider<Cloudinary> cloudinaryProvider) {
        this.cloudinaryProvider = cloudinaryProvider;
    }

    public UploadedImage upload(MultipartFile image, String folder) {
        validarImagem(image);
        Cloudinary cloudinary = requireCloudinary();

        try {
            Map<?, ?> result = cloudinary.uploader().upload(
                    image.getBytes(),
                    ObjectUtils.asMap("folder", folder, "resource_type", "image"));
            Object secureUrl = result.get("secure_url");
            Object publicId = result.get("public_id");

            if (!(secureUrl instanceof String url) || url.isBlank()
                    || !(publicId instanceof String id) || id.isBlank()) {
                logger.error("Cloudinary returned an incomplete image upload response.");
                throw new ResponseStatusException(
                        HttpStatus.BAD_GATEWAY,
                        "O serviço de imagens retornou uma resposta inválida.");
            }

            return new UploadedImage(url, id);
        } catch (IOException error) {
            logger.error("Could not upload image to Cloudinary.", error);
            throw new ResponseStatusException(
                    HttpStatus.BAD_GATEWAY,
                    "Não foi possível enviar a imagem ao serviço de armazenamento.",
                    error);
        }
    }

    public void delete(String publicId) {
        if (publicId == null || publicId.isBlank()) {
            return;
        }

        Cloudinary cloudinary = requireCloudinary();
        try {
            Map<?, ?> result = cloudinary.uploader().destroy(
                    publicId,
                    ObjectUtils.asMap("resource_type", "image", "invalidate", true));
            Object status = result.get("result");
            if (!"ok".equals(status) && !"not found".equals(status)) {
                logger.error("Cloudinary did not confirm deletion of image {}: {}", publicId, status);
                throw new ResponseStatusException(
                        HttpStatus.BAD_GATEWAY,
                        "Não foi possível remover a imagem do serviço de armazenamento.");
            }
        } catch (IOException error) {
            logger.error("Could not delete image {} from Cloudinary.", publicId, error);
            throw new ResponseStatusException(
                    HttpStatus.BAD_GATEWAY,
                    "Não foi possível remover a imagem do serviço de armazenamento.",
                    error);
        }
    }

    private Cloudinary requireCloudinary() {
        Cloudinary cloudinary = cloudinaryProvider.getIfAvailable();
        if (cloudinary == null) {
            throw new ResponseStatusException(
                    HttpStatus.SERVICE_UNAVAILABLE,
                    "O Cloudinary não está configurado. Configure as três variáveis de ambiente no backend.");
        }
        return cloudinary;
    }

    private void validarImagem(MultipartFile image) {
        if (image == null || image.isEmpty()) {
            throw new IllegalArgumentException("Selecione uma imagem.");
        }
        if (image.getSize() > MAX_IMAGE_SIZE) {
            throw new IllegalArgumentException("A imagem deve ter no máximo 5 MB.");
        }
        String contentType = image.getContentType();
        if (!"image/jpeg".equals(contentType)
                && !"image/png".equals(contentType)
                && !"image/webp".equals(contentType)) {
            throw new IllegalArgumentException("Use uma imagem JPG, PNG ou WEBP.");
        }
    }

    public record UploadedImage(String secureUrl, String publicId) {
    }
}
