using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;

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
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public bool AtualizarProduto(tbProduto produto)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open(); 
                MySqlCommand cmd = new MySqlCommand("update from tbProduto set NomeProduto=@nomeProduto, Valor=@valor, Descricao=@descricao, Quantidade=@quantidade" + "where IdProduto=@idProduto", conexao);
                cmd.Parameters.AddWithValue("@nomeProduto", produto.NomeProduto);
                cmd.Parameters.AddWithValue("@valor", produto.Valor);
                cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
                cmd.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                int linhasAfetadas = cmd.ExecuteNonQuery();
                return linhasAfetadas > 0;

            }
        }

        public IEnumerable<tbPacote> TodosProdutos()
        {
            List<tbProduto> ProdutoLista = new List<tbProduto>();
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbProduto", conexao);
               MySqlDataAdapter da = new MySqlDataAdapter(cmd);
               DataTable dt = new DataTable();
                da.Fill(dt);

            }
        }
    }
}
