namespace PMS.Domain.Entities
{
    public class PolicyMember : BaseEntity
    {
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
