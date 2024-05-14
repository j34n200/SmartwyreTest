using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Interfaces
{
    public interface IRebateCalculateStrategy
    {
        decimal CalculateRebate(Rebate rebate, Product product, CalculateRebateRequest request);
        bool IsIncentiveValid(Rebate rebate, Product product, CalculateRebateRequest request);
    }
}
