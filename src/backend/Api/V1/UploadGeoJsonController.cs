using Api.Controllers;
using Api.V1.Models;
using Core.Notifications;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Api.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UploadGeoJsonController : ApiController
    {
        private readonly Notifiable _notifiable;

        public UploadGeoJsonController(Notifiable notifiable ,INotifications notifications) : base(notifications)
        {
            _notifiable = notifiable;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (file == null)
            {
                await _notifiable.Notify("file","Arquivo Inválido.");
                return ResponseBadRequest();
            }

            var fileInfo = new FileInfo(file.FileName);

            if(fileInfo.Extension != ".geojson")
            {
                await _notifiable.Notify("file", "O formato do arquivo esta inválido. [Formato Aceito: '.geojson']");
                return ResponseBadRequest();
            }

            var size = file.Length;

            if (file.Length > 0)
            {
                using StreamReader sr = new StreamReader(file.OpenReadStream());
                string geoJson = await sr.ReadToEndAsync();
                var reader = new GeoJsonReader();
                var featureCollection = reader.Read<FeatureCollection>(geoJson);

                var geo = featureCollection.ToEntityGeo<Teste>().ToArray();

                return Response(new UploadGeoJsonModel(
                
                    file.FileName,
                    size,
                    geo.Length,
                    MD5Hash(geoJson)
                ));
            }

            return ResponseBadRequest();
        }

        public static string MD5Hash(string input)
        {
            using var md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

    }
}
