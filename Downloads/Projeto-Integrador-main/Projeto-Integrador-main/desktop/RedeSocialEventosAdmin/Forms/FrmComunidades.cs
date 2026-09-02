using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RedeSocialEventosAdmin.DAO;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmComunidades : Form
  {
    private readonly ComunidadeDAO _comunidadeDAO;

    public FrmComunidades()
    {
      InitializeComponent();
      _comunidadeDAO = new ComunidadeDAO();
    }

    private void FrmComunidades_Load(object sender, EventArgs e)
    {
      AtualizarGrid();
    }

    private void AtualizarGrid()
    {
      try
      {
        string termo = txtPesquisa.Text.Trim();
        DataTable dt = string.IsNullOrEmpty(termo) ? _comunidadeDAO.Listar() : _comunidadeDAO.Pesquisar(termo);

        dgvComunidades.DataSource = dt;
        lblTotalRegistros.Text = $"Exibindo {dt.Rows.Count} comunidade(s) ativa(s).";
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao carregar lista de comunidades: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void txtPesquisa_TextChanged(object sender, EventArgs e) => AtualizarGrid();

    private void btnRefresh_Click(object sender, EventArgs e)
    {
      txtPesquisa.Clear();
      AtualizarGrid();
    }

    private void btnNovo_Click(object sender, EventArgs e)
    {
      using (FrmComunidadeModal frm = new FrmComunidadeModal())
      {
        if (frm.ShowDialog() == DialogResult.OK)
        {
          AtualizarGrid();
        }
      }
    }

    private void btnEditar_Click(object sender, EventArgs e)
    {
      if (dgvComunidades.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione uma comunidade para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      long id = Convert.ToInt64(dgvComunidades.SelectedRows[0].Cells["ID"].Value);
      Comunidade? comunidade = _comunidadeDAO.BuscarPorId(id);

      if (comunidade != null)
      {
        using (FrmComunidadeModal frm = new FrmComunidadeModal(comunidade))
        {
          if (frm.ShowDialog() == DialogResult.OK)
          {
            AtualizarGrid();
          }
        }
      }
    }

    private void btnMembros_Click(object sender, EventArgs e)
    {
      if (dgvComunidades.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione uma comunidade para visualizar os membros.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      long id = Convert.ToInt64(dgvComunidades.SelectedRows[0].Cells["ID"].Value);
      string nome = dgvComunidades.SelectedRows[0].Cells["Nome da Comunidade"].Value?.ToString() ?? "";

      try
      {
        DataTable membros = _comunidadeDAO.ListarMembros(id);
        if (membros.Rows.Count == 0)
        {
          MessageBox.Show($"Nenhum membro inscrito na comunidade '{nome}'.", "Membros", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        Form frmMembros = new Form();
        frmMembros.Text = $"Membros da Comunidade: {nome} ({membros.Rows.Count})";
        frmMembros.Size = new Size(700, 420);
        frmMembros.StartPosition = FormStartPosition.CenterParent;
        frmMembros.BackColor = Color.FromArgb(248, 250, 252);

        DataGridView grid = new DataGridView();
        grid.Dock = DockStyle.Fill;
        grid.DataSource = membros;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        frmMembros.Controls.Add(grid);
        frmMembros.ShowDialog();
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao carregar membros: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnExcluir_Click(object sender, EventArgs e)
    {
      if (dgvComunidades.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione uma comunidade para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      long id = Convert.ToInt64(dgvComunidades.SelectedRows[0].Cells["ID"].Value);
      string nome = dgvComunidades.SelectedRows[0].Cells["Nome da Comunidade"].Value?.ToString() ?? "";

      DialogResult resultado = MessageBox.Show($" ATENÇÃO ADMINISTRADOR\n\nA exclusão da comunidade '{nome}' (ID {id}) removerá:\n- Todas as publicações e comentários feitos dentro dela\n- Todos os votos e interações associados\n- Vínculos de todos os membros\n\nDeseja prosseguir com a exclusão em cascata?", "Excluir Comunidade", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

      if (resultado == DialogResult.Yes)
      {
        try
        {
          if (_comunidadeDAO.Excluir(id))
          {
            MessageBox.Show("Comunidade excluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AtualizarGrid();
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Erro ao excluir comunidade: {ex.Message}", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }
  }
}
