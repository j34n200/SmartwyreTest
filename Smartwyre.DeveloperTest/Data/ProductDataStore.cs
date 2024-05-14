using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    public Product GetProduct(string productIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        List<Product> availableRebates = new List<Product>();
        availableRebates.Add(new Product { Identifier = "P1", SupportedIncentives = SupportedIncentiveType.FixedCashAmount });
        availableRebates.Add(new Product { Identifier = "P2", SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 100 });
        availableRebates.Add(new Product { Identifier = "P3", SupportedIncentives = SupportedIncentiveType.AmountPerUom });

        return availableRebates.FirstOrDefault(x => x.Identifier == productIdentifier);
    }
}
