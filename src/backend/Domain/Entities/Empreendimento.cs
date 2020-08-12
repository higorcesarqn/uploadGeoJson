using Core.Models;
using NetTopologySuite.Features;

namespace Domain.Entities
{
    public class Empreendimento : EntityGeo
    {
        public Empreendimento(IFeature feature)
        {

            var attributesTable = feature.Attributes;

            var properties = GetType().GetProperties();

            foreach (var propertie in properties)
            {
                if(attributesTable.Exists(propertie.Name))
                {
                    var value = attributesTable[propertie.Name];
                    propertie.SetValue(this, value.ToString().Trim());
                }
            }

            Geometry = feature.Geometry;
        }

        public Empreendimento()
        {

        }

        public string Empreedimento { get; set; }
        public string Lote { get; set; }
        public string NumeroCadastro { get; set; }
        public string Area { get; set; }
        public string AreaDesapropriar { get; set; }
        public string NumeroProcesso { get; set; }
        public string Localizacao { get; set; }
    }
}
