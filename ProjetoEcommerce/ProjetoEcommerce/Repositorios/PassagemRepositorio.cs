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
                MySqlCommand cmdBuscarViagem = new MySqlCommand("select 1 from tbViagem where IdViagem=@idViagem", conexao);
                cmdBuscarViagem.Parameters.AddWithValue("@idViagem", passagem.IdViagem);

                using (var drViagem = cmdBuscarViagem.ExecuteReader())
                {
                    if (!drViagem.HasRows)
                    {
                        Console.WriteLine("Viagem não existente");
                        return false;
                    }
                }
                MySqlCommand cmdBuscarAssento = new MySqlCommand("select 1 from tbPassagem where Assento=@assento", conexao);
                cmdBuscarAssento.Parameters.AddWithValue("@assento", passagem.Assento);
                using (var drAssento = cmdBuscarAssento.ExecuteReader())
                {
                    if (drAssento.HasRows)
                    {
                        Console.WriteLine("Assento já existente");
                        return false;
                    }

                }
                MySqlCommand cmdInsert = new MySqlCommand("insert into tbPassagem(Assento, Valor, Situacao, IdViagem,Translado) values (@assento,@valor,@situacao,@idViagem, @translado)", conexao);
                cmdInsert.Parameters.Add("@assento", MySqlDbType.VarChar).Value = passagem.Assento;
                cmdInsert.Parameters.Add("@valor", MySqlDbType.Decimal).Value = passagem.Valor;
                cmdInsert.Parameters.Add("@situacao", MySqlDbType.VarChar).Value = passagem.Situacao;
                cmdInsert.Parameters.Add("@idViagem", MySqlDbType.Int32).Value = passagem.IdViagem;
                cmdInsert.Parameters.Add("@translado", MySqlDbType.VarChar).Value = passagem.Translado;
                cmdInsert.ExecuteNonQuery();
                return true;
            } 
        }               
      
     
        public bool AtualizarPassagem(tbPassagem passagem)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update tbPassagem set Assento=@assento, Valor=@valor,Situacao=@situacao,IdViagem=@idViagem, Translado=@translado" + "where IdPassagem=@passagem", conexao);
                cmd.Parameters.AddWithValue("@assento", passagem.Assento);
                cmd.Parameters.AddWithValue("@situacao", passagem.Situacao);
                cmd.Parameters.AddWithValue("@valor", passagem.Valor);
                cmd.Parameters.AddWithValue("@idViagem", passagem.IdViagem);
                cmd.Parameters.AddWithValue("@translado", passagem.Translado);
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
                        Situacao = ((string)dr["Situacao"]),
                        Translado = ((string)dr["Translado"])
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
                cmdBuscarId.Parameters.AddWithValue("@idPassagem", id);
                using (var drPassagemPacote = cmdBuscarId.ExecuteReader())
                {
                    cmdBuscarId.Parameters.AddWithValue("@idPassagem", id);
                    if (drPassagemPacote.HasRows)
                    {

                        MySqlCommand cmdExcluirPassagemPacote = new MySqlCommand("delete from tbPacote where IdPassagem=@IdPassagemPacote ", conexao);
                        cmdExcluirPassagemPacote.Parameters.AddWithValue("@IdPassagemPacote", id);
                        cmdExcluirPassagemPacote.ExecuteNonQuery();
                    }
                    else
                    {
                        MySqlCommand cmdExcluirPassagem = new MySqlCommand("delete from tbPassagem where IdPassagem=@IdPassagem", conexao);
                        cmdExcluirPassagem.Parameters.AddWithValue("@IdPassagem", id);
                        cmdExcluirPassagem.ExecuteNonQuery();
                    }
                }
                int i = cmdBuscarId.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public tbPassagem ObterPassagem(int Codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select 1 from tbPassagem where IdPassagem=@codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", Codigo);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                tbPassagem passagem = new tbPassagem();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    passagem.IdPassagem = Convert.ToInt32(dr["IdPassagem"]);
                    passagem.Assento = ((string)dr["Assento"]);
                    passagem.Situacao = ((string)dr["Situacao"]);
                    passagem.Valor = (decimal)(dr["Valor"]);
                    passagem.IdViagem = Convert.ToInt32(dr["IdPassagem"]);
                    passagem.Translado = ((string)dr["Translado"]);

                }
                return passagem;
            }
        }
    }
}
