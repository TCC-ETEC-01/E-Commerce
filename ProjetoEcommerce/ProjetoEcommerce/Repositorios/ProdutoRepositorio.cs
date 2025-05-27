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
                MySqlCommand cmd = new MySqlCommand("insert into tbProduto NomeProduto")
            }
        }
    }
}
