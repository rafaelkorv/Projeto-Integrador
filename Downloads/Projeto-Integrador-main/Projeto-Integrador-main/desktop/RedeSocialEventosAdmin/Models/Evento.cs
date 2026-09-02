using System;

namespace RedeSocialEventosAdmin.Models
{
  public class Evento
  {
    public long IdEvento { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataEvento { get; set; }
    public TimeSpan HorarioInicio { get; set; }
    public TimeSpan? HorarioFim { get; set; }
    public DateTime? EncerramentoInscricoes { get; set; }
    public string LocalEvento { get; set; } = string.Empty;
    public long? ComunidadeId { get; set; }
    public string? NomeComunidade { get; set; }
    public long? CriadorId { get; set; }
    public string? NomeCriador { get; set; }
    public int? LimiteParticipantes { get; set; }
    public string Status { get; set; } = "AGENDADO"; // 'AGENDADO', 'ACONTECENDO_AGORA', 'ENCERRADO', 'CANCELADO'
    public bool ExigeCheckin { get; set; }
    public string? Categoria { get; set; }
    public string? ImagemCapa { get; set; }
    public int TotalParticipantes { get; set; }
  }
}
