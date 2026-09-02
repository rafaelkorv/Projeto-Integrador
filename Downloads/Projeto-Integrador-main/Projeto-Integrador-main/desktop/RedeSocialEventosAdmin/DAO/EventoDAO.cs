using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.DAO
{
  public class EventoDAO
  {
    public DataTable Listar(string? filtroStatus = null, string? filtroCategoria = null)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          e.id_evento AS 'ID',
          e.titulo AS 'Título do Evento',
          e.categoria AS 'Categoria',
          e.data_evento AS 'Data',
          e.horario_inicio AS 'Início',
          e.horario_fim AS 'Término',
          e.local_evento AS 'Local',
          e.status AS 'Status',
          COALESCE(c.nome, 'Sem Comunidade') AS 'Comunidade',
          COALESCE(u.nome, u.username, 'Admin') AS 'Organizador',
          (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento) AS 'Inscritos',
          e.limite_participantes AS 'Vagas'
          FROM eventos e
          LEFT JOIN comunidades c ON e.comunidade_id = c.id_comunidade
          LEFT JOIN usuarios u ON e.criador_id = u.id_usuario
          WHERE 1=1";

        if (!string.IsNullOrWhiteSpace(filtroStatus) && filtroStatus != "TODOS")
        {
          sql += " AND e.status = @status";
        }
        if (!string.IsNullOrWhiteSpace(filtroCategoria) && filtroCategoria != "TODAS")
        {
          sql += " AND e.categoria = @categoria";
        }

        sql += " ORDER BY e.id_evento DESC";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          if (!string.IsNullOrWhiteSpace(filtroStatus) && filtroStatus != "TODOS")
            cmd.Parameters.AddWithValue("@status", filtroStatus);
          if (!string.IsNullOrWhiteSpace(filtroCategoria) && filtroCategoria != "TODAS")
            cmd.Parameters.AddWithValue("@categoria", filtroCategoria);

          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch (Exception ex)
            {
              throw new Exception("Erro ao listar eventos: " + ex.Message);
            }
          }
        }
      }
      return dt;
    }

    public DataTable Pesquisar(string termo, string? filtroStatus = null)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          e.id_evento AS 'ID',
          e.titulo AS 'Título do Evento',
          e.categoria AS 'Categoria',
          e.data_evento AS 'Data',
          e.horario_inicio AS 'Início',
          e.horario_fim AS 'Término',
          e.local_evento AS 'Local',
          e.status AS 'Status',
          COALESCE(c.nome, 'Sem Comunidade') AS 'Comunidade',
          COALESCE(u.nome, u.username, 'Admin') AS 'Organizador',
          (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento) AS 'Inscritos',
          e.limite_participantes AS 'Vagas'
          FROM eventos e
          LEFT JOIN comunidades c ON e.comunidade_id = c.id_comunidade
          LEFT JOIN usuarios u ON e.criador_id = u.id_usuario
          WHERE (e.titulo LIKE @termo OR e.local_evento LIKE @termo OR e.categoria LIKE @termo)";

        if (!string.IsNullOrWhiteSpace(filtroStatus) && filtroStatus != "TODOS")
        {
          sql += " AND e.status = @status";
        }

        sql += " ORDER BY e.id_evento DESC";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
          if (!string.IsNullOrWhiteSpace(filtroStatus) && filtroStatus != "TODOS")
            cmd.Parameters.AddWithValue("@status", filtroStatus);

          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch (Exception ex)
            {
              throw new Exception("Erro ao pesquisar eventos: " + ex.Message);
            }
          }
        }
      }
      return dt;
    }

    public Evento? BuscarPorId(long idEvento)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT e.*, c.nome AS nome_comunidade, u.nome AS nome_criador,
          (SELECT COUNT(1) FROM usuario_evento ue WHERE ue.evento_id = e.id_evento) AS total_inscritos
          FROM eventos e
          LEFT JOIN comunidades c ON e.comunidade_id = c.id_comunidade
          LEFT JOIN usuarios u ON e.criador_id = u.id_usuario
          WHERE e.id_evento = @id LIMIT 1";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", idEvento);
          try
          {
            conn.Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
              if (reader.Read())
              {
                return new Evento
                {
                  IdEvento = Convert.ToInt64(reader["id_evento"]),
                  Titulo = reader["titulo"].ToString() ?? "",
                  Descricao = reader["descricao"] != DBNull.Value ? reader["descricao"].ToString() : null,
                  DataEvento = Convert.ToDateTime(reader["data_evento"]),
                  HorarioInicio = (TimeSpan)reader["horario_inicio"],
                  HorarioFim = reader["horario_fim"] != DBNull.Value ? (TimeSpan?)reader["horario_fim"] : null,
                  EncerramentoInscricoes = reader["encerramento_inscricoes"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["encerramento_inscricoes"]) : null,
                  LocalEvento = reader["local_evento"].ToString() ?? "",
                  ComunidadeId = reader["comunidade_id"] != DBNull.Value ? Convert.ToInt64(reader["comunidade_id"]) : null,
                  NomeComunidade = reader["nome_comunidade"] != DBNull.Value ? reader["nome_comunidade"].ToString() : null,
                  CriadorId = reader["criador_id"] != DBNull.Value ? Convert.ToInt64(reader["criador_id"]) : null,
                  NomeCriador = reader["nome_criador"] != DBNull.Value ? reader["nome_criador"].ToString() : null,
                  LimiteParticipantes = reader["limite_participantes"] != DBNull.Value ? Convert.ToInt32(reader["limite_participantes"]) : null,
                  Status = reader["status"].ToString() ?? "AGENDADO",
                  ExigeCheckin = reader["exige_checkin"] != DBNull.Value && Convert.ToBoolean(reader["exige_checkin"]),
                  Categoria = reader["categoria"] != DBNull.Value ? reader["categoria"].ToString() : null,
                  ImagemCapa = reader["imagem_capa"] != DBNull.Value ? reader["imagem_capa"].ToString() : null,
                  TotalParticipantes = Convert.ToInt32(reader["total_inscritos"])
                };
              }
            }
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao buscar evento por ID: " + ex.Message);
          }
        }
      }
      return null;
    }

    public bool Inserir(Evento evento)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"INSERT INTO eventos 
          (titulo, descricao, data_evento, horario_inicio, horario_fim, encerramento_inscricoes, 
           local_evento, comunidade_id, criador_id, limite_participantes, status, exige_checkin, categoria, imagem_capa) 
          VALUES 
          (@titulo, @descricao, @data_evento, @horario_inicio, @horario_fim, @encerramento_inscricoes, 
           @local_evento, @comunidade_id, @criador_id, @limite_participantes, @status, @exige_checkin, @categoria, @imagem_capa)";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@titulo", evento.Titulo);
          cmd.Parameters.AddWithValue("@descricao", (object?)evento.Descricao ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@data_evento", evento.DataEvento.ToString("yyyy-MM-dd"));
          cmd.Parameters.AddWithValue("@horario_inicio", evento.HorarioInicio);
          cmd.Parameters.AddWithValue("@horario_fim", (object?)evento.HorarioFim ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@encerramento_inscricoes", evento.EncerramentoInscricoes.HasValue ? evento.EncerramentoInscricoes.Value.ToString("yyyy-MM-dd HH:mm:ss") : DBNull.Value);
          cmd.Parameters.AddWithValue("@local_evento", evento.LocalEvento);
          cmd.Parameters.AddWithValue("@comunidade_id", (object?)evento.ComunidadeId ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@criador_id", (object?)evento.CriadorId ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@limite_participantes", (object?)evento.LimiteParticipantes ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(evento.Status) ? "AGENDADO" : evento.Status);
          cmd.Parameters.AddWithValue("@exige_checkin", evento.ExigeCheckin ? 1 : 0);
          cmd.Parameters.AddWithValue("@categoria", (object?)evento.Categoria ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@imagem_capa", (object?)evento.ImagemCapa ?? DBNull.Value);

          try
          {
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao criar evento: " + ex.Message);
          }
        }
      }
    }

    public bool Atualizar(Evento evento)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"UPDATE eventos SET 
          titulo = @titulo,
          descricao = @descricao,
          data_evento = @data_evento,
          horario_inicio = @horario_inicio,
          horario_fim = @horario_fim,
          encerramento_inscricoes = @encerramento_inscricoes,
          local_evento = @local_evento,
          comunidade_id = @comunidade_id,
          limite_participantes = @limite_participantes,
          status = @status,
          exige_checkin = @exige_checkin,
          categoria = @categoria,
          imagem_capa = @imagem_capa
          WHERE id_evento = @id";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@titulo", evento.Titulo);
          cmd.Parameters.AddWithValue("@descricao", (object?)evento.Descricao ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@data_evento", evento.DataEvento.ToString("yyyy-MM-dd"));
          cmd.Parameters.AddWithValue("@horario_inicio", evento.HorarioInicio);
          cmd.Parameters.AddWithValue("@horario_fim", (object?)evento.HorarioFim ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@encerramento_inscricoes", evento.EncerramentoInscricoes.HasValue ? evento.EncerramentoInscricoes.Value.ToString("yyyy-MM-dd HH:mm:ss") : DBNull.Value);
          cmd.Parameters.AddWithValue("@local_evento", evento.LocalEvento);
          cmd.Parameters.AddWithValue("@comunidade_id", (object?)evento.ComunidadeId ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@limite_participantes", (object?)evento.LimiteParticipantes ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@status", evento.Status);
          cmd.Parameters.AddWithValue("@exige_checkin", evento.ExigeCheckin ? 1 : 0);
          cmd.Parameters.AddWithValue("@categoria", (object?)evento.Categoria ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@imagem_capa", (object?)evento.ImagemCapa ?? DBNull.Value);
          cmd.Parameters.AddWithValue("@id", evento.IdEvento);

          try
          {
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao atualizar evento: " + ex.Message);
          }
        }
      }
    }

    public bool AlterarStatus(long idEvento, string novoStatus)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = "UPDATE eventos SET status = @status WHERE id_evento = @id";
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@status", novoStatus);
          cmd.Parameters.AddWithValue("@id", idEvento);
          try
          {
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
          }
          catch (Exception ex)
          {
            throw new Exception("Erro ao alterar status do evento: " + ex.Message);
          }
        }
      }
    }

    public bool Excluir(long idEvento)
    {
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        conn.Open();
        using (MySqlTransaction trans = conn.BeginTransaction())
        {
          try
          {
            // 1. Limpar participantes inscritos
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM usuario_evento WHERE evento_id = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idEvento);
              cmd.ExecuteNonQuery();
            }

            // 2. Deletar o evento
            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM eventos WHERE id_evento = @id", conn, trans))
            {
              cmd.Parameters.AddWithValue("@id", idEvento);
              int linhas = cmd.ExecuteNonQuery();
              trans.Commit();
              return linhas > 0;
            }
          }
          catch (Exception ex)
          {
            trans.Rollback();
            throw new Exception("Erro ao excluir evento e dependências: " + ex.Message);
          }
        }
      }
    }

    public DataTable ListarParticipantes(long idEvento)
    {
      DataTable dt = new DataTable();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = @"SELECT 
          ue.id_participacao AS 'ID Inscrição',
          u.id_usuario AS 'ID Usuário',
          u.username AS 'Username',
          COALESCE(u.nome, u.nome_completo) AS 'Nome',
          u.email AS 'E-mail',
          ue.status AS 'Status Inscrição',
          ue.token_ingresso AS 'Token',
          ue.data_checkin AS 'Check-in Realizado'
          FROM usuario_evento ue
          INNER JOIN usuarios u ON ue.usuario_id = u.id_usuario
          WHERE ue.evento_id = @id
          ORDER BY ue.id_participacao DESC";

        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", idEvento);
          using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
          {
            try
            {
              da.Fill(dt);
            }
            catch (Exception ex)
            {
              throw new Exception("Erro ao carregar participantes do evento: " + ex.Message);
            }
          }
        }
      }
      return dt;
    }

    public List<string> ObterCategorias()
    {
      var list = new List<string>();
      using (MySqlConnection conn = Conexao.GetConnection())
      {
        string sql = "SELECT DISTINCT categoria FROM eventos WHERE categoria IS NOT NULL AND categoria != '' ORDER BY categoria";
        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        {
          try
          {
            conn.Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
              while (reader.Read())
              {
                list.Add(reader.GetString(0));
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
