using ProjetoEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography;
using Microsoft.VisualBasic;
namespace ProjetoEcommerce.Repositorios
{
    public class ViagemRepositorio (IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public void CadastrarViagem(tbViagem viagem)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("insert into tbViagem(DataRetorno,Descricao,Origem,Destino,TipoTransporte,DataPartida)values" +
                    "(@dataretorno,@descricao,@origem,@destino,@tipotransporte,@datapartida)", conexao);
                cmd.Parameters.Add("@dataretorno",MySqlDbType.DateTime).Value = viagem.DataRetorno;
                cmd.Parameters.Add("@descricao", MySqlDbType.Text).Value = viagem.Descricao;
                cmd.Parameters.Add("@origem", MySqlDbType.VarChar).Value = viagem.Origem;
                cmd.Parameters.Add("@destino", MySqlDbType.VarChar).Value = viagem.Destino;
                cmd.Parameters.Add("@tipotransporte", MySqlDbType.VarChar).Value = viagem.TipoTransporte;
                cmd.Parameters.Add("@datapartida",MySqlDbType.DateTime).Value = viagem.DataPartida;
                cmd.ExecuteNonQuery();
            }
        }
        public bool EditarViagem(tbViagem viagem)
        {
            try
            {


                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("update tbViagem set DataRetorno=@dataretorno,Descricao=@descricao,Origem=@origem,Destino=@destino," +
                        "TipoTransporte=@tipotransporte,DataPartida=@datapartida" + "where IdViagem=@IdViagem", conexao);
                    cmd.Parameters.Add("@IdViagem", MySqlDbType.Int32).Value = viagem.IdViagem;
                    cmd.Parameters.Add("@DataRetorno", MySqlDbType.DateTime).Value = viagem.DataRetorno;
                    cmd.Parameters.Add("@Descricao", MySqlDbType.VarChar).Value = viagem.Descricao;
                    cmd.Parameters.Add("@Origem", MySqlDbType.VarChar).Value = viagem.Origem;
                    cmd.Parameters.Add("@Destino", MySqlDbType.VarChar).Value = viagem.Destino;
                    cmd.Parameters.Add("@TipoTransporte", MySqlDbType.VarChar).Value = viagem.TipoTransporte;
                    cmd.Parameters.Add("@DataPartida", MySqlDbType.VarChar).Value = viagem.DataPartida;
                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    return linhasAfetadas > 0;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar o Viagem: {ex.Message}");
                return false;
            }
        }
        public IEnumerable<tbViagem> TodasViagens()
        {
            List<tbViagem> ListaViagens = new List<tbViagem>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbViagem", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    ListaViagens.Add(new tbViagem
                    {
                        IdViagem = Convert.ToInt32(dr["IdViagem"]),
                        DataRetorno = ((DateTime)dr["DataRetorno"]),
                        Descricao = ((string)dr["Descricao"]),
                        Origem = ((string)dr["Origem"]),
                        Destino = ((string)dr["Destino"]),
                        TipoTransporte = ((string)dr["TipoTransporte"]),
                        DataPartida = ((DateTime)dr["DataPartida"])
                    });
                } 
                return ListaViagens;
            }
        }
        public void ExcluirViagem(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from tbViagem where IdViagem=@Id", conexao);
                cmd.Parameters.AddWithValue("IdViagem", Id);
                int i = cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public tbViagem ObterViagem(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbViagem where IdViagem=@IdViagem", conexao);
                cmd.Parameters.Add("IdViagem",MySqlDbType.VarChar).Value = Id;

                using(MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    tbViagem viagem = null;
                    if(dr.Read())
                    {
                        viagem = new tbViagem
                        {
                            IdViagem = Convert.ToInt32(dr["IdViagem"]),
                            DataRetorno = ((DateTime)dr["DataRetorno"]),
                            Descricao = ((string)dr["Descricao"]),
                            Origem = ((string)dr["Origem"]),
                            Destino = ((string)dr["Destino"]),
                            TipoTransporte = ((string)dr["TipoTransporte"]),
                            DataPartida = ((DateTime)dr["DataPartida"])
                        };
                    } return viagem;
                }
            }
        }
    }
}
