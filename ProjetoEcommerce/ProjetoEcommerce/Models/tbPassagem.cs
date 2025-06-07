namespace ProjetoEcommerce.Models
{
    public class tbPassagem
    {
        public int IdPassagem { get; set; }
        public string Assento { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCompra {  get; set; }
        public int IdViagem { get; set; }
        public int IdCliente { get; set; }
        public string Situacao { get; set; }
        public string Translado { get; set;
    }
}
