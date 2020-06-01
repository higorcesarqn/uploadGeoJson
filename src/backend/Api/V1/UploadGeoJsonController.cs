using Api.Controllers;
using Api.V1.Models;
using Application.Commands.GeoJsonCommands.Salvar;
using Core.Bus;
using Core.Notifications;
using Core.UnitOfWork;
using Domain.Entities;
using Egl.Sit.Api.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        private readonly IMediatorHandler _mediatorHandler;
        private readonly Notifiable _notifiable;

        public UploadGeoJsonController(IMediatorHandler mediatorHandler, Notifiable notifiable, INotifications notifications) : base(notifications)
        {
            _mediatorHandler = mediatorHandler;
            _notifiable = notifiable;
        }


        /// <summary>
        /// Salva as featres de um geojson.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(UploadGeoJsonModel), 201)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        [ProducesResponseType(typeof(JsonErrorResponse), 500)]
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (file == null)
            {
                await _notifiable.Notify("file", "Arquivo Inválido.");
                return ResponseBadRequest();
            }

            var fileInfo = new FileInfo(file.FileName);

            if (fileInfo.Extension != ".geojson")
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

                var commandReponse = await _mediatorHandler.SendCommand(new SalvarGeoJsonCommand(file.FileName, size, featureCollection));

                return Response(new UploadGeoJsonModel(commandReponse.Id,
                    file.FileName,
                    size,
                    commandReponse.Geometrias.Count(),
                    MD5Hash(geoJson)
                ));
            }

            return ResponseBadRequest();
        }

        [HttpGet("{idGeojson}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> Get([FromServices] IUnitOfWork unitOfWork, [FromRoute] Guid idGeojson, [FromRoute] int pageIndex = 0, [FromRoute] int pageSize = 10)
        {
            var repositorio = unitOfWork.GetRepository<Geojson>();

            var result = await repositorio
                .GetPagedListAsync(
                   predicate: p => p.Id == idGeojson,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    selector: geojson => geojson.Geometrias
                    .Select(s => new { id = s.Id, properties = JObject.Parse(s.Properties) }));

            return Response(result);
        }

        [HttpGet("listar/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> Listar([FromServices] IUnitOfWork unitOfWork, [FromRoute] int pageIndex = 0, [FromRoute] int pageSize = 10)
        {
            var repositorio = unitOfWork.GetRepository<Geojson>();

            var result = await repositorio
                .GetPagedListAsync(
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    selector: geojson => new {geojson.Id ,geojson.FileName, geojson.Size });

            return Response(result);
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
