using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.DAO
{
  public class ComunidadeDAO
  {
    public DataTable Listar()
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          c.id_comunidade AS 'ID',
          c.nome AS 'Nome da Comunidade',
          c.categoria AS 'Categoria',
          c.cor AS 'Cor Hex',
          COALESCE(u.nome, u.username, 'Admin') AS 'Criador',
          (SELECT COUNT(1) FROM usuario_comunidade uc WHERE uc.comunidade_id = c.id_comunidade) AS 'Membros',
          (SELECT COUNT(1) FROM posts p WHERE p.id_comunidade = c.id_comunidade) AS 'Total Posts',
          c.data_criacao AS 'Data Criação'
          FROM comunidades c
          LEFT JOIN usuarios u ON c.criador_id = u.id_usuario
          ORDER BY c.id_comunidade DESC";

        using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
        {
          try
          {
            da.Fill(dt);
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao listar comunidades: " + ex.Message);
          }
        }
      }
      return dt;
    }

    public DataTable Pesquisar(string termo)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          c.id_comunidade AS 'ID',
          c.nome AS 'Nome da Comunidade',
          c.categoria AS 'Categoria',
          c.cor AS 'Cor Hex',
          COALESCE(u.nome, u.username, 'Admin') AS 'Criador',
          (SELECT COUNT(1) FROM usuario_comunidade uc WHERE uc.comunidade_id = c.id_comunidade) AS 'Membros',
          (SELECT COUNT(1) FROM posts p WHERE p.id_comunidade = c.id_comunidade) AS 'Total Posts',
          c.data_criacao AS 'Data Criação'
          FROM comunidades c
          LEFT JOIN usuarios u ON c.criador_id = u.id_usuario
          WHERE c.nome LIKE @termo OR c.categoria LIKE @termo OR c.descricao LIKE @termo
          ORDER BY c.id_comunidade DESC";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch (Exception ex)
            {
              throw new Exception("Erro ao pesquisar comunidades: " + ex.Message);
            }
          }
        }
      }
      return dt;
    }

    public Comunidade? BuscarPorId(long idComunidade)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT c.*, u.nome AS nome_criador,
          (SELECT COUNT(1) FROM usuario_comunidade uc WHERE uc.comunidade_id = c.id_comunidade) AS total_membros,
          (SELECT COUNT(1) FROM posts p WHERE p.id_comunidade = c.id_comunidade) AS total_posts
          FROM comunidades c
          LEFT JOIN usuarios u ON c.criador_id = u.id_usuario
          WHERE c.id_comunidade = @id LIMIT 1";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", idComunidade);
          try
          {
            conn.Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
              if (reader.Read())
              {
                return new Comunidade
                {
                  IdComunidade = Convert.ToInt64(reader["id_comunidade"]),
                  Nome = reader["nome"] != DBNull.Value ? reader["nome"].ToString() ?? "" : "",
                  Descricao = reader["descricao"] != DBNull.Value ? reader["descricao"].ToString() : null,
                  DataCriacao = reader["data_criacao"] != DBNull.Value ? Convert.ToDateTime(reader["data_criacao"]) : DateTime.Now,
                  CriadorId = reader["criador_id"] != DBNull.Value ? Convert.ToInt64(reader["criador_id"]) : null,
                  NomeCriador = reader["nome_criador"] != DBNull.Value ? reader["nome_criador"].ToString() : null,
                  Categoria = reader["categoria"] != DBNull.Value ? reader["categoria"].ToString() : null,
                  Cor = reader["cor"] != DBNull.Value ? reader["cor"].ToString() ?? "#EA3F74" : "#EA3F74",
                  ImagemComunidade = reader["imagem_comunidade"] != DBNull.Value ? reader["imagem_comunidade"].ToString() : null,
                  TotalMembros = Convert.ToInt32(reader["total_membros"]),
                  TotalPosts = Convert.ToInt32(reader["total_posts"])
                };
              }
            }
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao buscar comunidade por ID: " + ex.Message);
          }
        }
      }
      return null;
    }

    public bool Inserir(Comunidade comunidade)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"INSERT INTO comunidades 
          (nome, descricao, criador_id, categoria, cor, imagem_comunidade, data_criacao) 
          VALUES 
          (@nome, @descricao, @criador_id, @categoria, @cor, @imagem_comunidade, NOW())";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@nome", comunidade.Nome);
          cmd.Parameters.AddWithValue("@descricao", (object?)comunidade.Descricao ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@criador_id", (object?)comunidade.CriadorId ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@categoria", (object?)comunidade.Categoria ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@cor", string.IsNullOrWhiteSpace(comunidade.Cor) ? "#EA3F74" : comunidade.Cor);
          cmd.Parameters.AddWithValue("@imagem_comunidade", (object?)comunidade.ImagemComunidade ?? DBNull.Value);

          try
          {
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao criar comunidade: " + ex.Message);
          }
        }
      }
    }

    public bool Atualizar(Comunidade comunidade)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"UPDATE comunidades SET 
          nome = @nome, 
          descricao = @descricao, 
          categoria = @categoria, 
          cor = @cor, 
          imagem_comunidade = @imagem_comunidade 
          WHERE id_comunidade = @id";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@nome", comunidade.Nome);
          cmd.Parameters.AddWithValue("@descricao", (object?)comunidade.Descricao ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@categoria", (object?)comunidade.Categoria ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@cor", comunidade.Cor);
          cmd.Parameters.AddWithValue("@imagem_comunidade", (object?)comunidade.ImagemComunidade ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@id", comunidade.IdComunidade);

          try
          {
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao atualizar comunidade: " + ex.Message);
          }
        }
      }
    }

    public bool Excluir(long idComunidade)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        conn.Open();
        using (MySqlTransaction trans = conn.BeginTransaction())
        {
          try
          {
            // 1. Limpar votos em comentários dos posts da comunidade
            using (MySqlCommand cmd = new MySqlCommand(@"DELETE vc FROM votos_comentario vc 
              INNER JOIN comentarios c ON vc.id_comentario = c.id_comentario 
              INNER JOIN posts p ON c.id_post = p.id_post 
              WHERE p.id_comunidade = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComunidade);
              cmd.ExecuteNonQuery();
            }

            // 2. Limpar comentários dos posts da comunidade
            using (MySqlCommand cmd = new MySqlCommand(@"DELETE c FROM comentarios c 
              INNER JOIN posts p ON c.id_post = p.id_post 
              WHERE p.id_comunidade = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComunidade);
              cmd.ExecuteNonQuery();
            }

            // 3. Limpar votos dos posts da comunidade
            using (MySqlCommand cmd = new MySqlCommand(@"DELETE v FROM votos v 
              INNER JOIN posts p ON v.id_post = p.id_post 
              WHERE p.id_comunidade = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComunidade);
              cmd.ExecuteNonQuery();
            }

            // 4. Limpar posts_salvos dos posts da comunidade
            using (MySqlCommand cmd = new MySqlCommand(@"DELETE ps FROM posts_salvos ps 
              INNER JOIN posts p ON ps.id_post = p.id_post 
              WHERE p.id_comunidade = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComunidade);
              cmd.ExecuteNonQuery();
            }

            // 5. Limpar imagens dos posts da comunidade
            using (MySqlCommand cmd = new MySqlCommand(@"DELETE ip FROM imagens_post ip 
              INNER JOIN posts p ON ip.id_post = p.id_post 
              WHERE p.id_comunidade = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComunidade);
              cmd.ExecuteNonQuery();
            }

            // 6. Limpar interações dos posts da comunidade
            using (MySqlCommand cmd = new MySqlCommand(@"DELETE iu FROM interacoes_usuario iu 
              INNER JOIN posts p ON iu.id_post = p.id_post 
              WHERE p.id_comunidade = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComunidade);
              cmd.ExecuteNonQuery();
            }

            // 7. Deletar posts da comunidade
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM posts WHERE id_comunidade = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComunidade);
              cmd.ExecuteNonQuery();
            }

            // 8. Desvincular membros da comunidade
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM usuario_comunidade WHERE comunidade_id = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComunidade);
              cmd.ExecuteNonQuery();
            }

            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM membros_comunidade WHERE id_comunidade = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComunidade);
              cmd.ExecuteNonQuery();
            }

            // 9. Desvincular eventos associados à comunidade
            using (MySqlCommand cmd = new MySqlCommand("UPDATE eventos SET comunidade_id = NULL WHERE comunidade_id = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComunidade);
              cmd.ExecuteNonQuery();
            }

            // 10. Deletar a comunidade
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM comunidades WHERE id_comunidade = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComunidade);
              int linhas = cmd.ExecuteNonQuery();
              trans.Commit();
              return linhas > 0;
            }
          }
          catch (Exception ex)
          {
            trans.Rollback();
            throw new Exception("Erro ao excluir comunidade: " + ex.Message);
          }
        }
      }
    }

    public DataTable ObterComunidadesCombo()
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = "SELECT id_comunidade, nome FROM comunidades ORDER BY nome ASC";
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

    public DataTable ListarMembros(long idComunidade)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          u.id_usuario AS 'ID Usuário',
          u.username AS 'Username',
          COALESCE(u.nome, u.nome_completo) AS 'Nome',
          u.email AS 'E-mail',
          u.role AS 'Role',
          u.status AS 'Status'
          FROM usuario_comunidade uc
          INNER JOIN usuarios u ON uc.usuario_id = u.id_usuario
          WHERE uc.comunidade_id = @id
          ORDER BY u.nome ASC";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", idComunidade);
          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch (Exception ex)
            {
              throw new Exception("Erro ao carregar membros da comunidade: " + ex.Message);
            }
          }
        }
      }
      return dt;
    }
  }
}
