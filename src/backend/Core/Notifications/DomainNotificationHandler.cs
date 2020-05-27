using Core.Events;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Notifications
{
    public class DomainNotificationHandler : EventHandler<DomainNotification>, INotifications
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationHandler(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _notifications = new List<DomainNotification>();
        }

        public IReadOnlyList<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }

        protected override Task Process(DomainNotification @event, CancellationToken cancellationToken)
        {
            _notifications.Add(@event);
            return Task.CompletedTask;
        }
    }
}