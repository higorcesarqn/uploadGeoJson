using Core.Models;

namespace Domain.Entities
{
    public class Teste : EntityGeo
    {
        public string Empreendimento { get; set; }
        public string Lote { get; set; }
        public string NumeroCadastro { get; set; }
        public string SituacaoProcesso { get; set; }
        public double KmInicial { get; set; }
        public double KmFinal { get; set; }
    }
}
