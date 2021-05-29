using System;
using Insurance.Domain;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Data
{
    public class InsuranceContext: DbContext
    {
        /// <summary>
        /// For being able to inject different Database provider(ex:EntityFrameworkCore.InMemory provider for testing purpose)
        /// </summary>
        /// <param name="options"></param>
        public InsuranceContext(DbContextOptions<InsuranceContext> options) : base(options)
        {
        }
        public InsuranceContext()
        {

        }
        public DbSet<ProductTypeSurchargeRate> ProductSurchargeRates { get; set; }
    }
}
