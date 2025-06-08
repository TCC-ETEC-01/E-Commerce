namespace ProjetoEcommerce.Models
{
    public class tbVenda
    {
        public tbPassagem IdPassagem { get; set; }
        public tbCliente IdCliente { get; set; }
        public tbProduto IdProduto { get; set; }
        public int IdVenda { get; set; }
        public decimal Valor { get; set; }
        public tbFuncionario IdFuncionario { get; set; }
    }
}
