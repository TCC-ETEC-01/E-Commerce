using MySql.Data.MySqlClient;
using System.Data;
using ProjetoEcommerce.Models;
using System.Net.Mail;

namespace ProjetoEcommerce.Repositorios
{
    public class ClienteRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public async Task<tbCliente> ObterCliente(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("select * from tbCliente where IdCliente = @idCliente", conexao);
                cmd.Parameters.Add("@idCliente", MySqlDbType.VarChar).Value = Id;

                using (MySqlDataReader dr = (MySqlDataReader)await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    tbCliente cliente = null;
                    if (await dr.ReadAsync())
                    {
                        cliente = new tbCliente
                        {
                            Nome = dr["Nome"].ToString(),
                            Sexo = dr["Sexo"].ToString(),
                            Email = dr["Email"].ToString(),
                            Telefone = dr["Email"].ToString(),
                            Cpf = dr["Cpf"].ToString(),
                            IdCliente = Convert.ToInt32(dr["IdCliente"])
                        };
                    }
                    return cliente;
                }
            }
        }

        public async Task<bool> CadastrarCliente(tbCliente cliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand verify = new MySqlCommand("select 1 from tbCliente where Email=@email", conexao);
                verify.Parameters.AddWithValue("@email", cliente.Email);
                using (var conf = await verify.ExecuteReaderAsync())
                {
                    if (conf.HasRows)
                    {
                        Console.WriteLine("Email ja cadastrado!");
                        return false;
                    }
                }

                MySqlCommand verifyCpf = new MySqlCommand("select 1 from tbCliente where Cpf=@cpf", conexao);
                verifyCpf.Parameters.AddWithValue("@cpf", cliente.Cpf);
                using (var conf = await verifyCpf.ExecuteReaderAsync())
                {
                    if (conf.HasRows)
                    {
                        Console.WriteLine("Cpf ja cadastrado!");
                        return false;
                    }
                }

                MySqlCommand verifyTelefone = new MySqlCommand("select 1 from tbCliente where Telefone=@telefone", conexao);
                verifyTelefone.Parameters.AddWithValue("@telefone", cliente.Telefone);
                using (var conf = await verifyTelefone.ExecuteReaderAsync())
                {
                    if (conf.HasRows)
                    {
                        Console.WriteLine("Telefone ja cadastrado!");
                        return false;
                    }
                }

                MySqlCommand cmd = new MySqlCommand("insert into tbCliente (Nome,Sexo,Email,Telefone,Cpf)Values(@nome,@sexo,@email,@telefone,@cpf)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = cliente.Nome;
                cmd.Parameters.Add("@sexo", MySqlDbType.VarChar).Value = cliente.Sexo;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = cliente.Cpf;
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
        }

        public async Task<bool> EditarCliente(tbCliente cliente)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    await conexao.OpenAsync();
                    MySqlCommand cmd = new MySqlCommand("update tbCliente set Nome=@nome,Sexo=@sexo,Email=@email,Telefone=@telefone,Cpf=@cpf where IdCliente=@IdCliente", conexao);
                    cmd.Parameters.Add("@IdCliente", MySqlDbType.Int32).Value = cliente.IdCliente;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = cliente.Nome;
                    cmd.Parameters.Add("@sexo", MySqlDbType.VarChar).Value = cliente.Sexo;
                    cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = cliente.Email;
                    cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                    cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = cliente.Cpf;
                    int linhasAfestadas = await cmd.ExecuteNonQueryAsync();
                    return linhasAfestadas > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o funcionario: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<tbCliente>> TodosClientes()
        {
            List<tbCliente> ListaCliente = new List<tbCliente>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("select * from tbCliente", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    ListaCliente.Add(new tbCliente
                    {
                        IdCliente = Convert.ToInt32(dr["IdCliente"]),
                        Nome = ((string)dr["Nome"]),
                        Email = ((string)dr["Email"]),
                        Sexo = ((string)dr["Sexo"]),
                        Telefone = ((string)dr["Telefone"]),
                        Cpf = ((string)dr["Cpf"])
                    });
                }
                return ListaCliente;
            }
        }

        public async Task ExcluirCliente(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("delete from tbCliente where IdCliente=@IdCliente", conexao);
                cmd.Parameters.AddWithValue("IdCliente", id);
                await cmd.ExecuteNonQueryAsync();
                await conexao.CloseAsync();
            }
        }

        public async Task<List<tbCliente>> BuscarCliente(string nome)
        {
            var lista = new List<tbCliente>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                string query = @"select * from tbCliente where Nome like @Nome";

                using (var cmdPesquisar = new MySqlCommand(query, conexao))
                {
                    string termoBusca = string.IsNullOrEmpty(nome) ? "%" : $"%{nome}%";
                    cmdPesquisar.Parameters.AddWithValue("@Nome", termoBusca);

                    using (var reader = await cmdPesquisar.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new tbCliente
                            {
                                IdCliente = Convert.ToInt32(reader["IdCliente"]),
                                Nome = reader["Nome"].ToString(),
                                Email = reader["Email"].ToString(),
                                Sexo = reader["Sexo"].ToString(),
                                Telefone = reader["Telefone"].ToString(),
                                Cpf = reader["Cpf"].ToString()
                            });
                        }
                    }

                    return lista;
                }
            }
        }

    }
}
