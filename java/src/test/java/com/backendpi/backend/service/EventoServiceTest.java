package com.backendpi.backend.service;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.ArgumentMatchers.anyLong;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.ArgumentMatchers.isNull;
import static org.mockito.Mockito.inOrder;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.util.List;
import java.util.Optional;
import java.util.Set;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.ArgumentCaptor;
import org.mockito.InOrder;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import com.backendpi.backend.model.Evento;
import com.backendpi.backend.model.EventoListaEspera;
import com.backendpi.backend.model.ParticipacaoEvento;
import com.backendpi.backend.repository.EventoListaEsperaRepository;
import com.backendpi.backend.repository.EventoRepository;
import com.backendpi.backend.repository.ParticipacaoEventoRepository;

@ExtendWith(MockitoExtension.class)
class EventoServiceTest {

    @Mock
    private EventoRepository eventoRepository;

    @Mock
    private ParticipacaoEventoRepository participacaoEventoRepository;

    @Mock
    private EventoListaEsperaRepository listaEsperaRepository;

    @Mock
    private NotificacaoEventoService notificacaoService;

    @Mock
    private ParticipacaoEventoService participacaoService;

    @InjectMocks
    private EventoService eventoService;

    @Test
    void deveNotificarInscritosEListaDeEsperaAntesDeExcluirEvento() {
        Evento evento = new Evento();
        evento.setId(5L);
        evento.setCriadorId(1L);
        evento.setTitulo("Feira de Tecnologia");

        ParticipacaoEvento participante = new ParticipacaoEvento();
        participante.setUsuarioId(10L);
        ParticipacaoEvento usuarioTambemNaFila = new ParticipacaoEvento();
        usuarioTambemNaFila.setUsuarioId(11L);
        EventoListaEspera aguardando = new EventoListaEspera();
        aguardando.setUsuarioId(11L);
        EventoListaEspera segundoAguardando = new EventoListaEspera();
        segundoAguardando.setUsuarioId(12L);

        when(eventoRepository.findById(5L)).thenReturn(Optional.of(evento));
        when(participacaoEventoRepository.findByEventoId(5L))
                .thenReturn(List.of(participante, usuarioTambemNaFila));
        when(listaEsperaRepository.findByEventoIdOrderByAdicionadoEmAscIdAsc(5L))
                .thenReturn(List.of(aguardando, segundoAguardando));

        eventoService.deletar(5L, 1L);

        ArgumentCaptor<Long> usuarioCaptor = ArgumentCaptor.forClass(Long.class);
        ArgumentCaptor<Long> eventoCaptor = ArgumentCaptor.forClass(Long.class);
        ArgumentCaptor<String> tipoCaptor = ArgumentCaptor.forClass(String.class);
        ArgumentCaptor<String> mensagemCaptor = ArgumentCaptor.forClass(String.class);
        verify(notificacaoService, times(3)).notificarUsuario(
                usuarioCaptor.capture(),
                eventoCaptor.capture(),
                tipoCaptor.capture(),
                mensagemCaptor.capture()
        );

        assertEquals(Set.of(10L, 11L, 12L), Set.copyOf(usuarioCaptor.getAllValues()));
        assertTrue(eventoCaptor.getAllValues().stream().allMatch(id -> id == null));
        assertTrue(tipoCaptor.getAllValues().stream().allMatch("EVENTO_EXCLUIDO"::equals));
        assertTrue(mensagemCaptor.getAllValues().stream()
                .allMatch(mensagem -> mensagem.contains("Feira de Tecnologia")));

        InOrder ordem = inOrder(notificacaoService, eventoRepository);
        ordem.verify(notificacaoService, times(3)).notificarUsuario(
                anyLong(),
                isNull(),
                eq("EVENTO_EXCLUIDO"),
                anyString()
        );
        ordem.verify(eventoRepository).delete(evento);
    }
}
