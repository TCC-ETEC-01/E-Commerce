namespace ProjetoEcommerce.Models
{
    public class PacoteComPassagemProduto
    {
        public int IdPacote {  get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public string Assento { get; set; }
        public string Situacao { get; set; }
        public string Translado { get; set; }
    }
}
