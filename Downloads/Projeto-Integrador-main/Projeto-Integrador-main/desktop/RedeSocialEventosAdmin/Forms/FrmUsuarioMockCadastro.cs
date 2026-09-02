using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RedeSocialEventosAdmin.DAO;
using RedeSocialEventosAdmin.Models;
using RedeSocialEventosAdmin.Utils;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmUsuarioMockCadastro : Form
  {
    private readonly UsuarioDAO _usuarioDAO;
    private readonly Usuario? _usuarioExistente;
    private readonly bool _isEdicao;
    private bool _senhaVisivel = false;

    public FrmUsuarioMockCadastro()
    {
      InitializeComponent();
      _usuarioDAO = new UsuarioDAO();
      _isEdicao = false;
      _usuarioExistente = null;
      lblOperacao.Text = "Cadastrar Novo Usuário";
      lblAvisoSenha.Text = "* Digite uma senha com no mínimo 6 caracteres.";
      chkRoleUser.Checked = true;
      btnToggleSenha.Click += btnToggleSenha_Click;
    }

    public FrmUsuarioMockCadastro(Usuario usuario)
    {
      InitializeComponent();
      _usuarioDAO = new UsuarioDAO();
      _usuarioExistente = usuario;
      _isEdicao = true;
      lblOperacao.Text = $"Editar Usuário: {usuario.Username}";
      lblAvisoSenha.Text = "* Deixe a senha em branco para manter a atual sem alteração.";
      btnToggleSenha.Click += btnToggleSenha_Click;
      PreencherCampos();
    }

    private void PreencherCampos()
    {
      if (_usuarioExistente == null) return;

      txtUsername.Text = _usuarioExistente.Username;
      txtNome.Text = _usuarioExistente.NomeCompleto ?? _usuarioExistente.Nome ?? "";
      txtEmail.Text = _usuarioExistente.Email;
      txtTelefone.Text = _usuarioExistente.Telefone ?? "";
      txtBio.Text = _usuarioExistente.Bio ?? "";
      
      // IMPORTANTE: NÃO preenchemos a senha com o texto do banco (segurança e requisito do usuário)
      txtSenha.Text = "";

      // Preencher Status
      cmbStatus.SelectedItem = _usuarioExistente.Status.ToLower();

      // Preencher Roles
      string roles = _usuarioExistente.Role.ToLower();
      chkRoleAdmin.Checked = roles.Contains("admin");
      chkRoleModerator.Checked = roles.Contains("moderator");
      chkRolePremium.Checked = roles.Contains("premium");
      chkRoleTester.Checked = roles.Contains("tester");
      chkRoleBeta.Checked = roles.Contains("betatester");
      chkRoleUser.Checked = roles.Contains("user");

      txtCustomRole.Text = _usuarioExistente.Role;
    }

    private string ObterRolesSelecionadas()
    {
      if (!string.IsNullOrWhiteSpace(txtCustomRole.Text))
      {
        return txtCustomRole.Text.Trim();
      }

      var rolesList = new List<string>();
      if (chkRoleAdmin.Checked) rolesList.Add("admin");
      if (chkRoleModerator.Checked) rolesList.Add("moderator");
      if (chkRolePremium.Checked) rolesList.Add("premium");
      if (chkRoleTester.Checked) rolesList.Add("tester");
      if (chkRoleBeta.Checked) rolesList.Add("betatester");
      if (chkRoleUser.Checked) rolesList.Add("user");

      if (rolesList.Count == 0) rolesList.Add("user");

      return string.Join(",", rolesList);
    }

    private void btnSalvar_Click(object sender, EventArgs e)
    {
      string username = txtUsername.Text.Trim();
      string nomeCompleto = txtNome.Text.Trim();
      string email = txtEmail.Text.Trim();
      string telefone = txtTelefone.Text.Trim();
      string bio = txtBio.Text.Trim();
      string senha = txtSenha.Text;
      string status = cmbStatus.SelectedItem?.ToString() ?? "ativo";
      string role = ObterRolesSelecionadas();

      if (string.IsNullOrWhiteSpace(username))
      {
        MessageBox.Show("O nome de usuário (username) é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtUsername.Focus();
        return;
      }

      if (string.IsNullOrWhiteSpace(email) || !Validador.ValidarEmail(email))
      {
        MessageBox.Show("Informe um e-mail válido.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtEmail.Focus();
        return;
      }

      if (!_isEdicao && (string.IsNullOrWhiteSpace(senha) || !Validador.ValidarSenha(senha)))
      {
        MessageBox.Show("Para novos usuários, a senha é obrigatória e deve ter pelo menos 6 caracteres.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtSenha.Focus();
        return;
      }

      if (_isEdicao && !string.IsNullOrWhiteSpace(senha) && !Validador.ValidarSenha(senha))
      {
        MessageBox.Show("A nova senha deve possuir pelo menos 6 caracteres.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtSenha.Focus();
        return;
      }

      try
      {
        // Verificar se e-mail já existe para outro usuário
        Usuario? emailVerif = _usuarioDAO.BuscarPorEmail(email);
        if (emailVerif != null && (!_isEdicao || emailVerif.IdUsuario != _usuarioExistente!.IdUsuario))
        {
          MessageBox.Show("Este endereço de e-mail já pertence a outro usuário cadastrado.", "E-mail em Uso", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        if (_isEdicao && _usuarioExistente != null)
        {
          _usuarioExistente.Username = username;
          _usuarioExistente.Nome = nomeCompleto;
          _usuarioExistente.NomeCompleto = nomeCompleto;
          _usuarioExistente.Email = email;
          _usuarioExistente.Telefone = telefone;
          _usuarioExistente.Bio = bio;
          _usuarioExistente.Status = status;
          _usuarioExistente.Role = role;

          bool alterarSenha = !string.IsNullOrWhiteSpace(senha);
          if (alterarSenha)
          {
            _usuarioExistente.Senha = senha;
          }

          if (_usuarioDAO.Atualizar(_usuarioExistente, alterarSenha))
          {
            MessageBox.Show($"Usuário '{username}' atualizado com sucesso!\nRoles: {role}\nStatus: {status}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
          }
        }
        else
        {
          Usuario novo = new Usuario
          {
            Username = username,
            Nome = nomeCompleto,
            NomeCompleto = nomeCompleto,
            Email = email,
            Telefone = telefone,
            Bio = bio,
            Senha = senha,
            Status = status,
            Role = role
          };

          if (_usuarioDAO.Inserir(novo))
          {
            MessageBox.Show($"Usuário '{username}' cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao salvar usuário: {ex.Message}", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnToggleSenha_Click(object? sender, EventArgs e)
    {
      _senhaVisivel = !_senhaVisivel;
      txtSenha.PasswordChar = _senhaVisivel ? '\0' : '\u25CF';
      btnToggleSenha.IconChar = _senhaVisivel ? FontAwesome.Sharp.IconChar.EyeSlash : FontAwesome.Sharp.IconChar.Eye;
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
  }
}
