namespace ProjetoEcommerce.Models
{
    public class tbViagem
    {
        public int? IdViagem { get; set; }
        public DateTime? DataRetorno { get; set; } 
        public string? Descricao { get; set;}
        public string? Origem { get; set; }
        public string? Destino { get; set; }
        public DateTime ?DataPartida { get; set; }
    }
}
