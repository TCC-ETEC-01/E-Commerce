using ProjetoEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProjetoEcommerce.Repositorios
{
    public class FuncionarioRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public async Task<bool> CadastrarFuncionario(tbFuncionario funcionario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand verifyEmail = new MySqlCommand("select 1 from tbFuncionario where Email=@email", conexao);
                verifyEmail.Parameters.AddWithValue("@email", funcionario.Email);
                using (var conf = await verifyEmail.ExecuteReaderAsync())
                {
                    if (conf.HasRows)
                    {
                        Console.WriteLine("Email já cadastrado!");
                        return false;
                    }
                }

                MySqlCommand verifyCpf = new MySqlCommand("select 1 from tbFuncionario where Cpf=@cpf", conexao);
                verifyCpf.Parameters.AddWithValue("@cpf", funcionario.Cpf);
                using (var conf = await verifyCpf.ExecuteReaderAsync())
                {
                    if (conf.HasRows)
                    {
                        Console.WriteLine("Cpf já cadastrado!");
                        return false;
                    }
                }

                MySqlCommand verifyTelefone = new MySqlCommand("select 1 from tbFuncionario where Telefone=@telefone", conexao);
                verifyTelefone.Parameters.AddWithValue("@telefone", funcionario.Telefone);
                using (var conf = await verifyTelefone.ExecuteReaderAsync())
                {
                    if (conf.HasRows)
                    {
                        Console.WriteLine("Telefone já cadastrado!");
                        return false;
                    }
                }

                MySqlCommand cmd = new MySqlCommand("insert into tbFuncionario(Nome,Sexo,Email,Telefone,Cargo,Cpf,Senha)values(@nome,@sexo,@email,@telefone,@cargo,@cpf,@senha)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = funcionario.Nome;
                cmd.Parameters.Add("@sexo", MySqlDbType.VarChar).Value = funcionario.Sexo;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = funcionario.Email;
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = funcionario.Telefone;
                cmd.Parameters.Add("@cargo", MySqlDbType.VarChar).Value = funcionario.Cargo;
                cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = funcionario.Cpf;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = funcionario.Senha;
                await cmd.ExecuteNonQueryAsync();

                return true;
            }
        }

        public async Task<bool> EditarFuncionario(tbFuncionario funcionario)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    await conexao.OpenAsync();
                    MySqlCommand cmd = new MySqlCommand("update tbFuncionario set Nome=@nome,Sexo=@sexo,Email=@email,Telefone=@telefone,Cargo=@cargo,Cpf=@cpf,Senha=@senha where IdFuncionario=@IdFuncionario", conexao);
                    cmd.Parameters.Add("@IdFuncionario", MySqlDbType.Int32).Value = funcionario.IdFuncionario;
                    cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = funcionario.Nome;
                    cmd.Parameters.Add("@Sexo", MySqlDbType.VarChar).Value = funcionario.Sexo;
                    cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = funcionario.Email;
                    cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = funcionario.Telefone;
                    cmd.Parameters.Add("@Cargo", MySqlDbType.VarChar).Value = funcionario.Cargo;
                    cmd.Parameters.Add("@Cpf", MySqlDbType.VarChar).Value = funcionario.Cpf;
                    cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = funcionario.Senha;
                    int linhaAfestadas = await cmd.ExecuteNonQueryAsync();
                    return linhaAfestadas > 0;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar o funcionario: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<tbFuncionario>> TodosFuncionarios()
        {
            List<tbFuncionario> FuncList = new List<tbFuncionario>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("select * from tbFuncionario", conexao);
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        FuncList.Add(new tbFuncionario
                        {
                            IdFuncionario = Convert.ToInt32(dr["IdFuncionario"]),
                            Nome = dr["Nome"].ToString(),
                            Sexo = dr["Sexo"].ToString(),
                            Email = dr["Email"].ToString(),
                            Telefone = dr["Telefone"].ToString(),
                            Cargo = dr["Cargo"].ToString(),
                            Cpf = dr["Cpf"].ToString(),
                            Senha = dr["Senha"].ToString()
                        });
                    }
                }
                return FuncList;
            }
        }

        public async Task<tbFuncionario> ObterFuncionarioEmail(string email)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("select * from tbFuncionario where Email=@email", conexao);
                cmd.Parameters.AddWithValue("@email", email);
                using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    if (await dr.ReadAsync())
                    {
                        return new tbFuncionario
                        {
                            Email = dr["Email"].ToString(),
                            Senha = dr["Senha"].ToString()
                        };
                    }
                }
                return null;
            }
        }

        public async Task<tbFuncionario> ObterFuncionarioID(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("select * from tbFuncionario where IdFuncionario=@id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
                using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    if (await dr.ReadAsync())
                    {
                        return new tbFuncionario
                        {
                            IdFuncionario = Convert.ToInt32(dr["IdFuncionario"]),
                            Nome = dr["Nome"].ToString(),
                            Sexo = dr["Sexo"].ToString(),
                            Email = dr["Email"].ToString(),
                            Telefone = dr["Telefone"].ToString(),
                            Cargo = dr["Cargo"].ToString(),
                            Cpf = dr["Cpf"].ToString(),
                            Senha = dr["Senha"].ToString()
                        };
                    }
                }
                return null;
            }
        }

        public async Task ExcluirFuncionario(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("delete from tbFuncionario where IdFuncionario=@IdFuncionario", conexao);
                cmd.Parameters.AddWithValue("@IdFuncionario", Id);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<List<tbFuncionario>> BuscarFuncionario(string nome)
        {
            var lista = new List<tbFuncionario>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                string query = @"select * from tbFuncionario where Nome like @Nome";

                using (var cmdPesquisar = new MySqlCommand(query, conexao))
                {
                    string termoBusca = string.IsNullOrEmpty(nome) ? "%" : $"%{nome}%";
                    cmdPesquisar.Parameters.AddWithValue("@Nome", termoBusca);

                    using (var reader = await cmdPesquisar.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new tbFuncionario
                            {
                                IdFuncionario = Convert.ToInt32(reader["IdFuncionario"]),
                                Nome = reader["Nome"].ToString(),
                                Sexo = reader["Sexo"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefone = reader["Telefone"].ToString(),
                                Cargo = reader["Cargo"].ToString(),
                                Cpf = reader["Cpf"].ToString(),
                                Senha = reader["Senha"].ToString()
                            });
                        }
                    }

                    return lista;
                }
            }
        }

    }
}