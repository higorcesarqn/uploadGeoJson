using Core.Commands;
using Core.UnitOfWork;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Features;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.GeoJsonCommands.Salvar
{
    public class SalvarGeoJsonCommandHandler<TDbContext> : CommandHandler<SalvarGeoJsonCommand, int>
        where TDbContext : DbContext
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalvarGeoJsonCommandHandler(IUnitOfWork<TDbContext> unitOfWork, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _unitOfWork = unitOfWork;
        }

        protected override async Task<int> Process(SalvarGeoJsonCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<GeoJson>();

            var geoJsons = request
                .FeatureCollection.
                CleanerFeatureCollection().Select(s => FeatureToGeoJson(s)).ToArray();

            await repository.InsertAsync(geoJsons);

            await _unitOfWork.SaveChangesAsync();

            return geoJsons.Count();
        }

        public static GeoJson FeatureToGeoJson(IFeature feature)
        {
            var keyValuePairs = new Dictionary<string, object>();

            foreach (var attributeName in feature.Attributes.GetNames())
            {
                SetValue(keyValuePairs, attributeName, feature.Attributes[attributeName]);
            }

            return new GeoJson { Geometry = feature.Geometry, Properties = JsonConvert.SerializeObject(keyValuePairs) };

            static void SetValue(IDictionary<string, object> keyValuePairs, string key, object value)
            {

                if (value is string valuestr)
                {
                    keyValuePairs[key.ToLower()] = valuestr.ToLower();
                }
                else
                {
                    keyValuePairs[key.ToLower()] = value;
                }
            }
        }

    }
}
