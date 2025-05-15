using PMS.Domain.Enumeration;

namespace PMS.Domain.Entities
{
    public class Policy : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PolicyType PolicyType { get; set; }
    }
}
