using Npgsql;
using BibliotecaJK.Model;
using System.Collections.Generic;
using System;

namespace BibliotecaJK.DAL
{
    public class ReservaDAL
    {
        public void Inserir(Reserva r)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "INSERT INTO Reserva (id_aluno, id_livro, data_reserva, status) VALUES (@idaluno,@idlivro,@datares,@status)";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idaluno", r.IdAluno);
                cmd.Parameters.AddWithValue("@idlivro", r.IdLivro);
                cmd.Parameters.AddWithValue("@datares", r.DataReserva.Date);
                cmd.Parameters.AddWithValue("@status", r.Status);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao inserir reserva: {ex.Message}", ex);
            }
        }

        public List<Reserva> Listar()
        {
            var lista = new List<Reserva>();
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Reserva";
                using var cmd = new NpgsqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();

                // Cache ordinals for performance
                int ordId = reader.GetOrdinal("id_reserva");
                int ordIdAluno = reader.GetOrdinal("id_aluno");
                int ordIdLivro = reader.GetOrdinal("id_livro");
                int ordDataRes = reader.GetOrdinal("data_reserva");
                int ordStatus = reader.GetOrdinal("status");

                while (reader.Read())
                {
                    lista.Add(new Reserva
                    {
                        Id = reader.GetInt32(ordId),
                        IdAluno = reader.GetInt32(ordIdAluno),
                        IdLivro = reader.GetInt32(ordIdLivro),
                        DataReserva = reader.GetDateTime(ordDataRes),
                        Status = reader.GetString(ordStatus)
                    });
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao listar reservas: {ex.Message}", ex);
            }
            return lista;
        }

        public Reserva? ObterPorId(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Reserva WHERE id_reserva=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Reserva
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_reserva")),
                        IdAluno = reader.GetInt32(reader.GetOrdinal("id_aluno")),
                        IdLivro = reader.GetInt32(reader.GetOrdinal("id_livro")),
                        DataReserva = reader.GetDateTime(reader.GetOrdinal("data_reserva")),
                        Status = reader.GetString(reader.GetOrdinal("status"))
                    };
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao obter reserva por ID: {ex.Message}", ex);
            }
            return null;
        }

        public void Atualizar(Reserva r)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "UPDATE Reserva SET id_aluno=@idaluno, id_livro=@idlivro, data_reserva=@datares, status=@status WHERE id_reserva=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idaluno", r.IdAluno);
                cmd.Parameters.AddWithValue("@idlivro", r.IdLivro);
                cmd.Parameters.AddWithValue("@datares", r.DataReserva.Date);
                cmd.Parameters.AddWithValue("@status", r.Status);
                cmd.Parameters.AddWithValue("@id", r.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao atualizar reserva: {ex.Message}", ex);
            }
        }

        public void Excluir(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "DELETE FROM Reserva WHERE id_reserva=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao excluir reserva: {ex.Message}", ex);
            }
        }
    }
}
