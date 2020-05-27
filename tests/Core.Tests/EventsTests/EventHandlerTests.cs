using Core.Events;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Threading;
using Xunit;

namespace Core.Tests.EventsTests
{
    public class EventHandlerTests
    {
        [Fact]
        [Trait("Core", "EventHandler")]
        public async void eventHandler_executa_process()
        {
            var loggerFactory = new LoggerFactory();

            var commandHandleBaseMock = new Mock<EventHandler<Event>>(loggerFactory);

            await commandHandleBaseMock.Object.Handle(It.IsAny<Event>(), It.IsAny<CancellationToken>());

            commandHandleBaseMock.Protected().Verify("Process", Times.Once(), ItExpr.IsNull<Event>(), ItExpr.IsNull<CancellationToken>());
        }
    }
}
