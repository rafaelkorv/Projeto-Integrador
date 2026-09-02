using System;
using System.Data;
using MySql.Data.MySqlClient;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.DAO
{
  public class DashboardDAO
  {
    public DashboardStats ObterEstatisticasGerais()
    {
      DashboardStats stats = new DashboardStats();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        try
        {
          conn.Open();

          // 1. Usuários Totais
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM usuarios", conn))
          {
            stats.TotalUsuarios = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 2. Usuários cadastrados hoje
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM usuarios WHERE DATE(data_criacao) = CURDATE()", conn))
          {
            stats.UsuariosHoje = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 3. Usuários com role admin
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM usuarios WHERE role LIKE '%admin%'", conn))
          {
            stats.TotalAdmins = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 4. Usuários suspensos
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM usuarios WHERE status = 'suspenso'", conn))
          {
            stats.TotalSuspensos = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 5. Total de Eventos
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM eventos", conn))
          {
            stats.TotalEventos = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 6. Eventos Agendados
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM eventos WHERE status = 'AGENDADO'", conn))
          {
            stats.EventosAgendados = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 7. Total de Inscrições em Eventos
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM usuario_evento", conn))
          {
            stats.TotalInscricoesEventos = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 8. Total de Comunidades
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM comunidades", conn))
          {
            stats.TotalComunidades = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 9. Total de Posts
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM posts", conn))
          {
            stats.TotalPosts = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 10. Total de Comentários
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM comentarios", conn))
          {
            stats.TotalComentarios = Convert.ToInt32(cmd.ExecuteScalar());
          }
        }
        catch (Exception ex)
        {
          throw new Exception("Erro ao carregar métricas do Dashboard: " + ex.Message);
        }
      }
      return stats;
    }

    public DataTable ObterUltimosUsuarios(int limite = 6)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          id_usuario AS 'ID', 
          username AS 'Usuário',
          COALESCE(nome, username) AS 'Nome', 
          email AS 'E-mail', 
          role AS 'Role',
          status AS 'Status',
          data_criacao AS 'Data Cadastro' 
          FROM usuarios 
          ORDER BY id_usuario DESC LIMIT @limite";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@limite", limite);
          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch (Exception ex)
            {
              throw new Exception("Erro ao obter últimos usuários: " + ex.Message);
            }
          }
        }
      }
      return dt;
    }

    public DataTable ObterProximosEventos(int limite = 5)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          e.id_evento AS 'ID',
          e.titulo AS 'Evento',
          e.data_evento AS 'Data',
          e.local_evento AS 'Local',
          e.status AS 'Status',
          (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento) AS 'Inscritos'
          FROM eventos e
          ORDER BY e.data_evento DESC, e.id_evento DESC LIMIT @limite";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@limite", limite);
          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch (Exception ex)
            {
              throw new Exception("Erro ao obter próximos eventos: " + ex.Message);
            }
          }
        }
      }
      return dt;
    }

    public DataTable ObterDistribuicaoRoles()
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          role AS 'Role', 
          COUNT(1) AS 'Quantidade' 
          FROM usuarios 
          GROUP BY role 
          ORDER BY Quantidade DESC";

        using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
        {
          try
          {
            da.Fill(dt);
          }
          catch { }
        }
      }
      return dt;
    }

    public DataTable ObterTopComunidades(int limite = 5)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          c.nome AS 'Comunidade',
          c.categoria AS 'Categoria',
          (SELECT COUNT(1) FROM usuario_comunidade uc WHERE uc.comunidade_id = c.id_comunidade) AS 'Membros',
          (SELECT COUNT(1) FROM posts p WHERE p.id_comunidade = c.id_comunidade) AS 'Posts'
          FROM comunidades c
          ORDER BY Membros DESC LIMIT @limite";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@limite", limite);
          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch { }
          }
        }
      }
      return dt;
    }
  }
}