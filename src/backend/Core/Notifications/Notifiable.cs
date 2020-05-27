using Core.Bus;
using Core.Commands;
using FluentValidation.Results;
using System.Threading.Tasks;

namespace Core.Notifications
{
    public class Notifiable
    {
        private readonly IMediatorHandler _bus;
        private readonly INotifications _notifications;

        public Notifiable(IMediatorHandler bus, INotifications notifications)
        {
            _bus = bus;
            _notifications = notifications;
        }

        public async Task ValidateAndNotifyValidationErrors(ICommandBase message)
        {
            await message.IsValid();
            await NotifyValidationErrors(message.ValidationResult);
        }

        public async Task NotifyValidationErrors(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                await Notify(error.PropertyName, error.ErrorMessage);
            }
        }

        public Task Notify(string key, string value)
        {
            return Notify(new DomainNotification(key, value));
        }

        public Task Notify(DomainNotification notification)
        {
            return _bus.RaiseEvent(notification);
        }

        public bool IsValid()
        {
            return !_notifications.HasNotifications();
        }
    }
}