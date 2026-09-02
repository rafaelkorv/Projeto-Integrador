using System;

namespace RedeSocialEventosAdmin.Models
{
  public class EstatisticaUsuario
  {
    public long IdEstatistica { get; set; }
    public long IdUsuario { get; set; }
    public int TempoTotalUso { get; set; }
    public int PostsVisualizados { get; set; }
    public int PostsCriados { get; set; }
    public int ComentariosFeitos { get; set; }
    public int VotosRealizados { get; set; }
    public DateTime UltimoAcesso { get; set; }
  }
}