using Npgsql;
using BibliotecaJK.Model;
using System.Collections.Generic;
using System;

namespace BibliotecaJK.DAL
{
    public class LogAcaoDAL
    {
        public void Inserir(LogAcao log)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "INSERT INTO Log_Acao (id_funcionario, acao, descricao, data_hora) VALUES (@idfunc,@acao,@desc,@datahora)";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idfunc", (object?)log.IdFuncionario ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@acao", (object?)log.Acao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@desc", (object?)log.Descricao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@datahora", log.DataHora);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao inserir log de acao: {ex.Message}", ex);
            }
        }

        public List<LogAcao> Listar()
        {
            var lista = new List<LogAcao>();
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Log_Acao";
                using var cmd = new NpgsqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();

                // Cache ordinals for performance
                int ordId = reader.GetOrdinal("id_log");
                int ordIdFunc = reader.GetOrdinal("id_funcionario");
                int ordAcao = reader.GetOrdinal("acao");
                int ordDesc = reader.GetOrdinal("descricao");
                int ordDataHora = reader.GetOrdinal("data_hora");

                while (reader.Read())
                {
                    lista.Add(new LogAcao
                    {
                        Id = reader.GetInt32(ordId),
                        IdFuncionario = reader.IsDBNull(ordIdFunc) ? (int?)null : reader.GetInt32(ordIdFunc),
                        Acao = reader.IsDBNull(ordAcao) ? null : reader.GetString(ordAcao),
                        Descricao = reader.IsDBNull(ordDesc) ? null : reader.GetString(ordDesc),
                        DataHora = reader.GetDateTime(ordDataHora)
                    });
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao listar logs de acao: {ex.Message}", ex);
            }
            return lista;
        }

        public LogAcao? ObterPorId(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Log_Acao WHERE id_log=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new LogAcao
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_log")),
                        IdFuncionario = reader.IsDBNull(reader.GetOrdinal("id_funcionario")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("id_funcionario")),
                        Acao = reader.IsDBNull(reader.GetOrdinal("acao")) ? null : reader.GetString(reader.GetOrdinal("acao")),
                        Descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? null : reader.GetString(reader.GetOrdinal("descricao")),
                        DataHora = reader.GetDateTime(reader.GetOrdinal("data_hora"))
                    };
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao obter log de acao por ID: {ex.Message}", ex);
            }
            return null;
        }

        public void Atualizar(LogAcao log)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "UPDATE Log_Acao SET id_funcionario=@idfunc, acao=@acao, descricao=@desc, data_hora=@datahora WHERE id_log=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idfunc", (object?)log.IdFuncionario ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@acao", (object?)log.Acao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@desc", (object?)log.Descricao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@datahora", log.DataHora);
                cmd.Parameters.AddWithValue("@id", log.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao atualizar log de acao: {ex.Message}", ex);
            }
        }

        public void Excluir(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "DELETE FROM Log_Acao WHERE id_log=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao excluir log de acao: {ex.Message}", ex);
            }
        }
    }
}
