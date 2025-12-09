using Npgsql;
using BibliotecaJK.Model;
using System;
using System.Collections.Generic;

namespace BibliotecaJK.DAL
{
    public class FuncionarioDAL
    {
        public void Inserir(Funcionario f)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "INSERT INTO Funcionario (nome, cpf, cargo, login, senha_hash, perfil, primeiro_login) VALUES (@nome,@cpf,@cargo,@login,@senha,@perfil,@primeiro)";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@nome", f.Nome);
                cmd.Parameters.AddWithValue("@cpf", f.CPF);
                cmd.Parameters.AddWithValue("@cargo", (object?)f.Cargo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@login", f.Login);
                cmd.Parameters.AddWithValue("@senha", f.SenhaHash);
                cmd.Parameters.AddWithValue("@perfil", f.Perfil);
                cmd.Parameters.AddWithValue("@primeiro", f.PrimeiroLogin);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao inserir funcionario: {ex.Message}", ex);
            }
        }

        public List<Funcionario> Listar()
        {
            var lista = new List<Funcionario>();
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Funcionario";
                using var cmd = new NpgsqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();

                // Cache ordinals for performance
                int ordId = reader.GetOrdinal("id_funcionario");
                int ordNome = reader.GetOrdinal("nome");
                int ordCpf = reader.GetOrdinal("cpf");
                int ordCargo = reader.GetOrdinal("cargo");
                int ordLogin = reader.GetOrdinal("login");
                int ordSenha = reader.GetOrdinal("senha_hash");
                int ordPerfil = reader.GetOrdinal("perfil");
                int ordPrimeiro = reader.GetOrdinal("primeiro_login");

                while (reader.Read())
                {
                    lista.Add(new Funcionario
                    {
                        Id = reader.GetInt32(ordId),
                        Nome = reader.GetString(ordNome),
                        CPF = reader.GetString(ordCpf),
                        Cargo = reader.IsDBNull(ordCargo) ? null : reader.GetString(ordCargo),
                        Login = reader.GetString(ordLogin),
                        SenhaHash = reader.GetString(ordSenha),
                        Perfil = reader.GetString(ordPerfil),
                        PrimeiroLogin = reader.GetBoolean(ordPrimeiro)
                    });
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao listar funcionarios: {ex.Message}", ex);
            }
            return lista;
        }

        public Funcionario? ObterPorId(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Funcionario WHERE id_funcionario=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Funcionario
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_funcionario")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        CPF = reader.GetString(reader.GetOrdinal("cpf")),
                        Cargo = reader.IsDBNull(reader.GetOrdinal("cargo")) ? null : reader.GetString(reader.GetOrdinal("cargo")),
                        Login = reader.GetString(reader.GetOrdinal("login")),
                        SenhaHash = reader.GetString(reader.GetOrdinal("senha_hash")),
                        Perfil = reader.GetString(reader.GetOrdinal("perfil")),
                        PrimeiroLogin = reader.GetBoolean(reader.GetOrdinal("primeiro_login"))
                    };
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao obter funcionario por ID: {ex.Message}", ex);
            }
            return null;
        }

        public void Atualizar(Funcionario f)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "UPDATE Funcionario SET nome=@nome, cpf=@cpf, cargo=@cargo, login=@login, senha_hash=@senha, perfil=@perfil, primeiro_login=@primeiro WHERE id_funcionario=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@nome", f.Nome);
                cmd.Parameters.AddWithValue("@cpf", f.CPF);
                cmd.Parameters.AddWithValue("@cargo", (object?)f.Cargo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@login", f.Login);
                cmd.Parameters.AddWithValue("@senha", f.SenhaHash);
                cmd.Parameters.AddWithValue("@perfil", f.Perfil);
                cmd.Parameters.AddWithValue("@primeiro", f.PrimeiroLogin);
                cmd.Parameters.AddWithValue("@id", f.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao atualizar funcionario: {ex.Message}", ex);
            }
        }

        public void Excluir(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "DELETE FROM Funcionario WHERE id_funcionario=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao excluir funcionario: {ex.Message}", ex);
            }
        }
    }
}
