using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.DAO
{
  public class RelatorioDAO
  {
    public RelatorioUsuarioModel? ObterRelatorioIndividualUsuario(long idUsuario)
    {
      RelatorioUsuarioModel? model = null;

      using (MySqlConnection conn = Conexao.GetConnection())
      {
        try
        {
          conn.Open();

          // 1. Dados Básicos do Usuário
          string sqlUser = "SELECT * FROM usuarios WHERE id_usuario = @id LIMIT 1";
          using (MySqlCommand cmd = new MySqlCommand(sqlUser, conn))
          {
            cmd.Parameters.AddWithValue("@id", idUsuario);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
              if (reader.Read())
              {
                model = new RelatorioUsuarioModel
                {
                  IdUsuario = Convert.ToInt64(reader["id_usuario"]),
                  Username = reader["username"]?.ToString() ?? "",
                  NomeCompleto = reader["nome_completo"]?.ToString() ?? reader["nome"]?.ToString() ?? reader["username"]?.ToString() ?? "",
                  Email = reader["email"]?.ToString() ?? "",
                  Telefone = reader["telefone"]?.ToString() ?? "",
                  Role = reader["role"]?.ToString() ?? "user",
                  Status = reader["status"]?.ToString() ?? "ativo",
                  DataCriacao = reader["data_criacao"] != DBNull.Value ? Convert.ToDateTime(reader["data_criacao"]) : DateTime.Now
                };
              }
            }
          }

          if (model == null) return null;

          // 2. Eventos Criados
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM eventos WHERE criador_id = @id", conn))
          {
            cmd.Parameters.AddWithValue("@id", idUsuario);
            model.EventosCriados = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 3. Inscrições em Eventos e Check-ins
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM usuario_evento WHERE usuario_id = @id", conn))
          {
            cmd.Parameters.AddWithValue("@id", idUsuario);
            model.EventosInscritos = Convert.ToInt32(cmd.ExecuteScalar());
          }

          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM usuario_evento WHERE usuario_id = @id AND data_checkin IS NOT NULL", conn))
          {
            cmd.Parameters.AddWithValue("@id", idUsuario);
            model.CheckinsRealizados = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 4. Comunidades em que é Membro
          string sqlComunidades = @"SELECT c.nome FROM usuario_comunidade uc 
            JOIN comunidades c ON c.id_comunidade = uc.comunidade_id 
            WHERE uc.usuario_id = @id";
          using (MySqlCommand cmd = new MySqlCommand(sqlComunidades, conn))
          {
            cmd.Parameters.AddWithValue("@id", idUsuario);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
              while (reader.Read())
              {
                string nomeCom = reader["nome"]?.ToString() ?? "";
                if (!string.IsNullOrEmpty(nomeCom))
                {
                  model.ComunidadesNomes.Add(nomeCom);
                }
              }
            }
          }
          model.TotalComunidades = model.ComunidadesNomes.Count;

          // 5. Total de Posts
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM posts WHERE id_usuario = @id", conn))
          {
            cmd.Parameters.AddWithValue("@id", idUsuario);
            model.TotalPosts = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 6. Total de Comentários
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM comentarios WHERE id_usuario = @id", conn))
          {
            cmd.Parameters.AddWithValue("@id", idUsuario);
            model.TotalComentarios = Convert.ToInt32(cmd.ExecuteScalar());
          }

          // 7. Total de Votos / Interações
          using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(1) FROM votos WHERE id_usuario = @id", conn))
          {
            cmd.Parameters.AddWithValue("@id", idUsuario);
            try
            {
              model.TotalVotos = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { model.TotalVotos = 0; }
          }
        }
        catch (Exception ex)
        {
          throw new Exception("Erro ao obter relatório individual do usuário: " + ex.Message);
        }
      }

      return model;
    }

    public DataTable ObterRelatorioConsolidadoUsuarios()
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          u.id_usuario AS 'ID',
          u.username AS 'Usuário',
          COALESCE(u.nome_completo, u.nome, u.username) AS 'Nome Completo',
          u.email AS 'E-mail',
          u.role AS 'Role/Permissão',
          u.status AS 'Status',
          (SELECT COUNT(1) FROM eventos e WHERE e.criador_id = u.id_usuario) AS 'Eventos Criados',
          (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.usuario_id = u.id_usuario) AS 'Eventos Inscritos',
          (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.usuario_id = u.id_usuario AND ue.data_checkin IS NOT NULL) AS 'Check-ins',
          (SELECT COUNT(1) FROM usuario_comunidade uc WHERE uc.usuario_id = u.id_usuario) AS 'Comunidades',
          (SELECT COUNT(1) FROM posts p WHERE p.id_usuario = u.id_usuario) AS 'Posts',
          (SELECT COUNT(1) FROM comentarios c WHERE c.id_usuario = u.id_usuario) AS 'Comentários',
          ((SELECT COUNT(1) FROM posts p WHERE p.id_usuario = u.id_usuario) * 5 + 
           (SELECT COUNT(1) FROM comentarios c WHERE c.id_usuario = u.id_usuario) * 3 + 
           (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.usuario_id = u.id_usuario) * 4 + 
           (SELECT COUNT(1) FROM usuario_comunidade uc WHERE uc.usuario_id = u.id_usuario) * 2) AS 'Score Engajamento'
          FROM usuarios u
          ORDER BY u.id_usuario DESC";

        using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
        {
          try { da.Fill(dt); } catch (Exception ex) { throw new Exception("Erro ao consolidar usuários: " + ex.Message); }
        }
      }
      return dt;
    }

    public DataTable ObterRelatorioConsolidadoEventos()
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          e.id_evento AS 'ID',
          e.titulo AS 'Título do Evento',
          COALESCE(e.categoria, 'Geral') AS 'Categoria',
          e.data_evento AS 'Data do Evento',
          e.local_evento AS 'Local',
          e.status AS 'Status',
          COALESCE(e.limite_participantes, 0) AS 'Capacidade Máxima',
          (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento) AS 'Total Inscritos',
          GREATEST(0, COALESCE(e.limite_participantes, 0) - (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento)) AS 'Vagas Restantes',
          ROUND(IF(COALESCE(e.limite_participantes, 0) > 0, 
            LEAST(100.0, ((SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento) / e.limite_participantes) * 100.0), 
            100.0), 1) AS 'Taxa Ocupação (%)',
          (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento AND ue.data_checkin IS NOT NULL) AS 'Check-ins Realizados',
          ROUND(IF((SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento) > 0,
            ((SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento AND ue.data_checkin IS NOT NULL) / (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento)) * 100.0,
            0.0), 1) AS 'Taxa Comparecimento (%)'
          FROM eventos e
          ORDER BY e.data_evento DESC, e.id_evento DESC";

        using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
        {
          try { da.Fill(dt); } catch (Exception ex) { throw new Exception("Erro ao consolidar eventos: " + ex.Message); }
        }
      }
      return dt;
    }

    public DataTable ObterRelatorioConsolidadoComunidades()
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          c.id_comunidade AS 'ID',
          c.nome AS 'Nome da Comunidade',
          COALESCE(c.categoria, 'Geral') AS 'Categoria',
          c.cor AS 'Cor Hex',
          (SELECT COUNT(1) FROM usuario_comunidade uc WHERE uc.comunidade_id = c.id_comunidade) AS 'Total de Membros',
          (SELECT COUNT(1) FROM posts p WHERE p.id_comunidade = c.id_comunidade) AS 'Total de Posts',
          (SELECT COUNT(1) FROM comentarios com JOIN posts p ON p.id_post = com.id_post WHERE p.id_comunidade = c.id_comunidade) AS 'Total de Comentários',
          ROUND(IF((SELECT COUNT(1) FROM posts p WHERE p.id_comunidade = c.id_comunidade) > 0,
            ((SELECT COUNT(1) FROM comentarios com JOIN posts p ON p.id_post = com.id_post WHERE p.id_comunidade = c.id_comunidade) / (SELECT COUNT(1) FROM posts p WHERE p.id_comunidade = c.id_comunidade)),
            0.0), 1) AS 'Média Comentários/Post',
          ((SELECT COUNT(1) FROM usuario_comunidade uc WHERE uc.comunidade_id = c.id_comunidade) * 2 + 
           (SELECT COUNT(1) FROM posts p WHERE p.id_comunidade = c.id_comunidade) * 5 + 
           (SELECT COUNT(1) FROM comentarios com JOIN posts p ON p.id_post = com.id_post WHERE p.id_comunidade = c.id_comunidade) * 3) AS 'Índice de Atividade'
          FROM comunidades c
          ORDER BY c.id_comunidade DESC";

        using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
        {
          try { da.Fill(dt); } catch (Exception ex) { throw new Exception("Erro ao consolidar comunidades: " + ex.Message); }
        }
      }
      return dt;
    }

    public List<Tuple<string, int>> ObterDadosGraficoRoles()
    {
      var list = new List<Tuple<string, int>>();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = "SELECT role, COUNT(1) as total FROM usuarios GROUP BY role ORDER BY total DESC";
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          try
          {
            conn.Open();
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
              while (r.Read())
              {
                string role = r["role"]?.ToString() ?? "user";
                int total = Convert.ToInt32(r["total"]);
                list.Add(new Tuple<string, int>(role.ToUpper(), total));
              }
            }
          }
          catch { }
        }
      }
      return list;
    }

    public List<Tuple<string, int, int>> ObterDadosGraficoTopEventos(int limite = 5)
    {
      var list = new List<Tuple<string, int, int>>();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          e.titulo, 
          COALESCE(e.limite_participantes, 0) as limite,
          (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento) as inscritos
          FROM eventos e 
          ORDER BY inscritos DESC, e.id_evento DESC LIMIT @limite";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@limite", limite);
          try
          {
            conn.Open();
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
              while (r.Read())
              {
                string titulo = r["titulo"]?.ToString() ?? "";
                int limitePart = Convert.ToInt32(r["limite"]);
                int inscritos = Convert.ToInt32(r["inscritos"]);
                list.Add(new Tuple<string, int, int>(titulo, inscritos, limitePart));
              }
            }
          }
          catch { }
        }
      }
      return list;
    }

    public List<Tuple<string, int, int>> ObterDadosGraficoTopComunidades(int limite = 5)
    {
      var list = new List<Tuple<string, int, int>>();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          c.nome, 
          (SELECT COUNT(1) FROM usuario_comunidade uc WHERE uc.comunidade_id = c.id_comunidade) as membros,
          (SELECT COUNT(1) FROM posts p WHERE p.id_comunidade = c.id_comunidade) as posts
          FROM comunidades c 
          ORDER BY membros DESC, posts DESC LIMIT @limite";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@limite", limite);
          try
          {
            conn.Open();
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
              while (r.Read())
              {
                string nome = r["nome"]?.ToString() ?? "";
                int membros = Convert.ToInt32(r["membros"]);
                int posts = Convert.ToInt32(r["posts"]);
                list.Add(new Tuple<string, int, int>(nome, membros, posts));
              }
            }
          }
          catch { }
        }
      }
      return list;
    }
  }
}