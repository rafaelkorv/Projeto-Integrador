using System;
using System.Drawing;
using System.Windows.Forms;
using RedeSocialEventosAdmin.DAO;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmComunidadeModal : Form
  {
    private readonly ComunidadeDAO _comunidadeDAO;
    private readonly Comunidade? _comunidadeExistente;
    private readonly bool _isEdicao;

    public FrmComunidadeModal()
    {
      InitializeComponent();
      _comunidadeDAO = new ComunidadeDAO();
      _isEdicao = false;
      _comunidadeExistente = null;
      lblOperacao.Text = "Criar Nova Comunidade";
    }

    public FrmComunidadeModal(Comunidade comunidade)
    {
      InitializeComponent();
      _comunidadeDAO = new ComunidadeDAO();
      _comunidadeExistente = comunidade;
      _isEdicao = true;
      lblOperacao.Text = $"Editar Comunidade #{comunidade.IdComunidade}";
      PreencherCampos();
    }

    private void PreencherCampos()
    {
      if (_comunidadeExistente == null) return;

      txtNome.Text = _comunidadeExistente.Nome;
      txtCategoria.Text = _comunidadeExistente.Categoria ?? "";
      txtCor.Text = _comunidadeExistente.Cor ?? "#EA3F74";
      txtDescricao.Text = _comunidadeExistente.Descricao ?? "";
      txtImagem.Text = _comunidadeExistente.ImagemComunidade ?? "";
      AtualizarCorPreview();
    }

    private void txtCor_TextChanged(object sender, EventArgs e)
    {
      AtualizarCorPreview();
    }

    private void AtualizarCorPreview()
    {
      try
      {
        string corHex = txtCor.Text.Trim();
        if (!corHex.StartsWith("#")) corHex = "#" + corHex;
        pnlCorPreview.BackColor = ColorTranslator.FromHtml(corHex);
      }
      catch
      {
        pnlCorPreview.BackColor = Color.FromArgb(234, 63, 116);
      }
    }

    private void btnSalvar_Click(object sender, EventArgs e)
    {
      string nome = txtNome.Text.Trim();
      string categoria = txtCategoria.Text.Trim();
      string cor = txtCor.Text.Trim();
      string desc = txtDescricao.Text.Trim();
      string img = txtImagem.Text.Trim();

      if (string.IsNullOrWhiteSpace(nome))
      {
        MessageBox.Show("O nome da comunidade é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtNome.Focus();
        return;
      }

      if (string.IsNullOrWhiteSpace(cor)) cor = "#EA3F74";
      if (!cor.StartsWith("#")) cor = "#" + cor;

      try
      {
        if (_isEdicao && _comunidadeExistente != null)
        {
          _comunidadeExistente.Nome = nome;
          _comunidadeExistente.Categoria = categoria;
          _comunidadeExistente.Cor = cor;
          _comunidadeExistente.Descricao = desc;
          _comunidadeExistente.ImagemComunidade = img;

          if (_comunidadeDAO.Atualizar(_comunidadeExistente))
          {
            MessageBox.Show("Comunidade atualizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
          }
        }
        else
        {
          long adminId = Program.UsuarioLogado?.IdUsuario ?? 1;

          Comunidade nova = new Comunidade
          {
            Nome = nome,
            Categoria = categoria,
            Cor = cor,
            Descricao = desc,
            ImagemComunidade = img,
            CriadorId = adminId
          };

          if (_comunidadeDAO.Inserir(nova))
          {
            MessageBox.Show("Comunidade criada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao salvar comunidade: {ex.Message}", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
  }
}
