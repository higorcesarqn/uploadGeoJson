using Core.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Behaviors
{
    public abstract class CommandPipelineBehaviorBase<TCommand, TResponse>
        : IPipelineBehavior<TCommand, TResponse>
        where TCommand : ICommandBase
    {
        protected CommandPipelineBehaviorBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected ILogger Logger { get; }

        public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                Logger.LogTrace("Processing Command pipeline request '{request}' ...", request);
                var watch = Stopwatch.StartNew();

                var response = await Process(request, cancellationToken, next).ConfigureAwait(false);

                watch.Stop();
                Logger.LogTrace("Processed Command pipeline request '{request}': {elapsed} ms", request, watch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error handling pipeline request '{request}': {errorMessage}", request, ex.Message);
                throw;
            }
        }

        protected abstract Task<TResponse> Process(TCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next);

    }
}
