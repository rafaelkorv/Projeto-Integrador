using System;
using System.Windows.Forms;
using RedeSocialEventosAdmin.DAO;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmDashboard : Form
  {
    private readonly DashboardDAO _dashboardDAO;

    public FrmDashboard()
    {
      InitializeComponent();
      _dashboardDAO = new DashboardDAO();
    }

    private void FrmDashboard_Load(object sender, EventArgs e)
    {
      CarregarDashboard();
    }

    private void CarregarDashboard()
    {
      try
      {
        DashboardStats stats = _dashboardDAO.ObterEstatisticasGerais();

        lblTotalUsuarios.Text = stats.TotalUsuarios.ToString("N0");
        lblUsuariosSub.Text = $"+{stats.UsuariosHoje} hoje | {stats.TotalAdmins} admins";

        lblTotalEventos.Text = stats.TotalEventos.ToString("N0");
        lblEventosSub.Text = $"{stats.EventosAgendados} agendados | {stats.TotalInscricoesEventos} inscrições";

        lblTotalComunidades.Text = stats.TotalComunidades.ToString("N0");
        lblComunidadesSub.Text = "Grupos e canais ativos";

        lblTotalPosts.Text = stats.TotalPosts.ToString("N0");
        lblConteudoSub.Text = $"{stats.TotalComentarios} comentários";

        dgvProximosEventos.DataSource = _dashboardDAO.ObterProximosEventos(7);
        dgvUltimosUsuarios.DataSource = _dashboardDAO.ObterUltimosUsuarios(7);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Falha ao carregar métricas do dashboard: {ex.Message}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
      CarregarDashboard();
    }
  }
}
