using Autofac;
using Core.Behaviors;
using Core.Bus;
using Core.Commands;
using Core.Notifications;
using Core.UnitOfWork;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Unit = Core.Tango.Types.Unit;

namespace Core
{
    public static class AutofacExtensions
    {
        private static ContainerBuilder Register<TImplementer>(ContainerBuilder container)
        {
            var typeImplementer = typeof(TImplementer);

            container.RegisterType(typeImplementer)
               .AsImplementedInterfaces()
               .IfNotRegistered(typeImplementer)
               .InstancePerLifetimeScope();

            return container;
        }

        public static ContainerBuilder ConfigureCore(this ContainerBuilder container, Assembly assembly)
        {
            Register<DomainNotificationHandler>(container);

            Register<MediatorHandler>(container);

            container.RegisterType(typeof(Notifiable))
                .AsSelf()
                .InstancePerLifetimeScope();

            container.AddMediatR(assembly);

            return container;
        }

        /// <summary>
        /// Adiciona um Novo Comando no IoC.
        /// Todo Commando Adicionado é adicionado para o Commando um CommandValidatorBehavior no pipeline
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TComandHandler"></typeparam>
        /// <param name="container"></param>
        /// <returns></returns>
        public static ContainerBuilder AddCommand<TCommand, TComandHandler>(this ContainerBuilder container)
             where TCommand : ICommand
             where TComandHandler : CommandHandler<TCommand>
        {
            Register<TComandHandler>(container);

            AddCommandPipelineBehavior<CommandValidatorBehavior<TCommand, Unit>, TCommand, Unit>(container);

            return container;
        }

        /// <summary>
        /// Adiciona um Novo Comando no IoC.
        /// Todo Commando Adicionado é adicionado para o Commando um CommandValidatorBehavior no pipeline
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TComandHandler"></typeparam>
        /// <param name="container"></param>
        /// <returns></returns>
        public static ContainerBuilder AddCommand<TCommand, TResponse, TComandHandler>(this ContainerBuilder container)
            where TCommand : ICommand<TResponse>
            where TComandHandler : CommandHandler<TCommand, TResponse>
        {
            Register<TComandHandler>(container);

            AddCommandPipelineBehavior<CommandValidatorBehavior<TCommand, TResponse>, TCommand, TResponse>(container);

            return container;
        }

        /// <summary>
        /// Adiciona um Novo CommandPipelineBehaviorBase no IoC.
        /// </summary>
        /// <typeparam name="TBehavior"></typeparam>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="container"></param>
        /// <returns></returns>
        public static ContainerBuilder AddCommandPipelineBehavior<TBehavior, TCommand, TResponse>(this ContainerBuilder container)
            where TBehavior : CommandPipelineBehaviorBase<TCommand, TResponse>
            where TCommand : ICommandBase
        {
            Register<TBehavior>(container);

            return container;
        }


        public static ContainerBuilder AddUnitOfWork<TContext>(this ContainerBuilder container)
            where TContext : DbContext
        {
            Register<UnitOfWork<TContext>>(container);

            return container;
        }
    }
}
