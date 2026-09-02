package com.backendpi.backend.websocket;

import java.net.URI;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

import org.springframework.stereotype.Component;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;

@Component
public class ChatWebSocketHandler
        extends TextWebSocketHandler {

    private final Map<Long, WebSocketSession> sessoes
            = new ConcurrentHashMap<>();

    @Override
    public void afterConnectionEstablished(
            WebSocketSession session
    ) throws Exception {

        Long usuarioId
                = extrairUsuarioId(session);

        if (usuarioId == null) {
            session.close(
                    CloseStatus.BAD_DATA
            );
            return;
        }

        sessoes.put(
                usuarioId,
                session
        );

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

            if (partes.length == 2
                    && partes[0].equals("usuarioId")) {

                try {
                    return Long.valueOf(
                            partes[1]
                    );
                } catch (NumberFormatException erro) {
                    return null;
                }
            }
        }

        return null;
    }
}
