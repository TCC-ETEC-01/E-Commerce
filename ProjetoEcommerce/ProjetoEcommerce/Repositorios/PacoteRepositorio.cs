using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using ProjetoEcommerce.Models;
using System.Data;

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

                MySqlCommand cmdInsertPacote = new MySqlCommand("insert into tbPacote (IdPassagem, IdProduto, NomePacote, Descricao, Valor) values(@idPassagem, @idProduto, @nomePacote, @descricao, @valor)", conexao);
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
                MySqlCommand cmdAtualizarPacote = new MySqlCommand("update tbPacote set NomePacote=@nomePacote, Descricao=@descricao, Valor=@valor, IdPassagem=idProduto, IdProduto=@idProduto" + "where IdPacote=@idPacote");
                cmdAtualizarPacote.Parameters.AddWithValue("@nomePacote", pacote.NomePacote);
                cmdAtualizarPacote.Parameters.AddWithValue("@descricao", pacote.Descricao);
                cmdAtualizarPacote.Parameters.AddWithValue("@valor", pacote.Valor);
                cmdAtualizarPacote.Parameters.AddWithValue("@idPassagem", pacote.IdPassagem);
                cmdAtualizarPacote.Parameters.AddWithValue("@idProduto", pacote.IdProduto);
                int linhasAfetadas = cmdAtualizarPacote.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }
        public IEnumerable<tbPacote> TodosPacotes()
        {
            List<tbPacote> PacoteLista = new List<tbPacote>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbPacote", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    PacoteLista.Add(new tbPacote
                    {
                        IdPassagem = Convert.ToInt32(dr["IdPassagem"]),
                        IdProduto = Convert.ToInt32(dr["IdProduto"]),
                        NomePacote = ((string)dr["NomePacote"]),
                        Descricao = ((string)dr["Descricao"]),
                        Valor = ((decimal)dr["Valor"])
                       
                    });
                }
                return PacoteLista;
            }
        }
        public void ExcluirPacote(int Id)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from tbPacote where IdPacote=@idPacote", conexao);
                cmd.Parameters.AddWithValue("@IdPacote", Id);
                int i = cmd.ExecuteNonQuery();
                conexao.Close() ;
            }
        }
        public tbPacote ObterPacote(int Codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbPacote where IdPacote=@codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", Codigo);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                tbPacote pacote = new tbPacote();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    pacote.IdPacote = Convert.ToInt32(dr["IdPacote"]);
                    pacote.NomePacote = ((string)dr["NomePacote"]);
                    pacote.Descricao = ((string)dr["Descricao"]);
                    pacote.Valor = (decimal)(dr["Preco"]);
                    pacote.IdPassagem = Convert.ToInt32(dr["IdPassagem"]);
                    pacote.IdProduto = Convert.ToInt32(dr["IdProduto"]);
                }
                return pacote;
            }
        }
    }
}
