using Core.Commands;
using Core.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Behaviors
{
    public class CommandValidatorBehavior<TCommand, TResponse> : CommandPipelineBehaviorBase<TCommand, TResponse>
        where TCommand : ICommandBase
    {
        private readonly Notifiable _notifiable;

        public CommandValidatorBehavior(ILoggerFactory loggerFactory,  Notifiable notifiable) : base(loggerFactory)
        {
            _notifiable = notifiable;
        }

        protected override async Task<TResponse> Process(TCommand command, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //se o Command estiver inválido, os erros são notificados.
            if (!await command.IsValid())
            {
                await _notifiable.NotifyValidationErrors(command.ValidationResult);
                return default;
            }

            return await next();
        }
    }
}
