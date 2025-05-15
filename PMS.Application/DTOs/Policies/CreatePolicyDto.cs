using PMS.Domain.Enumeration;

namespace PMS.Application.DTOs.Policies
{
    public class CreatePolicyDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public PolicyType PolicyType { get; set; }
    }
}
