using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Security.Cryptography;

namespace ProjetoEcommerce.Repositorios
{
    public class PassagemRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public bool CadastrarPassagem(tbPassagem passagem)
        {

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmdBuscarId = new MySqlCommand("select * from tbViagem where IdViagem=@idViagem", conexao);
                cmdBuscarId.Parameters.AddWithValue("@idViagem", passagem.IdViagem);

                using (var drViagem = cmdBuscarId.ExecuteReader())
                {
                    if (!drViagem.HasRows)
                    {
                        Console.WriteLine("Id da viagem não existente");
                        return false;
                    }
                
                    else if (drViagem.HasRows)
                    {
                        MySqlCommand cmd = new MySqlCommand("select * from tbPassagem where Assento=@assento", conexao);
                        cmd.Parameters.AddWithValue("@assento", passagem.Assento);
                        using (var drAssento = cmd.ExecuteReader())
                        {
                            if (drAssento.HasRows)
                            {
                                Console.WriteLine("Assento já existente");
                                return false;
                            }
                            else
                            {
                                MySqlCommand cmdInsert = new MySqlCommand("insert into tbPassagem(Assento, Valor, Situacao, IdViagem) values (@assento,@valor,@situacao,@idViagem)", conexao);
                                cmdInsert.Parameters.Add("@assento", MySqlDbType.VarChar).Value = passagem.Assento;
                                cmdInsert.Parameters.Add("@valor", MySqlDbType.Decimal).Value = passagem.Valor;
                                cmdInsert.Parameters.Add("@situacao", MySqlDbType.VarChar).Value = passagem.Situacao;
                                cmdInsert.Parameters.Add("@idVIagem", MySqlDbType.Int32).Value = passagem.IdViagem;
                                cmdInsert.ExecuteNonQuery();
                            }
                        }
                    }
                    return true;
                }
                conexao.Close();
            }

        }

        public bool AtualizarPassagem(tbPassagem passagem)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update tbPassagem set Assento=@assento, Valor=@valor,Situacao=@situacao,IdViagem=@idViagem" + "where IdPassagem=@passagem", conexao);
                cmd.Parameters.AddWithValue("@assento", passagem.Assento);
                cmd.Parameters.AddWithValue("@situacao", passagem.Situacao);
                cmd.Parameters.AddWithValue("@valor", passagem.Valor);
                cmd.Parameters.AddWithValue("@idViagem", passagem.IdViagem);
                int linhasAfetadas = cmd.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }
            
            }
}

