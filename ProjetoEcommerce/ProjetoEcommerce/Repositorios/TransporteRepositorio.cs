﻿using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;

namespace ProjetoEcommerce.Repositorios
{
    public class TransporteRepositorio
    {
        private readonly string _conexaoMySQL;

        public TransporteRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        public async Task<bool> CadastrarTransporte(tbTransporte transporte)
        {
            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            var cmd = new MySqlCommand("insert into tbTransporte (CodigoTransporte, Companhia, TipoTransporte) VALUES (@codigo, @companhia, @tipo)", conexao);
            cmd.Parameters.AddWithValue("@codigo", transporte.CodigoTransporte);
            cmd.Parameters.AddWithValue("@companhia", transporte.Companhia);
            cmd.Parameters.AddWithValue("@tipo", transporte.TipoTransporte);

            int linhasAfetadas = await cmd.ExecuteNonQueryAsync();
            return linhasAfetadas > 0;
        }

        public async Task<bool> AtualizarTransporte(tbTransporte transporte)
        {
            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            var cmd = new MySqlCommand("update tbTransporte set CodigoTransporte=@codigo, Companhia=@companhia, TipoTransporte=@tipo WHERE IdTransporte=@id", conexao);
            cmd.Parameters.AddWithValue("@codigo", transporte.CodigoTransporte);
            cmd.Parameters.AddWithValue("@companhia", transporte.Companhia);
            cmd.Parameters.AddWithValue("@tipo", transporte.TipoTransporte);
            cmd.Parameters.AddWithValue("@id", transporte.IdTransporte);

            int linhasAfetadas = await cmd.ExecuteNonQueryAsync();
            return linhasAfetadas > 0;
        }

        public async Task<bool> ExcluirTransporte(int id)
        {
            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            var cmd = new MySqlCommand("delete from tbTransporte WHERE IdTransporte=@id", conexao);
            cmd.Parameters.AddWithValue("@id", id);

            int linhasAfetadas = await cmd.ExecuteNonQueryAsync();
            return linhasAfetadas > 0;
        }

        public async Task<tbTransporte> ObterTransporte(int id)
        {
            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            var cmd = new MySqlCommand("select * from tbTransporte WHERE IdTransporte=@id", conexao);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new tbTransporte
                {
                    IdTransporte = reader.GetInt32("IdTransporte"),
                    CodigoTransporte = reader.GetString("CodigoTransporte"),
                    Companhia = reader.GetString("Companhia"),
                    TipoTransporte = reader.GetString("TipoTransporte")
                };
            }

            return null;
        }

        public async Task<List<tbTransporte>> ListarTransportes()
        {
            var lista = new List<tbTransporte>();

            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            var cmd = new MySqlCommand("select * from tbTransporte", conexao);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new tbTransporte
                {
                    IdTransporte = reader.GetInt32("IdTransporte"),
                    CodigoTransporte = reader.GetString("CodigoTransporte"),
                    Companhia = reader.GetString("Companhia"),
                    TipoTransporte = reader.GetString("TipoTransporte")
                });
            }

            return lista;
        }
    }
}
