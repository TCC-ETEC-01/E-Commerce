using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using ProjetoEcommerce.Models;

namespace ProjetoEcommerce.Repositorios
{
    public class PacoteRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public bool CadastrarPacote(tbPacote pacote)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmdBuscarPassagem = new MySqlCommand("select 1 from tbPassagem where IdPassagem=@idPassagem", conexao);
                cmdBuscarPassagem.Parameters.AddWithValue("@idPassagem", pacote.IdPassagem);

                using (var drBuscarPassagem = cmdBuscarPassagem.ExecuteReader())
                {
                    if (!drBuscarPassagem.HasRows)
                    {
                        Console.WriteLine("Passagem não encontrada!");
                        return false;
                    }
                }
                MySqlCommand cmdBuscarProduto = new MySqlCommand("select 1 from tbProduto where IdProduto=@idProduto", conexao);
                cmdBuscarProduto.Parameters.AddWithValue("@idProduto", pacote.IdProduto);

                using (var drBuscarProduto = cmdBuscarProduto.ExecuteReader())
                {
                    if (!drBuscarProduto.HasRows)
                    {
                        Console.WriteLine("Produto não encontrado!");
                        return false;
                    }
                }

                MySqlCommand cmdInsertPacote = new MySqlCommand("insert into tbPacote IdPassagem, IdProduto, NomePacote, Descricao, Valor) values(@idPassagem, @idProduto, @nomePacote, @descricao)", conexao);
                cmdInsertPacote.Parameters.AddWithValue("@idPassagem", pacote.IdPassagem);
                cmdInsertPacote.Parameters.AddWithValue("@idProduto", pacote.IdProduto);
                cmdInsertPacote.Parameters.AddWithValue("@nomePacote", pacote.NomePacote);
                cmdInsertPacote.Parameters.AddWithValue("@descricao", pacote.Descricao);
                cmdInsertPacote.Parameters.AddWithValue("@valor", pacote.Valor);
                cmdInsertPacote.ExecuteNonQuery();
                return true;
            }
        }

        public bool AtualizarPacote(tbPacote pacote)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                MySqlCommand cmdAtualizarPacote = new MySqlCommand("update tbPacote set NomePacote=@nomePacote, Descricao=@descricao, Valor=@valor" + "where IdPacote=@idPacote");
                cmdAtualizarPacote.Parameters.AddWithValue("@nomePacote", pacote.NomePacote);
                cmdAtualizarPacote.Parameters.AddWithValue("@descricao", pacote.Descricao);
                cmdAtualizarPacote.Parameters.AddWithValue("@valor", pacote.Valor);
                int linhasAfetadas = cmdAtualizarPacote.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }
    }
}
