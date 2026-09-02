using System;

namespace RedeSocialEventosAdmin.Models
{
  public class DashboardStats
  {
    public int TotalUsuarios { get; set; }
    public int UsuariosHoje { get; set; }
    public int TotalAdmins { get; set; }
    public int TotalSuspensos { get; set; }

    public int TotalEventos { get; set; }
    public int EventosAgendados { get; set; }
    public int TotalInscricoesEventos { get; set; }

    public int TotalComunidades { get; set; }

    public int TotalPosts { get; set; }
    public int TotalComentarios { get; set; }
  }
}