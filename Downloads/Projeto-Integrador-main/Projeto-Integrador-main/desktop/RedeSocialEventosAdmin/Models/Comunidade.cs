using System;

namespace RedeSocialEventosAdmin.Models
{
  public class Comunidade
  {
    public long IdComunidade { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataCriacao { get; set; }
    public long? CriadorId { get; set; }
    public string? NomeCriador { get; set; }
    public string? Categoria { get; set; }
    public string Cor { get; set; } = "#EA3F74";
    public string? ImagemComunidade { get; set; }
    public int TotalMembros { get; set; }
    public int TotalPosts { get; set; }
  }
}
