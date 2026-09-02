using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RedeSocialEventosAdmin.DAO;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmModeracao : Form
  {
    private readonly ModeracaoDAO _moderacaoDAO;
    private bool _modoPosts = true;

    public FrmModeracao()
    {
      InitializeComponent();
      _moderacaoDAO = new ModeracaoDAO();
    }

    private void FrmModeracao_Load(object sender, EventArgs e)
    {
      AtualizarGrid();
    }

    private void btnTabPosts_Click(object sender, EventArgs e)
    {
      _modoPosts = true;
      btnTabPosts.FillColor = Color.FromArgb(79, 70, 229);
      btnTabPosts.ForeColor = Color.White;
      btnTabComentarios.FillColor = Color.FromArgb(241, 245, 249);
      btnTabComentarios.ForeColor = Color.FromArgb(51, 65, 85);
      AtualizarGrid();
    }

    private void btnTabComentarios_Click(object sender, EventArgs e)
    {
      _modoPosts = false;
      btnTabComentarios.FillColor = Color.FromArgb(79, 70, 229);
      btnTabComentarios.ForeColor = Color.White;
      btnTabPosts.FillColor = Color.FromArgb(241, 245, 249);
      btnTabPosts.ForeColor = Color.FromArgb(51, 65, 85);
      AtualizarGrid();
    }

    private void AtualizarGrid()
    {
      try
      {
        string termo = txtPesquisa.Text.Trim();
        DataTable dt;

        if (_modoPosts)
        {
          dt = _moderacaoDAO.ListarPosts(termo);
          lblTotalRegistros.Text = $"Exibindo {dt.Rows.Count} publicações encontradas na rede.";
        }
        else
        {
          dt = _moderacaoDAO.ListarComentarios(termo);
          lblTotalRegistros.Text = $"Exibindo {dt.Rows.Count} comentários encontrados na rede.";
        }

        dgvConteudo.DataSource = dt;
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao carregar itens para moderação: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void txtPesquisa_TextChanged(object sender, EventArgs e) => AtualizarGrid();

    private void btnRefresh_Click(object sender, EventArgs e)
    {
      txtPesquisa.Clear();
      AtualizarGrid();
    }

    private void btnExcluir_Click(object sender, EventArgs e)
    {
      if (dgvConteudo.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione um item na tabela para realizar a exclusão.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      if (_modoPosts)
      {
        long idPost = Convert.ToInt64(dgvConteudo.SelectedRows[0].Cells["ID Post"].Value);
        string autor = dgvConteudo.SelectedRows[0].Cells["Autor"].Value?.ToString() ?? "";
        string titulo = dgvConteudo.SelectedRows[0].Cells[1].Value?.ToString() ?? "";

        DialogResult result = MessageBox.Show($" AÇÃO DE MODERAÇÃO ADMINISTRATIVA\n\nDeseja EXCLUIR DEFINITIVAMENTE a publicação:\n'{titulo}' de @{autor} (ID {idPost})?\n\nTodos os comentários e votos vinculados a este post serão removidos.", "Excluir Post", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
          try
          {
            if (_moderacaoDAO.ExcluirPost(idPost))
            {
              MessageBox.Show("Publicação excluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
              AtualizarGrid();
            }
          }
          catch (Exception ex)
          {
            MessageBox.Show($"Erro ao excluir publicação: {ex.Message}", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
      }
      else
      {
        long idComentario = Convert.ToInt64(dgvConteudo.SelectedRows[0].Cells["ID Comentário"].Value);
        string autor = dgvConteudo.SelectedRows[0].Cells["Autor"].Value?.ToString() ?? "";

        DialogResult result = MessageBox.Show($"Deseja remover o comentário #{idComentario} de @{autor}?", "Excluir Comentário", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
          try
          {
            if (_moderacaoDAO.ExcluirComentario(idComentario))
            {
              MessageBox.Show("Comentário excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
              AtualizarGrid();
            }
          }
          catch (Exception ex)
          {
            MessageBox.Show($"Erro ao excluir comentário: {ex.Message}", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
      }
    }
  }
}
