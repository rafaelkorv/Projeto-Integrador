package com.backendpi.backend.service;

import com.google.api.client.googleapis.auth.oauth2.GoogleAuthorizationCodeFlow;
import com.google.api.client.googleapis.auth.oauth2.GoogleClientSecrets;
import com.google.api.client.googleapis.auth.oauth2.GoogleTokenResponse;
import com.google.api.client.googleapis.javanet.GoogleNetHttpTransport;
import com.google.api.client.http.InputStreamContent;
import com.google.api.client.http.javanet.NetHttpTransport;
import com.google.api.client.json.gson.GsonFactory;
import com.google.api.client.auth.oauth2.Credential;
import com.google.api.services.drive.Drive;
import com.google.api.services.drive.DriveScopes;
import com.google.api.services.drive.model.File;
import com.google.api.services.drive.model.FileList;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;

import java.io.InputStream;
import java.util.Collections;
import java.util.List;

@Service
public class GoogleDriveService {

    private static final GsonFactory JSON_FACTORY =
            GsonFactory.getDefaultInstance();

    private final NetHttpTransport transport;

    private final String clientId =
            System.getenv("GOOGLE_CLIENT_ID");

    private final String clientSecret =
            System.getenv("GOOGLE_CLIENT_SECRET");

    private final String refreshToken =
            System.getenv("GOOGLE_REFRESH_TOKEN");

    private final String folderId =
            System.getenv("GOOGLE_DRIVE_FOLDER_ID");

    //private final String redirectUri = "http://localhost:8080/google/drive/callback";

    private final String redirectUri = "https://projeto-integrador-m4jn.onrender.com/google/drive/callback";


    public GoogleDriveService() throws Exception {
        transport = GoogleNetHttpTransport.newTrustedTransport();
    }


    // =====================================================
    // GERA A URL PARA AUTORIZAR O GOOGLE
    // =====================================================

    public String getAuthorizationUrl() throws Exception {

        GoogleClientSecrets.Details details =
                new GoogleClientSecrets.Details()
                        .setClientId(clientId)
                        .setClientSecret(clientSecret);

        GoogleClientSecrets secrets =
                new GoogleClientSecrets()
                        .setWeb(details);

        GoogleAuthorizationCodeFlow flow =
                new GoogleAuthorizationCodeFlow.Builder(
                        transport,
                        JSON_FACTORY,
                        secrets,
                        Collections.singletonList(
                                DriveScopes.DRIVE_FILE
                        )
                )
                        .setAccessType("offline")
                        .build();

        return flow.newAuthorizationUrl()
                .setRedirectUri(redirectUri)
                .setAccessType("offline")
                .set("prompt", "consent")
                .build();
    }


    // =====================================================
    // TRANSFORMA O CODE EM REFRESH TOKEN
    // =====================================================

    public String getRefreshToken(String code) throws Exception {

        GoogleClientSecrets.Details details =
                new GoogleClientSecrets.Details()
                        .setClientId(clientId)
                        .setClientSecret(clientSecret);

        GoogleClientSecrets secrets =
                new GoogleClientSecrets()
                        .setWeb(details);

        GoogleAuthorizationCodeFlow flow =
                new GoogleAuthorizationCodeFlow.Builder(
                        transport,
                        JSON_FACTORY,
                        secrets,
                        Collections.singletonList(
                                DriveScopes.DRIVE_FILE
                        )
                )
                        .setAccessType("offline")
                        .build();

        GoogleTokenResponse response =
                flow.newTokenRequest(code)
                        .setRedirectUri(redirectUri)
                        .execute();

        return response.getRefreshToken();
    }


    // =====================================================
    // CONECTA AO GOOGLE DRIVE
    // =====================================================

    private Drive getDrive() throws Exception {

        if (clientId == null ||
                clientSecret == null ||
                refreshToken == null) {

            throw new RuntimeException(
                    "GOOGLE_CLIENT_ID, GOOGLE_CLIENT_SECRET ou GOOGLE_REFRESH_TOKEN não configurado."
            );
        }

        Credential credential =
                new Credential.Builder(
                        com.google.api.client.auth.oauth2.BearerToken
                                .authorizationHeaderAccessMethod()
                )
                        .setTransport(transport)
                        .setJsonFactory(JSON_FACTORY)
                        .setTokenServerEncodedUrl(
                                "https://oauth2.googleapis.com/token"
                        )
                        .setClientAuthentication(
                                new com.google.api.client.auth.oauth2.ClientParametersAuthentication(
                                        clientId,
                                        clientSecret
                                )
                        )
                        .build();

        credential.setRefreshToken(refreshToken);

        boolean atualizado = credential.refreshToken();

        if (!atualizado) {
            throw new RuntimeException(
                    "O Google recusou o refresh token."
            );
        }

        if (credential.getAccessToken() == null) {
            throw new RuntimeException(
                    "Google não forneceu um access token."
            );
        }

        return new Drive.Builder(
                transport,
                JSON_FACTORY,
                credential
        )
                .setApplicationName("Backend PI")
                .build();
    }


    // =====================================================
// UPLOAD / SUBSTITUIÇÃO DA FOTO DE PERFIL
// =====================================================

    public String salvarFotoPerfil(
            MultipartFile foto,
            Long idUsuario,
            String username
    ) throws Exception {

        Drive drive = getDrive();

        if (folderId == null || folderId.isBlank()) {
            throw new RuntimeException(
                    "GOOGLE_DRIVE_FOLDER_ID não configurado."
            );
        }

        // =================================================
        // PEGA A EXTENSÃO DA NOVA IMAGEM
        // =================================================

        String nomeOriginal =
                foto.getOriginalFilename();

        String extensao = "";

        if (nomeOriginal != null &&
                nomeOriginal.contains(".")) {

            extensao =
                    nomeOriginal.substring(
                            nomeOriginal.lastIndexOf(".")
                    ).toLowerCase();
        }

        // =================================================
        // NOVO NOME
        //
        // Exemplo:
        // 15@cauebueno_perfil.jpg
        // =================================================

        String nomeArquivo =
                idUsuario +
                        "@" +
                        username +
                        "_perfil" +
                        extensao;


        // =================================================
        // PROCURA QUALQUER FOTO DESSE USUÁRIO
        //
        // Não importa qual username estava no nome antigo.
        //
        // Procura:
        // 15@*_perfil.*
        // =================================================

        String query =
                "'" + folderId + "' in parents " +
                        "and trashed = false";

        FileList resultado =
                drive.files()
                        .list()
                        .setQ(query)
                        .setSpaces("drive")
                        .setFields(
                                "files(id,name,mimeType)"
                        )
                        .execute();

        List<File> arquivos =
                resultado.getFiles();


        File arquivoAntigo = null;

        if (arquivos != null) {

            String prefixo =
                    idUsuario + "@";

            String sufixo =
                    "_perfil";

            for (File arquivo : arquivos) {

                String nome =
                        arquivo.getName();

                if (nome == null) {
                    continue;
                }

                /*
                 * Verifica se o arquivo pertence
                 * ao usuário.
                 *
                 * Exemplos encontrados:
                 *
                 * 15@caue_perfil.jpg
                 * 15@cauebueno_perfil.png
                 */

                if (nome.startsWith(prefixo) &&
                        nome.contains(sufixo + ".")) {

                    arquivoAntigo = arquivo;
                    break;
                }
            }
        }


        // =================================================
        // PREPARA A NOVA IMAGEM
        // =================================================

        InputStreamContent mediaContent =
                new InputStreamContent(
                        foto.getContentType(),
                        foto.getInputStream()
                );

        mediaContent.setLength(
                foto.getSize()
        );


        File arquivo;


        // =================================================
        // SE JÁ EXISTE FOTO → SUBSTITUI
        // =================================================

        if (arquivoAntigo != null) {

            File metadata =
                    new File()
                            .setName(nomeArquivo);

            arquivo =
                    drive.files()
                            .update(
                                    arquivoAntigo.getId(),
                                    metadata,
                                    mediaContent
                            )
                            .setFields(
                                    "id,name,mimeType"
                            )
                            .execute();

        }

        // =================================================
        // SE NÃO EXISTE → CRIA
        // =================================================

        else {

            File metadata =
                    new File();

            metadata.setName(nomeArquivo);

            metadata.setParents(
                    Collections.singletonList(
                            folderId
                    )
            );

            arquivo =
                    drive.files()
                            .create(
                                    metadata,
                                    mediaContent
                            )
                            .setFields(
                                    "id,name,mimeType"
                            )
                            .execute();
        }


        return arquivo.getId();
    }


    // =====================================================
    // MÉTODO ANTIGO DE UPLOAD
    // =====================================================

    public String uploadFoto(
            MultipartFile foto,
            String nomeArquivo
    ) throws Exception {

        Drive drive = getDrive();

        File metadata =
                new File();

        metadata.setName(nomeArquivo);

        metadata.setParents(
                Collections.singletonList(folderId)
        );

        InputStreamContent mediaContent =
                new InputStreamContent(
                        foto.getContentType(),
                        foto.getInputStream()
                );

        mediaContent.setLength(
                foto.getSize()
        );

        File arquivo =
                drive.files()
                        .create(
                                metadata,
                                mediaContent
                        )
                        .setFields(
                                "id,name,mimeType,webViewLink"
                        )
                        .execute();

        return arquivo.getId();
    }


    // =====================================================
    // BAIXAR FOTO DO DRIVE
    // =====================================================

    public byte[] baixarFoto(
            String fileId
    ) throws Exception {

        Drive drive = getDrive();

        try (InputStream inputStream =
                     drive.files()
                             .get(fileId)
                             .executeMediaAsInputStream()) {

            return inputStream.readAllBytes();
        }
    }


    // =====================================================
    // PEGAR MIME TYPE
    // =====================================================

    public String getMimeType(
            String fileId
    ) throws Exception {

        Drive drive = getDrive();

        File arquivo =
                drive.files()
                        .get(fileId)
                        .setFields("mimeType")
                        .execute();

        return arquivo.getMimeType();
    }
    // =====================================================
// RENOMEAR FOTO DE PERFIL NO DRIVE
// =====================================================

    public String renomearFotoPerfil(
            String fileId,
            Long idUsuario,
            String novoUsername
    ) throws Exception {

        Drive drive = getDrive();

        File arquivoAtual =
                drive.files()
                        .get(fileId)
                        .setFields("id,name,mimeType")
                        .execute();

        String nomeAtual = arquivoAtual.getName();

        String extensao = "";

        if (nomeAtual != null &&
                nomeAtual.contains(".")) {

            extensao =
                    nomeAtual.substring(
                            nomeAtual.lastIndexOf(".")
                    );
        }

        String novoNome =
                idUsuario +
                        "@" +
                        novoUsername +
                        "_perfil" +
                        extensao;

        File metadata =
                new File()
                        .setName(novoNome);

        File arquivoRenomeado =
                drive.files()
                        .update(
                                fileId,
                                metadata
                        )
                        .setFields(
                                "id,name,mimeType"
                        )
                        .execute();

        return arquivoRenomeado.getId();
    }
}