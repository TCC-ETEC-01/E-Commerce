using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;

namespace ProjetoEcommerce.Repositorios
{
    public class PassagemComViagemRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public async Task<IEnumerable<tbPassagemComViagem>> PassagemComViagem()
        {
            var lista = new List<tbPassagemComViagem>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                string query = @"select  p.IdPassagem, p.Valor, v.Origem, v.Destino, p.Assento,v.Descricao, v.DataPartida as Partida, v.DataRetorno as Retorno, t.TipoTransporte as Transporte, t.Companhia, t.CodigoTransporte as Cod_Transporte
	                            from tbPassagem p
	                            inner join tbViagem v on p.IdViagem = v.IdViagem
	                            inner join tbTransporte t on p.IdTransporte = t.IdTransporte;";

                using (MySqlCommand join = new MySqlCommand(query, conexao))
                {
                    using (var drPassagemComViagem = await join.ExecuteReaderAsync())
                    {
                        while (await drPassagemComViagem.ReadAsync())
                        {
                            lista.Add(new tbPassagemComViagem
                            {
                                IdPassagem = drPassagemComViagem.GetInt32("IdPassagem"),
                                Origem = drPassagemComViagem.GetString("Origem"),
                                Destino = drPassagemComViagem.GetString("Destino"),
                                Descricao = drPassagemComViagem.GetString("Descricao"),
                                DataPartida = drPassagemComViagem.GetDateTime("Partida"),
                                DataRetorno = drPassagemComViagem.GetDateTime("Retorno"),
                                TipoTransporte = drPassagemComViagem.GetString("Transporte"),
                                CodigoTransporte = drPassagemComViagem.GetString("Cod_Transporte"),
                                Companhia = drPassagemComViagem.GetString("Companhia"),
                                Valor = drPassagemComViagem.GetDecimal("Valor")
                            });
                        }
                        return lista;
                    }
                }
            }
        }

        public async Task<List<tbPassagemComViagem>> BuscarPassagem(string destino)
        {
            var lista = new List<tbPassagemComViagem>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                string query = @"select  p.IdPassagem, p.Valor, v.Origem, v.Destino, p.Assento,v.Descricao, v.DataPartida as Partida, v.DataRetorno as Retorno, t.TipoTransporte as Transporte, t.Companhia, t.CodigoTransporte as Cod_Transporte
	                            from tbPassagem p
	                            inner join tbViagem v on p.IdViagem = v.IdViagem
	                            inner join tbTransporte t on p.IdTransporte = t.IdTransporte
	                            where v.Destino like @destino";

                using (var cmdPesquisar = new MySqlCommand(query, conexao))
                {
                    string termoBusca = string.IsNullOrEmpty(destino) ? "%" : $"%{destino}%";
                    cmdPesquisar.Parameters.AddWithValue("@destino", termoBusca);

                    using (var dr = await cmdPesquisar.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new tbPassagemComViagem
                            {
                                IdPassagem = dr.GetInt32("IdPassagem"),
                                Origem = dr.GetString("Origem"),
                                Destino = dr.GetString("Destino"),
                                Descricao = dr.GetString("Descricao"),
                                DataPartida = dr.GetDateTime("Partida"),
                                DataRetorno = dr.GetDateTime("Retorno"),
                                TipoTransporte = dr.GetString("Transporte"),
                                CodigoTransporte = dr.GetString("Cod_Transporte"),
                                Companhia = dr.GetString("Companhia"),
                                Valor = dr.GetDecimal("Valor")
                            });
                        }
                    }
                    return lista;
                }
            }
        }
    }
}
