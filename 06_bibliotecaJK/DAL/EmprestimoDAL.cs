using Npgsql;
using BibliotecaJK.Model;
using System.Collections.Generic;
using System;

namespace BibliotecaJK.DAL
{
    public class EmprestimoDAL
    {
        public void Inserir(Emprestimo e)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "INSERT INTO Emprestimo (id_aluno, id_livro, data_emprestimo, data_prevista, data_devolucao, multa) " +
                             "VALUES (@idaluno,@idlivro,@dataemp,@dataprev,@datadev,@multa)";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idaluno", e.IdAluno);
                cmd.Parameters.AddWithValue("@idlivro", e.IdLivro);
                cmd.Parameters.AddWithValue("@dataemp", e.DataEmprestimo.Date);
                cmd.Parameters.AddWithValue("@dataprev", e.DataPrevista.Date);
                cmd.Parameters.AddWithValue("@datadev", (object?)e.DataDevolucao?.Date ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@multa", e.Multa);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao inserir emprestimo: {ex.Message}", ex);
            }
        }

        public List<Emprestimo> Listar()
        {
            var lista = new List<Emprestimo>();
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Emprestimo";
                using var cmd = new NpgsqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();

                // Cache ordinals for performance
                int ordId = reader.GetOrdinal("id_emprestimo");
                int ordIdAluno = reader.GetOrdinal("id_aluno");
                int ordIdLivro = reader.GetOrdinal("id_livro");
                int ordDataEmp = reader.GetOrdinal("data_emprestimo");
                int ordDataPrev = reader.GetOrdinal("data_prevista");
                int ordDataDev = reader.GetOrdinal("data_devolucao");
                int ordMulta = reader.GetOrdinal("multa");

                while (reader.Read())
                {
                    lista.Add(new Emprestimo
                    {
                        Id = reader.GetInt32(ordId),
                        IdAluno = reader.GetInt32(ordIdAluno),
                        IdLivro = reader.GetInt32(ordIdLivro),
                        DataEmprestimo = reader.GetDateTime(ordDataEmp),
                        DataPrevista = reader.GetDateTime(ordDataPrev),
                        DataDevolucao = reader.IsDBNull(ordDataDev) ? (DateTime?)null : reader.GetDateTime(ordDataDev),
                        Multa = reader.GetDecimal(ordMulta)
                    });
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao listar emprestimos: {ex.Message}", ex);
            }
            return lista;
        }

        public Emprestimo? ObterPorId(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Emprestimo WHERE id_emprestimo=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Emprestimo
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_emprestimo")),
                        IdAluno = reader.GetInt32(reader.GetOrdinal("id_aluno")),
                        IdLivro = reader.GetInt32(reader.GetOrdinal("id_livro")),
                        DataEmprestimo = reader.GetDateTime(reader.GetOrdinal("data_emprestimo")),
                        DataPrevista = reader.GetDateTime(reader.GetOrdinal("data_prevista")),
                        DataDevolucao = reader.IsDBNull(reader.GetOrdinal("data_devolucao")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("data_devolucao")),
                        Multa = reader.GetDecimal(reader.GetOrdinal("multa"))
                    };
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao obter emprestimo por ID: {ex.Message}", ex);
            }
            return null;
        }

        public void Atualizar(Emprestimo e)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "UPDATE Emprestimo SET id_aluno=@idaluno, id_livro=@idlivro, data_emprestimo=@dataemp, data_prevista=@dataprev, data_devolucao=@datadev, multa=@multa WHERE id_emprestimo=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idaluno", e.IdAluno);
                cmd.Parameters.AddWithValue("@idlivro", e.IdLivro);
                cmd.Parameters.AddWithValue("@dataemp", e.DataEmprestimo.Date);
                cmd.Parameters.AddWithValue("@dataprev", e.DataPrevista.Date);
                cmd.Parameters.AddWithValue("@datadev", (object?)e.DataDevolucao?.Date ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@multa", e.Multa);
                cmd.Parameters.AddWithValue("@id", e.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao atualizar emprestimo: {ex.Message}", ex);
            }
        }

        public void Excluir(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "DELETE FROM Emprestimo WHERE id_emprestimo=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao excluir emprestimo: {ex.Message}", ex);
            }
        }
    }
}
