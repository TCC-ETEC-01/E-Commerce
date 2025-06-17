namespace ProjetoEcommerce.Models
{
    public class tbVenda
    {
        public tbPassagem IdPassagem { get; set; }
        public tbCliente IdCliente { get; set; }
        public int IdVenda { get; set; }
        public decimal Valor { get; set; }
        public tbFuncionario IdFuncionario { get; set; }
        public string FormaPagamento { get; set; }
        public DateTime DataVenda { get; set; }
    }
}
