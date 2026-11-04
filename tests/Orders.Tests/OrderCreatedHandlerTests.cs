using Moq;
using Orders.Abstractions;
using Orders.Handlers;
using Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Tests
{
    public class OrderCreatedHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ValidMessage_SendsNotification()
        { 
            var message = new OrderCreated(Guid.NewGuid()); 
            var notificationSenderMock = new Mock<INotificationSender>(); 
            var handler = new OrderCreatedHandler(notificationSenderMock.Object); 
            
            await handler.HandleAsync(message, CancellationToken.None); 
            
            notificationSenderMock.Verify( x => x.SendAsync(It.Is<string>(s => s.Contains(message.OrderId.ToString()))), Times.Once); }
        }
}
