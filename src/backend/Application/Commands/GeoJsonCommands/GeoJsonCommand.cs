using NetTopologySuite.Features;

namespace Application.Commands.GeoJsonCommands
{
    public class GeoJsonCommand 
    {
        public FeatureCollection FeatureCollection { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
    }
}
