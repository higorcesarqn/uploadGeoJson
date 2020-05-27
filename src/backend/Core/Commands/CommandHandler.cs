using MediatR;
using Microsoft.Extensions.Logging;
using Unit = Core.Tango.Types.Unit;

namespace Core.Commands
{
    public abstract class CommandHandler<TCommand> : CommandHandlerBase<TCommand, Unit>, IRequestHandler<TCommand, Unit>
        where TCommand : ICommand
    {
        protected CommandHandler(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }

    public abstract class CommandHandler<TCommand, TResponse> : CommandHandlerBase<TCommand, TResponse>, IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        protected CommandHandler(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}
