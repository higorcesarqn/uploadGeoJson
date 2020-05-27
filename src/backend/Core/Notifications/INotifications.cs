using System.Collections.Generic;
using System.Linq;

namespace Core.Notifications
{
    public interface INotifications
    {
        IReadOnlyList<DomainNotification> GetNotifications();
        bool HasNotifications() => GetNotifications().Any();
    }
}
