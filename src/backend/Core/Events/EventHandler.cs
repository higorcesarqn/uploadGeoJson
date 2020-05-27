using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Events
{
    public abstract class EventHandler<TEvent> : INotificationHandler<TEvent>
        where TEvent : Event
    {
        protected ILogger Logger { get; }

        protected EventHandler(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }


        public async Task Handle(TEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogTrace($"Processing Event '{ @event}' ...");
                var watch = Stopwatch.StartNew();

                await Process(@event, cancellationToken).ConfigureAwait(false);

                watch.Stop();
                Logger.LogTrace($"Processed Event '{@event}': {watch.ElapsedMilliseconds} ms");

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error processing Event '{@event}': {ex.Message}");
                throw ex;
            }
        }

        protected abstract Task Process(TEvent @event, CancellationToken cancellationToken);
    }
}
