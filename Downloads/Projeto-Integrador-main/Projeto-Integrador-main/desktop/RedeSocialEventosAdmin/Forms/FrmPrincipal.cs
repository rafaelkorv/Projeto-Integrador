using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmPrincipal : Form
  {
    private IconButton? btnAtual;
    private Form? frmAtivo;

    public FrmPrincipal()
    {
      InitializeComponent();
    }

    private void FrmPrincipal_Load(object sender, EventArgs e)
    {
      CarregarUsuarioHeader();
      MudarTela(btnDashboard, new FrmDashboard());
    }

    private void CarregarUsuarioHeader()
    {
      Usuario? admin = Program.UsuarioLogado;
      if (admin != null)
      {
        lblAdminHeaderNome.Text = string.IsNullOrWhiteSpace(admin.NomeCompleto) ? (admin.Nome ?? admin.Username) : admin.NomeCompleto;
        chipHeaderRole.Text = admin.Role.ToUpper();
      }
      else
      {
        lblAdminHeaderNome.Text = "Administrador";
        chipHeaderRole.Text = "SUPER ADMIN";
      }
    }

    private void AtivarBotao(object? remetente)
    {
      if (remetente != null)
      {
        DesativarBotao();
        btnAtual = (IconButton)remetente;
        btnAtual.BackColor = Color.FromArgb(79, 70, 229); // Cor primária Indigo (#4F46E5)
        btnAtual.ForeColor = Color.White;
        btnAtual.IconColor = Color.White;
      }
    }

    private void DesativarBotao()
    {
      foreach (Control btnMenu in pnlMenuLateral.Controls)
      {
        if (btnMenu is IconButton btn && btn != btnSair)
        {
          btn.BackColor = Color.FromArgb(15, 23, 42); // Obsidian Slate (#0F172A)
          btn.ForeColor = Color.FromArgb(148, 163, 184); // Slate 400
          btn.IconColor = Color.FromArgb(148, 163, 184);
        }
      }
    }

    private void MudarTela(IconButton botao, Form novoForm)
    {
      AtivarBotao(botao);

      if (frmAtivo != null)
      {
        frmAtivo.Close();
        pnlConteudo.Controls.Remove(frmAtivo);
        frmAtivo.Dispose();
      }

      frmAtivo = novoForm;
      novoForm.TopLevel = false;
      novoForm.FormBorderStyle = FormBorderStyle.None;
      novoForm.Dock = DockStyle.Fill;
      pnlConteudo.Controls.Add(novoForm);
      pnlConteudo.Tag = novoForm;
      novoForm.BringToFront();
      novoForm.Show();
      lblTituloJanela.Text = botao.Text.Trim();
    }

    private void btnDashboard_Click(object sender, EventArgs e) => MudarTela(btnDashboard, new FrmDashboard());
    private void btnUsuarios_Click(object sender, EventArgs e) => MudarTela(btnUsuarios, new FrmUsuarios());
    private void btnEventos_Click(object sender, EventArgs e) => MudarTela(btnEventos, new FrmEventos());
    private void btnComunidades_Click(object sender, EventArgs e) => MudarTela(btnComunidades, new FrmComunidades());
    private void btnModeracao_Click(object sender, EventArgs e) => MudarTela(btnModeracao, new FrmModeracao());
    private void btnRelatorios_Click(object sender, EventArgs e) => MudarTela(btnRelatorios, new FrmRelatorios());
    private void btnPerfil_Click(object sender, EventArgs e) => MudarTela(btnPerfil, new FrmPerfil());

    private void btnSair_Click(object sender, EventArgs e)
    {
      DialogResult result = MessageBox.Show("Deseja realmente desconectar e sair do painel administrativo?", "Encerrar Sessão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      if (result == DialogResult.Yes)
      {
        Application.Exit();
      }
    }
  }
}
