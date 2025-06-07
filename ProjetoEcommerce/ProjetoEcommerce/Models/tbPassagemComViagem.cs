namespace ProjetoEcommerce.Models
{
    public class tbPassagemComViagem
    {
        public int IdPassagem { get; set; }
        public DateTime? DataRetorno { get; set; }
        public string? Descricao { get; set; }
        public string? Origem { get; set; }
        public string? Destino { get; set; }
        public string? TipoTransporte { get; set; }
        public DateTime? DataPartida { get; set; }
        public string Assento { get; set; }
        public string Translado { get; set; }
    }
}
