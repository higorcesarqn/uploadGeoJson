using Autofac;
using Core.Bus;
using Core.Commands;
using Core.Notifications;
using FluentValidation;
using FluentValidation.Results;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Core.Tests.NotificationsTests
{
    public class NotifiableTests
    {
        private class CommandTest : ICommandBase
        {
            public CommandTest(int value)
            {
                Value = value;
            }

            public int Value { get; protected set; }

            public ValidationResult ValidationResult { get; set; }

            public async Task<bool> IsValid()
            {
                ValidationResult = await new Validator().ValidateAsync(this).ConfigureAwait(false);
                return ValidationResult.IsValid;
            }

            public class Validator : AbstractValidator<CommandTest>
            {
                public Validator()
                {
                    RuleFor(x => x.Value).GreaterThan(10).WithMessage("ERRO");
                }
            }
        }

        [Fact]
        [Trait("Core", "Notifications")]
        public async void valida_e_notifica_erros_de_um_comando_invalido()
        {
            var container = new ContainerBuilder();
            container.AddMediatR(typeof(NotifiableTests).Assembly);

            container.RegisterInstance(new LoggerFactory()).AsImplementedInterfaces();
            container.RegisterType<DomainNotificationHandler>().InstancePerLifetimeScope().AsImplementedInterfaces();
            container.RegisterType<MediatorHandler>().InstancePerLifetimeScope().AsImplementedInterfaces();
            container.RegisterType<Notifiable>().InstancePerLifetimeScope().AsSelf();

            var scope = container.Build().BeginLifetimeScope();

            var notifiable = scope.Resolve<Notifiable>();

            await notifiable.ValidateAndNotifyValidationErrors(new CommandTest(9));

            var notifications = scope.Resolve<INotifications>();

            Assert.False(notifiable.IsValid());
            Assert.True(notifications.HasNotifications());

            var notificacao = notifications.GetNotifications().FirstOrDefault();
            Assert.Equal("Value", notificacao.Key);
            Assert.Equal("ERRO", notificacao.Value);

        }

        [Fact]
        [Trait("Core", "Notifications")]
        public async void um_comando_valido_nao_notifica()
        {
            var container = new ContainerBuilder();
            container.AddMediatR(typeof(NotifiableTests).Assembly);

            container.RegisterInstance(new LoggerFactory()).AsImplementedInterfaces();
            container.RegisterType<DomainNotificationHandler>().InstancePerLifetimeScope().AsImplementedInterfaces();
            container.RegisterType<MediatorHandler>().InstancePerLifetimeScope().AsImplementedInterfaces();
            container.RegisterType<Notifiable>().InstancePerLifetimeScope().AsSelf();

            var scope = container.Build().BeginLifetimeScope();

            var notifiable = scope.Resolve<Notifiable>();

            await notifiable.ValidateAndNotifyValidationErrors(new CommandTest(11));

            var notifications = scope.Resolve<INotifications>();

            Assert.True(notifiable.IsValid());
            Assert.False(notifications.HasNotifications());
        }

        [Fact]
        [Trait("Core", "Notifications")]
        public async void valida_e_notifica_erros_de_um_comando_invalido_mock()
        {
            //mocks
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var notificationsMock = new Mock<INotifications>();
            var commandMock = new Mock<ICommandBase>();

            //setups
            var erros = new List<ValidationFailure>() { new ValidationFailure("erro", "erro") };
            commandMock.Setup(x => x.ValidationResult).Returns(new ValidationResult(erros));

            var notifiable = new Notifiable(mediatorHandlerMock.Object, notificationsMock.Object);

            await notifiable.ValidateAndNotifyValidationErrors(commandMock.Object);
            _ = notifiable.IsValid();

            mediatorHandlerMock.Verify(x => x.RaiseEvent(It.IsAny<DomainNotification>(), It.IsAny<CancellationToken>()), Times.Once);
            notificationsMock.Verify(x => x.HasNotifications(), Times.Once);
        }

        [Fact]
        [Trait("Core", "Notifications")]
        public async void um_comando_valido_nao_notifica_mock()
        {
            //mocks
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var notificationsMock = new Mock<INotifications>();
            var commandMock = new Mock<ICommandBase>();

            //setups
            commandMock.Setup(x => x.ValidationResult).Returns(new ValidationResult());

            var notifiable = new Notifiable(mediatorHandlerMock.Object, notificationsMock.Object);

            await notifiable.ValidateAndNotifyValidationErrors(commandMock.Object);
            _ = notifiable.IsValid();

            mediatorHandlerMock.Verify(x => x.RaiseEvent(It.IsAny<DomainNotification>(), It.IsAny<CancellationToken>()), Times.Never);
            notificationsMock.Verify(x => x.HasNotifications(), Times.Once);
        }
    }
}
