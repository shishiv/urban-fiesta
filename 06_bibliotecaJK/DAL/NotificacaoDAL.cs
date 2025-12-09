using Npgsql;
using BibliotecaJK.Model;
using System;
using System.Collections.Generic;

namespace BibliotecaJK.DAL
{
    public class NotificacaoDAL
    {
        /// <summary>
        /// Lista todas as notificacoes ordenadas por prioridade e data
        /// </summary>
        public List<Notificacao> Listar()
        {
            var lista = new List<Notificacao>();
            try
            {
                using var conn = Conexao.GetConnection();

                string sql = @"SELECT * FROM Notificacao
                              ORDER BY
                                CASE prioridade
                                  WHEN 'URGENTE' THEN 1
                                  WHEN 'ALTA' THEN 2
                                  WHEN 'NORMAL' THEN 3
                                  WHEN 'BAIXA' THEN 4
                                END,
                                data_criacao DESC";

                using var cmd = new NpgsqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(MapearNotificacao(reader));
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao listar notificacoes: {ex.Message}", ex);
            }
            return lista;
        }

        /// <summary>
        /// Lista notificacoes nao lidas
        /// </summary>
        public List<Notificacao> ListarNaoLidas()
        {
            var lista = new List<Notificacao>();
            try
            {
                using var conn = Conexao.GetConnection();

                string sql = @"SELECT * FROM Notificacao
                              WHERE lida = FALSE
                              ORDER BY
                                CASE prioridade
                                  WHEN 'URGENTE' THEN 1
                                  WHEN 'ALTA' THEN 2
                                  WHEN 'NORMAL' THEN 3
                                  WHEN 'BAIXA' THEN 4
                                END,
                                data_criacao DESC";

                using var cmd = new NpgsqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(MapearNotificacao(reader));
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao listar notificacoes nao lidas: {ex.Message}", ex);
            }
            return lista;
        }

        /// <summary>
        /// Lista notificacoes de um funcionario especifico
        /// </summary>
        public List<Notificacao> ListarPorFuncionario(int idFuncionario)
        {
            var lista = new List<Notificacao>();
            try
            {
                using var conn = Conexao.GetConnection();

                string sql = @"SELECT * FROM Notificacao
                              WHERE id_funcionario = @id_funcionario OR id_funcionario IS NULL
                              ORDER BY
                                CASE prioridade
                                  WHEN 'URGENTE' THEN 1
                                  WHEN 'ALTA' THEN 2
                                  WHEN 'NORMAL' THEN 3
                                  WHEN 'BAIXA' THEN 4
                                END,
                                data_criacao DESC";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id_funcionario", idFuncionario);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(MapearNotificacao(reader));
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao listar notificacoes por funcionario: {ex.Message}", ex);
            }
            return lista;
        }

        /// <summary>
        /// Conta quantas notificacoes nao lidas existem
        /// </summary>
        public int ContarNaoLidas()
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT COUNT(*) FROM Notificacao WHERE lida = FALSE";

                using var cmd = new NpgsqlCommand(sql, conn);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao contar notificacoes nao lidas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Marca uma notificacao como lida
        /// </summary>
        public void MarcarComoLida(int idNotificacao)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "UPDATE Notificacao SET lida = TRUE, data_leitura = CURRENT_TIMESTAMP WHERE id_notificacao = @id";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", idNotificacao);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao marcar notificacao como lida: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Marca todas as notificacoes como lidas
        /// </summary>
        public void MarcarTodasComoLidas()
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "UPDATE Notificacao SET lida = TRUE, data_leitura = CURRENT_TIMESTAMP WHERE lida = FALSE";

                using var cmd = new NpgsqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao marcar todas notificacoes como lidas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtem uma notificacao por ID
        /// </summary>
        public Notificacao? ObterPorId(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Notificacao WHERE id_notificacao = @id";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return MapearNotificacao(reader);
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao obter notificacao por ID: {ex.Message}", ex);
            }
            return null;
        }

        /// <summary>
        /// Exclui uma notificacao
        /// </summary>
        public void Excluir(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "DELETE FROM Notificacao WHERE id_notificacao = @id";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao excluir notificacao: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Exclui todas as notificacoes lidas
        /// </summary>
        public void ExcluirLidas()
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "DELETE FROM Notificacao WHERE lida = TRUE";

                using var cmd = new NpgsqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao excluir notificacoes lidas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Mapeia um reader para um objeto Notificacao
        /// </summary>
        private Notificacao MapearNotificacao(NpgsqlDataReader reader)
        {
            // Cache ordinals for performance
            int ordId = reader.GetOrdinal("id_notificacao");
            int ordTipo = reader.GetOrdinal("tipo");
            int ordTitulo = reader.GetOrdinal("titulo");
            int ordMensagem = reader.GetOrdinal("mensagem");
            int ordIdAluno = reader.GetOrdinal("id_aluno");
            int ordIdFunc = reader.GetOrdinal("id_funcionario");
            int ordIdEmp = reader.GetOrdinal("id_emprestimo");
            int ordIdRes = reader.GetOrdinal("id_reserva");
            int ordLida = reader.GetOrdinal("lida");
            int ordPrioridade = reader.GetOrdinal("prioridade");
            int ordDataCriacao = reader.GetOrdinal("data_criacao");
            int ordDataLeitura = reader.GetOrdinal("data_leitura");

            return new Notificacao
            {
                Id = reader.GetInt32(ordId),
                Tipo = reader.GetString(ordTipo),
                Titulo = reader.GetString(ordTitulo),
                Mensagem = reader.GetString(ordMensagem),
                IdAluno = reader.IsDBNull(ordIdAluno) ? null : reader.GetInt32(ordIdAluno),
                IdFuncionario = reader.IsDBNull(ordIdFunc) ? null : reader.GetInt32(ordIdFunc),
                IdEmprestimo = reader.IsDBNull(ordIdEmp) ? null : reader.GetInt32(ordIdEmp),
                IdReserva = reader.IsDBNull(ordIdRes) ? null : reader.GetInt32(ordIdRes),
                Lida = reader.GetBoolean(ordLida),
                Prioridade = reader.GetString(ordPrioridade),
                DataCriacao = reader.GetDateTime(ordDataCriacao),
                DataLeitura = reader.IsDBNull(ordDataLeitura) ? null : reader.GetDateTime(ordDataLeitura)
            };
        }
    }
}
