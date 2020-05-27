using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Commands
{
    public abstract class CommandHandlerBase<TCommand, TResponse>
        where TCommand : ICommandBase
    {
        protected ILogger Logger { get; }

        protected CommandHandlerBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogTrace($"Processing Command '{command}' ...");
                var watch = Stopwatch.StartNew();

                var response = await Process(command, cancellationToken).ConfigureAwait(false);

                watch.Stop();
                Logger.LogTrace($"Processed Command '{command}': {watch.ElapsedMilliseconds} ms");

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error processing Command '{command}': {ex.Message}");
                throw ex;
            }
        }

        protected abstract Task<TResponse> Process(TCommand request, CancellationToken cancellationToken);
    }
}
