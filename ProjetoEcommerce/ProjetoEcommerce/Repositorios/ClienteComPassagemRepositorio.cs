using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Configuration;

namespace ProjetoEcommerce.Repositorios
{
    public class ClienteComPassagemRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public async Task<IEnumerable<tbPassagemComViagem>> PassagemComViagem()
        {
            var lista = new List<tbPassagemComViagem>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                string query = @"select c.IdCliente, c.Nome as Nome, c.Email,
	                             p.IdPassagem, p.Assento, p.Valor, p.Situacao
	                             from tbPassagem p
	                             inner join tbCliente c on p.IdCliente = c.IdCliente;";

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
                    }
                }
            }
            return lista;
        }
    }
}
