using System;

namespace RedeSocialEventosAdmin.Models
{
  public class RelatorioComunidadeModel
  {
    public long IdComunidade { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public string CriadorNome { get; set; } = string.Empty;

    public int TotalMembros { get; set; }
    public int TotalPosts { get; set; }
    public int TotalComentarios { get; set; }
    public double MediaComentariosPorPost => TotalPosts > 0 ? Math.Round((double)TotalComentarios / TotalPosts, 1) : 0.0;
    public int IndiceAtividade => (TotalMembros * 2) + (TotalPosts * 5) + (TotalComentarios * 3);
  }
}