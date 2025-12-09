using Npgsql;
using BibliotecaJK.Model;
using System;
using System.Collections.Generic;

namespace BibliotecaJK.DAL
{
    public class LivroDAL
    {
        public void Inserir(Livro livro)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "INSERT INTO Livro (titulo, autor, isbn, editora, ano_publicacao, categoria, quantidade_total, quantidade_disponivel, localizacao) " +
                             "VALUES (@titulo, @autor, @isbn, @editora, @ano, @categoria, @total, @disp, @loc)";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@titulo", livro.Titulo);
                cmd.Parameters.AddWithValue("@autor", (object?)livro.Autor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isbn", (object?)livro.ISBN ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@editora", (object?)livro.Editora ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ano", (object?)livro.AnoPublicacao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@categoria", (object?)livro.Categoria ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@total", livro.QuantidadeTotal);
                cmd.Parameters.AddWithValue("@disp", livro.QuantidadeDisponivel);
                cmd.Parameters.AddWithValue("@loc", (object?)livro.Localizacao ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao inserir livro: {ex.Message}", ex);
            }
        }

        public List<Livro> Listar()
        {
            var lista = new List<Livro>();
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Livro";
                using var cmd = new NpgsqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();

                // Cache ordinals for performance
                int ordId = reader.GetOrdinal("id_livro");
                int ordTitulo = reader.GetOrdinal("titulo");
                int ordAutor = reader.GetOrdinal("autor");
                int ordIsbn = reader.GetOrdinal("isbn");
                int ordEditora = reader.GetOrdinal("editora");
                int ordAno = reader.GetOrdinal("ano_publicacao");
                int ordCategoria = reader.GetOrdinal("categoria");
                int ordTotal = reader.GetOrdinal("quantidade_total");
                int ordDisp = reader.GetOrdinal("quantidade_disponivel");
                int ordLoc = reader.GetOrdinal("localizacao");

                while (reader.Read())
                {
                    lista.Add(new Livro
                    {
                        Id = reader.GetInt32(ordId),
                        Titulo = reader.GetString(ordTitulo),
                        Autor = reader.IsDBNull(ordAutor) ? null : reader.GetString(ordAutor),
                        ISBN = reader.IsDBNull(ordIsbn) ? null : reader.GetString(ordIsbn),
                        Editora = reader.IsDBNull(ordEditora) ? null : reader.GetString(ordEditora),
                        AnoPublicacao = reader.IsDBNull(ordAno) ? null : reader.GetInt32(ordAno),
                        Categoria = reader.IsDBNull(ordCategoria) ? null : reader.GetString(ordCategoria),
                        QuantidadeTotal = reader.GetInt32(ordTotal),
                        QuantidadeDisponivel = reader.GetInt32(ordDisp),
                        Localizacao = reader.IsDBNull(ordLoc) ? null : reader.GetString(ordLoc)
                    });
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao listar livros: {ex.Message}", ex);
            }
            return lista;
        }

        public Livro? ObterPorId(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "SELECT * FROM Livro WHERE id_livro=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Livro
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_livro")),
                        Titulo = reader.GetString(reader.GetOrdinal("titulo")),
                        Autor = reader.IsDBNull(reader.GetOrdinal("autor")) ? null : reader.GetString(reader.GetOrdinal("autor")),
                        ISBN = reader.IsDBNull(reader.GetOrdinal("isbn")) ? null : reader.GetString(reader.GetOrdinal("isbn")),
                        Editora = reader.IsDBNull(reader.GetOrdinal("editora")) ? null : reader.GetString(reader.GetOrdinal("editora")),
                        AnoPublicacao = reader.IsDBNull(reader.GetOrdinal("ano_publicacao")) ? null : reader.GetInt32(reader.GetOrdinal("ano_publicacao")),
                        Categoria = reader.IsDBNull(reader.GetOrdinal("categoria")) ? null : reader.GetString(reader.GetOrdinal("categoria")),
                        QuantidadeTotal = reader.GetInt32(reader.GetOrdinal("quantidade_total")),
                        QuantidadeDisponivel = reader.GetInt32(reader.GetOrdinal("quantidade_disponivel")),
                        Localizacao = reader.IsDBNull(reader.GetOrdinal("localizacao")) ? null : reader.GetString(reader.GetOrdinal("localizacao"))
                    };
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao obter livro por ID: {ex.Message}", ex);
            }
            return null;
        }

        public void Atualizar(Livro livro)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "UPDATE Livro SET titulo=@titulo, autor=@autor, isbn=@isbn, editora=@editora, ano_publicacao=@ano, categoria=@categoria, " +
                             "quantidade_total=@total, quantidade_disponivel=@disp, localizacao=@loc WHERE id_livro=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@titulo", livro.Titulo);
                cmd.Parameters.AddWithValue("@autor", (object?)livro.Autor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isbn", (object?)livro.ISBN ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@editora", (object?)livro.Editora ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ano", (object?)livro.AnoPublicacao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@categoria", (object?)livro.Categoria ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@total", livro.QuantidadeTotal);
                cmd.Parameters.AddWithValue("@disp", livro.QuantidadeDisponivel);
                cmd.Parameters.AddWithValue("@loc", (object?)livro.Localizacao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", livro.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao atualizar livro: {ex.Message}", ex);
            }
        }

        public void Excluir(int id)
        {
            try
            {
                using var conn = Conexao.GetConnection();
                string sql = "DELETE FROM Livro WHERE id_livro=@id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao excluir livro: {ex.Message}", ex);
            }
        }
    }
}
