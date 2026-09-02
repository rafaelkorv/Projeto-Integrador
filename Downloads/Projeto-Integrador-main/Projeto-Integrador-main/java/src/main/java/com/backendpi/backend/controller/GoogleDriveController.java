package com.backendpi.backend.controller;

import com.backendpi.backend.service.GoogleDriveService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.util.Map;

@RestController
@RequestMapping("/google/drive")
public class GoogleDriveController {

    private final GoogleDriveService googleDriveService;

    public GoogleDriveController(
            GoogleDriveService googleDriveService
    ) {
        this.googleDriveService = googleDriveService;
    }


    // =====================================================
    // AUTORIZAÇÃO DO GOOGLE
    // =====================================================

    @GetMapping("/auth")
    public ResponseEntity<?> googleAuth() {

        try {

            String url =
                    googleDriveService.getAuthorizationUrl();

            return ResponseEntity.ok(
                    Map.of(
                            "success", true,
                            "authorizationUrl", url
                    )
            );

        } catch (Exception e) {

            e.printStackTrace();

            return ResponseEntity
                    .internalServerError()
                    .body(
                            Map.of(
                                    "success", false,
                                    "error", e.getMessage()
                            )
                    );
        }
    }


    // =====================================================
    // CALLBACK DO GOOGLE
    // =====================================================

    @GetMapping("/callback")
    public ResponseEntity<?> googleCallback(
            @RequestParam("code") String code
    ) {

        try {

            String refreshToken =
                    googleDriveService.getRefreshToken(code);

            return ResponseEntity.ok(
                    Map.of(
                            "success", true,
                            "refreshToken", refreshToken
                    )
            );

        } catch (Exception e) {

            e.printStackTrace();

            return ResponseEntity
                    .internalServerError()
                    .body(
                            Map.of(
                                    "success", false,
                                    "error", e.getMessage()
                            )
                    );
        }
    }


    // =====================================================
    // UPLOAD DA FOTO
    // =====================================================

    @PostMapping("/upload")
    public ResponseEntity<?> uploadFoto(
            @RequestParam("foto") MultipartFile foto
    ) {

        try {

            if (foto.isEmpty()) {

                return ResponseEntity
                        .badRequest()
                        .body(
                                Map.of(
                                        "success", false,
                                        "error", "Nenhuma foto enviada."
                                )
                        );
            }

            String fileId =
                    googleDriveService.uploadFoto(
                            foto,
                            foto.getOriginalFilename()
                    );

            return ResponseEntity.ok(
                    Map.of(
                            "success", true,
                            "fileId", fileId
                    )
            );

        } catch (Exception e) {

            e.printStackTrace();

            return ResponseEntity
                    .internalServerError()
                    .body(
                            Map.of(
                                    "success", false,
                                    "error", e.getMessage()
                            )
                    );
        }
    }


    // =====================================================
    // EXIBIR FOTO PRIVADA
    // =====================================================

    @GetMapping("/image/{fileId}")
    public ResponseEntity<?> visualizarFoto(
            @PathVariable String fileId
    ) {

        try {

            byte[] imagem =
                    googleDriveService.baixarFoto(fileId);

            String mimeType =
                    googleDriveService.getMimeType(fileId);

            MediaType mediaType;

            try {
                mediaType = MediaType.parseMediaType(mimeType);
            } catch (Exception e) {
                mediaType = MediaType.APPLICATION_OCTET_STREAM;
            }

            return ResponseEntity.ok()
                    .header(
                            HttpHeaders.CACHE_CONTROL,
                            "public, max-age=3600"
                    )
                    .contentType(mediaType)
                    .body(imagem);

        } catch (Exception e) {

            e.printStackTrace();

            return ResponseEntity
                    .internalServerError()
                    .body(
                            Map.of(
                                    "success", false,
                                    "error", e.getMessage()
                            )
                    );
        }
    }
}