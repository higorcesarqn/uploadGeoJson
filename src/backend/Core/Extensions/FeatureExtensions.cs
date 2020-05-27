using Core.Models;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Linq;

namespace NetTopologySuite.Features
{
    public static class FeatureExtensions
    {
        public static TEntityGeo ToEntityGeo<TEntityGeo>(this Feature feature)
            where TEntityGeo : EntityGeo, new()
        {
            return GeoJsonModelToEntityGeo<TEntityGeo>(feature);
        }

        private static TEntityGeo GeoJsonModelToEntityGeo<TEntityGeo>(GeoJsonModel geoJsonModel)
             where TEntityGeo : EntityGeo, new()
        {
            var entityGeo = new TEntityGeo() { Geometry = geoJsonModel.Geometry };

            var propertiesEntity = typeof(Entity).GetProperties().Select(x => x.Name);

            foreach (var property in entityGeo.GetType().GetProperties())
            {   
                if(propertiesEntity.Contains(property.Name))
                {
                    continue;
                }

                var value = geoJsonModel[property.Name.ToLower()];
                if (value != null)
                {
                    property.SetValue(entityGeo, value);
                }
                
            }

            return entityGeo;
        }
    }


    class GeoJsonModel
    {
        private readonly IDictionary<string, object> _properties = new Dictionary<string, object>();

        public object this[string name]
        {
            get
            {
                return GetValue(name);
            }
            set
            {
                SetValue(name, value);
            }
        }

        private object GetValue(string name) => _properties.ContainsKey(name.ToLower()) ? _properties[name] : default;

        private void SetValue(string name, object value)
        {
            if(value is string valuestr)
            {
                _properties[name.ToLower()] = valuestr.ToLower();
            }
            else
            {
                _properties[name.ToLower()] = value;
            }
           
        }
        

        public Geometry Geometry { get; set; }

        public static implicit operator GeoJsonModel(Feature feature)
        {
            var geometria = new GeoJsonModel
            {
                Geometry = feature.Geometry
            };

            foreach (var attributeName in feature.Attributes.GetNames())
            {
                geometria[attributeName] = feature.Attributes[attributeName];
            }

            return geometria;
        }
    }

}
