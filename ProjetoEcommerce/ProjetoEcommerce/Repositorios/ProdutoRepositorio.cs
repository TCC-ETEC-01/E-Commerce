using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;

namespace ProjetoEcommerce.Repositorios
{
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public bool CadastrarProduto(tbProduto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("insert into tbProduto (NomeProduto, Valor, Descricao, Quantidade) values(@nomeProduto,@valor, @descricao, @quantidade)", conexao);
                cmd.Parameters.AddWithValue("@nomeProduto", produto.NomeProduto);
                cmd.Parameters.AddWithValue("@valor", produto.Valor);
                cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
                cmd.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public bool AtualizarProduto(tbProduto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update tbProduto set NomeProduto=@nomeProduto, Valor=@valor, Descricao=@descricao, Quantidade=@quantidade" + "where IdProduto=@idProduto", conexao);
                cmd.Parameters.AddWithValue("@nomeProduto", produto.NomeProduto);
                cmd.Parameters.AddWithValue("@valor", produto.Valor);
                cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
                cmd.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                int linhasAfetadas = cmd.ExecuteNonQuery();
                return linhasAfetadas > 0;

            }
        }

        public IEnumerable<tbProduto> TodosProdutos()
        {
            List<tbProduto> ProdutoLista = new List<tbProduto>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbProduto", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    ProdutoLista.Add(new tbProduto
                    {
                        IdProduto = Convert.ToInt32(dr["IdProduto"]),
                        Quantidade = Convert.ToInt32(dr["Quantidade"]),
                        NomeProduto = ((string)dr["NomeProduto"]),
                        Valor = ((decimal)dr["Valor"]),
                        Descricao = ((string)dr["Descricao"])
                    });
                }
                return ProdutoLista;
            }
        }

        public void ExcluirProduto(int id)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmdBuscarId = new MySqlCommand("select 1 from tbPacote where IdProduto=@idProduto", conexao);
                cmdBuscarId.Parameters.AddWithValue("@idProduto", id);
                using (var drProduto = cmdBuscarId.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (drProduto.HasRows)
                    {
                        MySqlCommand cmdExcluirProdutoPacote = new MySqlCommand("delete from tbPacote where IdProduto=@idProduto", conexao);
                        cmdExcluirProdutoPacote.Parameters.AddWithValue("@idProduto", id);
                        cmdExcluirProdutoPacote.ExecuteNonQuery();
                    }
                    else
                    {
                        MySqlCommand cmdExcluirProduto = new MySqlCommand("delete from tbProduto where IdProduto=@idProduto", conexao);
                        cmdExcluirProduto.Parameters.AddWithValue("@idProduto", id);
                        cmdExcluirProduto.ExecuteNonQuery();
                    }
                    int i = cmdBuscarId.ExecuteNonQuery();
                    conexao.Close();
                }
            }
        }
        public tbProduto ObterProduto(int Codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select 1 from tbProduto where IdProduto=@codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", Codigo);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
               tbProduto produto = new tbProduto();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    produto.IdProduto = Convert.ToInt32(dr["IdProduto"]);
                    produto.Descricao = ((string)dr["Descricao"]);
                    produto.NomeProduto = ((string)dr["NomeProduto"]);
                    produto.Valor = (decimal)(dr["Valor"]);
                    produto.Quantidade = Convert.ToInt32(dr["Quantidade"]);
                }
                return produto;
            }
        }
    }
}
