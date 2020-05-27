using Core.Behaviors;
using Core.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Threading;
using Xunit;
using Unit = Core.Tango.Types.Unit;

namespace Core.Tests.BehaviorsTests
{
    public class CommandPipelineBehaviorBaseTests
    {
        [Fact]
        [Trait("Core", "Behaviors")]
        public async void command_pipeline_executa_process()
        {
            var loggerFactory = new LoggerFactory();

            var commandPipelineBehaviorBaseMock = new Mock<CommandPipelineBehaviorBase<ICommand, Unit>>(loggerFactory);

            await commandPipelineBehaviorBaseMock.Object.Handle(It.IsAny<ICommand>(), It.IsAny<CancellationToken>(),
                It.IsAny<RequestHandlerDelegate<Unit>>());

            commandPipelineBehaviorBaseMock.Protected().Verify("Process", Times.Once(), ItExpr.IsNull<ICommand>(), ItExpr.IsNull<CancellationToken>(),
                ItExpr.IsNull<RequestHandlerDelegate<Unit>>());
        }
    }
}
