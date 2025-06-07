namespace ProjetoEcommerce.Models
{
    public class tbClienteComPassagem
    {
        public int IdPassagem { get; set; }
        public string Assento { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCompra { get; set; }
        public int IdViagem { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Situacao { get; set; }
        public string Translado { get; set; }
    }
}
