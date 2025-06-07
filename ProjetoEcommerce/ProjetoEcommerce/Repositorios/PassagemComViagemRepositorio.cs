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
                string query = @"select  p.IdPassagem,v.Origem, v.Destino, p.Assento,p.Translado, v.Descricao,  
                    v.DataPartida as Partida, v.DataRetorno as Retorno,  v.TipoTransporte as Transporte
                    from tbPassagem p 
                    inner join tbViagem v on p.IdViagem = v.IdViagem;";
                using(MySqlCommand join = new MySqlCommand(query, conexao))
                {
                    using (MySqlDataReader drPassagemComViagem =  join.ExecuteReader())
                    {
                        while(drPassagemComViagem.Read())
                        {
                            lista.Add(new tbPassagemComViagem
                            {
                                IdPassagem = drPassagemComViagem.GetInt32("IdPassagem"),
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
