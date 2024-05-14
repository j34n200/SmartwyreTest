using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Strategy
{
    public class FixedRateRebateStrategy : IRebateCalculateStrategy
    {
        public decimal CalculateRebate(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return product.Price * rebate.Percentage * request.Volume;
        }

        public bool IsIncentiveValid(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate.Incentive == IncentiveType.FixedRateRebate &&
               product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate) &&
               rebate.Percentage != 0 && product.Price != 0 && request.Volume != 0;
        }
    }
}
