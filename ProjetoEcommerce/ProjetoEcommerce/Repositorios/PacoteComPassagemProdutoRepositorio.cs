using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;

namespace ProjetoEcommerce.Repositorios
{
    public class PacoteComPassagemProdutoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public async Task<IEnumerable<tbPacoteComPassagemProduto>> PacoteComPassagemProduto()
        {
            var lista = new List<tbPacoteComPassagemProduto>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                string query = @"select pac.IdPacote, pac.NomePacote, pac.Valor, prod.NomeProduto,
                         p.Assento as Assento, p.Situacao as Situacao, p.Translado, 
                         t.TipoTransporte as Transporte, t.Companhia, t.CodigoTransporte as Cod_Transporte
                         from tbPacote pac
                         inner join tbProduto prod on pac.IdProduto = prod.IdProduto
                         inner join tbPassagem p on pac.IdPassagem = p.IdPassagem
                         inner join tbTransporte t on p.IdTransporte = t.IdTransporte;";

                using (var join = new MySqlCommand(query, conexao))
                {
                    using (var drPacote = await join.ExecuteReaderAsync())
                    {
                        while (await drPacote.ReadAsync())
                        {
                            lista.Add(new tbPacoteComPassagemProduto
                            {
                                IdPacote = drPacote.GetInt32("IdPacote"),
                                NomePacote = drPacote.GetString("NomePacote"),
                                NomeProduto = drPacote.GetString("NomeProduto"),
                                Assento = drPacote.GetString("Assento"),
                                Situacao = drPacote.GetString("Situacao"),
                                Translado = drPacote.GetString("Translado"),
                                Companhia = drPacote.GetString("Companhia"),
                                CodigoTransporte = drPacote.GetString("Cod_Transporte"),
                                TipoTransporte = drPacote.GetString("Transporte"),
                                Valor = drPacote.GetDecimal("Valor")
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public async Task<List<tbPacoteComPassagemProduto>> BuscarPacote(string nomePacote)
        {
            var lista = new List<tbPacoteComPassagemProduto>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                string query = @"SELECT pac.IdPacote, pac.NomePacote, pac.Valor, prod.NomeProduto,
                                p.Assento AS Assento, p.Situacao AS Situacao, p.Translado, 
                                t.TipoTransporte AS Transporte, t.Companhia, t.CodigoTransporte AS Cod_Transporte
                         FROM tbPacote pac
                         INNER JOIN tbProduto prod ON pac.IdProduto = prod.IdProduto
                         INNER JOIN tbPassagem p ON pac.IdPassagem = p.IdPassagem
                         INNER JOIN tbTransporte t ON p.IdTransporte = t.IdTransporte
                         WHERE pac.NomePacote LIKE @nomePacote";

                using (var cmdPesquisar = new MySqlCommand(query, conexao))
                {
                    string termoBusca = string.IsNullOrEmpty(nomePacote) ? "%" : $"%{nomePacote}%";
                    cmdPesquisar.Parameters.AddWithValue("@nomePacote", termoBusca);

                    using (var drPacote = await cmdPesquisar.ExecuteReaderAsync())
                    {
                        while (await drPacote.ReadAsync())
                        {
                            lista.Add(new tbPacoteComPassagemProduto
                            {
                                IdPacote = drPacote.GetInt32("IdPacote"),
                                NomePacote = drPacote.GetString("NomePacote"),
                                NomeProduto = drPacote.GetString("NomeProduto"),
                                Assento = drPacote.GetString("Assento"),
                                Situacao = drPacote.GetString("Situacao"),
                                Translado = drPacote.GetString("Translado"),
                                Companhia = drPacote.GetString("Companhia"),
                                CodigoTransporte = drPacote.GetString("Cod_Transporte"),
                                TipoTransporte = drPacote.GetString("Transporte"),
                                Valor = drPacote.GetDecimal("Valor")
                            });
                        }
                    }
                    return lista;
                }
            }
        }

    }
}
