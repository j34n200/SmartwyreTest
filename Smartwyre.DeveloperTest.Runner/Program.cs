using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Strategy;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.AddConsole();
            })
            .AddTransient<IRebateDataStore, RebateDataStore>()
            .AddTransient<IProductDataStore, ProductDataStore>()
            .AddTransient<IRebateCalculateStrategy, FixedCashRebateStrategy>()
            .AddTransient<IRebateCalculateStrategy, FixedRateRebateStrategy>()
            .AddTransient<IRebateCalculateStrategy, AmountPerUomStrategy>()
            .AddTransient<IRebateService, RebateService>()
            .BuildServiceProvider();

        var logger = serviceProvider.GetService<ILogger<Program>>();

        try
        {
            var rebateService = serviceProvider.GetRequiredService<IRebateService>();

            Console.WriteLine("Insert Rebate Identifier: ");
            string rebateId = Console.ReadLine().ToString();

            Console.WriteLine("Insert Product Identifier: ");
            string productId = Console.ReadLine().ToString();

            Console.WriteLine("Insert volume: ");
            decimal volume = 0;
            if (!decimal.TryParse(Console.ReadLine(), out volume))
            {
                logger.LogError("Volume is not valid.");
                throw new Exception("Volume is not valid");
            }

            var request = new CalculateRebateRequest
            {
                RebateIdentifier = rebateId,
                ProductIdentifier = productId,
                Volume = volume
            };

            CalculateRebateResult result = rebateService.Calculate(request);

            if (result.Success)
            {
                logger.LogInformation("Rebate process successfuly finished.");
            }
            else
            {
                logger.LogError("Something went wrong during rebate calculation.");
            }

            Console.WriteLine($"Result: {result.Success}");

        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex}");
        }
    }
}
