package com.backendpi.backend.config;

import org.springframework.context.annotation.Configuration;
import org.springframework.web.socket.config.annotation.EnableWebSocket;
import org.springframework.web.socket.config.annotation.WebSocketConfigurer;
import org.springframework.web.socket.config.annotation.WebSocketHandlerRegistry;

import com.backendpi.backend.websocket.ChatWebSocketHandler;

@Configuration
@EnableWebSocket
public class WebSocketConfig
        implements WebSocketConfigurer {

    private final ChatWebSocketHandler chatWebSocketHandler;

    public WebSocketConfig(
            ChatWebSocketHandler chatWebSocketHandler
    ) {
        this.chatWebSocketHandler
                = chatWebSocketHandler;
    }

    @Override
    public void registerWebSocketHandlers(
            WebSocketHandlerRegistry registry
    ) {

        registry
                .addHandler(
                        chatWebSocketHandler,
                        "/ws/chat"
                )
                .setAllowedOrigins(
                        "http://127.0.0.1:5500",
                        "http://localhost:5500"
                );
    }
}
