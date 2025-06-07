using ProjetoEcommerce.Models;
using MySql.Data.MySqlClient;

namespace ProjetoEcommerce.Repositorios
{
    public class PacoteComPassagemProdutoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public IEnumerable<tbPacoteComPassagemProduto> PacoteComPassagemProduto()
        {
            var lista = new List<tbPacoteComPassagemProduto>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                string query = @"select pac.IdPacote as Codigo, pac.NomePacote as Pacote, prod.NomeProduto as Produto,
                                p.Assento,  p.Situacao, p.Translado                                  
                                from tbPacote pac
                                inner join tbProduto prod on pac.IdProduto = prod.IdProduto
                                inner join tbPassagem p on pac.IdPassagem = p.IdPassagem;";
                using (MySqlCommand join = new MySqlCommand(query, conexao)) 
                {
                    using (MySqlDataReader drPacote = join.ExecuteReader())
                    {
                        while (drPacote.Read())
                        {
                            lista.Add(new tbPacoteComPassagemProduto
                            {
                                IdPacote = drPacote.GetInt32("Codigo"),
                                NomePacote = drPacote.GetString("Pacote"),


                            });
                        }
                    }
                   
                }
        
            }
        }
    }
}
