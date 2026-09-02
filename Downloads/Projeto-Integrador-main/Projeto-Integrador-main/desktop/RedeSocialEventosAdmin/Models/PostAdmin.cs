using System;

namespace RedeSocialEventosAdmin.Models
{
  public class PostAdmin
  {
    public long IdPost { get; set; }
    public string? Titulo { get; set; }
    public string? Conteudo { get; set; }
    public long IdUsuario { get; set; }
    public string? AutorNome { get; set; }
    public string? AutorEmail { get; set; }
    public long? IdComunidade { get; set; }
    public string? NomeComunidade { get; set; }
    public DateTime DataPostagem { get; set; }
    public int TotalComentarios { get; set; }
    public int TotalVotos { get; set; }
  }

  public class ComentarioAdmin
  {
    public long IdComentario { get; set; }
    public string? Conteudo { get; set; }
    public long IdUsuario { get; set; }
    public string? AutorNome { get; set; }
    public string? AutorEmail { get; set; }
    public long? IdPost { get; set; }
    public string? TituloPost { get; set; }
    public DateTime DataComentario { get; set; }
  }
}
