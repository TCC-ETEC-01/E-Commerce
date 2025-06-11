using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;

namespace ProjetoEcommerce.Repositorios
{
    public class PassagemComViagemRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public IEnumerable<tbPassagemComViagem> PassagemComViagem()
        {
            var lista = new List<tbPassagemComViagem>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                string query = @"select  p.IdPassagem,v.Origem, v.Destino, p.Assento,v.Descricao, v.DataPartida as Partida,  
                        v.DataRetorno as Retorno, t.TipoTransporte as Transporte, t.Companhia, 
                        t.CodigoTransporte as Cod_Transporte
                        from tbPassagem p
                        inner join tbViagem v on p.IdViagem = v.IdViagem
                        inner join tbTransporte t on p.IdTransporte = t.IdTransporte;";
                using (MySqlCommand join = new MySqlCommand(query, conexao))
                {
                    using (MySqlDataReader drPassagemComViagem = join.ExecuteReader())
                    {
                        while (drPassagemComViagem.Read())
                        {
                            lista.Add(new tbPassagemComViagem
                            {
                                IdPassagem = drPassagemComViagem.GetInt32("Codigo"),
                                Origem = drPassagemComViagem.GetString("Origem"),
                                Destino = drPassagemComViagem.GetString("Assento"),
                                Descricao = drPassagemComViagem.GetString("Descricao"),
                                DataPartida = drPassagemComViagem.GetDateTime("Partida"),
                                DataRetorno = drPassagemComViagem.GetDateTime("Retorno"),
                                TipoTransporte = drPassagemComViagem.GetString("Transporte")
                            });
                        }
                    }
                    return lista;
                }
            }
        }
    }
}
