using Microsoft.Extensions.Logging;
using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IEnumerable<IRebateCalculateStrategy> _rebateCalculateStrategies;
    private readonly ILogger<RebateService> _logger;

    public RebateService(
    IRebateDataStore rebateDataStore,
    IProductDataStore productDataStore,
    IEnumerable<IRebateCalculateStrategy> rebateCalculateStrategies,
    ILogger<RebateService> logger
    )
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _rebateCalculateStrategies = rebateCalculateStrategies;
        _logger = logger;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult();
        result.Success = true;

        try
        {
            Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
            Product product = _productDataStore.GetProduct(request.ProductIdentifier);

            var rebateAmount = 0m;

            if (rebate == null || product == null)
            {
                _logger.LogError("Error, rebate and (or) product not found.");
                result.Success = false;
                return result;
            }

            var selectedStrategy = _rebateCalculateStrategies.FirstOrDefault(x => x.IsIncentiveValid(rebate, product, request));
            if (selectedStrategy == null)
            {
                _logger.LogError("Error, incentive type is not valid.");
                result.Success = false;
                return result;
            }

            rebateAmount = selectedStrategy.CalculateRebate(rebate, product, request);
            _logger.LogInformation($"Rebate Amount: {rebateAmount}");

            _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);

            return result;

        }
        catch (Exception ex)
        {
            result.Success = false;
            throw new Exception($"Error: {ex}");
        }
    }
}
