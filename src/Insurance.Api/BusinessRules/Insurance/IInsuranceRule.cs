using Insurance.Api.External.Models;

namespace Insurance.Api.BusinessRules.Insurance
{
    public interface IInsuranceRule<in T>
    {
        bool Match(T entity);
        float Calculate();
    }
}
