using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;
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
            public IEnumerable<tbPassagem> TodasPassagens()
            {
            List<tbPassagem> PassagemLista = new List<tbPassagem>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                conexao.Open();
              MySqlCommand cmd = new MySqlCommand("select * from tbPassagem", conexao);
              MySqlDataAdapter da = new MySqlDataAdapter(cmd);
              DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows) 
                {
                    PassagemLista.Add(new tbPassagem
                    {
                        IdPassagem = Convert.ToInt32(dr["IdPassagem"]),
                        IdViagem = Convert.ToInt32(dr["IdViagem"]),
                        Assento = ((string)dr["Assento"]),
                        Valor = ((decimal)dr["Valor"]),
                        Situacao = ((string)dr["Situacao"])
                    });
                }
                return PassagemLista;
                }
            }
        public void ExcluirPassagem(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmdBuscarId = new MySqlCommand("select 1 from tbPacote where IdPassagem=@idPassagem", conexao);
                cmdBuscarId.Parameters.AddWithValue("@idPassagem", tbPassagem.IdPassagem);
            }
        }
    }
}

