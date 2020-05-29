using Core.Commands;
using Core.UnitOfWork;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Features;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.GeoJsonCommands.Salvar
{
    public sealed class SalvarGeoJsonCommandHandler<TDbContext> : CommandHandler<SalvarGeoJsonCommand, int>
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
                .FeatureCollection
                .CleanerFeatureCollection()
                .Select(s => FeatureToGeoJson(s));

            await repository.InsertAsync(geoJsons);

            await _unitOfWork.SaveChangesAsync();

            return geoJsons.Count();
        }

        private static GeoJson FeatureToGeoJson(IFeature feature)
        {
            var keyValuePairs = AttributesTableToDictionary(feature.Attributes).ToImmutableDictionary();

            var json = JsonConvert.SerializeObject(keyValuePairs);

            return new GeoJson 
            {
                Geometry = feature.Geometry, 
                Properties = json
            };
        }

        private static IEnumerable<KeyValuePair<string, object>> AttributesTableToDictionary(IAttributesTable attributesTable)
        {
            foreach (var attributeName in attributesTable.GetNames())
            {
                var attributeValue = attributesTable[attributeName];

                if (attributeName.ToLower() == "id") continue;
                if (attributeValue == null) continue;
                if (attributeValue is IAttributesTable) continue;

                attributeValue = attributeValue is string valuestr ? valuestr.Trim() : attributeValue;

                yield return new KeyValuePair<string, object>(attributeName, attributeValue);
            }
        }
    }
}
