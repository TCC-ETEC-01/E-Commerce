using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Repositorios
{
    public class PacoteRepositorio
    {
        private readonly string _conexaoMySQL;

        public PacoteRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        public async Task<bool> CadastrarPacote(tbPacote pacote)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand cmdBuscarPassagem = new MySqlCommand("select 1 from tbPassagem where IdPassagem=@idPassagem", conexao);
                cmdBuscarPassagem.Parameters.AddWithValue("@idPassagem", pacote.IdPassagem);

                using (var drBuscarPassagem = await cmdBuscarPassagem.ExecuteReaderAsync())
                {
                    if (!drBuscarPassagem.HasRows)
                    {
                        Console.WriteLine("Passagem não encontrada!");
                        return false;
                    }
                }

                MySqlCommand cmdBuscarProduto = new MySqlCommand("select 1 from tbProduto where IdProduto=@idProduto", conexao);
                cmdBuscarProduto.Parameters.AddWithValue("@idProduto", pacote.IdProduto);

                using (var drBuscarProduto = await cmdBuscarProduto.ExecuteReaderAsync())
                {
                    if (!drBuscarProduto.HasRows)
                    {
                        Console.WriteLine("Produto não encontrado!");
                        return false;
                    }
                }

                MySqlCommand cmdInsertPacote = new MySqlCommand("insert into tbPacote (IdPassagem, IdProduto, NomePacote, Descricao, Valor) values(@idPassagem, @idProduto, @nomePacote, @descricao, @valor)", conexao);
                cmdInsertPacote.Parameters.AddWithValue("@idPassagem", pacote.IdPassagem);
                cmdInsertPacote.Parameters.AddWithValue("@idProduto", pacote.IdProduto);
                cmdInsertPacote.Parameters.AddWithValue("@nomePacote", pacote.NomePacote);
                cmdInsertPacote.Parameters.AddWithValue("@descricao", pacote.Descricao);
                cmdInsertPacote.Parameters.AddWithValue("@valor", pacote.Valor);

                await cmdInsertPacote.ExecuteNonQueryAsync();

                return true;
            }
        }

        public async Task<bool> AtualizarPacote(tbPacote pacote)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand cmdAtualizarPacote = new MySqlCommand("update tbPacote set NomePacote=@nomePacote, Descricao=@descricao, Valor=@valor, IdPassagem=@idPassagem, IdProduto=@idProduto where IdPacote=@idPacote", conexao);
                cmdAtualizarPacote.Parameters.AddWithValue("@nomePacote", pacote.NomePacote);
                cmdAtualizarPacote.Parameters.AddWithValue("@descricao", pacote.Descricao);
                cmdAtualizarPacote.Parameters.AddWithValue("@valor", pacote.Valor);
                cmdAtualizarPacote.Parameters.AddWithValue("@idPassagem", pacote.IdPassagem);
                cmdAtualizarPacote.Parameters.AddWithValue("@idProduto", pacote.IdProduto);
                cmdAtualizarPacote.Parameters.AddWithValue("@idPacote", pacote.IdPacote);

                int linhasAfetadas = await cmdAtualizarPacote.ExecuteNonQueryAsync();

                return linhasAfetadas > 0;
            }
        }

        public async Task ExcluirPacote(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand cmd = new MySqlCommand("delete from tbPacote where IdPacote=@idPacote", conexao);
                cmd.Parameters.AddWithValue("@idPacote", Id);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<tbPacote> ObterPacote(int Codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand cmd = new MySqlCommand("select * from tbPacote where IdPacote=@codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", Codigo);

                tbPacote pacote = new tbPacote();

                using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (await dr.ReadAsync())
                    {
                        pacote.IdPacote = Convert.ToInt32(dr["IdPacote"]);
                        pacote.NomePacote = (string)dr["NomePacote"];
                        pacote.Descricao = (string)dr["Descricao"];
                        pacote.Valor = (decimal)dr["Valor"];
                        pacote.IdPassagem = new tbPassagem
                        {
                            IdPassagem = Convert.ToInt32(dr["IdPassagem"])
                        };
                        pacote.IdProduto = new tbProduto
                        {
                            IdProduto = Convert.ToInt32(dr["IdProduto"])
                        };
                    }
                }
                return pacote;
            }
        }
    }
}
