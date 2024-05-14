using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    public Rebate GetRebate(string rebateIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        List<Rebate> availableRebates = new List<Rebate>();
        availableRebates.Add(new Rebate { Identifier = "R1", Incentive = IncentiveType.FixedCashAmount, Amount = 10 });
        availableRebates.Add(new Rebate { Identifier = "R2", Incentive = IncentiveType.FixedRateRebate, Percentage = 0.2m });
        availableRebates.Add(new Rebate { Identifier = "R3", Incentive = IncentiveType.AmountPerUom, Amount = 10 });

        return availableRebates.FirstOrDefault(x => x.Identifier == rebateIdentifier);
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}
