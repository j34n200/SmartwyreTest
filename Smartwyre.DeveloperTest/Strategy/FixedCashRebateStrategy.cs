using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Strategy
{
    public class FixedCashRebateStrategy : IRebateCalculateStrategy 
    {
        public decimal CalculateRebate(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate.Amount;
        }

        public bool IsIncentiveValid(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate.Incentive == IncentiveType.FixedCashAmount &&
               product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) &&
               rebate.Amount != 0;
        }
    }
}
