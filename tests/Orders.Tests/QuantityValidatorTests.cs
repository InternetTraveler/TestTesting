using Orders.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Tests;

public class QuantityValidatorTests
{
    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(101, false)] 
    public void IsValidQty_ChecksBounds(int qty, bool expected) 
    {
        var result = QuantityValidator.IsValidQty(qty); 
        Assert.Equal(expected, result); }
}
