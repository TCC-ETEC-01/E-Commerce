using ProjetoEcommerce.Models;

namespace ProjetoEcommerce.Repositorios
{
    public class PacoteComPassagemProdutoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public IEnumerable<tbPacoteComPassagemProduto> PacoteComPassagemProduto()
        {

        }
    }
}
