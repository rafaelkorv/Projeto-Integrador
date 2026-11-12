package com.backendpi.backend.websocket;

import java.net.URI;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

import org.springframework.stereotype.Component;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;

import com.backendpi.backend.security.AuthSessionService;

@Component
public class ChatWebSocketHandler
        extends TextWebSocketHandler {

    private final Map<Long, WebSocketSession> sessoes
            = new ConcurrentHashMap<>();

    private final AuthSessionService authSessionService;

    public ChatWebSocketHandler(AuthSessionService authSessionService) {
        this.authSessionService = authSessionService;
    }

    @Override
    public void afterConnectionEstablished(
            WebSocketSession session
    ) throws Exception {

        Long usuarioId = extrairUsuarioId(session);
        Long usuarioAutenticado = authSessionService
                .buscarUsuarioId(extrairToken(session))
                .orElse(null);

        if (usuarioId == null || !usuarioId.equals(usuarioAutenticado)) {
            session.close(
                    new CloseStatus(1008, "Sessão inválida para o usuário solicitado")
            );
            return;
        }

        WebSocketSession sessaoAnterior = sessoes.put(
                usuarioId,
                session
        );

        if (sessaoAnterior != null
                && sessaoAnterior.isOpen()
                && !sessaoAnterior.getId().equals(session.getId())) {
            sessaoAnterior.close(
                    new CloseStatus(
                            1000,
                            "Conexão substituída por uma sessão mais recente"
                    )
            );
        }

        System.out.println(
                "WebSocket conectado - usuário: "
                + usuarioId
        );
    }

    @Override
    public void afterConnectionClosed(
            WebSocketSession session,
            CloseStatus status
    ) throws Exception {

        sessoes.entrySet().removeIf(
                entrada
                -> entrada.getValue()
                        .getId()
                        .equals(session.getId())
        );

        System.out.println(
                "WebSocket desconectado: "
                + session.getId()
                + " - código: " + status.getCode()
                + (status.getReason() == null || status.getReason().isBlank()
                        ? ""
                        : " - motivo: " + status.getReason())
        );
    }

    public void enviarParaUsuario(
            Long usuarioId,
            String mensagem
    ) throws Exception {

        WebSocketSession session
                = sessoes.get(usuarioId);

        if (session != null
                && session.isOpen()) {

            session.sendMessage(
                    new org.springframework.web.socket.TextMessage(
                            mensagem
                    )
            );
        }
    }

    private Long extrairUsuarioId(
            WebSocketSession session
    ) {
        String valor = extrairParametro(session, "usuarioId");
        if (valor == null) {
            return null;
        }
        try {
            return Long.valueOf(valor);
        } catch (NumberFormatException erro) {
            return null;
        }
    }

    private String extrairToken(WebSocketSession session) {
        return extrairParametro(session, "token");
    }

    private String extrairParametro(WebSocketSession session, String nome) {
        URI uri = session.getUri();

        if (uri == null
                || uri.getQuery() == null) {
            return null;
        }

        String[] parametros
                = uri.getQuery().split("&");

        for (String parametro : parametros) {

            String[] partes
                    = parametro.split("=");

            if (partes.length == 2 && partes[0].equals(nome)) {
                return partes[1];
            }
        }

        return null;
    }
}
