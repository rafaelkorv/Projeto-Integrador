package com.backendpi.backend.controller;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.server.ResponseStatusException;

import com.backendpi.backend.model.Evento;
import com.backendpi.backend.repository.EventoRepository;
import com.backendpi.backend.service.PagamentoService;

@RestController
@RequestMapping("/api/pagamentos")
@CrossOrigin(origins = {"http://127.0.0.1:5500", "http://localhost:5500"})
public class PagamentoController {

    private final PagamentoService pagamentoService;
    private final EventoRepository eventoRepository;

    public PagamentoController(
            PagamentoService pagamentoService,
            EventoRepository eventoRepository) {
        this.pagamentoService = pagamentoService;
        this.eventoRepository = eventoRepository;
    }

    @GetMapping("/eventos/{idEvento}/calcular")
    public PagamentoService.ResultadoCalculo calcular(
            @PathVariable Long idEvento) {
        Evento evento = eventoRepository.findById(idEvento)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Evento não encontrado"
                ));

        if (evento.getPrecoIngresso() == null
                || evento.getPrecoIngresso().signum() <= 0) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "O evento não possui ingresso pago"
            );
        }

        return pagamentoService.calcularTaxaEValorLiquido(
                evento.getPrecoIngresso(),
                Boolean.TRUE.equals(evento.getTaxaRepassadaComprador())
        );
    }
}
