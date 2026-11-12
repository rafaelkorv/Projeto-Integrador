package com.backendpi.backend.service;

import java.math.BigDecimal;
import java.math.RoundingMode;

import org.springframework.stereotype.Service;

@Service
public class PagamentoService {

    private static final BigDecimal TAXA_PERCENTUAL = new BigDecimal("0.08");
    private static final BigDecimal TAXA_FIXA = new BigDecimal("2.00");

    public ResultadoCalculo calcularTaxaEValorLiquido(BigDecimal valorIngresso) {
        return calcularTaxaEValorLiquido(valorIngresso, false);
    }

    public ResultadoCalculo calcularTaxaEValorLiquido(
            BigDecimal valorIngresso,
            boolean taxaRepassadaComprador) {

        if (valorIngresso == null || valorIngresso.signum() <= 0) {
            throw new IllegalArgumentException(
                    "O valor do ingresso pago deve ser maior que zero"
            );
        }

        BigDecimal taxaPercentual = valorIngresso
                .multiply(TAXA_PERCENTUAL)
                .setScale(2, RoundingMode.HALF_UP);
        BigDecimal taxaPlataforma = taxaPercentual.add(TAXA_FIXA);
        BigDecimal valorTotalComprador = taxaRepassadaComprador
                ? valorIngresso.add(taxaPlataforma).setScale(2, RoundingMode.HALF_UP)
                : valorIngresso;
        BigDecimal valorLiquidoOrganizador = valorIngresso
                .subtract(taxaRepassadaComprador
                        ? BigDecimal.ZERO
                        : taxaPlataforma)
                .setScale(2, RoundingMode.HALF_UP);

        return new ResultadoCalculo(
                valorIngresso.setScale(2, RoundingMode.HALF_UP),
                taxaPlataforma,
                valorTotalComprador,
                valorLiquidoOrganizador
        );
    }

    public record ResultadoCalculo(
            BigDecimal subtotal,
            BigDecimal taxaPlataforma,
            BigDecimal totalComprador,
            BigDecimal valorLiquidoOrganizador) {
    }
}
