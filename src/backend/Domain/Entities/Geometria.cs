using Core.Models;
using NetTopologySuite.Features;

namespace Domain.Entities
{
    public class Geometria : EntityGeo
    {
        public Geometria(IAttributesTable attributesTable)
        {

            var properties = GetType().GetProperties();

            foreach (var propertie in properties)
            {
                if(attributesTable.Exists(propertie.Name))
                {
                    var value = attributesTable[propertie.Name];
                    propertie.SetValue(this, value);
                }
            }
        }

        public Geometria()
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
