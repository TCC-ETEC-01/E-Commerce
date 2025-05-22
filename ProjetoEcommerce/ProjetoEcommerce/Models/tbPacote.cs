namespace ProjetoEcommerce.Models
{
    public class tbPacote
    {
        public int IdPacote { get; set; }
        public int IdProduto { get; set; }
        public int IdPassagem { get; set; }
        public string NomePacote { get; set; }
        public string Descricao { get; set; }
        public decimal Valor {  get; set; }
    }
}
