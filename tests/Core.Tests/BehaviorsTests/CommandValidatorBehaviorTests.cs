using Core.Behaviors;
using Core.Bus;
using Core.Commands;
using Core.Notifications;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Unit = Core.Tango.Types.Unit;

namespace Core.Tests.BehaviorsTests
{
    public class CommandValidatorBehaviorTests
    {
        [Fact]
        [Trait("Core", "Behaviors")]
        public void um_comando_invalido_nao_e_executado()
        {
            var loggerFactory = new LoggerFactory();

            //mocks
            var commandMock = new Mock<ICommand>();
            var notifiableMock = new Mock<Notifiable>(It.IsAny<IMediatorHandler>(), It.IsAny<INotifications>());

            //setups
            //Set o command como invalido
            commandMock.Setup(m => m.IsValid()).ReturnsAsync(false);
            commandMock.Setup(x => x.ValidationResult).Returns(new ValidationResult());

            var executou = false;
            Task<Unit> Next()
            {
                executou = true;
                return Unit.Task;
            }

            ////criando comando
            var commandValidatorBehaviorMock = new CommandValidatorBehavior<ICommand, Unit>(
                    loggerFactory, notifiableMock.Object);

            var processMethod = commandValidatorBehaviorMock.GetType().GetMethod("Process", BindingFlags.NonPublic | BindingFlags.Instance);

            _ = processMethod.Invoke(commandValidatorBehaviorMock,
                new object[] { commandMock.Object, It.IsAny<CancellationToken>(), new RequestHandlerDelegate<Unit>(Next) });

            //verificações
            commandMock.Verify(x => x.IsValid(), Times.Once);
           // notifiableMock.Verify(x => x.NotifyValidationErrors(It.IsAny<ValidationResult>()), Times.Once);

            //asserts
            Assert.False(executou);
        }

        [Fact]
        [Trait("Core", "Behaviors")]
        public void um_comando_valido_e_executado()
        {
            var loggerFactory = new LoggerFactory();

            //mocks
            var commandMock = new Mock<ICommand>();
            var notifiableMock = new Mock<Notifiable>(It.IsAny<IMediatorHandler>(), It.IsAny<INotifications>());

            //setups
            //Set o command como valido
            commandMock.Setup(m => m.IsValid()).ReturnsAsync(true);
            commandMock.Setup(x => x.ValidationResult).Returns(new ValidationResult());

            var executou = false;
            Task<Unit> Next()
            {
                executou = true;
                return Unit.Task;
            }

            //criando comando
            var commandValidatorBehaviorMock = new CommandValidatorBehavior<ICommand, Unit>(
                    loggerFactory, notifiableMock.Object);

            var processMethod = commandValidatorBehaviorMock.GetType().GetMethod("Process", BindingFlags.NonPublic | BindingFlags.Instance);

            _ = processMethod.Invoke(commandValidatorBehaviorMock,
                new object[] { commandMock.Object, It.IsAny<CancellationToken>(), new RequestHandlerDelegate<Unit>(Next) });

            //verificações
            commandMock.Verify(x => x.IsValid(), Times.Once);
            //notifiableMock.Verify(x => x.NotifyValidationErrors(It.IsAny<ValidationResult>()), Times.Never);

            //asserts
            Assert.True(executou);
        }
    }
}
