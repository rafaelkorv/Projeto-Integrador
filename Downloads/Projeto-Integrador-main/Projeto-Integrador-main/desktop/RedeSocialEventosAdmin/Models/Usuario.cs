using System;

namespace RedeSocialEventosAdmin.Models
{
  public class Usuario
  {
    public long IdUsuario { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Nome { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public DateTime? DataNascimento { get; set; }
    public string? Bio { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataExclusao { get; set; }
    public string? FotoPerfil { get; set; }
    public string Status { get; set; } = "ativo"; // 'ativo', 'inativo', 'suspenso', 'deletando'
    public string Role { get; set; } = "user"; // SET: 'user','tester','betatester','premium','moderator','admin'

    public bool IsAdmin => !string.IsNullOrEmpty(Role) && Role.Contains("admin", StringComparison.OrdinalIgnoreCase);
  }
}