﻿namespace ProjetoEcommerce.Models
{
    public class tbPacoteComPassagemProduto
    {
        public int IdPacote {  get; set; }
        public string NomeProduto { get; set; }
        public string NomePacote { get; set; }
        public int Quantidade { get; set; }
        public string Assento { get; set; }
        public string Situacao { get; set; }
        public string Translado { get; set; }
        public string CodigoTransporte { get; set; }
        public decimal Valor {  get; set; }
        public string Companhia { get; set; }
        public string TipoTransporte { get; set; }
    }
}
