using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace RedeSocialEventosAdmin.DAO
{
  public class ModeracaoDAO
  {
    public DataTable ListarPosts(string? termo = null)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          p.id_post AS 'ID Post',
          p.titulo AS 'Título',
          p.conteudo AS 'Conteúdo',
          COALESCE(u.nome, u.username, 'Anônimo') AS 'Autor',
          u.email AS 'E-mail Autor',
          COALESCE(c.nome, 'Feed Geral') AS 'Comunidade',
          (SELECT COUNT(1) FROM comentarios com WHERE com.id_post = p.id_post) AS 'Comentários',
          (SELECT COUNT(1) FROM votos v WHERE v.id_post = p.id_post) AS 'Votos',
          p.data_postagem AS 'Data Postagem'
          FROM posts p
          LEFT JOIN usuarios u ON p.id_usuario = u.id_usuario
          LEFT JOIN comunidades c ON p.id_comunidade = c.id_comunidade
          WHERE 1=1";

        if (!string.IsNullOrWhiteSpace(termo))
        {
          sql += " AND (p.titulo LIKE @termo OR p.conteudo LIKE @termo OR u.nome LIKE @termo OR u.username LIKE @termo)";
        }

        sql += " ORDER BY p.id_post DESC LIMIT 200";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          if (!string.IsNullOrWhiteSpace(termo))
          {
            cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
          }

          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch (Exception ex)
            {
              throw new Exception("Erro ao listar publicações para moderação: " + ex.Message);
            }
          }
        }
      }
      return dt;
    }

    public bool ExcluirPost(long idPost)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        conn.Open();
        using (MySqlTransaction trans = conn.BeginTransaction())
        {
          try
          {
            // 1. Excluir votos de comentários deste post
            using (MySqlCommand cmd = new MySqlCommand(@"DELETE vc FROM votos_comentario vc 
              INNER JOIN comentarios c ON vc.id_comentario = c.id_comentario 
              WHERE c.id_post = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idPost);
              cmd.ExecuteNonQuery();
            }

            // 2. Excluir comentários deste post
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM comentarios WHERE id_post = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idPost);
              cmd.ExecuteNonQuery();
            }

            // 3. Excluir votos deste post
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM votos WHERE id_post = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idPost);
              cmd.ExecuteNonQuery();
            }

            // 4. Excluir posts salvos deste post
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM posts_salvos WHERE id_post = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idPost);
              cmd.ExecuteNonQuery();
            }

            // 5. Excluir imagens deste post
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM imagens_post WHERE id_post = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idPost);
              cmd.ExecuteNonQuery();
            }

            // 6. Excluir interações deste post
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM interacoes_usuario WHERE id_post = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idPost);
              cmd.ExecuteNonQuery();
            }

            // 7. Excluir post
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM posts WHERE id_post = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idPost);
              int linhas = cmd.ExecuteNonQuery();
              trans.Commit();
              return linhas > 0;
            }
          }
          catch (Exception ex)
          {
            trans.Rollback();
            throw new Exception("Erro ao excluir publicação: " + ex.Message);
          }
        }
      }
    }

    public DataTable ListarComentarios(string? termo = null)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          c.id_comentario AS 'ID Comentário',
          c.conteudo AS 'Conteúdo',
          COALESCE(u.nome, u.username, 'Anônimo') AS 'Autor',
          u.email AS 'E-mail Autor',
          COALESCE(p.titulo, 'Post ID #' + CAST(p.id_post AS CHAR)) AS 'Publicação de Origem',
          c.id_post AS 'ID Post',
          c.data_comentario AS 'Data'
          FROM comentarios c
          LEFT JOIN usuarios u ON c.id_usuario = u.id_usuario
          LEFT JOIN posts p ON c.id_post = p.id_post
          WHERE 1=1";

        if (!string.IsNullOrWhiteSpace(termo))
        {
          sql += " AND (c.conteudo LIKE @termo OR u.nome LIKE @termo OR u.username LIKE @termo)";
        }

        sql += " ORDER BY c.id_comentario DESC LIMIT 200";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          if (!string.IsNullOrWhiteSpace(termo))
          {
            cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
          }

          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch (Exception ex)
            {
              throw new Exception("Erro ao listar comentários para moderação: " + ex.Message);
            }
          }
        }
      }
      return dt;
    }

    public bool ExcluirComentario(long idComentario)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        conn.Open();
        using (MySqlTransaction trans = conn.BeginTransaction())
        {
          try
          {
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM votos_comentario WHERE id_comentario = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComentario);
              cmd.ExecuteNonQuery();
            }

            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM comentarios WHERE id_comentario = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idComentario);
              int linhas = cmd.ExecuteNonQuery();
              trans.Commit();
              return linhas > 0;
            }
          }
          catch (Exception ex)
          {
            trans.Rollback();
            throw new Exception("Erro ao excluir comentário: " + ex.Message);
          }
        }
      }
    }
  }
}
