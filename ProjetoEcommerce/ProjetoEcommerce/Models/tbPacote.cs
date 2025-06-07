namespace ProjetoEcommerce.Models
{
    public class tbPacote
    {
        public int IdPacote { get; set; }
        public tbProduto IdProduto { get; set; }
        public tbPassagem IdPassagem { get; set; }
        public string NomePacote { get; set; }
        public string Descricao { get; set; }
        public decimal Valor {  get; set; }
    }
}
