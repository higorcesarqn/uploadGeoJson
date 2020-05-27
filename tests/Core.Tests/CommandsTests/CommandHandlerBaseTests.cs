using Core.Commands;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Threading;
using Xunit;
using Unit = Core.Tango.Types.Unit;

namespace Core.Tests.CommandsTests
{
    public class CommandHandlerBaseTests
    {
        [Fact]
        [Trait("Core", "Commands")]
        public async void commandHandler_base_executa_process()
        {
            var loggerFactory = new LoggerFactory();

            var commandHandleBaseMock = new Mock<CommandHandlerBase<ICommand, Unit>>(loggerFactory);

            await commandHandleBaseMock.Object.Handle(It.IsAny<ICommand>(), It.IsAny<CancellationToken>());

            commandHandleBaseMock.Protected().Verify("Process", Times.Once(), ItExpr.IsNull<ICommand>(), ItExpr.IsNull<CancellationToken>());
        }

        [Fact]
        [Trait("Core", "Commands")]
        public async void commandHandler_executa_process()
        {
            var loggerFactory = new LoggerFactory();

            var commandHandleBaseMock = new Mock<CommandHandler<ICommand>>(loggerFactory);

            await commandHandleBaseMock.Object.Handle(It.IsAny<ICommand>(), It.IsAny<CancellationToken>());

            commandHandleBaseMock.Protected().Verify("Process", Times.Once(), ItExpr.IsNull<ICommand>(), ItExpr.IsNull<CancellationToken>());
        }

        [Fact]
        [Trait("Core", "Commands")]
        public async void commandHandler_response_executa_process()
        {
            var loggerFactory = new LoggerFactory();

            var commandHandleBaseMock = new Mock<CommandHandler<ICommand<Unit>, Unit>>(loggerFactory);

            await commandHandleBaseMock.Object.Handle(It.IsAny<ICommand<Unit>>(), It.IsAny<CancellationToken>());

            commandHandleBaseMock.Protected().Verify("Process", Times.Once(), ItExpr.IsNull<ICommand<Unit>>(), ItExpr.IsNull<CancellationToken>());
        }
    }
}
