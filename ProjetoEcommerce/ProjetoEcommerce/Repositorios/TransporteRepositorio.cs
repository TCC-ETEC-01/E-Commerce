using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;

namespace ProjetoEcommerce.Repositorios
{
    public class TransporteRepositorio(IConfiguration configuration)
    {
            private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

            public bool CadastrarTransporte(tbTransporte transporte)
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    var cmd = new MySqlCommand("INSERT INTO tbTransporte (CodigoTransporte, Companhia, TipoTransporte) VALUES (@codigo, @companhia, @tipo)", conexao);
                    cmd.Parameters.AddWithValue("@codigo", transporte.CodigoTransporte);
                    cmd.Parameters.AddWithValue("@companhia", transporte.Companhia);
                    cmd.Parameters.AddWithValue("@tipo", transporte.TipoTransporte);
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            public bool AtualizarTransporte(tbTransporte transporte)
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    var cmd = new MySqlCommand("UPDATE tbTransporte SET CodigoTransporte=@codigo, Companhia=@companhia, TipoTransporte=@tipo WHERE IdTransporte=@id", conexao);
                    cmd.Parameters.AddWithValue("@codigo", transporte.CodigoTransporte);
                    cmd.Parameters.AddWithValue("@companhia", transporte.Companhia);
                    cmd.Parameters.AddWithValue("@tipo", transporte.TipoTransporte);
                    cmd.Parameters.AddWithValue("@id", transporte.IdTransporte);
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            public bool ExcluirTransporte(int id)
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    var cmd = new MySqlCommand("DELETE FROM tbTransporte WHERE IdTransporte=@id", conexao);
                    cmd.Parameters.AddWithValue("@id", id);
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            public tbTransporte ObterTransporte(int id)
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    var cmd = new MySqlCommand("SELECT * FROM tbTransporte WHERE IdTransporte=@id", conexao);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new tbTransporte
                            {
                                IdTransporte = Convert.ToInt32(reader["IdTransporte"]),
                                CodigoTransporte = reader["CodigoTransporte"].ToString(),
                                Companhia = reader["Companhia"].ToString(),
                                TipoTransporte = reader["TipoTransporte"].ToString()
                            };
                        }
                    }
                }
                return null;
            }

            public List<tbTransporte> ListarTransportes()
            {
                var lista = new List<tbTransporte>();
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    var cmd = new MySqlCommand("SELECT * FROM tbTransporte", conexao);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new tbTransporte
                            {
                                IdTransporte = Convert.ToInt32(reader["IdTransporte"]),
                                CodigoTransporte = reader["CodigoTransporte"].ToString(),
                                Companhia = reader["Companhia"].ToString(),
                                TipoTransporte = reader["TipoTransporte"].ToString()
                            });
                        }
                    }
                }
                return lista;
            }
        }

    }
