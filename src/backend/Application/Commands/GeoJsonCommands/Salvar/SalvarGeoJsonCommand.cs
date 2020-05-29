using Core.Commands;
using FluentValidation.Results;
using NetTopologySuite.Features;
using System.Threading.Tasks;

namespace Application.Commands.GeoJsonCommands.Salvar
{
    public sealed class SalvarGeoJsonCommand : GeoJsonCommand, ICommand<int>
    {
        public SalvarGeoJsonCommand(FeatureCollection featuresCollection)
        {
            FeatureCollection = featuresCollection;
        }

        public ValidationResult ValidationResult { get; set; }

        public async Task<bool> IsValid()
        {
            return true;
        }
    }
}
