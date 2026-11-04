using Moq;
using Orders.Abstractions;
using Orders.Models;
using Orders.Services;
using Orders.Repositories;
using Xunit;

namespace Orders.Tests;

public class OrderServiceTests
{
    readonly Mock<IOrderRepository> _repo = new();
    readonly Mock<IMessagePublisher> _pub = new();
    readonly OrderService _sut;

    public OrderServiceTests() =>
        _sut = new(_repo.Object, _pub.Object, new DiscountCalculator());

    static NewOrder Cmd() =>
        new(new List<OrderItem> { new("Книга", 300m, 2) });

    // 11
    [Fact]
    public async Task CreateAsync_ValidOrder_PublishesEventWithSameId()
    {
        var id = await _sut.CreateAsync(Cmd());

        _pub.Verify(p => p.PublishAsync(
            It.Is<OrderCreated>(e => e.OrderId == id)), Times.Once());
    }

    // 12
    [Fact]
    public async Task CreateAsync_SaveFails_DoesNotPublish()
    {
        _repo.Setup(r => r.SaveAsync(It.IsAny<Order>()))
             .ThrowsAsync(new TimeoutException());

        await Assert.ThrowsAsync<TimeoutException>(() => _sut.CreateAsync(Cmd()));

        _pub.Verify(p => p.PublishAsync(It.IsAny<OrderCreated>()), Times.Never());
    }

    // 13
    [Fact]
    public async Task CreateAsync_NullCommand_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.CreateAsync(null!));
    }

    // 14
    [Fact]
    public async Task CreateAsync_EmptyItems_ThrowsAndDoesNotSave()
    {
        var cmd = new NewOrder(new List<OrderItem>());

        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(cmd));

        _repo.Verify(r => r.SaveAsync(It.IsAny<Order>()), Times.Never());
    }

    // 15
    [Fact]
    public async Task CreateAsync_PublishFails_ExceptionPropagates()
    {
        _pub.Setup(p => p.PublishAsync(It.IsAny<OrderCreated>()))
            .ThrowsAsync(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.CreateAsync(Cmd()));
    }
}