using Core.Commands;
using Core.Events;
using System.Threading;
using System.Threading.Tasks;
using Unit = Core.Tango.Types.Unit;

namespace Core.Bus
{
    public interface IMediatorHandler
    {
        Task PublishMessage<T>(T message, CancellationToken cancellationToken = default) where T : Message;

        Task<TResponse> SendCommand<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);

        Task<Unit> SendCommand(ICommand command, CancellationToken cancellationToken = default);

        Task RaiseEvent<T>(T @event, CancellationToken cancellationToken = default) where T : Event;
    }
}