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

                MySqlCommand cmdBuscarPassagem = new MySqlCommand("select 1 from tbPassagem where IdTransporte=@idTransporte" +
                    "and Assento=@assento");
                cmdBuscarPassagem.Parameters.AddWithValue("@idTransporte", passagem.IdTransporte);
                cmdBuscarPassagem.Parameters.AddWithValue("@assento", passagem.Assento);
                using (var drAssento = cmdBuscarPassagem.ExecuteReader())
                {
                    if (drAssento.HasRows)
                    {
                        Console.WriteLine("Assento já cadastrado");
                        return false;
                    }
                }

                MySqlCommand cmdInsert = new MySqlCommand("insert into tbPassagem(Assento, Valor, Situacao, IdViagem,Translado, IdTransporte) values (@assento,@valor,@situacao,@idViagem, @translado, @idTransporte)", conexao);
                cmdInsert.Parameters.Add("@assento", MySqlDbType.VarChar).Value = passagem.Assento;
                cmdInsert.Parameters.Add("@valor", MySqlDbType.Decimal).Value = passagem.Valor;
                cmdInsert.Parameters.Add("@situacao", MySqlDbType.VarChar).Value = passagem.Situacao;
                cmdInsert.Parameters.Add("@idViagem", MySqlDbType.Int32).Value = passagem.IdViagem;
                cmdInsert.Parameters.Add("@idTransporte", MySqlDbType.Int32).Value = passagem.IdTransporte;
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
                MySqlCommand cmd = new MySqlCommand("update tbPassagem set Assento=@assento, Valor=@valor,Situacao=@situacao,IdViagem=@idViagem, Translado=@translado, IdTransporte=@idTransporte" + "where IdPassagem=@passagem", conexao);
                cmd.Parameters.AddWithValue("@assento", passagem.Assento);
                cmd.Parameters.AddWithValue("@situacao", passagem.Situacao);
                cmd.Parameters.AddWithValue("@valor", passagem.Valor);
                cmd.Parameters.AddWithValue("@idViagem", passagem.IdViagem);
                cmd.Parameters.AddWithValue("@translado", passagem.Translado);
                cmd.Parameters.AddWithValue("@idTransporte", passagem.IdTransporte);
                int linhasAfetadas = cmd.ExecuteNonQuery();
                return linhasAfetadas > 0;
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
                    passagem.IdViagem = new tbViagem
                    {
                        IdViagem = Convert.ToInt32(dr["IdViagem"])
                    };
                    passagem.IdTransporte = new tbTransporte
                    {
                        IdTransporte = Convert.ToInt32(dr["IdTransporte"])
                    };
                    passagem.Translado = ((string)dr["Translado"]);

                }
                return passagem;
            }
        }

        public bool VendaPassagem(tbVenda venda)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmdVerificarFuncionario = new MySqlCommand("select 1 from tbFuncionario where IdFuncionario=@idFuncionario", conexao);
                cmdVerificarFuncionario.Parameters.AddWithValue("@idFuncionario", venda.IdFuncionario);
                using (var drVerificarFuncionario = cmdVerificarFuncionario.ExecuteReader())
                {
                    if (!drVerificarFuncionario.HasRows)
                    {
                        Console.WriteLine("Operação cancelada, funcionário inexistente");
                        return false;
                    }
                }
                MySqlCommand cmdVerificarPassagem = new MySqlCommand("select 1 from tbPassagem where IdPassagem = @idPassagem", conexao);
                cmdVerificarPassagem.Parameters.AddWithValue("@idPassagem", venda.IdPassagem);

                using (var drVerificarPassagem = cmdVerificarPassagem.ExecuteReader())
                {
                    if (!drVerificarPassagem.HasRows)
                    {
                        Console.WriteLine("Operação cancelada, passagem inexistente");
                        return false;
                    }
                }
                MySqlCommand cmdVerificarCliente = new MySqlCommand("select 1 from tbCliente where IdCliente=@idCliente", conexao);
                cmdVerificarCliente.Parameters.AddWithValue("@idCliente", venda.IdCliente);
                using (var drVerificarCliente = cmdVerificarCliente.ExecuteReader())
                {
                    if (!drVerificarCliente.HasRows)
                    {
                        Console.WriteLine("Operação cancelada, cliente inexistente");
                        return false;
                    }
                }

                MySqlCommand cmdSituacaoPassagem = new MySqlCommand("select Situacao from tbPassagem where IdPassagem = @idPassagem", conexao);
                cmdSituacaoPassagem.Parameters.AddWithValue("@idPassagem", venda.IdPassagem);
                var situacao = cmdSituacaoPassagem.ExecuteScalar();
                if (situacao.ToString() == "Indisponivel")
                {
                    Console.WriteLine("Impossivel realizar a operação, passagem indisponivel");
                    return false;
                }
                MySqlCommand cmdInserirVenda = new MySqlCommand("insert into tbVenda(IdCliente, IdPassagem, IdFuncionario, Valor, FormaPagamento, DataVenda) " +
                    "values(@idCliente, @idPassagem, @idFuncionario, @valor, @pagamento,@dataVenda)", conexao);
                cmdInserirVenda.Parameters.AddWithValue("@idCliente", venda.IdCliente);
                cmdInserirVenda.Parameters.AddWithValue("@idFuncionario", venda.IdFuncionario);
                cmdInserirVenda.Parameters.AddWithValue("@idPassagem", venda.IdPassagem);
                cmdInserirVenda.Parameters.AddWithValue("@valor", venda.Valor);
                cmdInserirVenda.Parameters.AddWithValue("@pagamento", venda.FormaPagamento);
                cmdInserirVenda.Parameters.AddWithValue("@dataVenda", venda.DataVenda);
                cmdInserirVenda.ExecuteNonQuery();
                var idVendaGerado = cmdInserirVenda.LastInsertedId;

                MySqlCommand cmdVerificarVenda = new MySqlCommand("select 1 from tbVenda where IdVenda=@idVenda and IdPassagem=@idPassagem", conexao);
                cmdVerificarVenda.Parameters.AddWithValue("@idPassagem", venda.IdPassagem);
                cmdVerificarVenda.Parameters.AddWithValue("@idVenda", idVendaGerado);
                using (var drVenda = cmdVerificarVenda.ExecuteReader())
                {
                    if (!drVenda.HasRows)
                    {
                        Console.WriteLine("Venda não realizada");
                        return false;
                    }
                }
                MySqlCommand cmdInserirClienteEmPassagem = new MySqlCommand("update tbPassagem set IdCliente=@idCliente where IdPassagem = @idPassagem", conexao);
                cmdInserirClienteEmPassagem.Parameters.AddWithValue("@idCliente", venda.IdCliente);
                cmdInserirClienteEmPassagem.Parameters.AddWithValue("@idPassagem", venda.IdPassagem);
                cmdInserirClienteEmPassagem.ExecuteNonQuery();

                MySqlCommand cmdInserirVendaEmVendaDetalhe = new MySqlCommand("insert into tbVendaDetalhe(IdVenda, IdPassagem) " +
                  "values(@idVenda, @idPassagem)", conexao);
                cmdInserirVendaEmVendaDetalhe.Parameters.AddWithValue("@idVenda", idVendaGerado);
                cmdInserirVendaEmVendaDetalhe.Parameters.AddWithValue("@idPassagem", venda.IdPassagem);
                cmdInserirVendaEmVendaDetalhe.ExecuteNonQuery();
                return true;
            }
        }
    }
}


