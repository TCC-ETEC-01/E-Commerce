namespace ProjetoEcommerce.Models
{
    public class tbPassagem
    {
        public int IdPassagem { get; set; }
        public string Assento { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCompra { get; set; }
        public tbViagem IdViagem { get; set; }
        public tbCliente IdCliente { get; set; }
        public string Situacao { get; set; }
        public tbTransporte IdTransporte { get; set; }
        public string Translado{get; set;}
    }
}
