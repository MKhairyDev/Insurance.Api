using Insurance.Api.External.Models;

namespace Insurance.Api.BusinessRules.Insurance
{
    public interface IInsuranceCalculator
    {
        float CalculateProductInsurance(ProductDto product);
    }
}