using ProjetoEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;
using MySqlX.XDevAPI;


namespace ProjetoEcommerce.Repositorios
{
    public class FuncionarioRepositorio (IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public bool CadastrarFuncionario(tbFuncionario funcionario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand verifyEmail = new MySqlCommand("select tbFuncionario where Email=@email", conexao);
                verifyEmail.Parameters.AddWithValue("@email", funcionario);
                using (var conf = verifyEmail.ExecuteReader())
                {
                    if (conf.HasRows)
                    {
                        Console.WriteLine("Email já cadastrado!");
                        return false;
                    }
                }

                MySqlCommand verifyCpf = new MySqlCommand("select tbFuncionario where Cpf=@cpf", conexao);
                verifyCpf.Parameters.AddWithValue("@cpf", funcionario);
                using (var conf = verifyCpf.ExecuteReader())
                {
                    if (conf.HasRows)
                    {
                        Console.WriteLine("Cpf já cadastrado!");
                        return false;
                    }
                }

                MySqlCommand verifyTelefone = new MySqlCommand("select tbFuncionario where Email=@email", conexao);
                verifyTelefone.Parameters.AddWithValue("@email", funcionario);
                using (var conf = verifyTelefone.ExecuteReader())
                {
                    if (conf.HasRows)
                    {
                        Console.WriteLine("Telefone já cadastrado!");
                        return false;
                    }
                }

                MySqlCommand cmd = new MySqlCommand("insert into tbFuncionario(Nome,Sexo,Email,Telefone,Cargo,Cpf,Senha)values(@nome,@sexo,@email,@telefone,@cargo,@cpf,@senha", conexao);
                cmd.Parameters.Add("@nome",MySqlDbType.VarChar).Value = funcionario.Nome;
                cmd.Parameters.Add("@sexo", MySqlDbType.VarChar).Value = funcionario.Sexo;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = funcionario.Email;
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = funcionario.Telefone;
                cmd.Parameters.Add("@cargo", MySqlDbType.VarChar).Value = funcionario.Cargo;
                cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = funcionario.Cpf;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = funcionario.Senha;
                cmd.ExecuteNonQuery();

                return true;
            }
        }

        public bool EditarFuncionario(tbFuncionario funcionario)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("update tbFuncionario set Nome=@nome,Sexo=@sexo,Email=@email,Telefone=@telefone,Cargo=@cargo,Cpf=@cpf,Senha=@senha" +
                        "where IdFuncionario=@IdFuncionario", conexao);
                    cmd.Parameters.Add("@IdFuncionario", MySqlDbType.Int32).Value = funcionario.IdFuncionario;
                    cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = funcionario.Nome;
                    cmd.Parameters.Add("@Sexo", MySqlDbType.VarChar).Value = funcionario.Sexo;
                    cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = funcionario.Email;
                    cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = funcionario.Telefone;
                    cmd.Parameters.Add("@Cargo", MySqlDbType.VarChar).Value = funcionario.Cargo;
                    cmd.Parameters.Add("@Cpf", MySqlDbType.VarChar).Value = funcionario.Cpf;
                    cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = funcionario.Senha;
                    int linhaAfestadas = cmd.ExecuteNonQuery();
                    return linhaAfestadas > 0;

                }
            }

            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar o funcionario: {ex.Message}");
                return false;
            }
        }
        public IEnumerable<tbFuncionario> TodosFuncionarios()
        {
            List<tbFuncionario> FuncList = new List<tbFuncionario>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbFuncionario", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    FuncList.Add(new tbFuncionario
                    {
                        IdFuncionario = Convert.ToInt32(dr["IdFuncionario"]),
                        Nome = ((string)dr["Nome"]),
                        Sexo = ((string)dr["Sexo"]),
                        Email = ((string)dr["Email"]),
                        Telefone = ((string)dr["Telefone"]),
                        Cargo = ((string)dr["Cargo"]),
                        Cpf = ((string)dr["Cpf"]),
                        Senha = ((string)dr["Senha"])
                    });
                }
                return FuncList;
            }
        }
        public tbFuncionario ObterFuncionarioEmail(string email)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbFuncionario where Email=@email", conexao);
                cmd.Parameters.AddWithValue("@email", email);
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dr.Read())
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
        public tbFuncionario ObterFuncionarioID(string id)
        {
            using ( var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbFUncionario where IdFuncionario=@id", conexao);
                cmd.Parameters.AddWithValue("IdFuncionario", id);
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dr.Read())
                    {
                        return new tbFuncionario
                        {
                            IdFuncionario = Convert.ToInt32(dr[id]),
                            Nome = ((string)dr["Nome"]),
                            Sexo = ((string)dr["Sexo"]),
                            Email = ((string)dr["Email"]),
                            Telefone = ((string)dr["Telefone"]),
                            Cargo = ((string)dr["Cargo"]),
                            Cpf = ((string)dr["Cpf"]),
                            Senha = ((string)dr["Senha"])
                        };
                    }
                    return null;
                }
            }
        }
        public void ExcluirFuncionario(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from tbFuncionario where IdFuncionario=@id", conexao);
                cmd.Parameters.AddWithValue("@IdFuncionario",Id);
                int i = cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
    }
}
