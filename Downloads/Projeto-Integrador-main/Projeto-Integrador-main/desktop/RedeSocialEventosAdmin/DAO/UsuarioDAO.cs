using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.DAO
{
  public enum LoginStatus
  {
    Sucesso,
    CredenciaisInvalidas,
    NaoEAdmin,
    UsuarioSuspenso,
    ErroConexao
  }

  public class LoginResult
  {
    public LoginStatus Status { get; set; }
    public Usuario? Usuario { get; set; }
    public string Mensagem { get; set; } = string.Empty;
  }

  public class UsuarioDAO
  {
    public LoginResult AutenticarAdmin(string loginOuEmail, string senha)
    {
      var result = new LoginResult();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = "SELECT * FROM usuarios WHERE (email = @login OR username = @login) AND senha = @senha LIMIT 1";
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@login", loginOuEmail);
          cmd.Parameters.AddWithValue("@senha", senha);

          try
          {
            conn.Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
              if (reader.Read())
              {
                var user = MapearUsuario(reader);

                if (user.Status.Equals("suspenso", StringComparison.OrdinalIgnoreCase))
                {
                  result.Status = LoginStatus.UsuarioSuspenso;
                  result.Mensagem = "Esta conta está suspensa no sistema.";
                  return result;
                }

                if (!user.IsAdmin)
                {
                  result.Status = LoginStatus.NaoEAdmin;
                  result.Mensagem = "Acesso Negado: Apenas usuários com a role 'admin' têm permissão de acesso ao painel desktop.";
                  return result;
                }

                result.Status = LoginStatus.Sucesso;
                result.Usuario = user;
                result.Mensagem = "Autenticação realizada com sucesso.";
                return result;
              }
              else
              {
                result.Status = LoginStatus.CredenciaisInvalidas;
                result.Mensagem = "E-mail/usuário ou senha incorretos.";
                return result;
              }
            }
          }
          catch (Exception ex)
          {
            result.Status = LoginStatus.ErroConexao;
            result.Mensagem = "Erro ao conectar com o banco de dados: " + ex.Message;
            return result;
          }
        }
      }
    }

    public bool Inserir(Usuario usuario)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"INSERT INTO usuarios 
          (username, nome, email, senha, telefone, nome_completo, data_nascimento, bio, foto_perfil, status, role, data_criacao) 
          VALUES 
          (@username, @nome, @email, @senha, @telefone, @nome_completo, @data_nascimento, @bio, @foto_perfil, @status, @role, NOW())";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@username", string.IsNullOrWhiteSpace(usuario.Username) ? usuario.Email.Split('@')[0] : usuario.Username);
          cmd.Parameters.AddWithValue("@nome", (object?)usuario.Nome ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@email", usuario.Email);
          cmd.Parameters.AddWithValue("@senha", usuario.Senha);
          cmd.Parameters.AddWithValue("@telefone", (object?)usuario.Telefone ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@nome_completo", string.IsNullOrWhiteSpace(usuario.NomeCompleto) ? (usuario.Nome ?? usuario.Username) : usuario.NomeCompleto);
          cmd.Parameters.AddWithValue("@data_nascimento", usuario.DataNascimento.HasValue ? usuario.DataNascimento.Value.ToString("yyyy-MM-dd") : DateTime.Now.AddYears(-20).ToString("yyyy-MM-dd"));
          cmd.Parameters.AddWithValue("@bio", (object?)usuario.Bio ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@foto_perfil", (object?)usuario.FotoPerfil ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(usuario.Status) ? "ativo" : usuario.Status);
          cmd.Parameters.AddWithValue("@role", string.IsNullOrWhiteSpace(usuario.Role) ? "user" : usuario.Role);

          try
          {
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao inserir usuário: " + ex.Message);
          }
        }
      }
    }

    public bool Atualizar(Usuario usuario, bool atualizarSenha = false)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"UPDATE usuarios SET 
          username = @username, 
          nome = @nome, 
          email = @email, 
          telefone = @telefone, 
          nome_completo = @nome_completo, 
          bio = @bio, 
          status = @status, 
          role = @role";

        if (atualizarSenha && !string.IsNullOrWhiteSpace(usuario.Senha))
        {
          sql += ", senha = @senha";
        }

        sql += " WHERE id_usuario = @id";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@username", usuario.Username);
          cmd.Parameters.AddWithValue("@nome", (object?)usuario.Nome ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@email", usuario.Email);
          cmd.Parameters.AddWithValue("@telefone", (object?)usuario.Telefone ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@nome_completo", string.IsNullOrWhiteSpace(usuario.NomeCompleto) ? (usuario.Nome ?? usuario.Username) : usuario.NomeCompleto);
          cmd.Parameters.AddWithValue("@bio", (object?)usuario.Bio ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@status", usuario.Status);
          cmd.Parameters.AddWithValue("@role", usuario.Role);
          cmd.Parameters.AddWithValue("@id", usuario.IdUsuario);

          if (atualizarSenha && !string.IsNullOrWhiteSpace(usuario.Senha))
          {
            cmd.Parameters.AddWithValue("@senha", usuario.Senha);
          }

          try
          {
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao atualizar usuário: " + ex.Message);
          }
        }
      }
    }

    public bool AlterarRole(long idUsuario, string novaRole)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = "UPDATE usuarios SET role = @role WHERE id_usuario = @id";
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@role", novaRole);
          cmd.Parameters.AddWithValue("@id", idUsuario);
          try
          {
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao alterar role do usuário: " + ex.Message);
          }
        }
      }
    }

    public bool AlterarStatus(long idUsuario, string novoStatus)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = "UPDATE usuarios SET status = @status WHERE id_usuario = @id";
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@status", novoStatus);
          cmd.Parameters.AddWithValue("@id", idUsuario);
          try
          {
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao alterar status do usuário: " + ex.Message);
          }
        }
      }
    }

    public bool Excluir(long idUsuario)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        conn.Open();
        using (MySqlTransaction trans = conn.BeginTransaction())
        {
          try
          {
            // 1. Votos de comentários do usuário
            ExecutarDelete("DELETE FROM votos_comentario WHERE id_usuario = @id", idUsuario, conn, trans);

            // 2. Votos em comentários de posts do usuário
            ExecutarDelete(@"DELETE vc FROM votos_comentario vc 
              INNER JOIN comentarios c ON vc.id_comentario = c.id_comentario 
              WHERE c.id_usuario = @id", idUsuario, conn, trans);

            // 3. Comentários feitos pelo usuário
            ExecutarDelete("DELETE FROM comentarios WHERE id_usuario = @id", idUsuario, conn, trans);

            // 4. Votos feitos pelo usuário em posts
            ExecutarDelete("DELETE FROM votos WHERE id_usuario = @id", idUsuario, conn, trans);

            // 5. Interações do usuário
            ExecutarDelete("DELETE FROM interacoes_usuario WHERE id_usuario = @id", idUsuario, conn, trans);

            // 6. Posts salvos pelo usuário
            ExecutarDelete("DELETE FROM posts_salvos WHERE id_usuario = @id", idUsuario, conn, trans);

            // 7. Vínculos em comunidades
            ExecutarDelete("DELETE FROM membros_comunidade WHERE id_usuario = @id", idUsuario, conn, trans);
            ExecutarDelete("DELETE FROM usuario_comunidade WHERE usuario_id = @id", idUsuario, conn, trans);

            // 8. Inscrições em eventos
            ExecutarDelete("DELETE FROM usuario_evento WHERE usuario_id = @id", idUsuario, conn, trans);

            // 9. Interesses do usuário
            ExecutarDelete("DELETE FROM usuario_interesses WHERE usuario_id = @id", idUsuario, conn, trans);

            // 10. Estatísticas do usuário
            ExecutarDelete("DELETE FROM estatisticas_usuario WHERE id_usuario = @id", idUsuario, conn, trans);

            // 11. Limpar dependências de posts criados pelo usuário
            ExecutarDelete(@"DELETE FROM votos_comentario WHERE id_comentario IN 
              (SELECT id_comentario FROM comentarios WHERE id_post IN (SELECT id_post FROM posts WHERE id_usuario = @id))", idUsuario, conn, trans);

            ExecutarDelete("DELETE FROM comentarios WHERE id_post IN (SELECT id_post FROM posts WHERE id_usuario = @id)", idUsuario, conn, trans);
            ExecutarDelete("DELETE FROM votos WHERE id_post IN (SELECT id_post FROM posts WHERE id_usuario = @id)", idUsuario, conn, trans);
            ExecutarDelete("DELETE FROM interacoes_usuario WHERE id_post IN (SELECT id_post FROM posts WHERE id_usuario = @id)", idUsuario, conn, trans);
            ExecutarDelete("DELETE FROM posts_salvos WHERE id_post IN (SELECT id_post FROM posts WHERE id_usuario = @id)", idUsuario, conn, trans);
            ExecutarDelete("DELETE FROM imagens_post WHERE id_post IN (SELECT id_post FROM posts WHERE id_usuario = @id)", idUsuario, conn, trans);
            ExecutarDelete("DELETE FROM posts WHERE id_usuario = @id", idUsuario, conn, trans);

            // 12. Desvincular ou deletar eventos criados pelo usuário
            ExecutarDelete("DELETE FROM usuario_evento WHERE evento_id IN (SELECT id_evento FROM eventos WHERE criador_id = @id)", idUsuario, conn, trans);
            ExecutarDelete("DELETE FROM eventos WHERE criador_id = @id", idUsuario, conn, trans);

            // 13. Desvincular criador de comunidades
            ExecutarDelete("UPDATE comunidades SET criador_id = NULL WHERE criador_id = @id", idUsuario, conn, trans);

            // 14. Deletar registro principal do usuário
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM usuarios WHERE id_usuario = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idUsuario);
              int linhasAfetadas = cmd.ExecuteNonQuery();
              trans.Commit();
              return linhasAfetadas > 0;
            }
          }
          catch (Exception ex)
          {
            trans.Rollback();
            throw new Exception("Erro transacional ao excluir usuário: " + ex.Message);
          }
        }
      }
    }

    private void ExecutarDelete(string sql, long id, MySqlConnection conn, MySqlTransaction trans)
    {
      try
      {
        using (MySqlCommand cmd = new MySqlCommand(sql, conn, trans))
        {
          cmd.Parameters.AddWithValue("@id", id);
          cmd.ExecuteNonQuery();
        }
      }
      catch
      {
        // Tratamento resiliente caso a tabela opcional ou registro não exista
      }
    }

    public Usuario? BuscarPorId(long idUsuario)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = "SELECT * FROM usuarios WHERE id_usuario = @id LIMIT 1";
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", idUsuario);
          try
          {
            conn.Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
              if (reader.Read())
              {
                return MapearUsuario(reader);
              }
            }
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao buscar usuário por ID: " + ex.Message);
          }
        }
      }
      return null;
    }

    public Usuario? BuscarPorEmail(string email)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = "SELECT * FROM usuarios WHERE email = @email LIMIT 1";
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@email", email);
          try
          {
            conn.Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
              if (reader.Read())
              {
                return MapearUsuario(reader);
              }
            }
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao buscar usuário por E-mail: " + ex.Message);
          }
        }
      }
      return null;
    }

    public DataTable Listar(string? filtroRole = null, string? filtroStatus = null)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          id_usuario AS 'ID', 
          username AS 'Usuário', 
          nome AS 'Nome', 
          email AS 'E-mail', 
          telefone AS 'Telefone',
          role AS 'Role/Permissão', 
          status AS 'Status', 
          data_criacao AS 'Data Criação' 
          FROM usuarios WHERE 1=1";

        if (!string.IsNullOrWhiteSpace(filtroRole) && filtroRole != "TODOS")
        {
          sql += " AND role LIKE @role";
        }
        if (!string.IsNullOrWhiteSpace(filtroStatus) && filtroStatus != "TODOS")
        {
          sql += " AND status = @status";
        }

        sql += " ORDER BY id_usuario DESC";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          if (!string.IsNullOrWhiteSpace(filtroRole) && filtroRole != "TODOS")
            cmd.Parameters.AddWithValue("@role", "%" + filtroRole.ToLower() + "%");
          if (!string.IsNullOrWhiteSpace(filtroStatus) && filtroStatus != "TODOS")
            cmd.Parameters.AddWithValue("@status", filtroStatus.ToLower());

          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch (Exception ex)
            {
              throw new Exception("Erro ao listar usuários: " + ex.Message);
            }
          }
        }
      }
      return dt;
    }

    public DataTable Pesquisar(string termo, string? filtroRole = null, string? filtroStatus = null)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          id_usuario AS 'ID', 
          username AS 'Usuário', 
          nome AS 'Nome', 
          email AS 'E-mail', 
          telefone AS 'Telefone',
          role AS 'Role/Permissão', 
          status AS 'Status', 
          data_criacao AS 'Data Criação' 
          FROM usuarios 
          WHERE (nome LIKE @termo OR email LIKE @termo OR username LIKE @termo)";

        if (!string.IsNullOrWhiteSpace(filtroRole) && filtroRole != "TODOS")
        {
          sql += " AND role LIKE @role";
        }
        if (!string.IsNullOrWhiteSpace(filtroStatus) && filtroStatus != "TODOS")
        {
          sql += " AND status = @status";
        }

        sql += " ORDER BY id_usuario DESC";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
          if (!string.IsNullOrWhiteSpace(filtroRole) && filtroRole != "TODOS")
            cmd.Parameters.AddWithValue("@role", "%" + filtroRole.ToLower() + "%");
          if (!string.IsNullOrWhiteSpace(filtroStatus) && filtroStatus != "TODOS")
            cmd.Parameters.AddWithValue("@status", filtroStatus.ToLower());

          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch (Exception ex)
            {
              throw new Exception("Erro ao pesquisar usuários: " + ex.Message);
            }
          }
        }
      }
      return dt;
    }

    private Usuario MapearUsuario(MySqlDataReader reader)
    {
      return new Usuario
      {
        IdUsuario = Convert.ToInt64(reader["id_usuario"]),
        Username = reader["username"] != DBNull.Value ? reader["username"].ToString() ?? "" : "",
        Nome = reader["nome"] != DBNull.Value ? reader["nome"].ToString() : null,
        Email = reader["email"] != DBNull.Value ? reader["email"].ToString() ?? "" : "",
        Senha = reader["senha"] != DBNull.Value ? reader["senha"].ToString() ?? "" : "",
        Telefone = reader["telefone"] != DBNull.Value ? reader["telefone"].ToString() : null,
        NomeCompleto = reader["nome_completo"] != DBNull.Value ? reader["nome_completo"].ToString() ?? "" : "",
        DataNascimento = reader["data_nascimento"] != DBNull.Value ? Convert.ToDateTime(reader["data_nascimento"]) : null,
        Bio = reader["bio"] != DBNull.Value ? reader["bio"].ToString() : null,
        DataCriacao = reader["data_criacao"] != DBNull.Value ? Convert.ToDateTime(reader["data_criacao"]) : DateTime.Now,
        DataExclusao = reader["data_exclusao"] != DBNull.Value ? Convert.ToDateTime(reader["data_exclusao"]) : null,
        FotoPerfil = reader["foto_perfil"] != DBNull.Value ? reader["foto_perfil"].ToString() : null,
        Status = reader["status"] != DBNull.Value ? reader["status"].ToString() ?? "ativo" : "ativo",
        Role = reader["role"] != DBNull.Value ? reader["role"].ToString() ?? "user" : "user"
      };
    }
  }
}