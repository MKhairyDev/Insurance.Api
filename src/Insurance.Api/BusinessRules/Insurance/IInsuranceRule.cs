using Insurance.Api.External.Models;

namespace Insurance.Api.BusinessRules.Insurance
{
    public interface IInsuranceRule
    {
        bool Match(ProductDto product);
        float Calculate();
    }
}
