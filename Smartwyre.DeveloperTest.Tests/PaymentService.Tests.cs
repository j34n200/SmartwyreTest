using Microsoft.Extensions.Logging;
using Moq;
using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Strategy;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    private readonly Mock<ILogger<RebateService>> _loggerMock;

    public PaymentServiceTests()
    {
        _loggerMock = new Mock<ILogger<RebateService>>();
    }

    [Fact]
    public void FixedCashRebate_Success_When_Data_Is_Valid()
    {
        // Arrange
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();
        var fixedCashStrategy = new FixedCashRebateStrategy();

        rebateDataStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(new Rebate { Identifier = "R1", Incentive = IncentiveType.FixedCashAmount, Amount = 10 });
        productDataStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(new Product { Identifier = "P1", SupportedIncentives = SupportedIncentiveType.FixedCashAmount });

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object, new[] { fixedCashStrategy }, _loggerMock.Object);
        var request = new CalculateRebateRequest();

        // Act
        var result = rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public void FixedRateRebate_Success_When_Data_Is_Valid()
    {
        // Arrange
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();
        var fixedRateRebateStrategy = new FixedRateRebateStrategy();

        rebateDataStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(new Rebate { Identifier = "R2", Incentive = IncentiveType.FixedRateRebate, Percentage = 0.2m });
        productDataStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(new Product { Identifier = "P2", SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 100 });

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object, new[] { fixedRateRebateStrategy }, _loggerMock.Object);
        var request = new CalculateRebateRequest();
        request.Volume = 10;

        // Act
        var result = rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public void FixedRateRebate_Fails_When_Percentage_Is_Zero()
    {
        // Arrange
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();
        var fixedRateRebateStrategy = new FixedRateRebateStrategy();

        rebateDataStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(new Rebate { Identifier = "R2", Incentive = IncentiveType.FixedRateRebate, Percentage = 0 });
        productDataStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(new Product { Identifier = "P2", SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 100 });

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object, new[] { fixedRateRebateStrategy }, _loggerMock.Object);
        var request = new CalculateRebateRequest();
        request.Volume = 10;

        // Act
        var result = rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void AmountPerUom_Success_When_Data_Is_Valid()
    {
        // Arrange
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();
        var amountPerUomStrategy = new AmountPerUomStrategy();

        rebateDataStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(new Rebate { Identifier = "R3", Incentive = IncentiveType.AmountPerUom, Amount = 10 });
        productDataStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(new Product { Identifier = "P3", SupportedIncentives = SupportedIncentiveType.AmountPerUom });

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object, new[] { amountPerUomStrategy }, _loggerMock.Object);
        var request = new CalculateRebateRequest();
        request.Volume = 10;

        // Act
        var result = rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public void RebateCalculation_Fails_When_Incentive_Is_Not_Supported()
    {
        // Arrange
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();
        var amountPerUomStrategy = new AmountPerUomStrategy();

        rebateDataStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(new Rebate { Identifier = "R3", Incentive = IncentiveType.FixedRateRebate, Amount = 10 });
        productDataStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(new Product { Identifier = "P3", SupportedIncentives = SupportedIncentiveType.AmountPerUom });

        var rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object, new[] { amountPerUomStrategy }, _loggerMock.Object);
        var request = new CalculateRebateRequest();
        request.Volume = 10;

        // Act
        var result = rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
    }
}
