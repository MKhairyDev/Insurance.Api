using System;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain
{
    public class ProductTypeSurchargeRate
    {
        [Key]
        public int ProductTypeId { get; set; }
        public float SurchargeRate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
