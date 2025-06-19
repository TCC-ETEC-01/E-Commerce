using MySql.Data.MySqlClient;
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

            MySqlCommand cmdBuscarTransporte = new MySqlCommand("select 1 from tbTransporte " +
            "where CodigoTransporte=@codigo", conexao);
            cmdBuscarTransporte.Parameters.AddWithValue("@codigo", transporte.CodigoTransporte);
            using (var drTransporte = await cmdBuscarTransporte.ExecuteReaderAsync())
            {
                if (drTransporte.HasRows)
                {
                    Console.WriteLine("Transporte já existente");
                    return false;
                }
            }
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

            var cmd = new MySqlCommand("update tbTransporte set CodigoTransporte=@codigo, Companhia=@companhia, TipoTransporte=@tipo where IdTransporte=@id", conexao);
            cmd.Parameters.AddWithValue("@codigo", transporte.CodigoTransporte);
            cmd.Parameters.AddWithValue("@companhia", transporte.Companhia);
            cmd.Parameters.AddWithValue("@tipo", transporte.TipoTransporte);
            cmd.Parameters.AddWithValue("@id", transporte.IdTransporte);

            int linhasAfetadas = await cmd.ExecuteNonQueryAsync();
            return linhasAfetadas > 0;
        }

        public async Task<bool> ExcluirTransporte(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();
                MySqlCommand cmdVerificarVenda = new MySqlCommand("select 1 from tbVenda where IdPassagem=@idPassagem", conexao);
                cmdVerificarVenda.Parameters.AddWithValue("idPassagem", id);
                using (var drVenda = await cmdVerificarVenda.ExecuteReaderAsync())
                {
                    if (await drVenda.ReadAsync())
                    {
                        drVenda.Close();
                        Console.WriteLine("Venda realizada");
                        return false;

                    }
                }
                MySqlCommand cmdBuscarId = new MySqlCommand("select 1 from tbPassagem where IdTransporte=@idTransporte", conexao);
                cmdBuscarId.Parameters.AddWithValue("@idTransporte", id);

                using (var drTransportePassagem = await cmdBuscarId.ExecuteReaderAsync())
                {
                    if (await drTransportePassagem.ReadAsync())
                    {
                        drTransportePassagem.Close();
                        MySqlCommand cmdExcluirPassagens = new MySqlCommand("delete from tbPassagem where IdTransporte=@idTransporte", conexao);
                        cmdExcluirPassagens.Parameters.AddWithValue("@idTransporte", id);
                        await cmdExcluirPassagens.ExecuteNonQueryAsync();
                        return true;
                    }
                }
                using (var drTransporte = await cmdBuscarId.ExecuteReaderAsync())
                {
                    if (!await drTransporte.ReadAsync())
                    {
                        drTransporte.Close();
                        MySqlCommand cmdExcluirTransporte = new MySqlCommand("delete from tbTransporte where IdTransporte=@idTransporte", conexao);
                        cmdExcluirTransporte.Parameters.AddWithValue("@idTransporte", id);
                        await cmdExcluirTransporte.ExecuteNonQueryAsync();

                    }
                    return true;
                }     
            }
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
        public async Task<List<tbTransporte>> BuscarTransporte(string termo)
        {
            var lista = new List<tbTransporte>();

            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            string query = @"select * from tbTransporte 
                             where Companhia like @termo or CodigoTransporte like @termo";

            await using var cmd = new MySqlCommand(query, conexao);

            string busca = string.IsNullOrEmpty(termo) ? "%" : $"%{termo}%";
            cmd.Parameters.AddWithValue("@termo", busca);

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