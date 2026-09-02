using System;
using System.Text.RegularExpressions;

namespace RedeSocialEventosAdmin.Utils
{
  public static class Validador
  {
    /// <summary>
    /// Valida se uma string de e-mail possui um formato eletrônico válido.
    /// </summary>
    public static bool ValidarEmail(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        return false;

      try
      {
        // Expressão regular padrão para validação de e-mail (RFC 5322)
        string modeloEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, modeloEmail, RegexOptions.IgnoreCase);
      }
      catch (RegexMatchTimeoutException)
      {
        return false;
      }
    }

    /// <summary>
    /// Valida se a senha atende ao requisito mínimo de 6 caracteres.
    /// </summary>
    public static bool ValidarSenha(string senha)
    {
      if (string.IsNullOrEmpty(senha))
        return false;

      return senha.Length >= 6;
    }

    /// <summary>
    /// Verifica se um campo de texto obrigatório está vazio ou preenchido apenas com espaços.
    /// </summary>
    public static bool CampoVazio(string texto)
    {
      return string.IsNullOrWhiteSpace(texto);
    }
  }
}