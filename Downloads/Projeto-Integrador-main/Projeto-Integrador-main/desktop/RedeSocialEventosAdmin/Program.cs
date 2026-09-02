using System;
using System.Windows.Forms;
using RedeSocialEventosAdmin.Forms;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin
{
  public static class Program
  {
    /// <summary>
    /// Usuário administrador autenticado na sessão do painel.
    /// </summary>
    public static Usuario? UsuarioLogado { get; set; }

    public static string EmailUsuarioLogado
    {
      get => UsuarioLogado?.Email ?? string.Empty;
      set
      {
        if (UsuarioLogado == null)
          UsuarioLogado = new Usuario { Email = value };
        else
          UsuarioLogado.Email = value;
      }
    }

    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      Application.Run(new FrmLogin());
    }
  }
}
