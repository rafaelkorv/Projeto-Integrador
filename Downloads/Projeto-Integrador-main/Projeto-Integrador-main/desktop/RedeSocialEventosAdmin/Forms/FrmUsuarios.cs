using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RedeSocialEventosAdmin.DAO;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmUsuarios : Form
  {
    private readonly UsuarioDAO _usuarioDAO;

    public FrmUsuarios()
    {
      InitializeComponent();
      _usuarioDAO = new UsuarioDAO();
    }

    private void FrmUsuarios_Load(object sender, EventArgs e)
    {
      AtualizarGrid();
    }

    private string? ObterFiltroRole()
    {
      if (cmbFiltroRole.SelectedIndex <= 0) return null;
      return cmbFiltroRole.SelectedItem?.ToString();
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
        string? role = ObterFiltroRole();
        string? status = ObterFiltroStatus();
        string termo = txtPesquisa.Text.Trim();

        DataTable dt;
        if (string.IsNullOrEmpty(termo))
        {
          dt = _usuarioDAO.Listar(role, status);
        }
        else
        {
          dt = _usuarioDAO.Pesquisar(termo, role, status);
        }

        dgvUsuarios.DataSource = dt;
        lblTotalRegistros.Text = $"Exibindo {dt.Rows.Count} conta(s) cadastrada(s) no sistema.";
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao carregar lista de usuários: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void txtPesquisa_TextChanged(object sender, EventArgs e)
    {
      AtualizarGrid();
    }

    private void Filtros_Changed(object sender, EventArgs e)
    {
      AtualizarGrid();
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
      txtPesquisa.Clear();
      cmbFiltroRole.SelectedIndex = 0;
      cmbFiltroStatus.SelectedIndex = 0;
      AtualizarGrid();
    }

    private void btnNovo_Click(object sender, EventArgs e)
    {
      using (FrmUsuarioMockCadastro frmCad = new FrmUsuarioMockCadastro())
      {
        if (frmCad.ShowDialog() == DialogResult.OK)
        {
          AtualizarGrid();
        }
      }
    }

    private void btnEditar_Click(object sender, EventArgs e)
    {
      if (dgvUsuarios.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione um usuário na tabela para realizar a edição.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      long idSelecionado = Convert.ToInt64(dgvUsuarios.SelectedRows[0].Cells["ID"].Value);
      Usuario? userParaEditar = _usuarioDAO.BuscarPorId(idSelecionado);

      if (userParaEditar != null)
      {
        using (FrmUsuarioMockCadastro frmEdit = new FrmUsuarioMockCadastro(userParaEditar))
        {
          if (frmEdit.ShowDialog() == DialogResult.OK)
          {
            AtualizarGrid();
          }
        }
      }
    }

    private void btnStatusToggle_Click(object sender, EventArgs e)
    {
      if (dgvUsuarios.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione um usuário para alterar o status.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      long id = Convert.ToInt64(dgvUsuarios.SelectedRows[0].Cells["ID"].Value);
      string user = dgvUsuarios.SelectedRows[0].Cells["Usuário"].Value?.ToString() ?? "";
      string statusAtual = dgvUsuarios.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "ativo";

      string novoStatus = statusAtual.Equals("suspenso", StringComparison.OrdinalIgnoreCase) ? "ativo" : "suspenso";
      string acaoNome = novoStatus == "suspenso" ? "SUSPENDER" : "REATIVAR";

      DialogResult confirm = MessageBox.Show($"Deseja realmente {acaoNome} a conta do usuário '@{user}'?", "Confirmação de Status", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      if (confirm == DialogResult.Yes)
      {
        try
        {
          if (_usuarioDAO.AlterarStatus(id, novoStatus))
          {
            MessageBox.Show($"Status alterado para '{novoStatus.ToUpper()}' com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AtualizarGrid();
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Erro ao atualizar status: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void btnExcluir_Click(object sender, EventArgs e)
    {
      if (dgvUsuarios.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione um usuário na tabela para realizar a exclusão.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      long idSelecionado = Convert.ToInt64(dgvUsuarios.SelectedRows[0].Cells["ID"].Value);
      string nomeSelecionado = dgvUsuarios.SelectedRows[0].Cells["Usuário"].Value?.ToString() ?? "";

      DialogResult resultado = MessageBox.Show($" ATENÇÃO - AÇÃO ADMINISTRATIVA DEFINITIVA\n\nA exclusão do usuário '@{nomeSelecionado}' (ID {idSelecionado}) removerá permanentemente:\n- Todos os posts e fotos publicados\n- Todos os comentários e votos\n- Vínculos de comunidades e inscrições em eventos\n\nDeseja prosseguir com a exclusão em cascata?", "Confirmação de Exclusão Absoluta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

      if (resultado == DialogResult.Yes)
      {
        try
        {
          if (_usuarioDAO.Excluir(idSelecionado))
          {
            MessageBox.Show("Usuário e todas as dependências foram deletados com sucesso do banco de dados.", "Exclusão Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AtualizarGrid();
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Erro na exclusão: {ex.Message}", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }
  }
}
