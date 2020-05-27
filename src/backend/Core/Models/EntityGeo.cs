using Humanizer;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using System.Linq;

namespace Core.Models
{
    public abstract class EntityGeo : Entity
    {
        public Geometry Geometry { get; set; }

        public void EditarGeom(Geometry geometry)
        {
            Geometry = geometry;
        }

        public static implicit operator Feature(EntityGeo entity)
        {
            var type = entity.GetType();

            var attributes = type
                .GetProperties()
                .Where(x => x.PropertyType != typeof(Geometry) && (x.Name != "CreatedAt" && x.Name != "UpdatedAt"))
                .ToDictionary(key => key.Name.Camelize(), value => value.GetValue(entity));

            return new Feature(entity.Geometry, new AttributesTable(attributes))
            {
                BoundingBox = entity.Geometry.Envelope.EnvelopeInternal
            };
        }
    }
}
