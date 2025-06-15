using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;

namespace ProjetoEcommerce.Repositorios
{
    public class ProdutoRepositorio
    {
        private readonly string _conexaoMySQL;

        public ProdutoRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        public async Task<bool> CadastrarProduto(tbProduto produto)
        {
            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            MySqlCommand cmd = new MySqlCommand(
                "insert into tbProduto (NomeProduto, Valor, Descricao, Quantidade) values(@nomeProduto,@valor, @descricao, @quantidade)",
                conexao);
            cmd.Parameters.AddWithValue("@nomeProduto", produto.NomeProduto);
            cmd.Parameters.AddWithValue("@valor", produto.Valor);
            cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
            cmd.Parameters.AddWithValue("@quantidade", produto.Quantidade);

            int linhasAfetadas = await cmd.ExecuteNonQueryAsync();

            return linhasAfetadas > 0;
        }

        public async Task<bool> AtualizarProduto(tbProduto produto)
        {
            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            MySqlCommand cmd = new MySqlCommand(
                "UPDATE tbProduto SET NomeProduto=@nomeProduto, Valor=@valor, Descricao=@descricao, Quantidade=@quantidade " +
                "WHERE IdProduto=@idProduto", conexao);

            cmd.Parameters.AddWithValue("@nomeProduto", produto.NomeProduto);
            cmd.Parameters.AddWithValue("@valor", produto.Valor);
            cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
            cmd.Parameters.AddWithValue("@quantidade", produto.Quantidade);
            cmd.Parameters.AddWithValue("@idProduto", produto.IdProduto);

            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<IEnumerable<tbProduto>> TodosProdutos()
        {
            var ProdutoLista = new List<tbProduto>();

            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("select * from tbProduto", conexao);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                ProdutoLista.Add(new tbProduto
                {
                    IdProduto = reader.GetInt32("IdProduto"),
                    NomeProduto = reader.GetString("NomeProduto"),
                    Valor = reader.GetDecimal("Valor"),
                    Descricao = reader.GetString("Descricao"),
                    Quantidade = reader.GetInt32("Quantidade")
                });
            }

            return ProdutoLista;
        }

        public async Task ExcluirProduto(int id)
        {
            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            MySqlCommand cmdBuscarId = new MySqlCommand("select 1 from tbPacote where IdProduto=@idProduto", conexao);
            cmdBuscarId.Parameters.AddWithValue("@idProduto", id);

            await using var drProduto = await cmdBuscarId.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            if (await drProduto.ReadAsync())
            {
                await conexao.CloseAsync();

                await using var conexao2 = new MySqlConnection(_conexaoMySQL);
                await conexao2.OpenAsync();

                MySqlCommand cmdExcluirProdutoPacote = new MySqlCommand("delete from tbPacote where IdProduto=@idProduto", conexao2);
                cmdExcluirProdutoPacote.Parameters.AddWithValue("@idProduto", id);
                await cmdExcluirProdutoPacote.ExecuteNonQueryAsync();

                MySqlCommand cmdExcluirProduto = new MySqlCommand("delete from tbProduto where IdProduto=@idProduto", conexao2);
                cmdExcluirProduto.Parameters.AddWithValue("@idProduto", id);
                await cmdExcluirProduto.ExecuteNonQueryAsync();
            }
            else
            {
                await conexao.CloseAsync();

                await using var conexao3 = new MySqlConnection(_conexaoMySQL);
                await conexao3.OpenAsync();

                MySqlCommand cmdExcluirProduto = new MySqlCommand("delete from tbProduto where IdProduto=@idProduto", conexao3);
                cmdExcluirProduto.Parameters.AddWithValue("@idProduto", id);
                await cmdExcluirProduto.ExecuteNonQueryAsync();
            }
        }

        public async Task<tbProduto> ObterProduto(int Codigo)
        {
            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("select * from tbProduto where IdProduto=@codigo", conexao);
            cmd.Parameters.AddWithValue("@codigo", Codigo);

            await using var dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            var produto = new tbProduto();

            if (await dr.ReadAsync())
            {
                produto.IdProduto = dr.GetInt32("IdProduto");
                produto.NomeProduto = dr.GetString("NomeProduto");
                produto.Valor = dr.GetDecimal("Valor");
                produto.Descricao = dr.GetString("Descricao");
                produto.Quantidade = dr.GetInt32("Quantidade");
            }

            return produto;
        }
        public async Task<List<tbProduto>> BuscarProduto(string nomeProduto)
        {
            var ProdutoLista = new List<tbProduto>();

            await using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            string query = "select * from tbProduto where NomeProduto like @nomeProduto";

            await using var cmd = new MySqlCommand(query, conexao);

            string termoBusca = string.IsNullOrEmpty(nomeProduto) ? "%" : $"%{nomeProduto}%";
            cmd.Parameters.AddWithValue("@nomeProduto", termoBusca);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                ProdutoLista.Add(new tbProduto
                {
                    IdProduto = reader.GetInt32("IdProduto"),
                    NomeProduto = reader.GetString("NomeProduto"),
                    Valor = reader.GetDecimal("Valor"),
                    Descricao = reader.GetString("Descricao"),
                    Quantidade = reader.GetInt32("Quantidade")
                });
            }

            return ProdutoLista;
        }
    }
}
