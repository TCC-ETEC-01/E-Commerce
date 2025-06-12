using ProjetoEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Repositorios
{
    public class ViagemRepositorio
    {
        private readonly string _conexaoMySQL;

        public ViagemRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        public async Task CadastrarViagem(tbViagem viagem)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            MySqlCommand cmd = new MySqlCommand(
                "insert into tbViagem(DataRetorno,Descricao,Origem,Destino,DataPartida) values" +
                "(@dataRetorno,@descricao,@origem,@destino,@dataPartida)", conexao);

            cmd.Parameters.Add("@dataRetorno", MySqlDbType.DateTime).Value = viagem.DataRetorno;
            cmd.Parameters.Add("@descricao", MySqlDbType.Text).Value = viagem.Descricao;
            cmd.Parameters.Add("@origem", MySqlDbType.VarChar).Value = viagem.Origem;
            cmd.Parameters.Add("@destino", MySqlDbType.VarChar).Value = viagem.Destino;
            cmd.Parameters.Add("@dataPartida", MySqlDbType.DateTime).Value = viagem.DataPartida;

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> EditarViagem(tbViagem viagem)
        {
            try
            {
                using var conexao = new MySqlConnection(_conexaoMySQL);
                await conexao.OpenAsync();

                MySqlCommand cmd = new MySqlCommand(
                    "update tbViagem set DataRetorno=@dataRetorno,Descricao=@descricao,Origem=@origem,Destino=@destino," +
                    "DataPartida=@dataPartida where IdViagem=@idViagem", conexao);

                cmd.Parameters.Add("@idViagem", MySqlDbType.Int32).Value = viagem.IdViagem;
                cmd.Parameters.Add("@dataRetorno", MySqlDbType.DateTime).Value = viagem.DataRetorno;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = viagem.Descricao;
                cmd.Parameters.Add("@origem", MySqlDbType.VarChar).Value = viagem.Origem;
                cmd.Parameters.Add("@destino", MySqlDbType.VarChar).Value = viagem.Destino;
                cmd.Parameters.Add("@dataPartida", MySqlDbType.DateTime).Value = viagem.DataPartida;

                int linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                return linhasAfetadas > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar o Viagem: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<tbViagem>> TodasViagens()
        {
            var ListaViagens = new List<tbViagem>();

            using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

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
                    DataRetorno = (DateTime)dr["DataRetorno"],
                    Descricao = (string)dr["Descricao"],
                    Origem = (string)dr["Origem"],
                    Destino = (string)dr["Destino"],
                    DataPartida = (DateTime)dr["DataPartida"]
                });
            }

            return ListaViagens;
        }

        public async Task ExcluirViagem(int Id)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("delete from tbViagem where IdViagem=@idViagem", conexao);
            cmd.Parameters.AddWithValue("@idViagem", Id);
            await cmd.ExecuteNonQueryAsync();

            conexao.Close();
        }

        public async Task<tbViagem> ObterViagem(int Id)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("select * from tbViagem where IdViagem=@idViagem", conexao);
            cmd.Parameters.Add("@idViagem", MySqlDbType.Int32).Value = Id;

            using var dr = (MySqlDataReader)await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            tbViagem viagem = null;
            if (await dr.ReadAsync())
            {
                viagem = new tbViagem
                {
                    IdViagem = Convert.ToInt32(dr["IdViagem"]),
                    DataRetorno = (DateTime)dr["DataRetorno"],
                    Descricao = (string)dr["Descricao"],
                    Origem = (string)dr["Origem"],
                    Destino = (string)dr["Destino"],
                    DataPartida = (DateTime)dr["DataPartida"]
                };
            }
            return viagem;
        }
    }
}
