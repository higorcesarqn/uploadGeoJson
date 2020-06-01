using Core.Commands;
using Domain.Entities;
using FluentValidation.Results;
using NetTopologySuite.Features;
using System.Threading.Tasks;

namespace Application.Commands.GeoJsonCommands.Salvar
{
    public sealed class SalvarGeoJsonCommand : GeoJsonCommand, ICommand<Geojson>
    {
        public SalvarGeoJsonCommand(string fileName, long size, FeatureCollection featuresCollection)
        {
            FileName = fileName;
            Size = size;
            FeatureCollection = featuresCollection;
        }

        public ValidationResult ValidationResult { get; set; }

        public async Task<bool> IsValid()
        {
            return true;
        }
    }
}
