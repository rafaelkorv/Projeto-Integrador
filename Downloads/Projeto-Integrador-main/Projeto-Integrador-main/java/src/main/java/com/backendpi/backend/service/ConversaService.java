package com.backendpi.backend.service;

import java.util.List;

import org.springframework.stereotype.Service;

import com.backendpi.backend.dto.ConversaDTO;
import com.backendpi.backend.model.Conversa;
import com.backendpi.backend.model.Usuario;
import com.backendpi.backend.repository.ConversaRepository;
import com.backendpi.backend.repository.UsuarioRepository;

@Service
public class ConversaService {

    private final ConversaRepository conversaRepository;
    private final UsuarioRepository usuarioRepository;

    public ConversaService(
            ConversaRepository conversaRepository,
            UsuarioRepository usuarioRepository
    ) {
        this.conversaRepository = conversaRepository;
        this.usuarioRepository = usuarioRepository;
    }

    public ConversaDTO criarOuBuscarConversa(
            Long idUsuarioA,
            Long idUsuarioB
    ) {

        if (idUsuarioA == null || idUsuarioB == null) {
            throw new RuntimeException("Os usuários são obrigatórios.");
        }

        if (idUsuarioA.equals(idUsuarioB)) {
            throw new RuntimeException(
                    "Não é possível criar uma conversa consigo mesmo."
            );
        }

        /*
         * Mantemos sempre o menor ID em usuario1
         * e o maior em usuario2.
         */
        Long idUsuario1 = Math.min(idUsuarioA, idUsuarioB);
        Long idUsuario2 = Math.max(idUsuarioA, idUsuarioB);

        Conversa conversaExistente
                = conversaRepository
                        .findByUsuario1_IdUsuarioAndUsuario2_IdUsuario(
                                idUsuario1,
                                idUsuario2
                        )
                        .orElse(null);

        if (conversaExistente != null) {
            return new ConversaDTO(
                    conversaExistente,
                    idUsuarioA
            );
        }

        Usuario usuario1
                = usuarioRepository.findById(idUsuario1)
                        .orElseThrow(()
                                -> new RuntimeException(
                                "Usuário não encontrado."
                        )
                        );

        Usuario usuario2
                = usuarioRepository.findById(idUsuario2)
                        .orElseThrow(()
                                -> new RuntimeException(
                                "Usuário não encontrado."
                        )
                        );

        Conversa conversa = new Conversa();

        conversa.setUsuario1(usuario1);
        conversa.setUsuario2(usuario2);

        conversa = conversaRepository.save(conversa);

        return new ConversaDTO(
                conversa,
                idUsuarioA
        );
    }

    public List<ConversaDTO> listarConversasDoUsuario(
            Long idUsuario
    ) {

        return conversaRepository
                .findByUsuario1_IdUsuarioOrUsuario2_IdUsuario(
                        idUsuario,
                        idUsuario
                )
                .stream()
                .map(conversa
                        -> new ConversaDTO(
                        conversa,
                        idUsuario
                )
                )
                .toList();
    }
}
