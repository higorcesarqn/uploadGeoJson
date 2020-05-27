using FluentValidation.Results;
using MediatR;
using System.Threading.Tasks;
using Unit = Core.Tango.Types.Unit;

namespace Core.Commands
{
    public interface ICommand : ICommandBase, IRequest<Unit> { }

    public interface ICommand<TResponse> : ICommandBase, IRequest<TResponse> { }

    public interface ICommandBase 
    {
        public ValidationResult ValidationResult { get; set; }

        public abstract Task<bool> IsValid();
    }
}
