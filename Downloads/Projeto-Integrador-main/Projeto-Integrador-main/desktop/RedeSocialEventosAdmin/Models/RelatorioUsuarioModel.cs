using System;
using System.Collections.Generic;

namespace RedeSocialEventosAdmin.Models
{
  public class RelatorioUsuarioModel
  {
    public long IdUsuario { get; set; }
    public string Username { get; set; } = string.Empty;
    public string NomeCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }

    // Métricas de Eventos
    public int EventosCriados { get; set; }
    public int EventosInscritos { get; set; }
    public int CheckinsRealizados { get; set; }
    public double TaxaPresencaPercentual => EventosInscritos > 0 ? Math.Round(((double)CheckinsRealizados / EventosInscritos) * 100.0, 1) : 0.0;

    // Métricas de Comunidades
    public int TotalComunidades { get; set; }
    public List<string> ComunidadesNomes { get; set; } = new List<string>();

    // Métricas de Conteúdo & Engajamento
    public int TotalPosts { get; set; }
    public int TotalComentarios { get; set; }
    public int TotalVotos { get; set; }

    // Pontuação Ponderada de Engajamento
    public int ScoreEngajamento => (TotalPosts * 5) + (TotalComentarios * 3) + (EventosInscritos * 4) + (CheckinsRealizados * 6) + (TotalComunidades * 2);
  }
}