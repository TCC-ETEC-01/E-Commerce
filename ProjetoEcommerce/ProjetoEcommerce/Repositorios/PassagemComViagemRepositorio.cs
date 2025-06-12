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

                string query = @"select c.IdCliente, c.Nome as Nome, c.Email, c.Cpf, c.Telefone,
	                            p.IdPassagem, p.Assento, p.Valor, p.Situacao, p.DataCompra, p.IdViagem, P.Translado
	                            from tbPassagem p
	                            inner join tbCliente c on p.IdCliente = c.IdCliente;";

                using (MySqlCommand join = new MySqlCommand(query, conexao))
                {
                    using (var drClientePassagem = await join.ExecuteReaderAsync())
                    {
                        while (await drClientePassagem.ReadAsync())
                        {
                            lista.Add(new tbClienteComPassagem
                            {
                                IdPassagem = drClientePassagem.GetInt32("IdPassagem"),
                                Assento = drClientePassagem.GetString("Assento"),
                                Valor = drClientePassagem.GetDecimal("Valor"),
                                DataCompra = drClientePassagem.GetDateTime("DataCompra"),
                                IdViagem = drClientePassagem.GetInt32("IdViagem"),
                                Nome = drClientePassagem.GetString("Nome"),
                                Email = drClientePassagem.GetString("Email"),
                                Cpf = drClientePassagem.GetString("Cpf"),
                                Telefone = drClientePassagem.GetString("Telefone"),
                                Situacao = drClientePassagem.GetString("Situacao"),
                                Translado = drClientePassagem.GetString("Translado")

                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
