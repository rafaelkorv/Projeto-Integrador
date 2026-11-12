package com.backendpi.backend.service;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

import java.math.BigDecimal;

import org.junit.jupiter.api.Test;

class PagamentoServiceTest {

    private final PagamentoService pagamentoService = new PagamentoService();

    @Test
    void deveCalcularTaxaERepasseDoIngresso() {
        PagamentoService.ResultadoCalculo resultado =
                pagamentoService.calcularTaxaEValorLiquido(
                        new BigDecimal("100.00"),
                        false
                );

        assertEquals(new BigDecimal("90.00"), resultado.valorLiquidoOrganizador());
        assertEquals(new BigDecimal("10.00"), resultado.taxaPlataforma());
        assertEquals(new BigDecimal("100.00"), resultado.totalComprador());
    }

    @Test
    void deveArredondarTaxaPercentualParaCentavos() {
        PagamentoService.ResultadoCalculo resultado =
                pagamentoService.calcularTaxaEValorLiquido(
                        new BigDecimal("19.99"),
                        false
                );

        assertEquals(new BigDecimal("16.39"), resultado.valorLiquidoOrganizador());
        assertEquals(new BigDecimal("3.60"), resultado.taxaPlataforma());
    }

    @Test
    void deveRepassarTaxaAoCompradorSemReduzirRepasseAoOrganizador() {
        PagamentoService.ResultadoCalculo resultado =
                pagamentoService.calcularTaxaEValorLiquido(
                        new BigDecimal("100.00"),
                        true
                );

        assertEquals(new BigDecimal("100.00"), resultado.subtotal());
        assertEquals(new BigDecimal("10.00"), resultado.taxaPlataforma());
        assertEquals(new BigDecimal("110.00"), resultado.totalComprador());
        assertEquals(new BigDecimal("100.00"), resultado.valorLiquidoOrganizador());
    }

    @Test
    void taxaRepassadaSomaAoSubtotalComArredondamentoMonetario() {
        PagamentoService.ResultadoCalculo resultado =
                pagamentoService.calcularTaxaEValorLiquido(
                        new BigDecimal("19.99"),
                        true
                );

        assertEquals(new BigDecimal("3.60"), resultado.taxaPlataforma());
        assertEquals(new BigDecimal("23.59"), resultado.totalComprador());
        assertEquals(new BigDecimal("19.99"), resultado.valorLiquidoOrganizador());
    }

    @Test
    void deveRejeitarIngressoSemValorPagoPositivo() {
        assertThrows(
                IllegalArgumentException.class,
                () -> pagamentoService.calcularTaxaEValorLiquido(BigDecimal.ZERO, true)
        );
        assertThrows(
                IllegalArgumentException.class,
                () -> pagamentoService.calcularTaxaEValorLiquido(null, false)
        );
    }
}
