using Core.Models;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Linq;

namespace NetTopologySuite.Features
{
    public static class FeatureCollectionExtensions
    {
        public static IEnumerable<TEntityGeo> ToEntityGeo<TEntityGeo>(this FeatureCollection featureCollection)
            where TEntityGeo : EntityGeo, new()
        {
            foreach (Feature feature in featureCollection.CleanerFeatureCollection())
            {
                yield return feature.ToEntityGeo<TEntityGeo>();
            }
        }

        public static FeatureCollection CleanerFeatureCollection(this FeatureCollection featureCollection)
        {
            static void Add(FeatureCollection featureCollection, IFeature feature)
            {
                if (feature.Geometry.GeometryType == "MultiPoint")
                {
                    var multiPoint = (MultiPoint)feature.Geometry;
                    foreach (var point in multiPoint.Geometries)
                    {
                        var featurePoint = new Feature(point, feature.Attributes);
                        featureCollection.Add(featurePoint);
                    }
                    return;
                }

                featureCollection.Add(feature);
            }

            var features = new FeatureCollection();
            featureCollection.Where(x => x.Geometry != null)
                .ToList()
                .ForEach(x => Add(features, x));

            return features;
        }
    }
}
