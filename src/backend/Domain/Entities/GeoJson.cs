using Core.Models;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Geojson : Entity
    {
        public Geojson()
        {
            Geometrias = new List<Empreendimento>();
        }

        public string FileName { get; set; }
        public long Size { get; set; }
        public List<Empreendimento> Geometrias { get; set; }

        public void AddGeometrias(List<Empreendimento> geometrias) => Geometrias.AddRange(geometrias);
    }
}
