using MySql.Data.MySqlClient;
using System.Data;
using ProjetoEcommerce.Models;

namespace ProjetoEcommerce.Repositorios
{
    public class ClienteRepositorio (IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public tbCliente ObterCliente(string email)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new("select * from tbCliente where Email = @email"), conexao;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;

                using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    tbCliente cliente = null;
                    if(dr.Read())
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
                    } return cliente;
                }
            }
        }
    }
}
