using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;

namespace ProjetoEcommerce.Repositorios
{
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public void CadastrarProduto(tbProduto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("insert into tbProduto (NomeProduto, Valor, Descricao, Quantidade) values(@nomeProduto,@valor, @descricao, @quantidade)", conexao);
                cmd.Parameters.AddWithValue("@nomeProduto", produto.NomeProduto);
                cmd.Parameters.AddWithValue("@valor", produto.Valor);
                cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
                cmd.Parameters.AddWithValue("@quantidade", produto.Quantidade);
            }
        }
    }
}
