using System;
using MySql.Data.MySqlClient;

namespace RedeSocialEventosAdmin.DAO
{
  public static class Conexao
  {
    private static readonly string ConnectionString = "Server=143.106.241.3;Database=cl203108;Uid=cl203108;Pwd=cl*29082007;CharSet=utf8mb4;Connect Timeout=30;";

    public static MySqlConnection GetConnection()
    {
      try
      {
        return new MySqlConnection(ConnectionString);
      }
      catch (Exception ex)
      {
        throw new Exception("Erro ao inicializar a conexão com o banco de dados: " + ex.Message);
      }
    }
  }
}