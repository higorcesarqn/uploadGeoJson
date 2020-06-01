using Application.Commands.GeoJsonCommands.Salvar;
using Autofac;
using Core;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder AddApplication<TDbContext>(this ContainerBuilder container)
            where TDbContext : DbContext
        {
            container.AddCommand<SalvarGeoJsonCommand, Geojson, SalvarGeoJsonCommandHandler<TDbContext>>();


            return container;
        }
    }
}
