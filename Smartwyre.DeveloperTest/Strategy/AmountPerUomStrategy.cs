using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Strategy
{
    public class AmountPerUomStrategy : IRebateCalculateStrategy
    {
        public decimal CalculateRebate(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate.Amount * request.Volume;
        }

        public bool IsIncentiveValid(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate.Incentive == IncentiveType.AmountPerUom &&
               product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) &&
               rebate.Amount != 0 && request.Volume != 0;
        }
    }
}
