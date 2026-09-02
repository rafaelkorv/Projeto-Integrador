package com.backendpi.backend.service;

import java.util.List;

import org.springframework.stereotype.Service;

import com.backendpi.backend.dto.MensagemDTO;
import com.backendpi.backend.model.Conversa;
import com.backendpi.backend.model.Mensagem;
import com.backendpi.backend.model.Usuario;
import com.backendpi.backend.repository.ConversaRepository;
import com.backendpi.backend.repository.MensagemRepository;
import com.backendpi.backend.repository.UsuarioRepository;
import com.backendpi.backend.websocket.ChatWebSocketHandler;

import tools.jackson.databind.ObjectMapper;

@Service
public class MensagemService {

    private final MensagemRepository mensagemRepository;
    private final ConversaRepository conversaRepository;
    private final UsuarioRepository usuarioRepository;
    private final ChatWebSocketHandler chatWebSocketHandler;
    private final ObjectMapper objectMapper;

    public MensagemService(
            MensagemRepository mensagemRepository,
            ConversaRepository conversaRepository,
            UsuarioRepository usuarioRepository,
            ChatWebSocketHandler chatWebSocketHandler,
            ObjectMapper objectMapper
    ) {
        this.mensagemRepository = mensagemRepository;
        this.conversaRepository = conversaRepository;
        this.usuarioRepository = usuarioRepository;
        this.chatWebSocketHandler = chatWebSocketHandler;
        this.objectMapper = objectMapper;
    }

    public MensagemDTO enviarMensagem(
            Long idConversa,
            Long idRemetente,
            String conteudo
    ) {

        if (conteudo == null || conteudo.trim().isEmpty()) {
            throw new RuntimeException(
                    "A mensagem não pode ficar vazia."
            );
        }

        Conversa conversa
                = conversaRepository.findById(idConversa)
                        .orElseThrow(()
                                -> new RuntimeException(
                                "Conversa não encontrada."
                        )
                        );

        Usuario remetente
                = usuarioRepository.findById(idRemetente)
                        .orElseThrow(()
                                -> new RuntimeException(
                                "Usuário não encontrado."
                        )
                        );

        boolean participaDaConversa
                = conversa.getUsuario1()
                        .getIdUsuario()
                        .equals(idRemetente)
                || conversa.getUsuario2()
                        .getIdUsuario()
                        .equals(idRemetente);

        if (!participaDaConversa) {
            throw new RuntimeException(
                    "Usuário não participa desta conversa."
            );
        }

        Mensagem mensagem = new Mensagem();

        mensagem.setConversa(conversa);
        mensagem.setRemetente(remetente);
        mensagem.setConteudo(conteudo.trim());

        mensagem = mensagemRepository.save(mensagem);

        MensagemDTO mensagemDTO
                = new MensagemDTO(mensagem);

        Long idDestinatario;

        if (conversa.getUsuario1()
                .getIdUsuario()
                .equals(idRemetente)) {
            idDestinatario
                    = conversa.getUsuario2()
                            .getIdUsuario();
        } else {
            idDestinatario
                    = conversa.getUsuario1()
                            .getIdUsuario();
        }

        try {

            String mensagemJson
                    = objectMapper.writeValueAsString(
                            mensagemDTO
                    );

            chatWebSocketHandler.enviarParaUsuario(
                    idDestinatario,
                    mensagemJson
            );

        } catch (Exception erro) {

            System.err.println(
                    "Erro ao enviar mensagem pelo WebSocket: "
                    + erro.getMessage()
            );

        }

        return mensagemDTO;
    }

    public List<MensagemDTO> listarMensagens(
            Long idConversa
    ) {

        return mensagemRepository
                .findByConversa_IdConversaOrderByDataEnvioAsc(
                        idConversa
                )
                .stream()
                .map(MensagemDTO::new)
                .toList();
    }
}
