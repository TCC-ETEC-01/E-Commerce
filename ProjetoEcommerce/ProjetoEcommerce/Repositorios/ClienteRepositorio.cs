﻿using MySql.Data.MySqlClient;
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
                MySqlCommand cmd = new("select * from tbCliente where Email = @email", conexao);
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;

                using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    tbCliente cliente = null;
                    if (dr.Read())
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
        public void CadastrarCliente (tbCliente cliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("insert into tbCliente (Nome,Sexo,Email,Telefone,Cpf)Values(@nome,@sexo,@email,@telefone,@cpf)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = cliente.Nome;
                cmd.Parameters.Add("@sexo", MySqlDbType.VarChar).Value = cliente.Sexo;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = cliente.Cpf;
                cmd.ExecuteNonQuery();

            }  
        }

        public bool EditarCliente(tbCliente cliente)
        {
            try
            {

                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("update tbCliente set Nome=@nome,Sexo=@sexo,Email=@email,Telefone=@telefone,Cpf=@cpf" +
                        "where IdCliente=@IdCliente", conexao);
                    cmd.Parameters.Add("@IdCliente", MySqlDbType.Int32).Value = cliente.IdCliente;
                    cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cliente.Nome;
                    cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = cliente.Email;
                    cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                    cmd.Parameters.Add("@Cpf", MySqlDbType.VarChar).Value = cliente.Cpf;
                    int linhasAfestadas = cmd.ExecuteNonQuery();
                    return linhasAfestadas > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o funcionario: {ex.Message}");
                return false;
                
            }
        }

        public IEnumerable<tbCliente> TodosClientes()
        {
            List<tbCliente> ListaCliente = new List<tbCliente>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbCliente", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                
                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    ListaCliente
                }
            }

        }
    }
}
