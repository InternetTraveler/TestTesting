using Orders.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Tests;

public class DiscountCalculatorTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(999.99, 0)]
    [InlineData(1000, 100)]
    [InlineData(2000, 200)] 
    public void Calculate_ReturnsExpected(decimal amount, decimal expected) 
    { 
        var calculator = new DiscountCalculator(); 
        var result = calculator.Calculate(amount); 
        Assert.Equal(expected, result); 
    }

    [Fact] 
    public void Calculate_NegativeTotal_Throws() 
    { 
        var calculator = new DiscountCalculator(); 
        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.Calculate(-1m));
    }
}
