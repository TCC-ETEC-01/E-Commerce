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
                string query = @"select pac.IdPacote, pac.NomePacote,pac.Valor,  prod.NomeProduto,p.Assento as Assento, p.Situacao as Situacao, p.Translado, 
                                t.TipoTransporte as Transporte, t.Companhia, t.CodigoTransporte as Cod_Transporte
                               from tbPacote pac
                                inner join tbProduto prod on pac.IdProduto = prod.IdProduto
                                inner join tbPassagem p on pac.IdPassagem = p.IdPassagem
                                inner join tbTransporte t on p.IdTransporte = t.IdTransporte;";
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
                                NomeProduto = drPacote.GetString("Produto"),
                                Assento = drPacote.GetString("Assento"),
                                Situacao = drPacote.GetString("Situacao"),
                                Translado = drPacote.GetString("Translado")
                            });
                        }
                    }
                    return lista;     
                }
            }
        }
    }
}
