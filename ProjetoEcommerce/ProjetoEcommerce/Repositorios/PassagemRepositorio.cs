using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Repositorios
{
    public class PassagemRepositorio
    {
        private readonly string _conexaoMySQL;

        public PassagemRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        public async Task<bool> CadastrarPassagem(tbPassagem passagem)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand cmdBuscarViagem = new MySqlCommand("select 1 from tbViagem where IdViagem=@idViagem", conexao);
                cmdBuscarViagem.Parameters.AddWithValue("@idViagem", passagem.IdViagem.IdViagem);

                using (var drViagem = await cmdBuscarViagem.ExecuteReaderAsync())
                {
                    if (!await drViagem.ReadAsync())
                    {
                        Console.WriteLine("Viagem não existente");
                        return false;
                    }
                    drViagem.Close();
                }

                MySqlCommand cmdBuscarPassagem = new MySqlCommand(
                    "select 1 from tbPassagem where IdTransporte=@idTransporte and Assento=@assento", conexao);
                cmdBuscarPassagem.Parameters.AddWithValue("@idTransporte", passagem.IdTransporte.IdTransporte);
                cmdBuscarPassagem.Parameters.AddWithValue("@assento", passagem.Assento);

                using (var drAssento = await cmdBuscarPassagem.ExecuteReaderAsync())
                {
                    if (await drAssento.ReadAsync())
                    {
                        Console.WriteLine("Assento já cadastrado");
                        return false;
                    }
                    drAssento.Close();
                }

                MySqlCommand cmdInsert = new MySqlCommand(
                    "insert into tbPassagem(Assento, Valor, Situacao, IdViagem, Translado, IdTransporte) " +
                    "values (@assento,@valor,@situacao,@idViagem,@translado,@idTransporte)", conexao);

                cmdInsert.Parameters.AddWithValue("@assento", passagem.Assento);
                cmdInsert.Parameters.AddWithValue("@valor", passagem.Valor);
                cmdInsert.Parameters.AddWithValue("@situacao", passagem.Situacao);
                cmdInsert.Parameters.AddWithValue("@idViagem", passagem.IdViagem.IdViagem);
                cmdInsert.Parameters.AddWithValue("@translado", passagem.Translado);
                cmdInsert.Parameters.AddWithValue("@idTransporte", passagem.IdTransporte.IdTransporte);

                await cmdInsert.ExecuteNonQueryAsync();
                return true;
            }
        }

        public async Task<bool> AtualizarPassagem(tbPassagem passagem)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand cmd = new MySqlCommand(
                    "update tbPassagem set Assento=@assento, Valor=@valor, Situacao=@situacao, IdViagem=@idViagem, Translado=@translado, IdTransporte=@idTransporte " +
                    "where IdPassagem=@idPassagem", conexao);

                cmd.Parameters.AddWithValue("@assento", passagem.Assento);
                cmd.Parameters.AddWithValue("@valor", passagem.Valor);
                cmd.Parameters.AddWithValue("@situacao", passagem.Situacao);
                cmd.Parameters.AddWithValue("@idViagem", passagem.IdViagem.IdViagem);
                cmd.Parameters.AddWithValue("@translado", passagem.Translado);
                cmd.Parameters.AddWithValue("@idTransporte", passagem.IdTransporte.IdTransporte);
                cmd.Parameters.AddWithValue("@idPassagem", passagem.IdPassagem);

                int linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                return linhasAfetadas > 0;
            }
        }

        public async Task ExcluirPassagem(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand cmdBuscarId = new MySqlCommand("select 1 from tbPacote where IdPassagem=@idPassagem", conexao);
                cmdBuscarId.Parameters.AddWithValue("@idPassagem", id);

                using (var drPassagemPacote = await cmdBuscarId.ExecuteReaderAsync())
                {
                    if (await drPassagemPacote.ReadAsync())
                    {
                        drPassagemPacote.Close();
                        MySqlCommand cmdExcluirPassagemPacote = new MySqlCommand("delete from tbPacote where IdPassagem=@IdPassagemPacote", conexao);
                        cmdExcluirPassagemPacote.Parameters.AddWithValue("@IdPassagemPacote", id);
                        await cmdExcluirPassagemPacote.ExecuteNonQueryAsync();
                    }
                }
                using (var drPassagem = await cmdBuscarId.ExecuteReaderAsync())
                { 
                    if (!await drPassagem.ReadAsync())
                    {
                        drPassagem.Close();
                        MySqlCommand cmdExcluirPassagem = new MySqlCommand("delete from tbPassagem where IdPassagem=@IdPassagem", conexao);
                        cmdExcluirPassagem.Parameters.AddWithValue("@IdPassagem", id);
                        await cmdExcluirPassagem.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public async Task<tbPassagem> ObterPassagem(int codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand cmd = new MySqlCommand("select * from tbPassagem where IdPassagem=@codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", codigo);

                using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    tbPassagem passagem = null;

                    if (await dr.ReadAsync())
                    {
                        passagem = new tbPassagem
                        {
                            IdPassagem = Convert.ToInt32(dr["IdPassagem"]),
                            Assento = dr["Assento"].ToString(),
                            Situacao = dr["Situacao"].ToString(),
                            Valor = Convert.ToDecimal(dr["Valor"]),
                            IdViagem = new tbViagem { IdViagem = Convert.ToInt32(dr["IdViagem"]) },
                            IdTransporte = new tbTransporte { IdTransporte = Convert.ToInt32(dr["IdTransporte"]) },
                            Translado = dr["Translado"].ToString()
                        };
                    }

                    return passagem;
                }
            }
        }

        public async Task<bool> VendaPassagem(tbVenda venda)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand cmdVerificarFuncionario = new MySqlCommand("select 1 from tbFuncionario where IdFuncionario=@idFuncionario", conexao);
                cmdVerificarFuncionario.Parameters.AddWithValue("@idFuncionario", venda.IdFuncionario.IdFuncionario);
                using (var drVerificarFuncionario = await cmdVerificarFuncionario.ExecuteReaderAsync())
                {
                    if (!await drVerificarFuncionario.ReadAsync())
                    {
                        Console.WriteLine("Operação cancelada, funcionário inexistente");
                        return false;
                    }
                }

                MySqlCommand cmdVerificarPassagem = new MySqlCommand("select 1 from tbPassagem where IdPassagem = @idPassagem", conexao);
                cmdVerificarPassagem.Parameters.AddWithValue("@idPassagem", venda.IdPassagem.IdPassagem);
                using (var drVerificarPassagem = await cmdVerificarPassagem.ExecuteReaderAsync())
                {
                    if (!await drVerificarPassagem.ReadAsync())
                    {
                        Console.WriteLine("Operação cancelada, passagem inexistente");
                        return false;
                    }
                }

                MySqlCommand cmdVerificarCliente = new MySqlCommand("select 1 from tbCliente where IdCliente=@idCliente", conexao);
                cmdVerificarCliente.Parameters.AddWithValue("@idCliente", venda.IdCliente.IdCliente);
                using (var drVerificarCliente = await cmdVerificarCliente.ExecuteReaderAsync())
                {
                    if (!await drVerificarCliente.ReadAsync())
                    {
                        Console.WriteLine("Operação cancelada, cliente inexistente");
                        return false;
                    }
                }

                MySqlCommand cmdSituacaoPassagem = new MySqlCommand("select Situacao from tbPassagem where IdPassagem = @idPassagem", conexao);
                cmdSituacaoPassagem.Parameters.AddWithValue("@idPassagem", venda.IdPassagem.IdPassagem);
                var situacao = await cmdSituacaoPassagem.ExecuteScalarAsync();
                if (situacao?.ToString() == "Indisponivel")
                {
                    Console.WriteLine("Impossivel realizar a operação, passagem indisponivel");
                    return false;
                }

                MySqlCommand cmdInserirVenda = new MySqlCommand(
                    "insert into tbVenda(IdCliente, IdPassagem, IdFuncionario, Valor, FormaPagamento, DataVenda) " +
                    "values(@idCliente, @idPassagem, @idFuncionario, @valor, @pagamento, @dataVenda)", conexao);

                cmdInserirVenda.Parameters.AddWithValue("@idCliente", venda.IdCliente.IdCliente);
                cmdInserirVenda.Parameters.AddWithValue("@idFuncionario", venda.IdFuncionario.IdFuncionario);
                cmdInserirVenda.Parameters.AddWithValue("@idPassagem", venda.IdPassagem.IdPassagem);
                cmdInserirVenda.Parameters.AddWithValue("@valor", venda.Valor);
                cmdInserirVenda.Parameters.AddWithValue("@pagamento", venda.FormaPagamento);
                cmdInserirVenda.Parameters.AddWithValue("@dataVenda", venda.DataVenda);

                await cmdInserirVenda.ExecuteNonQueryAsync();
                var idVendaGerado = cmdInserirVenda.LastInsertedId;

                MySqlCommand cmdVerificarVenda = new MySqlCommand("select 1 from tbVenda where IdVenda=@idVenda and IdPassagem=@idPassagem", conexao);
                cmdVerificarVenda.Parameters.AddWithValue("@idPassagem", venda.IdPassagem.IdPassagem);
                cmdVerificarVenda.Parameters.AddWithValue("@idVenda", idVendaGerado);

                using (var drVenda = await cmdVerificarVenda.ExecuteReaderAsync())
                {
                    if (await drVenda.ReadAsync())
                    {
                        MySqlCommand cmdAtualizarPassagem = new MySqlCommand("update tbPassagem set Situacao='Indisponivel' where IdPassagem=@idPassagem", conexao);
                        cmdAtualizarPassagem.Parameters.AddWithValue("@idPassagem", venda.IdPassagem.IdPassagem);
                        await cmdAtualizarPassagem.ExecuteNonQueryAsync();
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
