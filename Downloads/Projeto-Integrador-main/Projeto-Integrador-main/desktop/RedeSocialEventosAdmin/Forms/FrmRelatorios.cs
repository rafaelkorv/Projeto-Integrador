using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using RedeSocialEventosAdmin.DAO;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmRelatorios : Form
  {
    private enum ModoRelatorio
    {
      Usuarios,
      Eventos,
      Comunidades
    }

    private readonly RelatorioDAO _relatorioDAO;
    private readonly UsuarioDAO _usuarioDAO;
    private readonly EventoDAO _eventoDAO;
    private readonly ComunidadeDAO _comunidadeDAO;

    private ModoRelatorio _modoAtual = ModoRelatorio.Usuarios;
    private RelatorioUsuarioModel? _usuarioFichaAtual = null;

    public FrmRelatorios()
    {
      InitializeComponent();
      _relatorioDAO = new RelatorioDAO();
      _usuarioDAO = new UsuarioDAO();
      _eventoDAO = new EventoDAO();
      _comunidadeDAO = new ComunidadeDAO();
    }

    private void FrmRelatorios_Load(object sender, EventArgs e)
    {
      CarregarModoUsuarios();
    }

    #region Navegação de Abas Principais

    private void btnTabRelatoriosComuns_Click(object sender, EventArgs e)
    {
      btnTabRelatoriosComuns.FillColor = Color.FromArgb(79, 70, 229);
      btnTabRelatoriosComuns.ForeColor = Color.White;
      btnTabAnalyticsApp.FillColor = Color.FromArgb(241, 245, 249);
      btnTabAnalyticsApp.ForeColor = Color.FromArgb(51, 65, 85);

      pnlRelatoriosComuns.Visible = true;
      pnlAnalyticsApp.Visible = false;
    }

    private void btnTabAnalyticsApp_Click(object sender, EventArgs e)
    {
      btnTabAnalyticsApp.FillColor = Color.FromArgb(79, 70, 229);
      btnTabAnalyticsApp.ForeColor = Color.White;
      btnTabRelatoriosComuns.FillColor = Color.FromArgb(241, 245, 249);
      btnTabRelatoriosComuns.ForeColor = Color.FromArgb(51, 65, 85);

      pnlRelatoriosComuns.Visible = false;
      pnlAnalyticsApp.Visible = true;

      CarregarAnalyticsEngajamento();
    }

    #endregion

    #region Relatórios Comuns (Usuários, Eventos, Comunidades)

    private void btnSubUsuarios_Click(object sender, EventArgs e)
    {
      _modoAtual = ModoRelatorio.Usuarios;
      AtualizarBotoesSubMenu(btnSubUsuarios);
      CarregarModoUsuarios();
    }

    private void btnSubEventos_Click(object sender, EventArgs e)
    {
      _modoAtual = ModoRelatorio.Eventos;
      AtualizarBotoesSubMenu(btnSubEventos);
      CarregarModoEventos();
    }

    private void btnSubComunidades_Click(object sender, EventArgs e)
    {
      _modoAtual = ModoRelatorio.Comunidades;
      AtualizarBotoesSubMenu(btnSubComunidades);
      CarregarModoComunidades();
    }

    private void AtualizarBotoesSubMenu(Guna.UI2.WinForms.Guna2Button btnAtivo)
    {
      btnSubUsuarios.FillColor = Color.FromArgb(241, 245, 249);
      btnSubUsuarios.ForeColor = Color.FromArgb(51, 65, 85);
      btnSubEventos.FillColor = Color.FromArgb(241, 245, 249);
      btnSubEventos.ForeColor = Color.FromArgb(51, 65, 85);
      btnSubComunidades.FillColor = Color.FromArgb(241, 245, 249);
      btnSubComunidades.ForeColor = Color.FromArgb(51, 65, 85);

      btnAtivo.FillColor = Color.FromArgb(79, 70, 229);
      btnAtivo.ForeColor = Color.White;
    }

    private void CarregarModoUsuarios()
    {
      pnlFichaIndividual.Visible = false;
      dgvRelatorioComum.Location = new Point(0, 100);
      dgvRelatorioComum.Height = pnlRelatoriosComuns.Height - 100;

      btnVerFichaIndividual.Text = " Ver Ficha Individual";
      btnExportarIndividual.Visible = true;

      // Popular combobox com usuários
      cmbSelecaoItem.Items.Clear();
      DataTable dtUsers = _usuarioDAO.Listar();
      foreach (DataRow row in dtUsers.Rows)
      {
        cmbSelecaoItem.Items.Add(new ItemCombo { Id = Convert.ToInt64(row["ID"]), Descricao = $"{row["ID"]} - @{row[1]} ({row[2]})" });
      }
      if (cmbSelecaoItem.Items.Count > 0) cmbSelecaoItem.SelectedIndex = 0;

      // Popular grid consolidado
      DataTable dtConsol = _relatorioDAO.ObterRelatorioConsolidadoUsuarios();
      dgvRelatorioComum.DataSource = dtConsol;
    }

    private void CarregarModoEventos()
    {
      pnlFichaIndividual.Visible = false;
      dgvRelatorioComum.Location = new Point(0, 100);
      dgvRelatorioComum.Height = pnlRelatoriosComuns.Height - 100;

      btnVerFichaIndividual.Text = " Ver Detalhes Evento";
      btnExportarIndividual.Visible = true;

      // Popular combobox com eventos
      cmbSelecaoItem.Items.Clear();
      DataTable dtEventos = _eventoDAO.Listar();
      foreach (DataRow row in dtEventos.Rows)
      {
        cmbSelecaoItem.Items.Add(new ItemCombo { Id = Convert.ToInt64(row["ID"]), Descricao = $"{row["ID"]} - {row[1]} [{row["Status"]}]" });
      }
      if (cmbSelecaoItem.Items.Count > 0) cmbSelecaoItem.SelectedIndex = 0;

      // Popular grid consolidado
      DataTable dtConsol = _relatorioDAO.ObterRelatorioConsolidadoEventos();
      dgvRelatorioComum.DataSource = dtConsol;
    }

    private void CarregarModoComunidades()
    {
      pnlFichaIndividual.Visible = false;
      dgvRelatorioComum.Location = new Point(0, 100);
      dgvRelatorioComum.Height = pnlRelatoriosComuns.Height - 100;

      btnVerFichaIndividual.Text = " Ver Detalhes Comunidade";
      btnExportarIndividual.Visible = true;

      // Popular combobox com comunidades
      cmbSelecaoItem.Items.Clear();
      DataTable dtComunidades = _comunidadeDAO.Listar();
      foreach (DataRow row in dtComunidades.Rows)
      {
        cmbSelecaoItem.Items.Add(new ItemCombo { Id = Convert.ToInt64(row["ID"]), Descricao = $"{row["ID"]} - {row[1]} ({row["Categoria"]})" });
      }
      if (cmbSelecaoItem.Items.Count > 0) cmbSelecaoItem.SelectedIndex = 0;

      // Popular grid consolidado
      DataTable dtConsol = _relatorioDAO.ObterRelatorioConsolidadoComunidades();
      dgvRelatorioComum.DataSource = dtConsol;
    }

    private void cmbSelecaoItem_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (pnlFichaIndividual.Visible)
      {
        ExibirFichaIndividual();
      }
    }

    private void btnVerFichaIndividual_Click(object sender, EventArgs e)
    {
      if (cmbSelecaoItem.SelectedItem == null)
      {
        MessageBox.Show("Selecione um registro na lista suspensa acima.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      pnlFichaIndividual.Visible = true;
      dgvRelatorioComum.Location = new Point(0, 255);
      dgvRelatorioComum.Height = pnlRelatoriosComuns.Height - 255;

      ExibirFichaIndividual();
    }

    private void ExibirFichaIndividual()
    {
      if (cmbSelecaoItem.SelectedItem is not ItemCombo item) return;

      if (_modoAtual == ModoRelatorio.Usuarios)
      {
        _usuarioFichaAtual = _relatorioDAO.ObterRelatorioIndividualUsuario(item.Id);
        if (_usuarioFichaAtual != null)
        {
          lblFichaNome.Text = $"{_usuarioFichaAtual.NomeCompleto} (@{_usuarioFichaAtual.Username})";
          chipFichaRole.Text = _usuarioFichaAtual.Role.ToUpper();
          chipFichaStatus.Text = _usuarioFichaAtual.Status.ToUpper();
          chipFichaScore.Text = $"â­ Score: {_usuarioFichaAtual.ScoreEngajamento} pts";

          lblFichaInfo1.Text = $"E-mail: {_usuarioFichaAtual.Email} | Telefone: {(_usuarioFichaAtual.Telefone != "" ? _usuarioFichaAtual.Telefone : "Não informado")} | Cadastro: {_usuarioFichaAtual.DataCriacao:dd/MM/yyyy}";
          lblFichaInfo2.Text = $"Eventos: {_usuarioFichaAtual.EventosCriados} Criados | {_usuarioFichaAtual.EventosInscritos} Inscritos | {_usuarioFichaAtual.CheckinsRealizados} Check-ins ({_usuarioFichaAtual.TaxaPresencaPercentual}% presença)";
          lblFichaInfo3.Text = $"Engajamento de Conteúdo: {_usuarioFichaAtual.TotalPosts} Publicações | {_usuarioFichaAtual.TotalComentarios} Comentários | {_usuarioFichaAtual.TotalVotos} Votos";
          
          string listaCom = _usuarioFichaAtual.ComunidadesNomes.Count > 0 ? string.Join(", ", _usuarioFichaAtual.ComunidadesNomes) : "Nenhuma";
          lblFichaInfo4.Text = $"Comunidades ({_usuarioFichaAtual.TotalComunidades}): {listaCom}";
        }
      }
      else if (_modoAtual == ModoRelatorio.Eventos)
      {
        Evento? ev = _eventoDAO.BuscarPorId(item.Id);
        if (ev != null)
        {
          DataTable part = _eventoDAO.ListarParticipantes(item.Id);
          int inscritos = part.Rows.Count;
          int limite = ev.LimiteParticipantes ?? 0;
          double ocupacao = limite > 0 ? Math.Round(((double)inscritos / limite) * 100.0, 1) : 100.0;

          lblFichaNome.Text = $"Evento #{ev.IdEvento}: {ev.Titulo}";
          chipFichaRole.Text = (ev.Categoria ?? "GERAL").ToUpper();
          chipFichaStatus.Text = ev.Status.ToUpper();
          chipFichaScore.Text = $" Ocupação: {ocupacao}%";

          lblFichaInfo1.Text = $"Data: {ev.DataEvento:dd/MM/yyyy} ({ev.HorarioInicio:hh\\:mm} às {(ev.HorarioFim.HasValue ? ev.HorarioFim.Value.ToString(@"hh\:mm") : "--")}) | Local: {ev.LocalEvento}";
          lblFichaInfo2.Text = $"Capacidade: {limite} pessoas | Inscritos: {inscritos} | Vagas Restantes: {Math.Max(0, limite - inscritos)}";
          lblFichaInfo3.Text = $"Criador ID: #{ev.CriadorId ?? 0} | Exige Check-in: {(ev.ExigeCheckin ? "SIM" : "NÃO")}";
          string desc = ev.Descricao ?? "Sem descrição";
          lblFichaInfo4.Text = $"Descrição: {(desc.Length > 80 ? desc.Substring(0, 77) + "..." : desc)}";
        }
      }
      else if (_modoAtual == ModoRelatorio.Comunidades)
      {
        Comunidade? com = _comunidadeDAO.BuscarPorId(item.Id);
        if (com != null)
        {
          DataTable membros = _comunidadeDAO.ListarMembros(item.Id);
          int totalMembros = membros.Rows.Count;

          lblFichaNome.Text = $"Comunidade #{com.IdComunidade}: {com.Nome}";
          chipFichaRole.Text = (com.Categoria ?? "GERAL").ToUpper();
          chipFichaStatus.Text = "ATIVA";
          chipFichaScore.Text = $" {totalMembros} Membros";

          lblFichaInfo1.Text = $"Identidade Visual (Cor): {com.Cor} | Data de Criação: {com.DataCriacao:dd/MM/yyyy}";
          lblFichaInfo2.Text = $"Total de Membros Inscritos: {totalMembros} usuários";
          lblFichaInfo3.Text = $"Criador ID: #{com.CriadorId}";
          string desc = com.Descricao ?? "Sem descrição";
          lblFichaInfo4.Text = $"Descrição: {(desc.Length > 80 ? desc.Substring(0, 77) + "..." : desc)}";
        }
      }
    }

    private void btnExportarIndividual_Click(object sender, EventArgs e)
    {
      if (cmbSelecaoItem.SelectedItem is not ItemCombo item)
      {
        MessageBox.Show("Selecione um item para exportar seu relatório individual.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      using (SaveFileDialog sfd = new SaveFileDialog())
      {
        string safeName = item.Descricao.Replace(" ", "_").Replace("/", "-");
        sfd.Filter = "Arquivo de Texto (*.txt)|*.txt|Arquivo CSV (*.csv)|*.csv";
        sfd.FileName = $"Relatorio_Individual_{_modoAtual}_{item.Id}_{DateTime.Now:yyyyMMdd_HHmm}.txt";

        if (sfd.ShowDialog() == DialogResult.OK)
        {
          try
          {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("================================================================================");
            sb.AppendLine($"      RELATÓRIO INDIVIDUAL - SOCIALJOIN ({_modoAtual.ToString().ToUpper()})      ");
            sb.AppendLine("================================================================================");
            sb.AppendLine($"Data da Emissão: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine($"Emitido por: {Program.UsuarioLogado?.NomeCompleto ?? "Super Administrador"} (@{Program.UsuarioLogado?.Username ?? "admin"})");
            sb.AppendLine("--------------------------------------------------------------------------------");

            if (_modoAtual == ModoRelatorio.Usuarios)
            {
              var u = _relatorioDAO.ObterRelatorioIndividualUsuario(item.Id);
              if (u != null)
              {
                sb.AppendLine($"ID do Usuário: {u.IdUsuario}");
                sb.AppendLine($"Username: @{u.Username}");
                sb.AppendLine($"Nome Completo: {u.NomeCompleto}");
                sb.AppendLine($"E-mail: {u.Email}");
                sb.AppendLine($"Telefone: {u.Telefone}");
                sb.AppendLine($"Role de Acesso: {u.Role}");
                sb.AppendLine($"Status da Conta: {u.Status}");
                sb.AppendLine($"Data de Cadastro: {u.DataCriacao:dd/MM/yyyy HH:mm:ss}");
                sb.AppendLine();
                sb.AppendLine("--- MÉTRICAS DE PARTICIPAÇÃO E ENGAJAMENTO ---");
                sb.AppendLine($"Score Ponderado de Engajamento: {u.ScoreEngajamento} pontos");
                sb.AppendLine($"Eventos Criados pelo Usuário: {u.EventosCriados}");
                sb.AppendLine($"Eventos em que se Inscreveu: {u.EventosInscritos}");
                sb.AppendLine($"Check-ins Confirmados em Eventos: {u.CheckinsRealizados}");
                sb.AppendLine($"Taxa de Presença nos Eventos: {u.TaxaPresencaPercentual}%");
                sb.AppendLine($"Total de Comunidades Participantes: {u.TotalComunidades}");
                sb.AppendLine($"Lista de Comunidades: {(u.ComunidadesNomes.Count > 0 ? string.Join(", ", u.ComunidadesNomes) : "Nenhuma")}");
                sb.AppendLine($"Publicações (Posts) Criadas: {u.TotalPosts}");
                sb.AppendLine($"Comentários Feitos: {u.TotalComentarios}");
                sb.AppendLine($"Reações e Votos: {u.TotalVotos}");
              }
            }
            else if (_modoAtual == ModoRelatorio.Eventos)
            {
              Evento? ev = _eventoDAO.BuscarPorId(item.Id);
              if (ev != null)
              {
                DataTable part = _eventoDAO.ListarParticipantes(item.Id);
                int limite = ev.LimiteParticipantes ?? 0;
                sb.AppendLine($"ID do Evento: {ev.IdEvento}");
                sb.AppendLine($"Título: {ev.Titulo}");
                sb.AppendLine($"Categoria: {ev.Categoria}");
                sb.AppendLine($"Status: {ev.Status}");
                sb.AppendLine($"Data: {ev.DataEvento:dd/MM/yyyy}");
                sb.AppendLine($"Horário: {ev.HorarioInicio:hh\\:mm} às {(ev.HorarioFim.HasValue ? ev.HorarioFim.Value.ToString(@"hh\:mm") : "--")}");
                sb.AppendLine($"Local: {ev.LocalEvento}");
                sb.AppendLine($"Capacidade Máxima: {limite}");
                sb.AppendLine($"Inscritos Confirmados: {part.Rows.Count}");
                sb.AppendLine($"Vagas Restantes: {Math.Max(0, limite - part.Rows.Count)}");
                sb.AppendLine($"Descrição: {ev.Descricao ?? "Sem descrição"}");
              }
            }
            else if (_modoAtual == ModoRelatorio.Comunidades)
            {
              Comunidade? com = _comunidadeDAO.BuscarPorId(item.Id);
              if (com != null)
              {
                DataTable mem = _comunidadeDAO.ListarMembros(item.Id);
                sb.AppendLine($"ID da Comunidade: {com.IdComunidade}");
                sb.AppendLine($"Nome: {com.Nome}");
                sb.AppendLine($"Categoria: {com.Categoria}");
                sb.AppendLine($"Cor do Tema: {com.Cor}");
                sb.AppendLine($"Total de Membros: {mem.Rows.Count}");
                sb.AppendLine($"Data de Criação: {com.DataCriacao:dd/MM/yyyy}");
                sb.AppendLine($"Descrição: {com.Descricao ?? "Sem descrição"}");
              }
            }

            sb.AppendLine("================================================================================");
            File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Relatório individual exportado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
          catch (Exception ex)
          {
            MessageBox.Show($"Erro ao exportar relatório: {ex.Message}", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
      }
    }

    private void btnExportarGeral_Click(object sender, EventArgs e)
    {
      if (dgvRelatorioComum.DataSource is not DataTable dt || dt.Rows.Count == 0)
      {
        MessageBox.Show("Nenhum dado disponível na tabela para exportação.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      using (SaveFileDialog sfd = new SaveFileDialog())
      {
        sfd.Filter = "Arquivo CSV (*.csv)|*.csv";
        sfd.FileName = $"SocialJoin_{_modoAtual}_{DateTime.Now:yyyyMMdd_HHmm}.csv";

        if (sfd.ShowDialog() == DialogResult.OK)
        {
          try
          {
            StringBuilder sb = new StringBuilder();

            // Cabeçalhos
            for (int i = 0; i < dt.Columns.Count; i++)
            {
              sb.Append(dt.Columns[i].ColumnName);
              if (i < dt.Columns.Count - 1) sb.Append(";");
            }
            sb.AppendLine();

            // Linhas
            foreach (DataRow row in dt.Rows)
            {
              for (int i = 0; i < dt.Columns.Count; i++)
              {
                sb.Append(row[i]?.ToString()?.Replace(";", ","));
                if (i < dt.Columns.Count - 1) sb.Append(";");
              }
              sb.AppendLine();
            }

            File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Tabela consolidada exportada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
          catch (Exception ex)
          {
            MessageBox.Show($"Erro ao exportar tabela: {ex.Message}", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
      }
    }

    #endregion

    #region Analytics & Engajamento Global da Rede

    private void CarregarAnalyticsEngajamento()
    {
      try
      {
        // 1. Gráfico de Donut: Distribuição de Roles
        var rolesData = _relatorioDAO.ObterDadosGraficoRoles();
        chartDonutRoles.SetData(rolesData);

        // 2. Gráfico de Barras: Top Eventos (Inscritos vs Capacidade)
        var eventosData = _relatorioDAO.ObterDadosGraficoTopEventos(5);
        chartBarEventos.SetData(eventosData);

        // 3. Gráfico de Barras: Top Comunidades (Membros vs Posts)
        var comunidadesData = _relatorioDAO.ObterDadosGraficoTopComunidades(5);
        chartBarComunidades.SetData(comunidadesData);

        // 4. Métricas do Top Bar
        DashboardStats stats = new DashboardDAO().ObterEstatisticasGerais();
        lblKpiEngajamentoSub.Text = $"Consolidação em tempo real: {stats.TotalUsuarios} usuários ({stats.TotalAdmins} admins), {stats.TotalEventos} eventos ({stats.TotalInscricoesEventos} participações), {stats.TotalComunidades} comunidades e {stats.TotalPosts + stats.TotalComentarios} interações totais.";
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao atualizar gráficos de engajamento: {ex.Message}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void btnExportarAnalyticsCsv_Click(object sender, EventArgs e)
    {
      using (SaveFileDialog sfd = new SaveFileDialog())
      {
        sfd.Filter = "Arquivo CSV (*.csv)|*.csv";
        sfd.FileName = $"SocialJoin_Analytics_Global_{DateTime.Now:yyyyMMdd_HHmm}.csv";

        if (sfd.ShowDialog() == DialogResult.OK)
        {
          try
          {
            StringBuilder sb = new StringBuilder();
            DashboardStats stats = new DashboardDAO().ObterEstatisticasGerais();

            sb.AppendLine("RELATÓRIO DE ANALYTICS E ENGAJAMENTO GLOBAL - SOCIALJOIN");
            sb.AppendLine($"Data da Extração;{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Total de Usuários;{stats.TotalUsuarios}");
            sb.AppendLine($"Usuários Cadastrados Hoje;{stats.UsuariosHoje}");
            sb.AppendLine($"Administradores Master;{stats.TotalAdmins}");
            sb.AppendLine($"Usuários Suspensos;{stats.TotalSuspensos}");
            sb.AppendLine($"Total de Eventos;{stats.TotalEventos}");
            sb.AppendLine($"Eventos Agendados;{stats.EventosAgendados}");
            sb.AppendLine($"Inscrições em Eventos;{stats.TotalInscricoesEventos}");
            sb.AppendLine($"Total de Comunidades;{stats.TotalComunidades}");
            sb.AppendLine($"Total de Publicações;{stats.TotalPosts}");
            sb.AppendLine($"Total de Comentários;{stats.TotalComentarios}");
            sb.AppendLine();

            sb.AppendLine("DISTRIBUIÇÃO DE ROLES");
            var roles = _relatorioDAO.ObterDadosGraficoRoles();
            foreach (var r in roles)
            {
              sb.AppendLine($"{r.Item1};{r.Item2}");
            }

            File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Relatório de Analytics exportado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
          catch (Exception ex)
          {
            MessageBox.Show($"Erro ao exportar Analytics: {ex.Message}", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
      }
    }

    #endregion

    private class ItemCombo
    {
      public long Id { get; set; }
      public string Descricao { get; set; } = string.Empty;
      public override string ToString() => Descricao;
    }
  }
}