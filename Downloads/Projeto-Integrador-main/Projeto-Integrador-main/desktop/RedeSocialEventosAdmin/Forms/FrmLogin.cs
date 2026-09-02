using System;
using System.Windows.Forms;
using RedeSocialEventosAdmin.DAO;
using RedeSocialEventosAdmin.Utils;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmLogin : Form
  {
    private readonly UsuarioDAO _usuarioDAO;
    private bool _senhaVisivel = false;

    public FrmLogin()
    {
      InitializeComponent();
      _usuarioDAO = new UsuarioDAO();
    }

    private void btnEntrar_Click(object sender, EventArgs e)
    {
      ExecutarLogin();
    }

    private void ExecutarLogin()
    {
      string login = txtEmail.Text.Trim();
      string senha = txtSenha.Text;

      if (Validador.CampoVazio(login) || Validador.CampoVazio(senha))
      {
        MessageBox.Show("Por favor, preencha todos os campos para autenticar.", "Campos Obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtEmail.Focus();
        return;
      }

      try
      {
        btnEntrar.Enabled = false;
        btnEntrar.Text = "VERIFICANDO PERMISSÕES...";

        var resultado = _usuarioDAO.AutenticarAdmin(login, senha);

        if (resultado.Status == LoginStatus.Sucesso && resultado.Usuario != null)
        {
          Program.UsuarioLogado = resultado.Usuario;
          Program.EmailUsuarioLogado = resultado.Usuario.Email;

          this.Hide();
          using (FrmPrincipal principal = new FrmPrincipal())
          {
            principal.ShowDialog();
          }
          this.Close();
        }
        else if (resultado.Status == LoginStatus.NaoEAdmin)
        {
          MessageBox.Show(" ACESSO NEGADO\n\nEsta conta foi autenticada, porém NÃO POSSUI a permissão 'admin'.\n\nO Painel Desktop é estritamente restrito para super administradores do sistema.", "Permissão Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        else if (resultado.Status == LoginStatus.UsuarioSuspenso)
        {
          MessageBox.Show(" CONTA SUSPENSA\n\nEsta conta foi suspensa por descumprimento dos termos de serviço.", "Acesso Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        else if (resultado.Status == LoginStatus.CredenciaisInvalidas)
        {
          MessageBox.Show(" CREDENCIAIS INCORRETAS\n\nE-mail/usuário ou senha incorretos. Verifique suas credenciais.", "Erro de Autenticação", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
          MessageBox.Show($"Falha na comunicação com o banco de dados:\n{resultado.Mensagem}", "Erro no Servidor", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Ocorreu um erro inesperado durante o login:\n{ex.Message}", "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        btnEntrar.Enabled = true;
        btnEntrar.Text = "AUTENTICAR COMO ADMIN";
      }
    }

    private void btnVerSenha_Click(object sender, EventArgs e)
    {
      _senhaVisivel = !_senhaVisivel;
      txtSenha.PasswordChar = _senhaVisivel ? '\0' : '●';
      btnVerSenha.IconChar = _senhaVisivel ? FontAwesome.Sharp.IconChar.EyeSlash : FontAwesome.Sharp.IconChar.Eye;
    }

    private void Campos_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        e.SuppressKeyPress = true;
        ExecutarLogin();
      }
    }

    private void btnFechar_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }
  }
}
