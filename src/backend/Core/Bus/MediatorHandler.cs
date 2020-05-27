using Core.Commands;
using Core.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Unit = Core.Tango.Types.Unit;

namespace Core.Bus
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishMessage<T>(T message, CancellationToken cancellationToken = default) where T : Message
        {
            await _mediator.Publish(message, cancellationToken).ConfigureAwait(false);
        }

        public async Task RaiseEvent<T>(T @event, CancellationToken cancellationToken = default) where T : Event
        {
            await _mediator.Publish(@event, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse> SendCommand<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task<Unit> SendCommand(ICommand command, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
