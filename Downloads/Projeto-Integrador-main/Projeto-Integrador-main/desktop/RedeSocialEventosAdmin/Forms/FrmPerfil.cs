using System;
using System.Windows.Forms;
using RedeSocialEventosAdmin.DAO;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmPerfil : Form
  {
    private readonly UsuarioDAO _usuarioDAO;

    public FrmPerfil()
    {
      InitializeComponent();
      _usuarioDAO = new UsuarioDAO();
    }

    private void FrmPerfil_Load(object sender, EventArgs e)
    {
      CarregarDadosPerfil();
    }

    private void CarregarDadosPerfil()
    {
      try
      {
        Usuario? admin = Program.UsuarioLogado;

        if (admin == null && !string.IsNullOrEmpty(Program.EmailUsuarioLogado))
        {
          admin = _usuarioDAO.BuscarPorEmail(Program.EmailUsuarioLogado);
          Program.UsuarioLogado = admin;
        }

        if (admin != null)
        {
          lblNomeAdmin.Text = string.IsNullOrWhiteSpace(admin.NomeCompleto) ? (admin.Nome ?? admin.Username) : admin.NomeCompleto;
          lblUsernameAdmin.Text = "@" + admin.Username;
          lblEmailAdmin.Text = admin.Email;
          chipRoleBadge.Text = admin.Role.ToUpper();
          chipStatusBadge.Text = admin.Status.ToUpper();
          lblDataCriacaoAdmin.Text = $"Membro desde: {admin.DataCriacao:dd/MM/yyyy}";
        }
        else
        {
          lblNomeAdmin.Text = "Super Administrador";
          lblUsernameAdmin.Text = "@adm";
          lblEmailAdmin.Text = Program.EmailUsuarioLogado;
          chipRoleBadge.Text = "ADMIN";
          chipStatusBadge.Text = "ATIVO";
          lblDataCriacaoAdmin.Text = $"Membro desde: {DateTime.Now:dd/MM/yyyy}";
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao carregar dados do perfil: {ex.Message}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
  }
}
