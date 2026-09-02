using System;

namespace RedeSocialEventosAdmin.Models
{
  public class RelatorioEventoModel
  {
    public long IdEvento { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime DataEvento { get; set; }
    public string LocalEvento { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string CriadorNome { get; set; } = string.Empty;

    public int LimiteParticipantes { get; set; }
    public int TotalInscritos { get; set; }
    public int VagasRestantes => Math.Max(0, LimiteParticipantes - TotalInscritos);
    public double TaxaOcupacaoPercentual => LimiteParticipantes > 0 ? Math.Min(100.0, Math.Round(((double)TotalInscritos / LimiteParticipantes) * 100.0, 1)) : 100.0;

    public int TotalCheckins { get; set; }
    public double TaxaComparecimentoPercentual => TotalInscritos > 0 ? Math.Round(((double)TotalCheckins / TotalInscritos) * 100.0, 1) : 0.0;
  }
}