namespace ProjetoEcommerce.Models
{
    public class tbProduto
    {
        public int IdProduto { get; set; }
        public string NomeProduto {  get; set; }
        public decimal? Valor { get; set; }
        public string Descricao { get; set; }
        public int Quantidade {  get; set; }
    }
}
