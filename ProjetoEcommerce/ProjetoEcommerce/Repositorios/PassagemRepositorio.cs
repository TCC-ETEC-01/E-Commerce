using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Security.Cryptography;

namespace ProjetoEcommerce.Repositorios
{
    public class PassagemRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL  = configuration.GetConnectionString("conexaoMySQL")

            public bool CadastrarPassagem(tbPassagem passagem)
        {
            using (var conexao = new MySqlConnection (_conexaoMySQL))
            {
                conexao.Open();

                bool duplicado = false;
                MySqlCommand cmd = new MySqlCommand("select * tbPassagem where Assento=@assento");
                cmd.Parameters.AddWithValue("@assento", passagem.Assento);
                    using (var dr = cmd.ExecuteReader)
                {
                    if (dr.HasRows)
                    {
                        duplicado = true;
                    }
                    if(duplicado)
                    {
                        Console.WriteLine("Assento já existente");
                        return false;
                    }
                    else
                    {
                        MySqlCommand cmd = new MySqlCommand("insert into tbPassagem(Assento=@assento, Valor=@valor, Situacao=@situacao)

                    }
                }
            }
        }
    }
}
