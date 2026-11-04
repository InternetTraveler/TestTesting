using Orders.Models;

namespace Orders.Tests;

public class OrderTests
{
    // Готовый пример теста. Остальные тесты вы пишете по аналогии.
    [Fact]
    public void Total_TwoItems_ReturnsSum()
    {
        // Arrange
        var order = new Order();
        order.Add("Книга", 300m, 2);
        order.Add("Ручка", 50m, 1);

        // Act
        var total = order.Total();

        // Assert
        Assert.Equal(650m, total);
    }

    [Fact]
    public void Total_EmptyOrder_ReturnsZero()
    {
        var order = new Order();
        var total = order.Total();
        Assert.Equal(0m, total);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Add_InvalidName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => {
            var Order = new Order(); 
            Order.Add(name, 100m, 1); 
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Add_NonPositivePrice_Throws(decimal price)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => {
            var Order = new Order();
            Order.Add("name", price, 1);
        });
    }

    [Fact]
    public void Constructor_WithItems_SameTotalAsAdd()
    { 
        var items = new List<OrderItem> { 
            new OrderItem("A", 100m, 2), 
            new OrderItem("B", 50m, 1) 
        }; 
        var orderViaCtor = new Order(items); 
        var orderViaAdd = new Order(); 
        orderViaAdd.Add("A", 100m, 2); 
        orderViaAdd.Add("B", 50m, 1); 
        
        var totalCtor = orderViaCtor.Total(); 
        var totalAdd = orderViaAdd.Total(); 
        
        Assert.Equal(totalAdd, totalCtor); }
    }
