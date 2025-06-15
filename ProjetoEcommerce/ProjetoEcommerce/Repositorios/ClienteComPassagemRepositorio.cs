using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Configuration;
using System.Data;

namespace ProjetoEcommerce.Repositorios
{
    public class ClienteComPassagemRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public async Task<IEnumerable<tbClienteComPassagem>> ClienteComPassagens()
        {
            var lista = new List<tbClienteComPassagem>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                string query = @"select c.IdCliente, c.Nome as Nome, c.Email, c.Cpf, c.Telefone,
	                            p.IdPassagem, p.Assento, p.Valor, p.Situacao, p.DataCompra, p.IdViagem, P.Translado
	                            from tbPassagem p
	                            inner join tbCliente c on p.IdCliente = c.IdCliente;";

                using (MySqlCommand join = new MySqlCommand(query, conexao))
                {
                    using (var drPassagemComViagem = await join.ExecuteReaderAsync())
                    {
                        while (await drPassagemComViagem.ReadAsync())
                        {
                            lista.Add(new tbClienteComPassagem
                            {
                                IdPassagem = drPassagemComViagem.GetInt32("IdPassagem"),
                                Assento = drPassagemComViagem.GetString("Assento"),
                                Valor = drPassagemComViagem.GetDecimal("Valor"),
                                Situacao = drPassagemComViagem.GetString("Situacao"),
                                DataCompra = drPassagemComViagem.GetDateTime("DataCompra"),
                                IdViagem = drPassagemComViagem.GetInt32("IdViagem"),
                                Translado = drPassagemComViagem.GetString("Translado"),
                                Nome = drPassagemComViagem.GetString("Nome"),
                                Email = drPassagemComViagem.GetString("Email"),
                                Cpf = drPassagemComViagem.GetString("Cpf"),
                                Telefone = drPassagemComViagem.GetString("Telefone")

                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
