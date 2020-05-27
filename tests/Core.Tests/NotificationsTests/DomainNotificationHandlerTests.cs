using Core.Notifications;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using System.Threading;
using Xunit;

namespace Core.Tests.NotificationsTests
{
    public class DomainNotificationHandlerTests
    {
        [Fact]
        [Trait("Core", "DomainNotificationHandler")]
        public void add_notification()
        {
            var domainNotification = new DomainNotification("Teste", "ERRO");
            INotifications domainNotificationHandler = new DomainNotificationHandler(new LoggerFactory());

            var processMethod = domainNotificationHandler.GetType().GetMethod("Process", BindingFlags.NonPublic | BindingFlags.Instance);

            _ = processMethod.Invoke(domainNotificationHandler, new object[] { domainNotification, It.IsAny<CancellationToken>() });

            Assert.True(domainNotificationHandler.HasNotifications());
        }
    }
}
