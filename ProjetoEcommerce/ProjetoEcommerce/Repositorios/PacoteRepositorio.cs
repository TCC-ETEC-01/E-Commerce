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
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmdBuscarPassagem = new MySqlCommand("select 1 from tbPassagem where IdPassagem=@idPassagem", conexao);
                cmdBuscarPassagem.Parameters.AddWithValue("@idPassagem", pacote.IdPassagem);

                using(var drBuscarPassagem = cmdBuscarPassagem.ExecuteReader())
                {
                    if (!drBuscarPassagem.HasRows)
                    {
                        Console.WriteLine("Passagem não encontrada!");
                        return false;
                    }
                }
                MySqlCommand cmdBuscarProduto = new MySqlCommand("select 1 from tbProduto where IdProduto=@idProduto", conexao);
                cmdBuscarProduto.Parameters.AddWithValue("@idProduto", pacote.IdProduto);
         
                using(var drBuscarProduto = cmdBuscarProduto.ExecuteReader())
                {
                    if(!drBuscarProduto.HasRows)
                    {
                        Console.WriteLine("Produto não encontrado!");
                        return false;
                    }
                }

                MySqlCommand cmdInsertPassagemPacote = new MySqlCommand("insert into tbPacote IdPassagem, IdProduto, NomePacote, Descricao, Valor) values(@idPassagem, @idProduto, @nomePacote, @descricao)", conexao);
                cmdInsertPassagemPacote.Parameters.AddWithValue("@idPassagem", pacote.IdPassagem);
                cmdInsertPassagemPacote.Parameters.AddWithValue("@idProduto", pacote.IdProduto);
                cmdInsertPassagemPacote.Parameters.AddWithValue("@nomePacote", pacote.NomePacote);
                cmdInsertPassagemPacote.Parameters.AddWithValue("@descricao", pacote.Descricao);
                cmdInsertPassagemPacote.Parameters.AddWithValue("@valor", pacote.Valor);
                cmdInsertPassagemPacote.ExecuteNonQuery();
                return true;
            }
        }
    }
}
