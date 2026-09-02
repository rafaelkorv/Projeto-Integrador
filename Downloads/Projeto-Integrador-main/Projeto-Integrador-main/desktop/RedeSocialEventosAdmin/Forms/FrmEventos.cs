using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RedeSocialEventosAdmin.DAO;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmEventos : Form
  {
    private readonly EventoDAO _eventoDAO;

    public FrmEventos()
    {
      InitializeComponent();
      _eventoDAO = new EventoDAO();
    }

    private void FrmEventos_Load(object sender, EventArgs e)
    {
      AtualizarGrid();
    }

    private string? ObterFiltroStatus()
    {
      if (cmbFiltroStatus.SelectedIndex <= 0) return null;
      return cmbFiltroStatus.SelectedItem?.ToString();
    }

    private void AtualizarGrid()
    {
      try
      {
        string? status = ObterFiltroStatus();
        string termo = txtPesquisa.Text.Trim();

        DataTable dt;
        if (string.IsNullOrEmpty(termo))
        {
          dt = _eventoDAO.Listar(status);
        }
        else
        {
          dt = _eventoDAO.Pesquisar(termo, status);
        }

        dgvEventos.DataSource = dt;
        lblTotalRegistros.Text = $"Exibindo {dt.Rows.Count} evento(s) no sistema.";
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao carregar lista de eventos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void txtPesquisa_TextChanged(object sender, EventArgs e) => AtualizarGrid();
    private void Filtros_Changed(object sender, EventArgs e) => AtualizarGrid();

    private void btnRefresh_Click(object sender, EventArgs e)
    {
      txtPesquisa.Clear();
      cmbFiltroStatus.SelectedIndex = 0;
      AtualizarGrid();
    }

    private void btnNovo_Click(object sender, EventArgs e)
    {
      using (FrmEventoModal frm = new FrmEventoModal())
      {
        if (frm.ShowDialog() == DialogResult.OK)
        {
          AtualizarGrid();
        }
      }
    }

    private void btnEditar_Click(object sender, EventArgs e)
    {
      if (dgvEventos.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione um evento na tabela para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      long id = Convert.ToInt64(dgvEventos.SelectedRows[0].Cells["ID"].Value);
      Evento? evento = _eventoDAO.BuscarPorId(id);

      if (evento != null)
      {
        using (FrmEventoModal frm = new FrmEventoModal(evento))
        {
          if (frm.ShowDialog() == DialogResult.OK)
          {
            AtualizarGrid();
          }
        }
      }
    }

    private void btnStatusToggle_Click(object sender, EventArgs e)
    {
      if (dgvEventos.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione um evento para alterar seu status.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      long id = Convert.ToInt64(dgvEventos.SelectedRows[0].Cells["ID"].Value);
      string statusAtual = dgvEventos.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "AGENDADO";

      string novoStatus = "AGENDADO";
      if (statusAtual == "AGENDADO") novoStatus = "ACONTECENDO_AGORA";
      else if (statusAtual == "ACONTECENDO_AGORA") novoStatus = "ENCERRADO";
      else if (statusAtual == "ENCERRADO") novoStatus = "CANCELADO";
      else novoStatus = "AGENDADO";

      DialogResult confirm = MessageBox.Show($"Alterar status do Evento #{id} de '{statusAtual}' para '{novoStatus}'?", "Confirmar Alteração de Status", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      if (confirm == DialogResult.Yes)
      {
        try
        {
          if (_eventoDAO.AlterarStatus(id, novoStatus))
          {
            MessageBox.Show($"Status do evento #{id} alterado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AtualizarGrid();
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Erro ao alterar status: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void btnParticipantes_Click(object sender, EventArgs e)
    {
      if (dgvEventos.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione um evento para visualizar os participantes inscritos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      long id = Convert.ToInt64(dgvEventos.SelectedRows[0].Cells["ID"].Value);
      string titulo = dgvEventos.SelectedRows[0].Cells[1].Value?.ToString() ?? "";

      try
      {
        DataTable part = _eventoDAO.ListarParticipantes(id);
        if (part.Rows.Count == 0)
        {
          MessageBox.Show($"Nenhum participante inscrito ainda no evento '{titulo}'.", "Inscrições", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        // Exibir modal simples com Grid de participantes
        Form frmPart = new Form();
        frmPart.Text = $"Inscritos no Evento: {titulo} ({part.Rows.Count})";
        frmPart.Size = new Size(750, 450);
        frmPart.StartPosition = FormStartPosition.CenterParent;
        frmPart.BackColor = Color.FromArgb(248, 250, 252);

        DataGridView grid = new DataGridView();
        grid.Dock = DockStyle.Fill;
        grid.DataSource = part;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        frmPart.Controls.Add(grid);
        frmPart.ShowDialog();
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao carregar participantes: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnExcluir_Click(object sender, EventArgs e)
    {
      if (dgvEventos.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione um evento na tabela para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      long id = Convert.ToInt64(dgvEventos.SelectedRows[0].Cells["ID"].Value);
      string titulo = dgvEventos.SelectedRows[0].Cells[1].Value?.ToString() ?? "";

      DialogResult resultado = MessageBox.Show($" ATENÇÃO ADMINISTRADOR\n\nDeseja EXCLUIR DEFINITIVAMENTE o evento:\n'{titulo}' (ID {id})?\n\nTodas as inscrições dos usuários neste evento serão canceladas e removidas permanentemente.", "Confirmação de Exclusão de Evento", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

      if (resultado == DialogResult.Yes)
      {
        try
        {
          if (_eventoDAO.Excluir(id))
          {
            MessageBox.Show("Evento e todas as inscrições foram excluídos com sucesso.", "Exclusão Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AtualizarGrid();
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Erro ao excluir evento: {ex.Message}", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }
  }
}
