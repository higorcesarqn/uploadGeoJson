using Core.Models;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Geojson : Entity
    {
        public Geojson()
        {
            Geometrias = new List<Geometria>();
        }

        public string FileName { get; set; }
        public long Size { get; set; }
        public List<Geometria> Geometrias { get; set; }

        public void AddGeometrias(List<Geometria> geometrias) => Geometrias.AddRange(geometrias);
    }
}
