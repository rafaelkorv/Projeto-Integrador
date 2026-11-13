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
    private final WebConfig webConfig;

    public WebSocketConfig(
            ChatWebSocketHandler chatWebSocketHandler,
            WebConfig webConfig
    ) {
        this.chatWebSocketHandler
                = chatWebSocketHandler;
        this.webConfig = webConfig;
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
                .setAllowedOrigins(webConfig.getAllowedOrigins());
    }
}
