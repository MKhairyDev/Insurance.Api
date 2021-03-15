using Insurance.Api.External.Models;

namespace Insurance.Api.BusinessRules.Insurance
{
    public interface IInsuranceCalculator<in T> where T:class
    {
        float Calculate(T entity);
    }
}