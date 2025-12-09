using Npgsql;
using BibliotecaJK.Model;
using System;
using System.Collections.Generic;

namespace BibliotecaJK.DAL
{
    public class AlunoDAL
    {
        public void Inserir(Aluno aluno)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "INSERT INTO Aluno (nome, cpf, matricula, turma, telefone, email) VALUES (@nome,@cpf,@matricula,@turma,@telefone,@email)";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@nome", aluno.Nome);
                cmd.Parameters.AddWithValue("@cpf", aluno.CPF);
                cmd.Parameters.AddWithValue("@matricula", aluno.Matricula);
                cmd.Parameters.AddWithValue("@turma", (object?)aluno.Turma ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@telefone", (object?)aluno.Telefone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@email", (object?)aluno.Email ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao inserir aluno: {ex.Message}", ex);
            }
        }

        public List<Aluno> Listar()
        {
            var lista = new List<Aluno>();
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Aluno";
                using var cmd = new NpgsqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();

                // Cache ordinals for performance
                int ordId = reader.GetOrdinal("id_aluno");
                int ordNome = reader.GetOrdinal("nome");
                int ordCpf = reader.GetOrdinal("cpf");
                int ordMatricula = reader.GetOrdinal("matricula");
                int ordTurma = reader.GetOrdinal("turma");
                int ordTelefone = reader.GetOrdinal("telefone");
                int ordEmail = reader.GetOrdinal("email");

                while (reader.Read())
                {
                    lista.Add(new Aluno
                    {
                        Id = reader.GetInt32(ordId),
                        Nome = reader.GetString(ordNome),
                        CPF = reader.GetString(ordCpf),
                        Matricula = reader.GetString(ordMatricula),
                        Turma = reader.IsDBNull(ordTurma) ? null : reader.GetString(ordTurma),
                        Telefone = reader.IsDBNull(ordTelefone) ? null : reader.GetString(ordTelefone),
                        Email = reader.IsDBNull(ordEmail) ? null : reader.GetString(ordEmail)
                    });
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao listar alunos: {ex.Message}", ex);
            }
            return lista;
        }

        public Aluno? ObterPorId(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Aluno WHERE id_aluno=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Aluno
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_aluno")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        CPF = reader.GetString(reader.GetOrdinal("cpf")),
                        Matricula = reader.GetString(reader.GetOrdinal("matricula")),
                        Turma = reader.IsDBNull(reader.GetOrdinal("turma")) ? null : reader.GetString(reader.GetOrdinal("turma")),
                        Telefone = reader.IsDBNull(reader.GetOrdinal("telefone")) ? null : reader.GetString(reader.GetOrdinal("telefone")),
                        Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"))
                    };
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao obter aluno por ID: {ex.Message}", ex);
            }
            return null;
        }

        public void Atualizar(Aluno aluno)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "UPDATE Aluno SET nome=@nome, cpf=@cpf, matricula=@matricula, turma=@turma, telefone=@telefone, email=@email WHERE id_aluno=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@nome", aluno.Nome);
                cmd.Parameters.AddWithValue("@cpf", aluno.CPF);
                cmd.Parameters.AddWithValue("@matricula", aluno.Matricula);
                cmd.Parameters.AddWithValue("@turma", (object?)aluno.Turma ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@telefone", (object?)aluno.Telefone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@email", (object?)aluno.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", aluno.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao atualizar aluno: {ex.Message}", ex);
            }
        }

        public void Excluir(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "DELETE FROM Aluno WHERE id_aluno=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao excluir aluno: {ex.Message}", ex);
            }
        }
    }
}
