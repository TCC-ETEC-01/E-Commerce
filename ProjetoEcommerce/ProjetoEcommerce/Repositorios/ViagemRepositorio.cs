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

        public async Task ExcluirViagem(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand cmdBuscarId = new MySqlCommand("select 1 from tbPassagem where IdViagem=@idViagem", conexao);
                cmdBuscarId.Parameters.AddWithValue("@idViagem", id);

                using (var drViagemPassagem = await cmdBuscarId.ExecuteReaderAsync())
                {
                    if (await drViagemPassagem.ReadAsync())
                    {
                        drViagemPassagem.Close();
                        MySqlCommand cmdExcluirPassagens = new MySqlCommand("delete from tbPassagem where IdViagem=@idViagem", conexao);
                        cmdExcluirPassagens.Parameters.AddWithValue("@idViagem", id);
                        await cmdExcluirPassagens.ExecuteNonQueryAsync();
                    }
                }

                using (var drViagem = await cmdBuscarId.ExecuteReaderAsync())
                {
                    if (!await drViagem.ReadAsync())
                    {
                        drViagem.Close();
                        MySqlCommand cmdExcluirViagem = new MySqlCommand("delete from tbViagem where IdViagem=@idViagem", conexao);
                        cmdExcluirViagem.Parameters.AddWithValue("@idViagem", id);
                        await cmdExcluirViagem.ExecuteNonQueryAsync();
                    }
                }
            }
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
        public async Task<IEnumerable<tbViagem>> BuscarViagens(string termo)
        {
            var lista = new List<tbViagem>();

            using var conexao = new MySqlConnection(_conexaoMySQL);
            await conexao.OpenAsync();

            string query = @"select * from tbViagem 
                     where Descricao like @termo or Destino like @termo";

            using var cmd = new MySqlCommand(query, conexao);
            string busca = string.IsNullOrEmpty(termo) ? "%" : $"%{termo}%";
            cmd.Parameters.AddWithValue("@termo", busca);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new tbViagem
                {
                    IdViagem = reader.GetInt32("IdViagem"),
                    DataRetorno = reader.GetDateTime("DataRetorno"),
                    Descricao = reader.GetString("Descricao"),
                    Origem = reader.GetString("Origem"),
                    Destino = reader.GetString("Destino"),
                    DataPartida = reader.GetDateTime("DataPartida")
                });
            }

            return lista;
        }
    }
}
