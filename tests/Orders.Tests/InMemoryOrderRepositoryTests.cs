using Orders.Models;
using Orders.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.Tests;

public class InMemoryOrderRepositoryTests
{
    readonly InMemoryOrderRepository _repo = new();

    static Order NewOrder()
    {
        var o = new Order();
        o.Add("Книга", 300m, 2);
        return o;
    }

    // 22
    [Fact]
    public async Task SaveAsync_ThenFindAsync_ReturnsSameOrder()
    {
        var order = NewOrder();

        await _repo.SaveAsync(order);
        var found = await _repo.FindAsync(order.Id);

        Assert.Same(order, found);
    }

    // 23
    [Fact]
    public async Task FindAsync_UnknownId_ReturnsNull()
    {
        var found = await _repo.FindAsync(Guid.NewGuid());

        Assert.Null(found);
    }

    // 24
    [Fact]
    public async Task SaveAsync_Null_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _repo.SaveAsync(null!));
    }
}