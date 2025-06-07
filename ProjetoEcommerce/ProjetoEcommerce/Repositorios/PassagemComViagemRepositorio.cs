using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;

namespace ProjetoEcommerce.Repositorios
{
    public class PassagemComViagemRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public IEnumerable<tbPassagemComViagem> PassagemComViagem()
        {
            var lista = new List<tbPassagemComViagem>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                string query = @"select  p.IdPassagem,v.Origem, v.Destino, p.Assento,v.Descricao,  
                    v.DataPartida, v.DataRetorno as Data_Retorno,  v.TipoTransporte
                    from tbPassagem p 
                    inner join tbViagem v on p.IdViagem = v.IdViagem;";

            }
        }
    }
}
